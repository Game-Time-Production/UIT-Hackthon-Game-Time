using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    private void Start()
    {
        // Spawn();
    }
    public GameObject Spawn()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 3f));
        return PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity);
    }
}
