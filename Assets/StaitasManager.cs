using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaitasManager : MonoBehaviour
{
    /// <summary>����������Ɣ��˕��̐���������</summary>
    [SerializeField]  float _weaponAdd = 0;

    /// <summary>�{�����オ��ƉΗ͂��オ��</summary>
    [SerializeField] float _attackPower = 1;

    /// <summary>�{����������ƃ��[�g�������Ȃ�</summary>
    [SerializeField] float _attackLate = 1;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>���˕��𑝂₷</summary>
    public void AddWepon(int add)
    {
        _weaponAdd+=add;
    }

    public void AttackPowerUp(float s)
    {
        _attackPower += _attackPower * s;
    }


    public void AttackLateUp(float s)
    {
        _attackLate -= _attackLate * s;
    }

}
