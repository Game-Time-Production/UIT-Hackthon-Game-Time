using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class PUNReloadGame : MonoBehaviourPunCallbacks 
{
    // seriously a dumb ass work around method to reload a scene in Photon PUN 
    void Start()
    {
        PhotonNetwork.LoadLevel("Game 1");
    }
}
