using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseLook : MonoBehaviour
{
    #region Enums
    #endregion

    #region Delegates
    public delegate void MyDelegate(Quaternion rotateY);
    public MyDelegate EventRotation;
    #endregion

    #region Structures
    #endregion

    #region Classes
    #endregion

    #region Fields
    public float horizontal;
    public float vertical;
    //sensivity
    public float mouseSens = 5;
    //Original rotation object
    Quaternion origRotation;
    public GameObject _obsticle;
    #endregion

    #region Events

    #endregion

    #region Properties
    #endregion

    #region Metods
    private void Start()

    {
        //original position for true Rotation
        origRotation = transform.rotation;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.RightAlt))
        //{
        //    StartCoroutine(moveToPosition, Camera.main.transform.position, Camera.main.transform.position);
        //}
    }


    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            horizontal += Input.GetAxis("Mouse X") * mouseSens;
            vertical += Input.GetAxis("Mouse Y") * mouseSens;
            vertical = Mathf.Clamp(vertical, -89, 89);
            //horizontal = Mathf.Clamp(horizontal, -45, 45);

            Quaternion rotationY = Quaternion.AngleAxis(horizontal, Vector3.up);
            Quaternion rotationX = Quaternion.AngleAxis(-vertical, Vector3.right);

            transform.rotation = origRotation * rotationY * rotationX;
           // EventRotation(rotationY);

        }
    }
    private IEnumerator moveToPosition(Vector3 v2, Vector3 v1)//корутина плавного изменения позиции
    {
        float timer = 0f;
        float maxTime = 1.5f;
        while (timer < maxTime)
        {
            float coeff = timer / maxTime;
            _obsticle.transform.position = Vector3.Lerp(v2, v1, timer);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    #region Event Handlers
    #endregion
}
