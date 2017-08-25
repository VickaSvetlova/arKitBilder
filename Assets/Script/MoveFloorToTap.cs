using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloorToTap : MonoBehaviour
{
    #region Enum
    private enum _state { up, down, lookTap,inside, walk }
    private _state direct;
    #endregion


    #region Deligates
    #endregion

    #region Propertes
    #endregion

    #region Fields
    private bool _move = false;
    public bool Hit { get; private set; }
    private GameObject floorObject = null;
    private Vector3 newSpotPositio;
    private GameObject heigtHiro;
    private string floorIgnore = null;
    private Vector3 SpotPosition;
    private bool _moveCorutine = false;
    private Vector3 hitTo;
    [SerializeField]
    private GameObject spotPatch;
    private GameObject spotOld = null;
    public GameObject _stage;
    [SerializeField]
    private string _floorNameTag = "floor";
    private string _floorNameThis;
    private RaycastHit hitDown;
    private float hitDowns;
    public bool moveTo;
    public bool ignoreFloor;

    private Collider floorColider;
    private Collider ignoreColider;

    private GameObject hiro;
    private bool moveToTap;


    #endregion

    #region Events
    #endregion

    #region Metods
    private void Awake()
    {
        _stage = GameObject.FindGameObjectWithTag("stage");
        hiro = GameObject.FindGameObjectWithTag("Player");

        
    }
    public void SetStage(){
		_stage = GameObject.FindGameObjectWithTag("stage");
    }
    private void Update()
    {
        controlKeys();
        controllMove();
    }
    private void controllMove()
    {
        if (!_moveCorutine && _move)
        {
            _moveCorutine = true;
            StartCoroutine(moveToPosition(_stage.transform.localPosition, SpotPosition));
            // GetComponentInChildren<Collider>().enabled=false;
           // GetComponentInChildren<Rigidbody>().isKinematic = true;
        }
    }

    private IEnumerator moveToPosition(Vector3 v1, Vector3 v2)//корутина плавного изменения позиции
    {
        float timer = 0f;
        float maxTime = 1.5f;
        while (timer < maxTime)
        {
            float coeff = timer / maxTime;
            _stage.transform.position = Vector3.Lerp(v1, v2, timer);
            timer += Time.deltaTime;
            yield return null;
        }
        // GetComponentInChildren<Collider>().enabled = true;
        //GetComponentInChildren<Rigidbody>().isKinematic = false;
        _move = false;
        _moveCorutine = false;
        moveToTap = false;
       
    }
    private void controlKeys()//команды управления.
    {
        if (!_move && _stage)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                getFloor(_state.up);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                getFloor(_state.down);
            }
            if (Input.GetMouseButtonDown(1))
            {
                getFloor(_state.lookTap);
            }
            if (Input.GetKey(KeyCode.W))
            {
                getFloor(_state.walk);
            }
        }
    }
    private void getFloor(_state State)
    {
        if (!_move)
        {
            switch (State)
            {
                case _state.inside:


					
                    if (rayCaster(Camera.main.transform.position,Camera.main.transform.forward ))
					{
						
						rayCaster(new Vector3(hitTo.x, hitTo.y + hitDowns, hitTo.z), Vector3.down);


						CalculateMove(hitTo);
					}
					break;
                case _state.lookTap:


                    ignoreColider = null;
                    moveToTap = true;
                    if (rayCaster(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction))
                    {
						ignoreColider = null;
                        rayCaster(new Vector3(hitTo.x,hitTo.y + hitDowns, hitTo.z), Vector3.down);

                       
                        CalculateMove(hitTo);
                    }
                    break;
                case _state.up:
                   
					if (rayCaster(Camera.main.transform.position, Vector3.up))
                    {                     
                        rayCaster(new Vector3(hitTo.x, hitTo.y + hitDowns, hitTo.z), Vector3.down);
                        CalculateMove(hitTo);
                    }
                    break;
                case _state.down:
                  
					
                    if (rayCaster(Camera.main.transform.position, Vector3.down))
                    {
                        rayCaster(new Vector3(hitTo.x, hitTo.y - hitDowns, hitTo.z), Vector3.down);
                        CalculateMove(hitTo);
                    }
                    break;

                case _state.walk:

                    Debug.Log("Walk");
                    Vector3 dir = Camera.main.transform.forward;
                    dir.y = 0f;
                    _stage.transform.Translate(dir * -Time.deltaTime * 5, Space.World);

                    break;
            }
        }
    }
    //booton
    public void Forward()
    {
        getFloor(_state.walk);
	}
	public void jumpToSpot()
	{
        ignoreColider = null;
        getFloor(_state.inside);	
	}
    public void liftUp(){
        getFloor(_state.up);
    }
    public void liftDown(){
        getFloor(_state.down);
    }
        
    private void CalculateMove(Vector3 hitToPos)
    {
        ignoreColider = null;
       
        float heghtHiro = 1.7f;// = GetComponentInChildren<Collider>().bounds.size.y / 2;		
        SpotPosition = _stage.transform.position + new Vector3(transform.position.x, transform.position.y - heghtHiro, transform.position.z) - hitToPos;
        instatientPatchSpot(SpotPosition);

        if (moveTo)
        {
            _move = true;
        }
    }
    private void instatientPatchSpot(Vector3 posSpot)
    {
        spotPatch.transform.parent = _stage.transform;
        spotPatch.transform.localPosition = _stage.transform.InverseTransformPoint(hitTo);
    }


    private bool rayCaster(Vector3 positionRay, Vector3 dir)
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(positionRay, dir, 1000f);
      

        for (int i = 0; i < hits.Length; i++)
        {         
            RaycastHit hit = hits[i];
            Collider Hit = hit.transform.GetComponent<Collider>();

            Debug.DrawRay(positionRay, dir*1000, Color.blue);
            if (Hit)
            {             
                if (hit.collider.tag == _floorNameTag && hit.collider!=ignoreColider)
                {
                    hitDowns =  hit.collider.gameObject.GetComponent<Renderer>().bounds.size.y + 0.01f;     					
                    floorColider = hit.collider;

                    if (moveToTap){					

					ignoreColider = hit.collider;

					}else{
					ignoreColider = null;

					}

					hitTo = hit.point;
					return Hit;                   
                }
                else
                {                   
                    hitTo = hit.point;
                }
            }
            floorObject = null;			
        }
      
        return false;
    }
    #endregion

}


