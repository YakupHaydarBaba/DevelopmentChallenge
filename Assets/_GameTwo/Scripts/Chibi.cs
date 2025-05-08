using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Chibi : MonoBehaviour
{
    public BridgeController bridgeController;
    [SerializeField] private float speed;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        PlayerLifeCycle();
    }

    private void PlayerLifeCycle()
    {
        StartCoroutine(PlayerLifeCycleCR());

        IEnumerator PlayerLifeCycleCR()
        {
            yield return new WaitForSeconds(2f);
            while (bridgeController.IsAlive)
            {
                yield return null;
                var newVector = Vector3.forward;
                newVector.x = bridgeController.previousCube.transform.position.x - transform.position.x;
                transform.position += newVector * speed * Time.deltaTime;
            }
        }
    }

    private void WinAnimation()
    {
        _animator.SetTrigger("WinAnimation");
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            bridgeController.GameWin();
            WinAnimation();
        }
    }
}