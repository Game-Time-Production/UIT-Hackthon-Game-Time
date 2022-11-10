using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerPowerUpController : MonoBehaviour
{
    [SerializeField] Transform projectileStartPos;
    [Header("Prefabs")]
    [SerializeField] GameObject bombPrefab;
    PhotonView view;
    [PunRPC]
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    public void ThrowBomb(Vector3 direction) // note change the way the bomb get its direction
    {
        Vector2 trueDirection = (projectileStartPos.position - transform.position).normalized;
        // Debug.Log(trueDirection);
        GameObject bomb = PhotonNetwork.Instantiate(bombPrefab.name, projectileStartPos.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().AddForce(trueDirection * 10f, ForceMode2D.Impulse);
    }
    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ThrowBomb(transform.forward.normalized * 5f);
            }
        }
    }
}
