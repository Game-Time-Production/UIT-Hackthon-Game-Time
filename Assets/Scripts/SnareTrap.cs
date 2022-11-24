using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SnareTrap : MonoBehaviour
{
    [SerializeField] float _lockTime;
    [SerializeField] float _lockCoolDown;
    [SerializeField] bool _locked;
    [SerializeField] Transform _lockPosition;
    Animator _animator;
    PhotonView _view;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _view = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !_locked)
        {

            _locked = true;
            _animator.SetTrigger("Shut");
            if (_view.IsMine)
            {
                _view.RPC(nameof(SyncLock), RpcTarget.All, _locked);
            }
            StartCoroutine(LockMovement(other.GetComponent<PUNPlayerController>()));
        }
    }

    [PunRPC]
    void SyncLock(bool value)
    {
        _locked = value;
    }
    public IEnumerator LockMovement(PUNPlayerController playerController)
    {
        _locked = true;
        playerController.view.RPC(
            nameof(playerController.LockMovement),
            RpcTarget.All,
            true,
            _lockPosition.position);
        // playerController.lockMovement = true;
        // playerController.rb.velocity = Vector2.zero;
        // playerController.transform.position = _lockPosition.position;
        float elapsedTime = 0f;
        while (elapsedTime < _lockTime)
        {
            elapsedTime += Time.deltaTime;
            if (playerController.transform.position != _lockPosition.position)
            {
                playerController.transform.position = _lockPosition.position;
                playerController.view.RPC(
                    nameof(playerController.LockMovement),
                    RpcTarget.All,
                    true,
                    _lockPosition.position);
            }
            yield return new WaitForEndOfFrame();
        }
        playerController.lockMovement = false;
        playerController.view.RPC(
           nameof(playerController.LockMovement),
           RpcTarget.All,
           false,
           _lockPosition.position);
        // snare trap reset cooldown
        yield return new WaitForSeconds(_lockCoolDown);
        _locked = false;
        _animator.SetTrigger("Open");
        if (_view.IsMine)
        {
            _view.RPC(nameof(SyncLock), RpcTarget.All, _locked);
        }
    }
}
