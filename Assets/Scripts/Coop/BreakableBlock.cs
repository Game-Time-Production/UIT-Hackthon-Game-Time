using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BreakableBlock : MonoBehaviour
{
    PhotonView _view;
    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    public void Break()
    {
        PhotonNetwork.Destroy(_view);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player Attack"))
        {
            Break();
        }
    }
}
  