﻿using System.Collections;
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
            objParent.transform.localScale = new Vector3(10, 10, 10);
            objParent.transform.position = Camera.main.transform.position;           
            
        }
        else
        {
            Debug.Log("smal");
            objParent.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            objParent.transform.position = Camera.main.transform.forward*1;
           
        }
        inside = !inside;
    }
   
}
