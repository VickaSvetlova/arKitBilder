﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchScript : MonoBehaviour
{
    public LayerMask touchInputMask;
    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private RaycastHit hit;
    private Vector3 trim;
    private float dist;
    private enum state { move, rotation, setpoint,inside,scale }
    private state Stat;
    public Text text;
    public bool setPos;

    private GameObject tempObjec = null;

    private float rotationSpeed = 1f;
    private bool _inside;

    public GameObject point1;
    public GameObject point2;
    public float SensevityScale = 0.0001f;

    private void Start()
    {
        Stat = state.move;
        text.text = Stat.ToString();
    }
    public void statesSwich(string str)
    {
        setPos = false;
        if (str == "move")
        {
            Stat = state.move;
        }
        if (str == "rotate")
        {
            Stat = state.rotation;
        }
        if (str == "setpoint")
        {
            Stat = state.setpoint;
            setPos = true;
        }
		if (str == "inside")
		{
            Stat = state.inside;
            _inside = true;
		}
		if (str == "scale")
		{
            Stat = state.scale;
		}
        text.text = Stat.ToString();
    }

    public void changeSens(float sens){
        rotationSpeed = sens;
        SensevityScale = sens;
        text.text = ""+rotationSpeed;

    }


    void Update()
    {
        if (!_inside)
        {
#if UNITY_EDITOR

            switch (Stat)
            {
                case state.move:

                    if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))

                    {
                        touchesOld = new GameObject[touchList.Count];
                        touchList.CopyTo(touchesOld);
                        touchList.Clear();


                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

                        if (Physics.Raycast(ray, out hit, touchInputMask))
                        {
                            GameObject recipient = hit.transform.gameObject;
                            touchList.Add(recipient);

                            if (Input.GetMouseButtonDown(0))
                            {

                                Debug.Log("pressDown");
                                dist = hit.distance; //hit.distance + Camera.main.nearClipPlane;
                                trim = hit.collider.transform.position - hit.point;
                             //   recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);

                            }
                            if (Input.GetMouseButtonUp(0))
                            {
                                //recipient.SendMessage("OnTouchUP", hit.point, SendMessageOptions.DontRequireReceiver);
                            }
                            if (Input.GetMouseButton(0))
                            {
                                Debug.Log("mouseButton");
                                Vector3 pos = Input.mousePosition;
                                pos.z = dist;
                                //pos = ray.origin + ray.direction * dist;
                                //pos = ray.direction+ trim;
                                pos = Camera.main.ScreenToWorldPoint(pos) + trim; // это новая координата


                                //if (recipient.GetComponent<Booton>())
                                //{
                                //    recipient.GetComponent<Booton>().touchPosition = pos;
                                //}
                                //recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                                if (recipient.tag == "stage")
                                {
                                    recipient.transform.position = pos;
                                }
                            }
                        }
                        foreach (var g in touchesOld)
                        {

                            if (!touchList.Contains(g))
                            {
                                g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                            }
                        }
                    }
                    break;

                //поворот
                case state.rotation:
                    if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))

                    {
                        touchesOld = new GameObject[touchList.Count];
                        touchList.CopyTo(touchesOld);
                        touchList.Clear();




                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

                        if (Physics.Raycast(ray, out hit, touchInputMask))
                        {
                            GameObject recipient = hit.transform.gameObject;
                            touchList.Add(recipient);
                            //if(recipient.transform.position== new Vector3(0,0,0)){
                            //    recipient.transform.position = recipient.transform.position;
                            //}

                            if (Input.GetMouseButtonDown(0))
                            {
                                // recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                                tempObjec = recipient;
                            }
                            if (Input.GetMouseButtonUp(0))
                            {
                                tempObjec = null;
                                //recipient.SendMessage("OnTouchUP", hit.point, SendMessageOptions.DontRequireReceiver);

                            }
                            if (tempObjec && Input.GetMouseButton(0))
                            {

                                //recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                                if (recipient.tag == "stage")
                                {
                                    float rotX = Input.GetAxis(("Mouse X")) * rotationSpeed;
                                    float rotY = Input.GetAxis(("Mouse Y")) * rotationSpeed;
                                    tempObjec.transform.Rotate(Vector3.up, -rotX, Space.World);
                                    tempObjec.transform.Rotate(Vector3.up, -rotY, Space.World);
                                }

                            }
                        }
                        foreach (var g in touchesOld)
                        {

                            if (!touchList.Contains(g))
                            {
                                g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                            }
                        }
                    }

                    break;

                case state.scale:



                    if (Input.GetAxis("Mouse ScrollWheel") != 0)
                    {
                       
                            objectScript script = (objectScript)FindObjectOfType(typeof(objectScript));
                            script.Scale(Input.GetAxis("Mouse ScrollWheel") * SensevityScale);

                    }
                    break;


            }



