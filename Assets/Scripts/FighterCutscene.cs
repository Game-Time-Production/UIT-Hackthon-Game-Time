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
        Done,
        Waiting
    }

    [SerializeField] GameObject pannelWaiting;
    bool isDone;
    int totalPlayerDone;
    bool isSkiping;
    CutSceneState state;
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView>();

        isSkiping = false;
        isDone = false;
        totalPlayerDone = 0;

        state = CutSceneState.Watch;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isSkiping == false)
        {
            Debug.Log("Skip");
            //isDone=true;
            isSkiping=true;
            Time.timeScale=5f;
        }

        if (state == CutSceneState.Watch && isDone == true)
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

        if(state == CutSceneState.Wait)
        {
            pannelWaiting.SetActive(true);
            state = CutSceneState.Waiting;
        }

        if (PhotonNetwork.IsMasterClient && state == CutSceneState.Waiting && totalPlayerDone >= 2)
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
        Time.timeScale = 1f;
        isDone = true;
    }
}
