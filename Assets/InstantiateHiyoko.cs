using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateHiyoko : InstantiateWeaponBase
{
    [SerializeField] GameObject _hiyoko;

    [SerializeField] GameObject _beamPos;

    public float _period;

    /// <summary>�U���\���ǂ���</summary>
    bool _isAttack = false;

    bool _isInstance = false;

    /// <summary>���̍U���ŏo��������A�o���I�������ǂ���</summary>
    bool _isInstanciateEnd = true;


    float _lifeTime;


    IEnumerator _instanciateCorutine;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _weaponManager = FindObjectOfType<WeaponData>();
    }

    void Update()
    {
        if (!_isLevelUpPause && !_isPause)
        {
            if (_level > 0)
            {

                if (_isAttack)
                {
                    _period = _speed*_mainStatas.Speed;
                    _instanciateCorutine = Attack();
                    StartCoroutine(_instanciateCorutine);
                }
                else if (_isInstanciateEnd)
                {
                    AttackLate();
                }
            }
        }
    }

    public void InstanceHiyoko()
    {
        if (!_isInstance)
        {
            _isInstance = true;
            _hiyoko.SetActive(true);
        }

    }

    void AttackLate()
    {
        _countTime -= Time.deltaTime;

        if (_countTime <= 0)
        {
            var setCoolTime = _coolTime * _mainStatas.CoolTime;
            _countTime = setCoolTime;
            _isAttack = true;
        }
    }

    IEnumerator Attack()
    {
        _isAttack = false;
        _isInstanciateEnd = false;

        //�r�[�����o�����Ԃ����߂�B(����̃X�e�[�^�X�ƁA���C���X�e�[�^�X�̍��v�l)
        _lifeTime = _number + _mainStatas.Number * 2;

        var go = Instantiate(_weaponObject);
       // go.transform.position = _hiyoko.transform.position + new Vector3(0,3,0);
        go.transform.SetParent(_hiyoko.transform);
        go.transform.position = _beamPos.transform.position;

        yield return new WaitForSeconds(_lifeTime);

        _isInstanciateEnd = true;
        _instanciateCorutine = null;
    }

    public override void LevelUpPause()
    {
        _isLevelUpPause = true;
        if (_instanciateCorutine != null)
        {
            StopCoroutine(_instanciateCorutine);
        }
    }

    public override void LevelUpResume()
    {
        _isLevelUpPause = false;

        if (_instanciateCorutine != null)
        {
            StartCoroutine(_instanciateCorutine);
        }
    }

    public override void Pause()
    {
        _isPause = true;
        if (!_isLevelUpPause)
        {
            if (_instanciateCorutine != null)
            {
                StopCoroutine(_instanciateCorutine);
            }
        }
    }

    public override void Resume()
    {
        _isPause = false;

        if (!_isLevelUpPause)
        {
            if (_instanciateCorutine != null)
            {
                StartCoroutine(_instanciateCorutine);
            }
        }
    }

}
