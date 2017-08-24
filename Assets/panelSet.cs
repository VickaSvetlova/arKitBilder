using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelSet : MonoBehaviour
{
    Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();

    }
        // Update is called once per frame

        public void OpenClouseMenu()
        {
        //Debug.Log("hideMenu");
            if (animator.GetBool("hide"))
            {
                animator.SetBool("hide", false);
            }
            else
            {
                animator.SetBool("hide", true);
            }
        }
    }

		
	

