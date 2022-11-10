using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BombController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameObject explodeEffect;
    [SerializeField] float explodeTime;
    PhotonView view;
    Vector2 inititalVelocity;
    [SerializeField] Collider2D explosionCollider2D;
    List<PUNPlayerController> targetsInRange;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        targetsInRange = new List<PUNPlayerController>();
    }
    private void Start()
    {
        inititalVelocity = rb.velocity;
        StartCoroutine(TickDown(explodeTime));
    }
    private void FixedUpdate()
    {

    }
    public IEnumerator TickDown(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            rb.velocity = new Vector2(inititalVelocity.x * (duration - elapsedTime), rb.velocity.y);
        }
        view.RPC("Explode", RpcTarget.All);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (view.IsMine)
        {
            PUNPlayerController playerController = other.gameObject.GetComponent<PUNPlayerController>();
            if (playerController && !playerController.view.IsMine)
            {
                view.RPC("Explode", RpcTarget.All);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PUNPlayerController player = other.GetComponent<PUNPlayerController>();
        if (player)
            targetsInRange.Add(player);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        PUNPlayerController player = other.GetComponent<PUNPlayerController>();
        if (player)
            targetsInRange.Remove(player);
    }
    [PunRPC]
    public void Explode()
    {
        // Debug.Log("bomb explode");
        Instantiate(explodeEffect, transform.position, Quaternion.identity);
        foreach (var player in targetsInRange)
        {
            GetComponentInChildren<PushBack>().KnockBack(player, (player.transform.position - transform.position).normalized);
        }
        if (view.IsMine)
        {
            PhotonNetwork.Destroy(view);
        }
    }
}
