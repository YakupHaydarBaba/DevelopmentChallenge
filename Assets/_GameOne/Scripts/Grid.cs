using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid : MonoBehaviour
{
    public Vector2Int gridPosition{ get; private set; }
    [SerializeField] private GameObject markObject;

    public bool IsMarked { get; private set; }

    public void Init(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    
    public void SetMarked()
    {
        this.IsMarked = true;
        markObject.SetActive(IsMarked);
    }
    public void ResetMarked()
    {
        this.IsMarked = false;
        markObject.SetActive(IsMarked);
    }
}