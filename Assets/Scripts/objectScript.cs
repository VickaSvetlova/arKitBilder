using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectScript : MonoBehaviour {
    private bool inside=false;
    public GameObject objParent;
    private Camera cam;
    public float scalleSmallFactor;


    private void Awake(){
        objParent.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        objParent.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }


	public void LookAT()

    {
        objParent.transform.LookAt(Camera.main.transform.position);
        objParent.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    public void Inside()
    {
        if (!inside)
        {
            //Debug.Log("1:1");
            objParent.transform.localScale = new Vector3(1, 1, 1);
            objParent.transform.position = Camera.main.transform.position;

        }
        else
        {
            Debug.Log("smal");
            objParent.transform.localScale = new Vector3(scalleSmallFactor, scalleSmallFactor, scalleSmallFactor);
            objParent.transform.position = Camera.main.transform.forward * 2;

        }
        inside = !inside;
    }
    public void Scale(float scaleFactor){


        objParent.transform.localScale += Vector3.one*scaleFactor;


    }
   
}
