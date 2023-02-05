using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAttack : MonoBehaviour
{
    private PopUpScreens popUp;
    private bool activate;

    private float attack = 20f;

    void Start()
    {
        popUp = GameObject.FindWithTag("PopUps").GetComponent<PopUpScreens>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // attack * popUp.broommodifier;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("squirrel"))
        {
            activate = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("squirrel"))
        {
            activate = true;
        }
    }
}
