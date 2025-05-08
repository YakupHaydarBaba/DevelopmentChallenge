using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private float speed;
    private bool _isMoving;
    private Coroutine MoveCR;
    

    public void Init(Vector3 cubeScale, Color color, bool move = false)
    {
        _renderer.material.color = color;
        transform.localScale = cubeScale;
        if (move) StartMovement();
    }

    private void StartMovement()
    {
        MoveCR =StartCoroutine(MovementCR());

        IEnumerator MovementCR()
        {
            var direction = new Vector3(-transform.position.x, 0, 0).normalized;
            yield return new WaitForSeconds(.5f);
            _isMoving = true;
            while (_isMoving)
            {
                yield return null;
                transform.localPosition +=  direction * speed * Time.deltaTime;
               
            }
        }
    }

    public void StopMovement() => StopCoroutine(MoveCR);

 
    public void GameOver()
    {
        _isMoving = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.5f;
    }
}