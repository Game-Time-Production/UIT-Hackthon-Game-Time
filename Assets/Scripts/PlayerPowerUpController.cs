using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerPowerUpController : MonoBehaviour
{
    [SerializeField] Transform _projectileStartPos;
    public Transform ProjectilStartPos
    {
        get { return _projectileStartPos; }
    }
    [Header("Prefabs")]
    [SerializeField] GameObject bombPrefab;
    PhotonView _view;
    public PhotonView view { get { return _view; } }
    public IPowerUp[] powerUp = new IPowerUp[2] { null, null };
    [PunRPC]
    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        // powerUp[0] = new BombPowerUp(_projectileStartPos, transform);
    }
    private void Start()
    {
        if (_view.IsMine)
        {
            PowerUpManager.instance.SetUpPowerUp(this);
        }
    }
    private void Update()
    {
        if (_view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("power up slot 1 pressed");
                powerUp[0]?.PerformAction();
                powerUp[0] = null;
                PowerUpSlotUI.instance.RemoveSlotIcon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("power up slot 2 pressed");
                powerUp[1]?.PerformAction();
                powerUp[1] = null;
                PowerUpSlotUI.instance.RemoveSlotIcon(1);
            }
        }
    }
    public int GetEmptyPowerUpSlot()
    {
        for (int i = 0; i < powerUp.Length; i++)
        {
            if (powerUp[i] == null)
                return i;
        }
        return -1;
    }
}
