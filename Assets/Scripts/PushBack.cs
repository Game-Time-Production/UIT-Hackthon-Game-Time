using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PushBack : MonoBehaviour
{
    // [SerializeField] PlayerController parentPlayer;
    [SerializeField] PUNPlayerController parentPlayer;
    [SerializeReference] float _knockBackForce;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PUNPlayerController>())
        {
            PUNPlayerController player = other.gameObject.GetComponent<PUNPlayerController>();
            if (!player.view.IsMine)
            {
                // player.Rb.velocity = (transform.position - transform.parent.position).normalized * _knockBackForce;
                // var evnt = PlayerTakeDamage.Create(player.entity);
                // evnt.Message = "id : " + player.entity.GetInstanceID() + " take damage";
                // evnt.KnockBackDirection = (transform.position - transform.parent.position).normalized * _knockBackForce;
                // evnt.Send();
                Vector3 KnockBackDirection = (transform.position - transform.parent.position).normalized * _knockBackForce;
                player.view.RPC("PushBack", Photon.Pun.RpcTarget.All, KnockBackDirection);

            }
        }
    }
}
