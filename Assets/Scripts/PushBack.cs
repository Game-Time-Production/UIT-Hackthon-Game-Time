using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
public class PushBack : MonoBehaviour
{
    [SerializeField] PlayerController parentPlayer;
    [SerializeReference] float _knockBackForce;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (!player.entity.IsOwner)
            {
                // player.Rb.velocity = (transform.position - transform.parent.position).normalized * _knockBackForce;
                var evnt = PlayerTakeDamage.Create(player.entity);
                evnt.Message = "id : " + player.entity.GetInstanceID() + " take damage";
                evnt.KnockBackDirection = (transform.position - transform.parent.position).normalized * _knockBackForce;
                evnt.Send();
            }
        }
    }
}
