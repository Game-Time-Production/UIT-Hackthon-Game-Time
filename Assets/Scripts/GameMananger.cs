using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static CustomPropertiesConstant;
public class GameMananger : MonoBehaviourPunCallbacks
{
    public List<SpriteLibraryAsset> CharacterSpriteLibraryAssets;
    public static GameMananger instance;
    public SpawnPlayer spawnManager;
    [SerializeField] GameObject chooseSkinButtonContainer;
    [SerializeField] TMP_InputField playerNameInputField;
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
    public void ChooseSkin(int index)
    {
        PUNPlayerController PUNplayer = spawnManager.Spawn().GetComponent<PUNPlayerController>();
        PUNplayer.SetData(playerNameInputField.text, index);
        if (PUNplayer.view.IsMine)
        {
            PUNplayer.player = PhotonNetwork.LocalPlayer;
            PUNplayer.player.NickName = playerNameInputField.text;
        }
        // player.GetComponent<PhotonView>().RPC("SyncSkin", RpcTarget.All);
        chooseSkinButtonContainer?.SetActive(false);
        playerNameInputField?.transform.parent.gameObject.SetActive(false);

    }

}
