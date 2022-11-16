using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SlowPorjectileController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameObject explodeEffect;
    [SerializeField] float slowPercent = .5f;
    [SerializeField] float slowDuration = 2f;
    PhotonView view;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        StartCoroutine(DestroyAfter(10f));

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (view.IsMine)
        {
            PUNPlayerController playerController = other.gameObject.GetComponent<PUNPlayerController>();
            if (playerController)
            {
                if (playerController.view.IsMine)
                    return;
                playerController.view.RPC("SlowDown", RpcTarget.All, slowDuration, slowPercent);
                // view.RPC("SlowProjectileExplode", RpcTarget.All);
            }
            view.RPC("SlowProjectileExplode", RpcTarget.All);
        }


    }

    [PunRPC]
    public void SlowProjectileExplode()
    {
        // Debug.Log("bomb explode");
        Instantiate(explodeEffect, transform.position, Quaternion.identity);
        if (view.IsMine)
        {
            PhotonNetwork.Destroy(view);
        }
    }
    private IEnumerator DestroyAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (view.IsMine)
        {
            view.RPC("SlowProjectileExplode", RpcTarget.All);
        }

    }


}
