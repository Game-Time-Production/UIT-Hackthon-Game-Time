using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public interface IPowerUp
{
    public void PerformAction();
    public void SetPlayerRefernce(PUNPlayerController playerController);
}

public interface IProjectilePowerUp : IPowerUp
{
    public void SetUpStartPosition(Transform startPos, Transform basePos);
}

public enum PowerUpType
{
    BOMB,
    SLOW_PROJECTILE,
    SPEEDBOOST,
    DASH,
}

public class PowerUp : MonoBehaviour
{
    [SerializeField] List<Sprite> powerUpSprites;
    [SerializeField] PowerUpType powerUpType;
    [SerializeField] Collider2D collider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float powerUpCoolDownTime = 5f;
    PhotonView view;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        SetPowerUpSprite();
        if (view.Owner.IsMasterClient && view.IsMine)
        {
            RandomizePowerUp();
            view.RPC("SyncPowerUpType", RpcTarget.All, (int)powerUpType);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPowerUpController playerPowerUpController = other.GetComponent<PlayerPowerUpController>();
        if (playerPowerUpController)
        {
            int powerUpSlotIndex = playerPowerUpController.GetEmptyPowerUpSlot();
            // Debug.Log(playerPowerupSlot == null);
            if (powerUpSlotIndex != -1)
            {
                playerPowerUpController.powerUp[powerUpSlotIndex] = PowerUpManager.instance.GetPowerUp(powerUpType);
                if (playerPowerUpController.view.IsMine)
                    PowerUpSlotUI.instance.SetSlotIcon(powerUpSlotIndex, powerUpSprites[(int)powerUpType]);
                view.RPC("StartCoolDown", RpcTarget.MasterClient);
            }
            // playerPowerUpController.powerUp[0] = PowerUpManager.instance.GetPowerUp(powerUpType);
            // StartCoroutine(CoolDown(powerUpCoolDownTime));
        }
    }

    private void SetUpProjectilePowerUp(Transform projectileStartPos, Transform basePos)
    {
        GetComponent<IProjectilePowerUp>().SetUpStartPosition(projectileStartPos, basePos);
    }

    [PunRPC]
    public void StartCoolDown()
    {
        StartCoroutine(CoolDown(powerUpCoolDownTime));
    }

    public IEnumerator CoolDown(float duration)
    {
        view.RPC("DisablePowerUp", RpcTarget.All);
        yield return new WaitForSeconds(duration);
        if (view.Owner.IsMasterClient && view.IsMine)
        {
            RandomizePowerUp();
            view.RPC("SyncPowerUpType", RpcTarget.All, (int)powerUpType);
        }

        view.RPC("SetUpPowerUp", RpcTarget.All);
    }

    [PunRPC]
    public void SetUpPowerUp()
    {
        collider.enabled = true;
        spriteRenderer.enabled = true;
        SetPowerUpSprite();
    }

    [PunRPC]
    public void DisablePowerUp()
    {
        collider.enabled = false;
        spriteRenderer.enabled = false;
    }

    [PunRPC]
    public void SetPowerUpSprite()
    {
        // spriteRenderer.sprite = powerUpSprites[(int)powerUpType];
    }

    [PunRPC]
    public void RandomizePowerUp()
    {
        powerUpType = (PowerUpType)UnityEngine.Random.Range(0, 4);
    }

    [PunRPC]
    public void SyncPowerUpType(int type)
    {
        powerUpType = (PowerUpType)type;
    }
}