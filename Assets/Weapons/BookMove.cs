using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMove : MonoBehaviour
{
    // �~�^������(+��-�ŉ�]�������ς��)
    private float _period = 2;

    MainStatas _mainStatas;

    BoockWeapon _boockWeapon;
    GameObject _player;
    Vector3 _center;

    private void Awake()
    {
        _mainStatas = FindObjectOfType<MainStatas>();
        _boockWeapon = GameObject.FindObjectOfType<BoockWeapon>();
        _period = _boockWeapon._period;
        _player = GameObject.FindGameObjectWithTag("Player");
        _center = _player.transform.position;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //     _player =  GameObject.FindGameObjectWithTag("Player");
        _center = _player.transform.position;

        var tr = transform;
        // ��]�̃N�H�[�^�j�I���쐬
        var angleAxis = Quaternion.AngleAxis(360 / _period * Time.deltaTime, transform.parent.forward);

        // �~�^���̈ʒu�v�Z
        var pos = tr.position;

        pos -= _center;
        pos = angleAxis * pos.normalized * 2.2f;
        pos += _center;

        tr.position = pos;
    }
}
