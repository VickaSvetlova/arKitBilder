using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectScript : MonoBehaviour {
    private bool inside=false;
    public GameObject objParent;
    private Camera cam;
    public float scalleSmallFactor;
	public void LookAT()

    {
        objParent.transform.LookAt(Camera.main.transform.position);
        objParent.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    public void Inside()
    {
        if (!inside)
        {
            Debug.Log("1:1");
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
        objParent.transform.localScale = new Vector3(objParent.transform.localScale.x+scaleFactor, objParent.transform.localScale.y+scaleFactor, objParent.transform.localScale.z+scaleFactor);

    }
   
}
