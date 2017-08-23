using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hiroScript : MonoBehaviour 
{
    #region Enums
    #endregion

    #region Delegates
    #endregion

    #region Structures
    #endregion

    #region Classes
    #endregion

    #region Fields
    #endregion

    #region Events
    #endregion

    #region Properties
    #endregion

    #region Methods
    private void Start()
    {
        var mouse = (MouseLook)FindObjectOfType(typeof(MouseLook));
        mouse.EventRotation+=copyRot;
        
        
    }
    public void copyRot(Quaternion rotations)
    {
        transform.rotation= rotations;

    }
    #endregion

    #region Event Handlers
    #endregion
}
