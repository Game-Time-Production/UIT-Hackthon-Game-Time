using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SlowBallPowerUp : ProjectilePowerUp
{
    public override void PerformAction()
    {
        Vector2 throwDirection = (_startPosition.position - _basePosition.position).normalized;
        GameObject slowBall = PhotonNetwork.Instantiate(projectilePrefab.name, _startPosition.position, Quaternion.identity);
        slowBall.GetComponent<Rigidbody2D>().AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }
}