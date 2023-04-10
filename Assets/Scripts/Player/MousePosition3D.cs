using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    private Camera _mainCamera;
    private ParticleSystem _particleSystem;
    [SerializeField] private LayerMask _layerMask;

    private void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    private void LateUpdate()
    {
        if (ControlChangeInGame.Instance.IsUsingKeyboard)
        {
            _particleSystem.gameObject.SetActive(true);
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _layerMask))
            {
                transform.position = raycastHit.point;
            }
        }
        else
            _particleSystem.gameObject.SetActive(false);
    }
}
