using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using static CustomPropertiesConstant;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerName;

    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    [SerializeField] Image playerSkin;
    [SerializeField] Sprite[] skins;

    Player player;

    public void SetPlayerInfo(Player player)
    {
        playerName.text = player.NickName;
        this.player = player;
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChange()
    {
        leftButton.SetActive(true);
        rightButton.SetActive(true);
        //playerProperties[PLAYER_SKIN] = 0;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties[PLAYER_SKIN] == 0)
        {
            playerProperties[PLAYER_SKIN] = skins.Length - 1;
        }
        else
        {
            playerProperties[PLAYER_SKIN] = (int)playerProperties[PLAYER_SKIN] - 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void OnClickRightArrow()
    {
        if ((int)playerProperties[PLAYER_SKIN] == skins.Length - 1)
        {
            playerProperties[PLAYER_SKIN] = 0;
        }
        else
        {
            playerProperties[PLAYER_SKIN] = (int)playerProperties[PLAYER_SKIN] + 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    private void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey(PLAYER_SKIN))
        {
            playerSkin.sprite = skins[(int)player.CustomProperties[PLAYER_SKIN]];
            playerProperties[PLAYER_SKIN] = (int)player.CustomProperties[PLAYER_SKIN];
        }
        else
        {
            playerProperties[PLAYER_SKIN] = 0;
        }
    }
}
