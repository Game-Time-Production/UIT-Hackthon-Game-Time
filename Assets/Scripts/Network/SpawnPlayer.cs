using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static CustomPropertiesConstant;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject[] playerPrefab;

    [SerializeField] Transform masterHostPosSpawn;
    [SerializeField] Transform otherPosSpawn;
    private void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        string playerName = PhotonNetwork.LocalPlayer.NickName;
        int playerSkin = (int)PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_SKIN];

        Vector2 spawnPos = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 3f));
        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerSkin].name, spawnPos, Quaternion.identity);
        PUNPlayerController PUNplayer = player.GetComponent<PUNPlayerController>();

        Debug.Log("Player name: " + playerName + ", skin: " + playerSkin);

        //PUNplayer.SetData(playerName, playerSkin);

        PUNplayer.SetData(playerName);

        if (PUNplayer.view.IsMine)
        {
            PUNplayer.player = PhotonNetwork.LocalPlayer;
            PUNplayer.player.NickName = playerName;
        }
        
        foreach (var item in GameMananger.instance.skillCoolDownUIControllers)
        {
            item.gameObject.SetActive(true);
        }
    }
}
