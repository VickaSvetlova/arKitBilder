using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectScript : MonoBehaviour {
    private bool inside=false;
    public GameObject objParent;
    private Camera cam;
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
            objParent.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            objParent.transform.position = new Vector3(Camera.main.transform.forward.x/4, Camera.main.transform.position.y, Camera.main.transform.forward.z/4);
           
        }
        inside = !inside;
    }
   
}
