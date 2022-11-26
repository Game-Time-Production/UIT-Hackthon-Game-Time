using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PushBack : MonoBehaviour
{
    // [SerializeField] PlayerController parentPlayer;
    [SerializeReference] float _knockBackForce;
    [SerializeField] bool pushOnCollision;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PUNPlayerController>() && pushOnCollision)
        {
            PUNPlayerController player = other.gameObject.GetComponent<PUNPlayerController>();
            if (!player.view.IsMine)
            {
                Vector3 KnockBackDirection = (transform.position - transform.parent.position).normalized;
                KnockBack(player, KnockBackDirection);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PUNPlayerController>() && pushOnCollision)
        {
            PUNPlayerController player = other.gameObject.GetComponent<PUNPlayerController>();
            if (!player.view.IsMine)
            {
                Vector3 KnockBackDirection = (transform.position - transform.parent.position).normalized;
                KnockBack(player, KnockBackDirection);
            }
        }
        // if (other.GetComponent<BreakableBlock>())
        // {
        //     other.GetComponent<BreakableBlock>().Break();
        // }
    }
    public void KnockBack(PUNPlayerController player, Vector3 KnockBackDirection)
    {

        player.view.RPC("PushBack", Photon.Pun.RpcTarget.All, KnockBackDirection * _knockBackForce);
    }
}
