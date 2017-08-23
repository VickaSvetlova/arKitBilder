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
      mat = GetComponentInChildren<Renderer>().material;
    }
   void OnTouchDown()
    {
        return;
         mat.color = SelectedColour;
       
    }
    void OnTouchUP()
    {
        return;
        mat.color = DefaultColor;
    }
    void OnTouchStay()
    {
        return;
       mat.color = SelectedColour;
        Calculate();
    }
    void OnTouchExit()
    {
        return;
      mat.color = DefaultColor;
    }
    void Calculate()
    {
        
        return;
        transform.position = new Vector3(touchPosition.x, touchPosition.y,touchPosition.z);
    }
}
