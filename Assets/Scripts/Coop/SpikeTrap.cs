using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpikeTrap : MonoBehaviour
{
    public Transform checkpoint;
    public float knockbackForce;
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     other.transform.position = checkpoint.position;
        // }
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 direction = (checkpoint.position - transform.position).normalized * knockbackForce;
            other.gameObject.GetComponent<PUNPlayerController>().view.RPC("PushBack", RpcTarget.All, direction);
        }
    }
}
