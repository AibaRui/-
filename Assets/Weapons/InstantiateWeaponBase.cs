using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class InstantiateWeaponBase : MonoBehaviour
{
    
    [Header("����̃I�u�W�F�N�g")]
    [Tooltip("����̃I�u�W�F�N�g")] [SerializeField] protected GameObject _weaponObject;

    [Header("����̖��O")]
    [Tooltip("����̖��O")] [SerializeField] protected string _weaponName = "";

    [Header("�Η�")]
    [Tooltip("�Η�")] [SerializeField] protected float _attackPower;

    [Header("1��U�����Ă��玟�̍U��������܂ł̃N�[������")]
    [Tooltip("������o����")] [SerializeField] protected float _coolTime = 1;

    [Header("�͈�")]
    [Tooltip("�͈�")] [SerializeField] protected float _eria;

    [Header("���x")]
    [Tooltip("���x")] [SerializeField] protected float _speed = 6;

    [Header("������o����")]
    [Tooltip("������o����")] [SerializeField] protected int _number;

   protected MainStatas _mainStatas;
    
    /// <summary>���x���A�b�v�e�[�u����ǂݍ��ނ���</summary>
    protected WeaponData _weaponManager = default;

    /// <summary>����̃��x��</summary>
    protected int _level = 0;

    [Header("����̍ő僌�x��")]
    [Tooltip("����̍ő僌�x��")] [SerializeField] protected int _maxLevel = 0;

    /// <summary>�v���C���[�̃p�����[�^�[</summary>
    public WeaponStats _weaponStats = default;


    /// <summary>�Ղꂢ��[�́@�I�u�W�F�N�g</summary>
    protected GameObject _player;
    /// <summary>�N�[���^�C�����v�Z����悤</summary>
    protected float _countTime = 0;


    protected bool _isPause = false;
    protected bool _isLevelUpPause = false;

    Animator _anim;


    PauseManager _pauseManager = default;
    private void Awake()
    {
        _mainStatas = FindObjectOfType<MainStatas>();   
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }

    /// <summary>
    /// ���x���A�b�v����
    /// </summary>
    /// <param name="level">���x���A�b�v�����������x����</param>
    public void LevelUp(int level = 1)
    {
        _weaponManager = FindObjectOfType<WeaponData>();
        if (_level < _maxLevel)
        {
            _level += level;
            _weaponStats = _weaponManager.GetData(_level, _weaponName);

            _attackPower = _weaponStats.Power;
            _coolTime = _weaponStats.CoolTime;
            _eria = _weaponStats.Eria;
            _speed = _weaponStats.Speed;
            _number = _weaponStats.Number;

            Debug.Log(_weaponName + "���x���A�b�v�I���݂̃��x����" + _level);
        }
        GameManager gm = FindObjectOfType<GameManager>();
        gm.WeaponLevelUp(_weaponName,_level);
    }


    ///////Parse����/////

    void OnEnable()
    {
        // �Ă�ŗ~�������\�b�h��o�^����B
        _pauseManager.OnPauseResume += PauseResume;
        _pauseManager.OnLevelUp += LevelUpPauseResume;
        _anim = gameObject.GetComponent<Animator>();
    }

    void OnDisable()
    {
        // OnDisable �ł̓��\�b�h�̓o�^���������邱�ƁB�����Ȃ��ƃI�u�W�F�N�g�������ɂ��ꂽ��j�����ꂽ�肵����ɃG���[�ɂȂ��Ă��܂��B
        _pauseManager.OnPauseResume -= PauseResume;
        _pauseManager.OnPauseResume -= LevelUpPauseResume;
    }

    void PauseResume(bool isPause)
    {
        if (isPause)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    void LevelUpPauseResume(bool isPause)
    {
        if (isPause)
        {
            LevelUpPause();
        }
        else
        {
            LevelUpResume();
        }
    }

    public abstract void LevelUpPause();


    public abstract void LevelUpResume();


    public abstract void Pause();

    public abstract void Resume(); 
}
