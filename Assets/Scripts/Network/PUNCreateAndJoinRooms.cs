using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class PUNCreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    /*public bool isDevMode;
    [SerializeField] TMP_InputField createRoom;
    [SerializeField] TMP_InputField joinRoom;
    [SerializeField] Button starGameButton;
    [SerializeField] GameObject lobbyRoomUI;
    [SerializeField] GameObject createAndJoinRoomUI;
    private void Awake()
    {
        lobbyRoomUI?.SetActive(false);
    }
    public void CreateRoom()
    {
        if (createRoom.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(createRoom.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
        }
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoom.text);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // if (!isDevMode)
        // {
        //     if (PhotonNetwork.IsMasterClient)
        //     {
        //         starGameButton.interactable = true;
        //     }
        //     else starGameButton.interactable = false;
        // }
        // createAndJoinRoomUI?.SetActive(false);
        // lobbyRoomUI?.SetActive(true);
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }
    public void StartGame()
    {

    }*/

}
