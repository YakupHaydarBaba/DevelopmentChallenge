using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public BridgeController towerController;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            towerController.PlaceCube();
            
        }
    }

}