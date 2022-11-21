using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DashPowerUp : MonoBehaviour, IPowerUp
{
    PUNPlayerController playerController;
    [SerializeField] float dashDuration = .15f;
    [SerializeField] float dashVelocity = 50f;
    public void PerformAction()
    {
        if (!playerController.lockMovement)
            StartCoroutine(Dash());
    }

    public void SetPlayerRefernce(PUNPlayerController playerController)
    {
        this.playerController = playerController;
    }
    IEnumerator Dash()
    {
        float elapsedTime = 0f;
        playerController.view.RPC("ToggleDashGhostEffect", RpcTarget.All, true);
        playerController.forcedMovement = true;
        playerController.rb.gravityScale = 0f;
        while (elapsedTime < dashDuration)
        {
            playerController.rb.velocity = playerController.transform.right * dashVelocity;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerController.rb.velocity = Vector2.zero;
        playerController.rb.gravityScale = playerController.baseGravityScale;
        playerController.forcedMovement = false;
        playerController.view.RPC("ToggleDashGhostEffect", RpcTarget.All, false);


    }
}
