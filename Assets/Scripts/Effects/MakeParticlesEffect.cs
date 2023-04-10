using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeParticlesEffect : _ObjectsMakeBase
{
    [SerializeField] private float _startDelay;
    [SerializeField] private int _makeCount;
    [SerializeField] private float _makeDelay;
    [SerializeField] private Vector3 _randomPos;
    [SerializeField] private Vector3 _randomRot;
    [SerializeField] private Vector3 _randomScale;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private bool _isObjectAttachToParent = true;

    private float _time;
    private float _time2;
    private float _delayTime;
    private float _count;
    private float _scalefactor;
    private int _damage;
    private float _maxDistance;
    private GameObject _obj;


    private void Start()
    {
        _time = _time2 = Time.time;
        _scalefactor = VariousEffectsScene.m_gaph_scenesizefactor; //transform.parent.localScale.x; 
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetMaxDistance(float maxDistance)
    {
        _maxDistance = maxDistance;
    }

    public void StartParticles()
    {
        Vector3 m_pos = transform.position + GetRandomVector(_randomPos) * _scalefactor; 
        Quaternion m_rot = transform.rotation * Quaternion.Euler(GetRandomVector(_randomRot));
        _obj = null;

        for (int i = 0; i < m_makeObjs.Length; i++)
        {
            _obj = Instantiate(m_makeObjs[i], m_pos, m_rot);
            _obj.transform.localScale = this.transform.localScale;
            //FindObjectOfType<FXManager>().PlaySound("projectile", FindObjectOfType<Player>().gameObject);

            if (_obj.GetComponent<HitBox>() != null)
            {
                _obj.GetComponent<HitBox>().SetDamage(_damage);
            }

            if(_obj.GetComponent<ParticleMove>() != null)
            {
                _obj.GetComponent<ParticleMove>().SetMaxDistance(_maxDistance);
            }

            //Vector3 m_scale = (m_makeObjs[i].transform.localScale + GetRandomVector2(_randomScale));
            if(_isObjectAttachToParent)
                _obj.transform.parent = this.transform;
            //m_obj.transform.localScale = m_scale;
        }

        Destroy(_obj, _destroyDelay);
    }

    public void EndParticles()
    {
        if( _obj != null )
            Destroy(_obj);
    }
}
