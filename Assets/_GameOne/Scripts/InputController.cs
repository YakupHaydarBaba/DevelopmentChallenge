using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]private LayerMask gridLayerMask;
    [SerializeField] private GridController gridController;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) HandleClick();
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, gridLayerMask))
        {
            hit.transform.GetComponent<Grid>().SetMarked();
        }   
        gridController.FindMarkedSequences();
    }
}
