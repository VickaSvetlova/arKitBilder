using UnityEngine;
using System.Collections;

public class Drag2 : MonoBehaviour
{
    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    private Transform toDrag;

    void Update()
    {
        Vector3 v3;

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Here");
            dragging = false;
            return;
        }

        //Touch touch = Input.touches[0];
        Vector3 pos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Draggable"))
            {
         
                toDrag = hit.transform;
                dist = hit.transform.position.z - Camera.main.transform.position.z;
                v3 = new Vector3(pos.x, pos.y, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                offset = toDrag.position - v3;
                dragging = true;
            }
        }
        if (dragging && Input.GetMouseButton(0))
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            toDrag.position = v3 + offset;
        }
        if (dragging && (Input.GetMouseButtonUp(0)))
        {
            dragging = false;
        }
    }
}