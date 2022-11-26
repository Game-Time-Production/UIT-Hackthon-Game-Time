using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;
    public Vector3 offset;
    [SerializeField] float _smoothPos;
    [SerializeField] float _smoothDampTime = 0.25f;
    [SerializeField] float _leftBoundaryValue;
    [SerializeField] float _rightBoundaryValue;
    [SerializeField] bool _lockInBound;
    Vector3 velocity;
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
    private void Update()
    {
        // if (target)
        //     transform.position = target.position + offset;
    }
    private void FixedUpdate()
    {
        if (target)
        {
            // calculate position and clamp its value to the hard coded camera boundary
            Vector3 targetPos = target.position + offset;
            if (_lockInBound)
                targetPos.x = Mathf.Clamp(targetPos.x, _leftBoundaryValue, _rightBoundaryValue);
            // transform.position = Vector3.Lerp(transform.position, targetPos, _smoothPos * Time.fixedDeltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, _smoothDampTime);


        }
    }
    private bool OutOfBound()
    {
        Debug.Log(transform.position.x);
        bool outOfBound = false;
        if (transform.position.x < _leftBoundaryValue)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(_leftBoundaryValue + 0.001f, target.position.y, transform.position.z),
                Time.fixedDeltaTime * _smoothPos);
            outOfBound = true;
        }
        if (transform.position.x > _rightBoundaryValue)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(_rightBoundaryValue - 0.001f, target.position.y, transform.position.z),
                Time.fixedDeltaTime * _smoothPos);
            outOfBound = true;
        }
        return outOfBound;
    }

}
