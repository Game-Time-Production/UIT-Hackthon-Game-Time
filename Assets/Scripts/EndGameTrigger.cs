using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PUNPlayerController playerController = other.GetComponent<PUNPlayerController>();
        if (playerController)
        {
            playerController.view.RPC("WinGame", RpcTarget.All, playerController.view.Owner.NickName);
        }
    }
}
