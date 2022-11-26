using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using UnityEngine.Experimental.U2D.Animation; // WHY 
using Photon.Realtime;

[System.Obsolete("This Skin Syncrhonization Script is deprecated\n Note to self: remove this later",true)]
public class SkinSynchronization : MonoBehaviourPunCallbacks
{
    int skinIndex;
    private void Start()
    {
        skinIndex = GetComponent<PUNPlayerController>().skinIndex;
    }
    // [PunRPC]
    // public void SyncSkin()
    // {
    //     GetComponent<SpriteLibrary>().spriteLibraryAsset = GameMananger.instance.CharacterSpriteLibraryAssets[skinIndex];
    // }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        GetComponent<PhotonView>().RPC("SyncSkin", RpcTarget.All);
    }
}
