using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
public class Trampoline : MonoBehaviour
{
    [SerializeField] float _trampolinePushVelocity;
    [SerializeField] float _trampoLinePushTime;
    [SerializeField] Transform _startPosition;
    [SerializeField] Transform _directionPosition;
    Vector2 _pushDirection;
    private void Awake()
    {
        _pushDirection = _startPosition.up;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PUNPlayerController playerController = other.gameObject.GetComponent<PUNPlayerController>();
            StartCoroutine(Push(playerController));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PUNPlayerController playerController = other.gameObject.GetComponent<PUNPlayerController>();
            playerController.transform.position = _startPosition.position;
            StartCoroutine(Push(playerController));
        }
    }
    IEnumerator Push(PUNPlayerController playerController)
    {

        float elapsedTime = 0f;
        Action PostCoroutineAction = null;
        if (transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            playerController.rb.gravityScale = 0f;
            playerController.forcedMovement = true;
            PostCoroutineAction = () =>
            {
                playerController.rb.gravityScale = playerController.baseGravityScale;
                playerController.forcedMovement = false;
            };
        }

        // playerController.transform.position = _startPosition.position;
        while (elapsedTime < _trampoLinePushTime)
        {
            playerController.rb.velocity = (_directionPosition.position - _startPosition.position) * _trampolinePushVelocity;
            // Debug.Log((_directionPosition.position - _startPosition.position));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        PostCoroutineAction?.Invoke();
        playerController.rb.velocity = Vector2.zero;
    }
}
