using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BridgeController : MonoBehaviour
{
    [SerializeField] private UıController uiController;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private float perfectTolerance = 0.1f;
    public int GoalAmount = 10;
    public Cube previousCube;
    public bool IsAlive = true;

    private Cube currentCube;
    private int cubeCount = 0;
    private Coroutine passCheckCR;

    private void Start()
    {
        SetFinishLine();
        TowerLifeCycle();
    }

    private void SetFinishLine()
    {
        finishLine.transform.position = new Vector3(0, 0, GoalAmount * 4f);
    }

    private void TowerLifeCycle()
    {
        StartCoroutine(LifeCycleCR());

        IEnumerator LifeCycleCR()
        {
            yield return new WaitForSeconds(1.5f);
            while (IsAlive)
            {
                yield return new WaitUntil(() => currentCube == null);

                var spawnPoint = previousCube.transform.position +
                                 Vector3.forward * previousCube.transform.localScale.z +
                                 (cubeCount % 2 == 1 ? Vector3.right * 5f : Vector3.left * 5f);
                currentCube = Instantiate(Resources.Load<Cube>("Cube"), spawnPoint, quaternion.identity);

                var scale = previousCube ? previousCube.transform.localScale : new Vector3(4, 0.5f, 4);
                currentCube.Init(scale, Random.ColorHSV(), true);
                cubeCount++;
                PassCheck();

                if (cubeCount >= GoalAmount) yield break;
            }
        }
    }

    private void PassCheck()
    {
        passCheckCR = StartCoroutine(PassCheckCR());

        IEnumerator PassCheckCR()
        {
            yield return new WaitUntil(() =>
                (Mathf.Abs(currentCube.transform.position.x - previousCube.transform.position.x) <
                 previousCube.transform.localScale.x ));
            while (true)
            {
                yield return null;
                if (Mathf.Abs(currentCube.transform.position.x - previousCube.transform.position.x) >
                    previousCube.transform.localScale.x)
                {
                    GameOver();
                }
            }
        }
    }

    public void PlaceCube()
    {
        if (!IsAlive || currentCube == null) return;

        currentCube.StopMovement();
        if (passCheckCR != null)
        {
            StopCoroutine(passCheckCR);
        }

        if (previousCube != null)
        {
            Vector3 newScale = previousCube.transform.localScale;

            Debug.Log((currentCube.transform.position.x - previousCube.transform.position.x));
            if (Mathf.Abs(currentCube.transform.position.x - previousCube.transform.position.x) > perfectTolerance)
            {
                newScale = CalculateCut(currentCube, previousCube);

                if (newScale == Vector3.zero)
                {
                    GameOver();

                    return;
                }

                currentCube.transform.localScale = newScale;
                SpawnFallingOverhang(currentCube, previousCube);
            }
            else
            {
                Debug.Log("Perfect fit");
                var newPosition = currentCube.transform.position;
                newPosition.x = previousCube.transform.position.x;
                currentCube.transform.position = newPosition;
            }
        }

        previousCube = currentCube;
        currentCube = null;
        uiController.SetScore(cubeCount, GoalAmount);
    }


    private Vector3 CalculateCut(Cube current, Cube previous)
    {
        Vector3 newScale = current.transform.localScale;
        Vector3 newPosition = current.transform.position;

        float delta = current.transform.position.x - previous.transform.position.x;

        if (Mathf.Abs(delta) <= perfectTolerance)
        {
            current.transform.position = new Vector3(previous.transform.position.x, current.transform.position.y,
                current.transform.position.z);

            return current.transform.localScale;
        }

        float maxSize = previous.transform.localScale.x;
        float newSize = Mathf.Max(0f, maxSize - Mathf.Abs(delta));

        if (newSize <= 0f)
        {
            Debug.Log("Game Over! Missed the stack completely.");
            return Vector3.zero;
        }

        newScale.x = newSize;
        newPosition.x = previous.transform.position.x + delta / 2f;

        newScale.z = 4f;
        newScale.y = 0.5f;

        current.transform.position = newPosition;
        return newScale;
    }


    private void SpawnFallingOverhang(Cube current, Cube previous)
    {
        Vector3 cutPosition = current.transform.position;
        Vector3 cutScale = current.transform.localScale;

        float
            delta = current.transform.position.x - previous.transform.position.x;
        float overhangSize = previous.transform.localScale.x - cutScale.x;


        Vector3 overhangPosition;
        if (delta > 0)
        {
            overhangPosition = cutPosition + Vector3.right * (cutScale.x / 2f + overhangSize / 2f);
        }
        else
        {
            overhangPosition = cutPosition - Vector3.right * (cutScale.x / 2f + overhangSize / 2f);
        }

        Vector3 overhangScale = new Vector3(overhangSize, cutScale.y, cutScale.z);

        Cube overhangCube = Instantiate(Resources.Load<Cube>("Cube"), overhangPosition, Quaternion.identity);

        Color darkColor = current.GetComponent<Renderer>().material.color * 0.7f;
        overhangCube.Init(overhangScale, darkColor, false);

        Rigidbody rb = overhangCube.gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.5f;

        Destroy(overhangCube.gameObject, 3f);
    }

    public void GameWin()
    {
        IsAlive = false;
        uiController.GameWin();
    }

    private void GameOver()
    {
        if (!IsAlive)return;
        IsAlive = false;
        uiController.GameOver();
        currentCube.GameOver();
    }
}