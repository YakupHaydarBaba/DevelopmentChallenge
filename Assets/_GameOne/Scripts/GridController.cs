using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int GridSize;
    public CinemachineTargetGroup targetGroup;
    
    [SerializeField]private Transform _gridsParent;

    private List<Grid> _grids = new();

    private void Start()
    {
        CreateNewGrid(); 
    }

    public void CreateNewGrid()
    {
        ClearGrids();
        
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                var grid = Instantiate(Resources.Load("Grid"), new Vector3(i,  j,0), Quaternion.identity,_gridsParent);
                _grids.Add(grid.GetComponent<Grid>());
            }
        } 
        
        var transforms = _grids.Select(x => x.transform).ToList();
        var targets = new CinemachineTargetGroup.Target[transforms.Count];

        for (int i = 0; i < transforms.Count; i++)
        {
            targets[i].target = transforms[i];
            targets[i].weight = 1f;
            targets[i].radius = 0f;
        }

        targetGroup.m_Targets = targets;
    }

    private void ClearGrids()
    {
        foreach (var grid in _grids)
        {
            Destroy(grid.gameObject);
        }
    }
    
}
