using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

    }
    public IPowerUp GetPowerUp(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.BOMB:
                return GetComponent<BombPowerUp>();
            case PowerUpType.SLOW_PROJECTILE:
                return GetComponent<SlowBallPowerUp>();
            case PowerUpType.SPEEDBOOST:
                return GetComponent<SpeedBoostPowerUp>();
            case PowerUpType.DASH:
                return GetComponent<DashPowerUp>();
            default:
                return null;
        }
    }
    public void SetUpProjectilePowerUp(Transform projectileStartPos, Transform basePos)
    {
        ProjectilePowerUp[] powerUps = GetComponents<ProjectilePowerUp>();
        foreach (var powerUp in powerUps)
        {
            powerUp.SetUpStartPosition(projectileStartPos, basePos);
        }
        // GetComponent<BombPowerUp>().SetUpStartPosition(projectileStartPos, basePos);
    }
    public void SetUpPowerUp(PlayerPowerUpController powerUpController)
    {
        SetUpProjectilePowerUp(powerUpController.ProjectilStartPos, powerUpController.transform);
        GetComponent<SpeedBoostPowerUp>().SetPlayerRefernce(powerUpController.GetComponent<PUNPlayerController>());
        GetComponent<DashPowerUp>().SetPlayerRefernce(powerUpController.GetComponent<PUNPlayerController>());
    }
}
