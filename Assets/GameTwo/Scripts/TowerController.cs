using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerController : MonoBehaviour
{
    [SerializeField] private List<Transform> SpawnPositions = new List<Transform>();
    [SerializeField] private GameObject tower;

    [SerializeField] private UıController uiController;

    private Cube currentCube;
    [SerializeField] private Cube previousCube;
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

                SetSpawnPositions();
                var spawnPoint = SpawnPositions[cubeCount % 2];
                currentCube = Instantiate(Resources.Load<Cube>("Cube"), spawnPoint.position, quaternion.identity,
                    spawnPoint);

                var scale = previousCube ? previousCube.transform.localScale : new Vector3(4, 0.5f, 4);
                currentCube.Init(scale, Random.ColorHSV(), true);
                cubeCount++;
            }
        }
    }

    private void SetSpawnPositions()
    {
        SpawnPositions[0].position = new Vector3(SpawnPositions[0].position.x, 0, previousCube.transform.position.z);
        SpawnPositions[1].position = new Vector3(previousCube.transform.position.x, 0, SpawnPositions[1].position.z);
    }

    public void PlaceCube()
    {
        if (!IsAlive || currentCube == null) return;

        currentCube.StopMovement();


        if (previousCube != null)
        {
            Vector3 newScale = CalculateCut(currentCube, previousCube);
            if (newScale == Vector3.zero)
            {
                GameOver();
                
                return;
            }

            currentCube.SetNewScale(newScale);
            SpawnFallingOverhang(currentCube, previousCube, (cubeCount % 2 == 1));
        }

        currentCube.transform.SetParent(tower.transform);
        previousCube = currentCube;
        currentCube = null;
        tower.transform.localPosition += Vector3.down * 0.5f;
        uiController.SetScore(cubeCount);
    }

    private Vector3 CalculateCut(Cube current, Cube previous)
    {
        Vector3 newScale = current.transform.localScale;
        Vector3 newPosition = current.transform.position;

        bool moveAlongX = (cubeCount % 2 == 1);

        if (moveAlongX)
        {
            float delta = current.transform.position.x - previous.transform.position.x;
            float maxSize = previous.transform.localScale.x;
            float newSize = maxSize - Mathf.Abs(delta);

            if (newSize <= 0f) return Vector3.zero;

            newScale.x = newSize;
            newPosition.x = previous.transform.position.x + delta / 2f;
        }
        else
        {
            float delta = current.transform.position.z - previous.transform.position.z;
            float maxSize = previous.transform.localScale.z;
            float newSize = maxSize - Mathf.Abs(delta);

            if (newSize <= 0f) return Vector3.zero;

            newScale.z = newSize;
            newPosition.z = previous.transform.position.z + delta / 2f;
        }

        newScale.y = 0.5f;
        newPosition.y = current.transform.position.y;

        current.transform.position = newPosition;
        return newScale;
    }


    private void SpawnFallingOverhang(Cube current, Cube previous, bool moveAlongX)
    {
        Vector3 cutPosition = current.transform.position;
        Vector3 cutScale = current.transform.localScale;

        float delta;
        float overhangSize;
        Vector3 overhangPosition = Vector3.zero;
        Vector3 overhangScale = Vector3.zero;

        if (moveAlongX)
        {
            delta = current.transform.position.x - previous.transform.position.x;
            overhangSize = previous.transform.localScale.x - cutScale.x;

            if (delta > 0)
                overhangPosition = cutPosition + Vector3.right * (cutScale.x / 2f + overhangSize / 2f);
            else
                overhangPosition = cutPosition - Vector3.right * (cutScale.x / 2f + overhangSize / 2f);

            overhangScale = new Vector3(overhangSize, cutScale.y, cutScale.z);
        }
        else
        {
            delta = current.transform.position.z - previous.transform.position.z;
            overhangSize = previous.transform.localScale.z - cutScale.z;

            if (delta > 0)
                overhangPosition = cutPosition + Vector3.forward * (cutScale.z / 2f + overhangSize / 2f);
            else
                overhangPosition = cutPosition - Vector3.forward * (cutScale.z / 2f + overhangSize / 2f);

            overhangScale = new Vector3(cutScale.x, cutScale.y, overhangSize);
        }

        Cube overhangCube = Instantiate(Resources.Load<Cube>("Cube"), overhangPosition, Quaternion.identity);

        Color darkColor = current.GetComponent<Renderer>().material.color * 0.7f;
        overhangCube.Init(overhangScale, darkColor, false);

        Rigidbody rb = overhangCube.gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.5f;

        Destroy(overhangCube.gameObject, 3f);
    }

    private void GameOver()
    {
        IsAlive = false;
        uiController.GameOver();
        currentCube.GameOver();
    }
}