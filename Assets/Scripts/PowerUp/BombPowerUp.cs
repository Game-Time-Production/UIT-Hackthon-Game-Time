using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BombPowerUp : ProjectilePowerUp, IProjectilePowerUp
{
    public override void PerformAction()
    {
        Vector2 throwDirection = (_startPosition.position - _basePosition.position).normalized;
        // Debug.Log(trueDirection);
        GameObject bomb = PhotonNetwork.Instantiate(projectilePrefab.name, _startPosition.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }

}
