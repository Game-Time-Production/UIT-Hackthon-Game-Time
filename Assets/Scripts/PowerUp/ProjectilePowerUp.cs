using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePowerUp : MonoBehaviour, IProjectilePowerUp
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float throwForce = 10f;
    [SerializeField] protected Transform _startPosition;
    [SerializeField] protected Transform _basePosition;
    public void SetUpStartPosition(Transform startPos, Transform basePos) // change this to SetPlayerReference
    {
        _startPosition = startPos;
        _basePosition = basePos;
    }
    public virtual void PerformAction()
    {

    }

    public void SetPlayerRefernce(PUNPlayerController playerController)
    {
        SetUpStartPosition(playerController.GetComponent<PlayerPowerUpController>().ProjectilStartPos, playerController.transform);
    }
}
