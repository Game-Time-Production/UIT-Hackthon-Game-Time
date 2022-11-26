using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CoopTransitionMP : MonoBehaviour
{
    [SerializeField] List<Transform> _waypoints;
    [SerializeField] int _currentWaypointIndex;
    [SerializeField] float _speed;
    [SerializeField] float _checkDistance;
    [SerializeField] List<PUNPlayerController> _playersOnPlatform;
    PhotonView _view;
    Rigidbody2D _rb;
    [SerializeField] Vector2 _direction;
    public bool transitioning;
    public GameObject lockObject;
    int _playerCount = 0;
    private void Awake()
    {
        transitioning = false;
        _view = GetComponent<PhotonView>();
        _playersOnPlatform = new List<PUNPlayerController>();
        _direction = (_waypoints[_currentWaypointIndex].position - transform.position).normalized;
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (transitioning)
        {
            _rb.velocity = _direction * _speed;
            if (_playersOnPlatform.Count > 0)
            {
                foreach (var player in _playersOnPlatform)
                {
                    if (player.view.IsMine)
                        player.transform.position += (Vector3)_direction * _speed * Time.fixedDeltaTime;
                }
            }
        }
        if (Vector2.Distance(transform.position, _waypoints[_currentWaypointIndex].position) <= _checkDistance)
        {
            GetNextWaypoint();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (!transitioning)
                {
                    _playerCount++;
                }
            }
            Debug.Log("Player count: " + _playerCount);

            // other.transform.parent = transform;
            //_playersOnPlatform.Add(other.gameObject.GetComponent<PUNPlayerController>());
            if (_playerCount >= 1)
            {
                lockObject.SetActive(true);
                _view.RPC(nameof(PhaseTranstion), RpcTarget.All, true);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!transitioning)
            {
                _playerCount--;
            }
            // other.transform.parent = other.gameObject.GetComponent<PUNPlayerController>().originalParent;
            //PUNPlayerController playerController = other.gameObject.GetComponent<PUNPlayerController>();
            //playerController.rb.velocity += _rb.velocity; // just for fun
            //_playersOnPlatform.Remove(playerController); // maybe this won't cause bug just maybe
            if (_playerCount < 1)
            {
                lockObject.SetActive(false);
                _view.RPC(nameof(PhaseTranstion), RpcTarget.All, false);
            }
        }

    }
    private void GetNextWaypoint()
    {
        // _currentWaypointIndex++;
        // if (_currentWaypointIndex >= _waypoints.Count)
        // {
        //     _currentWaypointIndex = 0;
        // }
        // _direction = (_waypoints[_currentWaypointIndex].position - transform.position).normalized;
        _rb.velocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        lockObject.SetActive(false);
    }
    [PunRPC]
    public void PhaseTranstion(bool value)
    {
        transitioning = value;
    }
}