#endif

            switch (Stat)
            {

                case state.move:

                    if (Input.touchCount > 0)
                    {
                        touchesOld = new GameObject[touchList.Count];
                        touchList.CopyTo(touchesOld);
                        touchList.Clear();

                        foreach (var touch in Input.touches)
                        {

                            Ray ray = Camera.main.ScreenPointToRay(touch.position);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, touchInputMask))
                            {
                                GameObject recipient = hit.transform.gameObject;
                                touchList.Add(recipient);
                                if (touch.phase == TouchPhase.Began)
                                {
                                    dist = hit.distance;
                                    trim = hit.collider.transform.position - hit.point;
                                    recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                                    //  recipient.GetComponent<Booton>().touchPosition = touch.position;
                                }
                                if (touch.phase == TouchPhase.Ended)
                                {
                                    recipient.SendMessage("OnTouchUP", hit.point, SendMessageOptions.DontRequireReceiver);
                                }
                                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                                {
                                    Vector3 pos = touch.position;
                                    pos.z = dist;
                                    pos = Camera.main.ScreenToWorldPoint(pos) + trim; // это новая координата

                                        recipient.transform.position = pos;

                                    //recipient.GetComponent<Booton>().touchPosition = pos;
                                    //recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);

                                }
                                if (touch.phase == TouchPhase.Canceled)
                                {
                                    recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                                }
                            }
                        }
                        foreach (var g in touchesOld)
                        {
                            if (!touchList.Contains(g))
                            {
                                g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                            }
                        }

                    }
                    break;
                case state.rotation:

                    if (Input.touchCount > 0)
                    {
                        touchesOld = new GameObject[touchList.Count];
                        touchList.CopyTo(touchesOld);
                        touchList.Clear();

                        foreach (var touch in Input.touches)
                        {

                            Ray ray = Camera.main.ScreenPointToRay(touch.position);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, touchInputMask))
                            {
                                GameObject recipient = hit.transform.gameObject;
                                touchList.Add(recipient);
                                if (touch.phase == TouchPhase.Began)
                                {
                                    tempObjec = recipient;
                                    recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);

                                }
                                if (touch.phase == TouchPhase.Ended)
                                {
                                    recipient.SendMessage("OnTouchUP", hit.point, SendMessageOptions.DontRequireReceiver);
                                    tempObjec = null;


                                }
                                if (tempObjec && touch.phase == TouchPhase.Moved)
                                {
                                    // recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);

                                        float rotX = touch.deltaPosition.x * rotationSpeed;
                                        float rotY = touch.deltaPosition.y * rotationSpeed;
                                        tempObjec.transform.Rotate(Vector3.up, -rotX, Space.World);
                                        tempObjec.transform.Rotate(Vector3.up, -rotY, Space.World);


                                }
                                if (touch.phase == TouchPhase.Canceled)
                                {
                                   // recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                                    // tempObjec = null;
                                }
                            }
                        }
                        foreach (var g in touchesOld)
                        {
                            if (!touchList.Contains(g))
                            {
                                g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                            }
                        }

                    }
                    break;
                case state.scale:
                    if (Input.touchCount == 2)
                    {

                        Touch touchZero = Input.GetTouch(0);
                        Touch touchOne = Input.GetTouch(1);


                        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;


                        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;


                        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


                        objectScript script = (objectScript)FindObjectOfType(typeof(objectScript));

                       
                            script.Scale(deltaMagnitudeDiff * SensevityScale);
                      

                    }


                    break;


            }
        }
    }
	public void Inside()
	{

		objectScript objTemp = (objectScript)FindObjectOfType(typeof(objectScript));
		if (objTemp != null)
		{
            if(!_inside){
                _inside = true;
            }else{
                _inside = false;
            }
            objTemp.Inside();			
		}
	}
}

