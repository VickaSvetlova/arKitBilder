using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectScript : MonoBehaviour {
    private bool inside=false;
	public void LookAT()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    public void Inside()
    {
        if (!inside)
        {
            transform.localScale = new Vector3(1, 1, 1);
            
        }
        else
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        inside = !inside;
    }
   
}
