using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chouseMenu : MonoBehaviour
{
    public GameObject[] Building;
    public int numberBuiding;
    private Animator animator;

     private void Start()
    {
        animator = GetComponent<Animator>();   
    }

    public void ChangeBuilding(int number){
        foreach (var item in Building)
        {
            item.SetActive(false);
        }
        Building[number].SetActive(true);
        OpenClouseMenu();
    }
    public void OpenClouseMenu(){

        if(animator.GetBool("Active")){
            animator.SetBool("Active",false);
        }else{
            animator.SetBool("Active", true);
        }
    }
}
