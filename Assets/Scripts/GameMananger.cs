using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static CustomPropertiesConstant;
using UnityEngine.UI;

public class GameMananger : MonoBehaviourPunCallbacks
{
    public PUNPlayerController clientPlayerController;
    public PUNPlayerController otherClientPlayerController;


    public static GameMananger instance;

    public SpawnPlayer spawnManager;

    //[SerializeField] GameObject chooseSkinButtonContainer;
    //[SerializeField] TMP_InputField playerNameInputField;
    [Header("Player Enter Notification")] [SerializeField]
    GameObject playerEnterNotificationContainer;

    [SerializeField] GameObject playerEnterTextPrefab;
    [Header("Fancy UI Stuffs")] public List<SkillCoolDownUIController> skillCoolDownUIControllers;
    [SerializeField] GameEndUI _endGameScreen;

    public PhotonView view;

    //------------------------- END GAME CONDITION -------------------------
    public float endGameTimer;
    public TextMeshProUGUI endTimerText;
    public Transform endPoint;
    public Transform startPoint;
    public Slider otherPlayerDistanceSlider;
    public float totalDistance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        view = GetComponent<PhotonView>();
        if (endPoint && startPoint)
        {
            otherPlayerDistanceSlider.value = 0;
            totalDistance = Vector2.Distance(endPoint.position, startPoint.position);
        }
    }
    /*public void ChooseSkin(int index)
    {
        PUNPlayerController PUNplayer = spawnManager.Spawn().GetComponent<PUNPlayerController>();

        */ /*string playerName = GetCurrentPlayerName();
        int playerSkin = GetCurrentPlayerSkin();*/ /*

        string playerName = PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_NAME].ToString();
        int playerSkin = (int)PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_SKIN];

        */ /*if (playerName == "" || playerSkin == -1)
        {
            Debug.LogError("Can't get player name or Skin!");
            return;
        }*/ /*

        PUNplayer.SetData(playerName, playerSkin);

        if (PUNplayer.view.IsMine)
        {
            PUNplayer.player = PhotonNetwork.LocalPlayer;
            PUNplayer.player.NickName = playerName;
        }

        */ /*PUNplayer.SetData(playerNameInputField.text, index);

        if (PUNplayer.view.IsMine)
        {
            PUNplayer.player = PhotonNetwork.LocalPlayer;
            PUNplayer.player.NickName = playerNameInputField.text;
        }
        // player.GetComponent<PhotonView>().RPC("SyncSkin", RpcTarget.All);

        chooseSkinButtonContainer?.SetActive(false);
        playerNameInputField?.transform.parent.gameObject.SetActive(false);*/ /*

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
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && endGameTimer > 0)
        {
            endGameTimer -= Time.deltaTime;
            endTimerText.text = endGameTimer.ToString("0.");
            view.RPC(nameof(SyncTimer), RpcTarget.Others, endGameTimer);
        }

        if (endGameTimer <= 0)
        {
            view.RPC(nameof(EndGamev2), RpcTarget.All);
            // Time.timeScale = 0f;
        }

        if (otherClientPlayerController)
        {
            float ratio = Vector2.Distance(otherClientPlayerController.transform.position, startPoint.position) /
                          totalDistance;

            otherPlayerDistanceSlider.value = ratio;
        }
    }

    public void ShowPlayerEnterNotification(string name)
    {
        GameObject notificationText =
            PhotonNetwork.Instantiate(playerEnterTextPrefab.name, Vector3.zero, Quaternion.identity);
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

    [PunRPC]
    public void EndGamev2()
    {
        Debug.Log("game ended");
        // Vector2.Distance
        if (PhotonNetwork.IsMasterClient)
        {
            if (Vector2.Distance(clientPlayerController.transform.position, endPoint.position) <
                Vector2.Distance(otherClientPlayerController.transform.position, endPoint.position))
            {
                clientPlayerController.view.RPC("WinGame", RpcTarget.All,clientPlayerController.view.Owner.NickName);
            }
            else
            {
                otherClientPlayerController.view.RPC("WinGame", RpcTarget.All, otherClientPlayerController.view.Owner.NickName);
            } 
        }
            
    }

    [PunRPC]
    public void SyncTimer(float time)
    {
        endGameTimer = time;
        endTimerText.text = endGameTimer.ToString("0.0");
    }
}