using Salesforce;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{ 

    //keybinds
    [Header("Keybinds")]
    [SerializeField]
    KeyCode interactButton = KeyCode.Mouse0;

    [SerializeField]
    Camera playerCamera;

    [SerializeField]
    float interactRange;

    //Detect when a player interacts with an object
    private void Interact()
    {
        //detect if player presses interact button
        if(Input.GetKeyDown(interactButton))
        {
            //when button is pressed, cast a ray 5 units out
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            //if the ray hits an object, set it's object canvas to active
            if(Physics.Raycast(ray, out hit, interactRange) && hit.transform.CompareTag("Object"))
            {
                hit.transform.GetChild(1).gameObject.SetActive(true);
                GetComponent<PlayerMovement>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                playerCamera.gameObject.GetComponent<CameraMovement>().enabled = false;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }
}