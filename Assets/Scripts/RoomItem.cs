using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    LobbyManager lobbyManager;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string name)
    {
        roomName.text = name;
    }

    public void OnClickItem()
    {
        lobbyManager.JoinRoom(roomName.text);
    }
}
