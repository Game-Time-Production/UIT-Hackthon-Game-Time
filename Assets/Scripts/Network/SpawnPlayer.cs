using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static CustomPropertiesConstant;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    private void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 3f));
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity);
        PUNPlayerController PUNplayer = player.GetComponent<PUNPlayerController>();

        string playerName = PhotonNetwork.LocalPlayer.NickName;
        int playerSkin = (int)PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_SKIN];

        Debug.Log("Player name: " + playerName + ", skin: " + playerSkin);

        PUNplayer.SetData(playerName, playerSkin);

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
