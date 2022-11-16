using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RigidBodyLagCompensation : MonoBehaviour, IPunObservable
{
    Rigidbody2D _rb;
    Vector2 _netPosition;
    Quaternion _netRotation;
    PhotonView view;
    [SerializeField] int _sendRate = 30;
    [SerializeField] int _serializationRate = 10;
    [SerializeField] bool _setRate = false;
    [Header("Lerp Value")]
    [SerializeField] float _smoothPos = 5.0f;
    [SerializeField] float _smoothRot = 5.0f;
    [Header("Distance To Teleport")]
    [SerializeField] float teleportIfDistance = 50f;
    private void Awake()
    {
        if (_setRate)
        {
            PhotonNetwork.SendRate = _sendRate;
            PhotonNetwork.SerializationRate = _serializationRate;
        }
        _rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }
    private void FixedUpdate()
    {
        if (view.IsMine) return;
        _rb.position = Vector2.Lerp(_rb.position, _netPosition, _smoothPos * Time.fixedDeltaTime);
        // _rb.rotation = Quaternion.Lerp(_rb.rotation, _netRotation, _smoothRot * Time.deltaTime);
        if (Vector3.Distance(_rb.position, _netPosition) > teleportIfDistance)
        {
            _rb.position = _netPosition;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.velocity);
            // stream.SendNext(_rb.rotation);

        }
        else
        {
            _netPosition = (Vector2)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();
            // _netRotation = (Quaternion)stream.ReceiveNext();


            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            _netPosition += (_rb.velocity * lag);
        }
    }
}
