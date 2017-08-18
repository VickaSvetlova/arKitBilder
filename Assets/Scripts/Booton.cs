using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booton : MonoBehaviour {
    public Color DefaultColor;
    public Color SelectedColour;
    private Material mat;
    public Vector3 touchPosition;

    private void Start()
    {
      mat = GetComponent<Renderer>().material;
    }
   void OnTouchDown()
    {
         mat.color = SelectedColour;
        Calculate();



    }
    void OnTouchUP()
    {
        mat.color = DefaultColor;
    }
    void OnTouchStay()
    {
       mat.color = SelectedColour;
        Calculate();
    }
    void OnTouchExit()
    {
      mat.color = DefaultColor;
    }
    void Calculate()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(touchPosition);
        float dist = Vector3.Distance(transform.position, Camera.main.transform.position);
        pos = new Vector3(touchPosition.x, touchPosition.y, dist);
        transform.position = pos;
    }
}
