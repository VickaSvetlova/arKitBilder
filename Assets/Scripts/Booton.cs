using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booton : MonoBehaviour {
    public Color DefaultColor;
    public Color SelectedColour;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    void OnTouchDown()
    {
        mat.color = SelectedColour;
    }
    void OnTouchUP()
    {
        mat.color = DefaultColor;
    }
    void OnTouchStay()
    {
        mat.color = SelectedColour;
    }
    void OnTouchExit()
    {
        mat.color = DefaultColor;
    }
}
