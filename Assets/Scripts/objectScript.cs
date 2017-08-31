using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectScript : MonoBehaviour {
    private bool inside=false;
    public GameObject objParent;
    private Camera cam;
    public float scalleSmallFactor;
    private Vector3 startPos;

    private void Awake()
    {
        objParent = this.gameObject;
        objParent.transform.localScale = new Vector3(scalleSmallFactor, scalleSmallFactor, scalleSmallFactor);
        objParent.transform.position = Camera.main.transform.forward * 2;
        startPos = objParent.transform.position;
    }

    public void Inside()
    {
        if (!inside)
        {
            Debug.Log("1:1");
            objParent.transform.localScale = new Vector3(1, 1, 1);
            startPos = objParent.transform.position;
            objParent.transform.position = Camera.main.transform.position;

        }
        else
        {
            Debug.Log("smal");
            objParent.transform.localScale = new Vector3(scalleSmallFactor, scalleSmallFactor, scalleSmallFactor);

            objParent.transform.position = startPos;

        }
        inside = !inside;
    }
    public void Scale(float scaleFactor)
    {
        
        objParent.transform.localScale *= (1+scaleFactor);
		objParent.transform.localScale = new Vector3(
	  Mathf.Clamp(objParent.transform.localScale.x, 0.01f, 1f),
	  Mathf.Clamp(objParent.transform.localScale.y, 0.01f, 1f),
	  Mathf.Clamp(objParent.transform.localScale.z, 0.01f, 1f)
			);
    }

    public void SetScale(Vector3 _scale)
    {
		objParent.transform.localScale = _scale;

        objParent.transform.localScale = new Vector3(
      Mathf.Clamp(_scale.x, 0.01f, 1f),
      Mathf.Clamp(_scale.y, 0.01f, 1f),
            Mathf.Clamp(_scale.z, 0.01f, 1f));

	}

}
