using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using static CustomPropertiesConstant;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomInputField;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject roomPanel;
    [SerializeField] TMP_Text roomName;

    [SerializeField] RoomItem roomItemPrefab;
    List<RoomItem> roomItemList = new List<RoomItem>();
    [SerializeField] Transform contentObject;

    private float timeBetweenUpdates = 0.3f;
    float nextTimeUpdate;

    [SerializeField] PlayerItem playerItemPrefab;
    List<PlayerItem> playerItemList = new List<PlayerItem>();
    [SerializeField] Transform playerItemParent;

    [SerializeField] Button playButton;

    private void Start()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        // Co the dem vao trong UpdateRoomList de giam so lan call
        if (PhotonNetwork.IsMasterClient)
        {
            playButton.gameObject.SetActive(true);
            if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            {
                playButton.interactable = false;
            }
            else
            {
                playButton.interactable = true;
            }
#if UNITY_EDITOR
            playButton.interactable = true;
#endif
        }
        else
        {
            playButton.gameObject.SetActive(false);
        }
    }

    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextTimeUpdate)
        {
            UpdateRoomList(roomList);
            nextTimeUpdate = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    private void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChange();
            }

            playerItemList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("CutScene");
    }
}
