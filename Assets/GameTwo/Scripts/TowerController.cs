using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerController : MonoBehaviour
{
    [SerializeField] private List<Transform> SpawnPositions = new List<Transform>();
    private Cube currentCube;
    private Cube previousCube;
    private int cubeCount = 0;
    public bool IsAlive = true;
    private void Start()
    {
        
        TowerLifeCycle();
    }

    private void TowerLifeCycle()
    {
        StartCoroutine(LifeCycleCR());

        IEnumerator LifeCycleCR()
        {
            yield return new WaitForSeconds(3);
            while (IsAlive)
            {
                yield return new WaitUntil(() => currentCube == null);
                var spawnPoint = SpawnPositions[cubeCount % 2];

                currentCube = Instantiate(Resources.Load<Cube>("Cube"), spawnPoint.position, quaternion.identity,
                    spawnPoint);
                
                var scale = previousCube? previousCube.transform.localScale : new Vector3(4,.5f,4);
                currentCube.Init(scale, Random.ColorHSV(), true);
                cubeCount++;
            }
        }
    }

    public void PlaceCube()
    {
        if (!IsAlive || currentCube == null) return;
        currentCube.StopMovement();
    }
}