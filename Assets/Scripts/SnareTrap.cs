using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnareTrap : MonoBehaviour
{
    [SerializeField] float _lockTime;
    [SerializeField] Transform _lockPosition;
    Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("Shut");
            StartCoroutine(LockMovement(other.GetComponent<PUNPlayerController>()));

        }
    }
    public IEnumerator LockMovement(PUNPlayerController playerController)
    {
        playerController.lockMovement = true;
        playerController.rb.velocity = Vector2.zero;
        playerController.transform.position = _lockPosition.position;
        float elapsedTime = 0f;
        while (elapsedTime < _lockTime)
        {
            elapsedTime += Time.deltaTime;
            if (!playerController.lockMovement || playerController.transform.position != _lockPosition.position)
            {
                playerController.lockMovement = true;
                playerController.transform.position = _lockPosition.position;
            }
            yield return new WaitForEndOfFrame();
        }
        _animator.SetTrigger("Open");
        playerController.lockMovement = false;
    }
}
