using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGround : MonoBehaviour
{
    [SerializeField] float slowPercent;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PUNPlayerController playerController = other.GetComponent<PUNPlayerController>();
            playerController.speed = playerController.baseSpeed * slowPercent;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PUNPlayerController playerController = other.GetComponent<PUNPlayerController>();
            playerController.speed = playerController.baseSpeed;
        }
    }
}
