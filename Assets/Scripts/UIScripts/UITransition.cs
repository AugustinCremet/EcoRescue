using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransition : MonoBehaviour
{
    [SerializeField] private GameObject _fox;
    [SerializeField] private Image _transitionImage;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void EnableFoxImage()
    {
        _fox.SetActive(true);
    }

    public void DisableFoxImage()
    {
        _fox.SetActive(false);
    }

    public void DisableAnimator()
    {
        EventManager.TriggerEvent(Events.UNLOCK_CONTROLS, null);

        _animator.SetBool("FadeOut", false);
        _animator.enabled = false;
        _transitionImage.color = new Color(_transitionImage.color.r, _transitionImage.color.g, _transitionImage.color.b, 0f);
    }
}
