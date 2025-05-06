using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private float speed;
    private bool _isMoving;

    public void Init(Vector3 cubeScale, Color color, bool move = false)
    {
        _renderer.material.color = color;
        transform.localScale = cubeScale;
        if (move) StartMovement();
    }

    private void StartMovement()
    {
        StartCoroutine(MovementCR());

        IEnumerator MovementCR()
        {
            yield return new WaitForSeconds(.5f);
            _isMoving = true;
            while (_isMoving)
            {
                yield return null;
                transform.localPosition += Vector3.back * speed * Time.deltaTime;
            }
        }
    }
    public void StopMovement() => _isMoving = false;

    public void SetNewScale(Vector3 scale) => transform.localScale = scale;
}