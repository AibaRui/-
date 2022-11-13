using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    [Header("�e����̃p�l��")]
    [Tooltip("�e����̃p�l��")] [SerializeField] List<GameObject> _panel = new List<GameObject>();

    [Header("���x���A�b�v�̂������Ƃ̃p�l��")]
    [Tooltip("���x���A�b�v�̂������Ƃ̃p�l��")] [SerializeField] GameObject _basePanel;

    [Header("���C�A�E�g�O���[�v")]
    [Tooltip("���C�A�E�g�O���[�v")] [SerializeField] LayoutGroup _basePanelLayoutGroup;

    List<GameObject> instanciatePanels = new List<GameObject>();

    [Header("��������Ă��镐��̖��O")]
    [Tooltip("��������Ă��镐��̖��O")] [SerializeField] string[] weapons = new string[2];


    [SerializeField] int weaponMaxLevel;

    /// <summary>�v���C���[�̃��x��</summary>
    private int _playerLevel = 1;

    /// <summary>���݂̑��o���l</summary>
    private float _exp;
    /// <summary>���̃��x���A�b�v�ɕK�v�Ȍo���l</summary>
    private float _nextLevelExp = 7;

    private float nextLevelUpPer = 1.5f;

    /// <summary>���x���̉��o�������</summary>
    int levelTass = 0;

    /// <summary>���݂̎�������Ă��镐��Ƃ��̃��x��������</summary>
    Dictionary<string, int> weaponsLevel = new Dictionary<string, int>();

    public WeaponInforMaition _weaponInforMaition = default;

    MainStatas _mainStatas;
    WeaponData weaponManger;


    GameSituation _gameSituation = GameSituation.InGame;
    PauseManager _pauseManager = default;
    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }
    /// <summary>�o���l�Q�b�g</summary>
    /// <param name="exp">�����o���l�̗�</param>
    public void AddExp(int exp)
    {
        //�����o���l�ɁA�o���l�{���������āA���݂̑��o���l�ɓ����o���l�𑫂�   
        var addExp = exp * _mainStatas.ExpUpper;
        _exp += addExp;

        //���o���l���A���̃��x���A�b�v�o���l�𒴂�����A
        //�E���̃��x���A�b�v�o���l���グ�ă��x���A�b�v�̏����̉񐔂𑫂�
        while (_exp >= _nextLevelExp)
        {
            _nextLevelExp = _nextLevelExp * nextLevelUpPer;
            levelTass++;
        }

        //���x���A�b�v�̏����̉񐔂�0�ȏゾ�����珈�������s����B
        if (levelTass > 0)
        {
            playerLevelUp();
            levelTass--;
        }

        Debug.Log("���܂ł̕K�v�o���l��" + _nextLevelExp);
        Debug.Log("���݂̌o���l��" + _exp);
    }

    /// <summary>���x���A�b�v�̏���</summary>
    public void playerLevelUp()
    {
        _pauseManager.PauseResumeLevelUp();
        Debug.Log("�v���C���[�̃��x�����オ�����I�I�I���݂̃��x��:" + _playerLevel);

        //���x��Max�ȊO�̃A�C�e�������Ă�����Dictionary
        Dictionary<string, int> dic = new Dictionary<string, int>();

        //���x��Max�ȊO�̃A�C�e���̖��O�������Ă���List
        List<string> ns = new List<string>();

        _basePanel.SetActive(true);

        //���ׂĂ̕���̃��x�������āAMax���x���ȊO�̂��̂�I�o
        foreach (var n in weapons)
        {
            if (weaponsLevel[n] != weaponMaxLevel)
            {
                dic.Add(n, weaponsLevel[n]);
                ns.Add(n);
            }
        }

        //�R�I�������o��
        for (int i = 0; i < 3; i++)
        {
            var r = Random.Range(0, ns.Count);

            //����̎��̃X�e�[�^�X�������Ă���B
            WeaponInforMaition weaponInformaition = weaponManger.GetInfomaitionData((dic[ns[r]] + 1), ns[r]);

            //���x���A�b�v���镐��̃p�l��������
            GameObject panel = _panel.Where(i => i.name.Contains(ns[r])).First();

            //����̃p�l����Text���X�V
            var text = panel.transform.GetChild(0).GetComponent<Text>();
            text.text = weaponInformaition.Te;

            //����̃p�l���̃��x���̕\�LTextt���X�V
            if (weaponsLevel[ns[r]] > 0)
            {
                var text2 = panel.transform.GetChild(1).GetComponent<Text>();
                text2.text = "Level:" + (weaponsLevel[ns[r]] + 1);
            }

            //����̃p�l�������C�A�E�g�O���[�v�̎q�I�u�W�F�N�g�ɂ���
            panel.transform.SetParent(_basePanelLayoutGroup.transform);
            //�o�����p�l����ۑ����Ă������X�g�ɒǉ�(��ŏ�������)
            instanciatePanels.Add(panel);

            //�I���̏d���𖳂���
            dic.Remove(ns[r]);
            ns.RemoveAt(r);
        }
    }

    /// <summary>���x���A�b�v���I��������ɌĂԁB�p�l���ɂ���</summary>
    public void EndLevelUp()
    {
        _pauseManager.PauseResumeLevelUp();
        //�X�e�[�g���u�Q�[�����v�ɕύX
        _gameSituation = GameSituation.InGame;
        //���o�p�̃p�l�����\���ɂ���
        instanciatePanels.ForEach(i => i.transform.position = new Vector3(3000, 3000));
        _basePanel.SetActive(false);
        _basePanelLayoutGroup.transform.DetachChildren();

        //���x�����܂��o�����������x
        if (levelTass > 0)
        {
            playerLevelUp();
            levelTass--;
        }
    }


    void Start()
    {
        _mainStatas = GameObject.FindObjectOfType<MainStatas>();
        weaponManger = GameObject.FindObjectOfType<WeaponData>();
        SetWeapon();
    }


    void Update()
    {

    }

    /// <summary>������m�F����</summary>
    void SetWeapon()
    {
        foreach (var n in weapons)
        {
            weaponsLevel.Add(n, 0);
        }
    }
    /// <summary>�e���킩��ĂԁB����̃��x�����Đݒ�</summary>
    public void WeaponLevelUp(string name, int level)
    {
        weaponsLevel[name] = level;
        Debug.Log(name + "�̓��x��" + level);
    }




    enum GameSituation
    {
        LevlUp,
        InGame,
    }
}
