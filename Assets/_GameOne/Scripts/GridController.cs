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
                var grid = Instantiate(Resources.Load<Grid>("Grid"), new Vector3(i,  j,0), Quaternion.identity,_gridsParent);
                grid.Init(new Vector2Int(i,j));
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
    
    public void FindMarkedSequences()
    {
        HashSet<Grid> result = new HashSet<Grid>();

        
        var gridDict = _grids.ToDictionary(g => g.gridPosition);

        
        Vector2Int[] directions =
        {
            new Vector2Int(1, 0), 
            new Vector2Int(0, 1), 
            new Vector2Int(1, 1), 
            new Vector2Int(1, -1),
        };

        foreach (var grid in _grids)
        {
            if (!grid.IsMarked) continue;

            foreach (var dir in directions)
            {
                List<Grid> sequence = new List<Grid> { grid };

                Vector2Int nextPos = grid.gridPosition + dir;

                while (gridDict.TryGetValue(nextPos, out var nextGrid) && nextGrid.IsMarked)
                {
                    sequence.Add(nextGrid);
                    nextPos += dir;
                }

                if (sequence.Count >= 3)
                {
                    foreach (var item in sequence)
                        result.Add(item);
                }
            }
        }

        foreach (var grid in result)
        {
            grid.ResetMarked();
        }
    }
    
}
