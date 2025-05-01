using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Vector2 _gridPosition;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private MeshRenderer _meshRenderer;

    public void Init(Vector2 gridPosition)
    {
        _gridPosition = gridPosition;
    }

    private void SetMaterial()
    {
    }
}