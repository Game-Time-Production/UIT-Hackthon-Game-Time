using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpeedBoostPowerUp : MonoBehaviour, IPowerUp
{
    PUNPlayerController playercontroller;
    [SerializeField] float speedIncreasePercent = 1f; // 1 = 100%
    [SerializeField] float bootstDuration = 3f;
    public void PerformAction()
    {
        playercontroller?.view.RPC("SpeedBoost", RpcTarget.All,bootstDuration,speedIncreasePercent); // seem unnecessary 
    }

    public void SetPlayerRefernce(PUNPlayerController playerController)
    {
        this.playercontroller = playerController;
    }


}
