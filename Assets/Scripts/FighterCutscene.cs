using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FighterCutscene : MonoBehaviour
{
    enum CutSceneState
    {
        Watch,
        Wait,
        Done
    }

    [SerializeField] bool isDone;
    [SerializeField] int totalPlayerDone;
    CutSceneState state;
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView>();

        isDone = false;
        totalPlayerDone = 0;

        state = CutSceneState.Watch;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            isDone=true;
        }

        if(state == CutSceneState.Watch && isDone == true)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                
                InscreasePlayerDoneAmount();
                Debug.Log(totalPlayerDone);
            }
            else
            {             
                view.RPC(nameof(InscreasePlayerDoneAmount), RpcTarget.MasterClient);
            }

            state = CutSceneState.Wait;
        }

        if (PhotonNetwork.IsMasterClient && state == CutSceneState.Wait && totalPlayerDone >= 2)
        {
            PhotonNetwork.LoadLevel("Game 1");
            state = CutSceneState.Done;
        }
    }

    [PunRPC]
    public void InscreasePlayerDoneAmount()
    {
        totalPlayerDone++;
    }

    public void DoneWatchingCutScene()
    {
        isDone = true;
    }
}
