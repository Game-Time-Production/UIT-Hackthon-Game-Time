using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static CustomPropertiesConstant;

public class GameMananger : MonoBehaviourPunCallbacks
{
    public PUNPlayerController clientPlayerController;
    public List<UnityEngine.U2D.Animation.SpriteLibraryAsset> CharacterSpriteLibraryAssets;
    public static GameMananger instance;
    public SpawnPlayer spawnManager;
    //[SerializeField] GameObject chooseSkinButtonContainer;
    //[SerializeField] TMP_InputField playerNameInputField;
    [Header("Player Enter Notification")]
    [SerializeField] GameObject playerEnterNotificationContainer;
    [SerializeField] GameObject playerEnterTextPrefab;
    [Header("Fancy UI Stuffs")]
    public List<SkillCoolDownUIController> skillCoolDownUIControllers;
    [SerializeField] GameEndUI _endGameScreen;
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
    /*public void ChooseSkin(int index)
    {
        PUNPlayerController PUNplayer = spawnManager.Spawn().GetComponent<PUNPlayerController>();

        *//*string playerName = GetCurrentPlayerName();
        int playerSkin = GetCurrentPlayerSkin();*//*

        string playerName = PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_NAME].ToString();
        int playerSkin = (int)PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_SKIN];

        *//*if (playerName == "" || playerSkin == -1)
        {
            Debug.LogError("Can't get player name or Skin!");
            return;
        }*//*

        PUNplayer.SetData(playerName, playerSkin);

        if (PUNplayer.view.IsMine)
        {
            PUNplayer.player = PhotonNetwork.LocalPlayer;
            PUNplayer.player.NickName = playerName;
        }

        *//*PUNplayer.SetData(playerNameInputField.text, index);

        if (PUNplayer.view.IsMine)
        {
            PUNplayer.player = PhotonNetwork.LocalPlayer;
            PUNplayer.player.NickName = playerNameInputField.text;
        }
        // player.GetComponent<PhotonView>().RPC("SyncSkin", RpcTarget.All);

        chooseSkinButtonContainer?.SetActive(false);
        playerNameInputField?.transform.parent.gameObject.SetActive(false);*//*

        foreach (var item in skillCoolDownUIControllers)
        {
            item.gameObject.SetActive(true);
        }
    }*/

    /*private string GetCurrentPlayerName()
    {
        string name = "";

        if (PlayerPrefs.HasKey("playerName"))
        {
            name = PlayerPrefs.GetString("playerName");
        }

        return name;
    }

    private int GetCurrentPlayerSkin()
    {
        int skin = -1;

        if (PlayerPrefs.HasKey("playerSkin"))
        {
            skin = PlayerPrefs.GetInt("playerSkin");
        }

        return skin;
    }*/

    public void ShowPlayerEnterNotification(string name)
    {
        GameObject notificationText = PhotonNetwork.Instantiate(playerEnterTextPrefab.name, Vector3.zero, Quaternion.identity);
        notificationText.transform.SetParent(playerEnterNotificationContainer.transform);
        notificationText.GetComponentInChildren<TextMeshProUGUI>().text = $"{name} has entered the game";
    }
    public void SetUpSkillUICooldown()
    {
        foreach (var item in skillCoolDownUIControllers)
        {
            switch (item.skillType)
            {
                case SkillType.PUSH:
                    item.SetUp(clientPlayerController.pushCoolDownTime);
                    clientPlayerController.OnPushSkillUse = item.OnSkillUse;
                    break;
                case SkillType.SHIELD:
                    item.SetUp(clientPlayerController.shieldCoolDownTime + clientPlayerController._shieldDuration);
                    clientPlayerController.OnShieldSkillUse = item.OnSkillUse;
                    break;
                default:
                    break;

            }
        }
    }
    public void EndGame(string playerName)
    {
        _endGameScreen.gameObject.SetActive(true);
        _endGameScreen.playerWinText.text = $"PLAYER {playerName} WIN!";
        clientPlayerController.enabled = false;
        Debug.Log("auto sync scene: " + PhotonNetwork.AutomaticallySyncScene);
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _endGameScreen.restartButton.gameObject.SetActive(false);
        }
    }
    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void RestartGame()
    {
        PhotonNetwork.LoadLevel("Reloading Game");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
