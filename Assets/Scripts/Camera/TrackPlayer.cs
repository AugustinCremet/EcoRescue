using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TrackPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _track;
    private Vector3 _initialPosition;
    private void Awake()
    {
        _initialPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.position = _track.Follow.position + _initialPosition;
    }
}
