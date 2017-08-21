using System;
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
    private enum state { move, rotation, setpoint }
    private state Stat;
    public Text text;
    public bool setPos;

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
        text.text = Stat.ToString();
    }

    void Update()
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
                            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);

                            Debug.Log("pressDown");
                            dist = hit.distance; //hit.distance + Camera.main.nearClipPlane;
                            trim = hit.collider.transform.position - hit.point;

                            recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                            //recipient.GetComponent<Booton>().touchPosition = hit.point;
                        }
                        if (Input.GetMouseButtonUp(0))
                        {
                            recipient.SendMessage("OnTouchUP", hit.point, SendMessageOptions.DontRequireReceiver);
                        }
                        if (Input.GetMouseButton(0))
                        {
                            Debug.Log("mouseButton");
                            Vector3 pos = Input.mousePosition;
                            pos.z = dist;
                            //pos = ray.origin + ray.direction * dist;
                            //pos = ray.direction+ trim;
                            pos = Camera.main.ScreenToWorldPoint(pos) + trim; // это новая координата

                            recipient.GetComponent<Booton>().touchPosition = pos;
                            recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
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


                break;
        }



#endif


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
                        recipient.GetComponent<Booton>().touchPosition = pos;
                        recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);

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
    }
}

