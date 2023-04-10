using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] protected AnimationCurve _speedCurve;

    protected Rigidbody _rb;
    protected GameObject _player;

    protected bool _canGo = false;
    protected bool _collected = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(Wait());
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Inventory"))
        {
            if (_canGo)
                _player = other.gameObject;
        }
    }

    protected void FixedUpdate()
    {
        if (_collected)
            Destroy(this.gameObject);

        if (_player != null)
        {
            var distance = Vector3.Distance(this.transform.position, _player.transform.position);
            var speed = _speedCurve.Evaluate(distance);
            var direction = _player.transform.position - this.transform.position;

            _rb.MovePosition(this.transform.position + direction * speed * Time.deltaTime);

            if (distance < 0.5f)
                OnCollected(_player.GetComponentInParent<Player>());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);

        _canGo = true;
    }

    protected abstract void OnCollected(Player player);
}
