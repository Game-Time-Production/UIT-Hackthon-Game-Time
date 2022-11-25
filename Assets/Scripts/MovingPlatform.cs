using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> _waypoints;
    [SerializeField] int _currentWaypointIndex;
    [SerializeField] float _speed;
    [SerializeField] float _checkDistance;
    [SerializeField] List<PUNPlayerController> _playersOnPlatform;
    PhotonView _view;
    Rigidbody2D _rb;
    [SerializeField] Vector2 _direction;
    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _playersOnPlatform = new List<PUNPlayerController>();
        _direction = (_waypoints[_currentWaypointIndex].position - transform.position).normalized;
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        // transform.position = Vector2.MoveTowards(
        //     transform.position,
        //     _waypoints[_currentWaypointIndex].position,
        //      _speed * Time.fixedDeltaTime);
        _rb.velocity = _direction * _speed;


        if (_playersOnPlatform.Count > 0)
        {
            foreach (var player in _playersOnPlatform)
            {
                if (player.view.IsMine)
                    player.transform.position += (Vector3)_direction * _speed * Time.fixedDeltaTime;
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
            // other.transform.parent = transform;
            _playersOnPlatform.Add(other.gameObject.GetComponent<PUNPlayerController>());
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // other.transform.parent = other.gameObject.GetComponent<PUNPlayerController>().originalParent;
            PUNPlayerController playerController = other.gameObject.GetComponent<PUNPlayerController>();
            playerController.rb.velocity += _rb.velocity; // just for fun
            _playersOnPlatform.Remove(playerController); // maybe this won't cause bug just maybe
        }

    }
    private void GetNextWaypoint()
    {
        _currentWaypointIndex++;
        if (_currentWaypointIndex >= _waypoints.Count)
        {
            _currentWaypointIndex = 0;
        }
        _direction = (_waypoints[_currentWaypointIndex].position - transform.position).normalized;
    }
}
