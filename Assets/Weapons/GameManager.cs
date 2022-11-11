using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _panel = new List<GameObject>();

    [SerializeField] GameObject _basePanel;
    [SerializeField] LayoutGroup _basePanelLayoutGroup;

    [SerializeField] int weaponMaxLevel;

    [SerializeField] int _playerLevel = 1;

    [SerializeField] string[] weapons = new string[2];

    float _exp;
    float _nextLevelExp = 7;
    float nextLevelUpPer = 1.5f;

    //���Z����Ă郌�x��
    int levelTass = 0;

    Dictionary<string, int> weaponsLevel = new Dictionary<string, int>();

    public WeaponInforMaition _weaponInforMaition = default;

    MainStatas _mainStatas;
    WEaponManger weaponManger;

    List<GameObject> instanciatePanels = new List<GameObject>();


    GameSituation _gameSituation = GameSituation.InGame;


   public void AddExp(int exp)
    {
        
        var addExp = exp * _mainStatas.ExpUpper;
        _exp += addExp;

 

        while(_exp>=_nextLevelExp)
        {
            _nextLevelExp = _nextLevelExp * nextLevelUpPer;
            levelTass++;
        }

        Debug.Log("���܂ł̕K�v�o���l��" + _nextLevelExp);
        Debug.Log("���݂̌o���l��" + _exp);

        if(levelTass>0)
        {
            playerLevelUp();
            levelTass--;
        }
    }

    public void playerLevelUp()
    {
        Debug.Log("�v���C���[�̃��x�����オ�����I�I�I���݂̃��x��:" + _playerLevel);

        Dictionary<string, int> dic = new Dictionary<string, int>();

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

            if (dic.Count > 0)
            {
                var r = Random.Range(0, ns.Count);


                //����̋������������Ă���B
                WeaponInforMaition weaponInformaition = weaponManger.GetInfomaitionData((dic[ns[r]]+1), ns[r]);

                //���x���A�b�v���镐��̃p�l����ҏW���Ă���
                GameObject panel = _panel.Where(i => i.name.Contains(ns[r])).First();
                //var go = Instantiate(panel);

                //���x���A�b�v����Text���X�V
                var text = panel.transform.GetChild(0).GetComponent<Text>();
                text.text = weaponInformaition.Te;

                var text2 = panel.transform.GetChild(1).GetComponent<Text>();


                //���x���̕\�L���X�V
                if (weaponsLevel[ns[r]] > 0)
                {
                    text2.text = "Level:" + (weaponsLevel[ns[r]]+1);
                }
                else
                {

                }

                panel.transform.SetParent(_basePanelLayoutGroup.transform);
                instanciatePanels.Add(panel);

                dic.Remove(ns[r]);

                ns.RemoveAt(r);
            }
        }
    }

    /// <summary>���x���A�b�v���I��������ɌĂԁB�p�l���ɂ���</summary>
    public void EndLevelUp()
    {
        _gameSituation = GameSituation.InGame;
        instanciatePanels.ForEach(i => i.transform.position = new Vector3(3000, 3000));
        _basePanel.SetActive(false);
        _basePanelLayoutGroup.transform.DetachChildren();
        if (levelTass > 0)
        {
            playerLevelUp();
            levelTass--;
        }
    }


    void Start()
    {
        _mainStatas = GameObject.FindObjectOfType<MainStatas>();
        weaponManger = GameObject.FindObjectOfType<WEaponManger>();
        SetWeapon();
    }


    void Update()
    {

    }
    void SetWeapon()
    {
        foreach (var n in weapons)
        {
            weaponsLevel.Add(n, 0);
        }
    }
    /// <summary>�e���킩��ĂԁB����̃��x�����A�b�v</summary>
    public void WeaponLevelUp(string name, int level)
    {
        Debug.Log(name + "�̓��x��" + level);
        weaponsLevel[name] = level;
    }




    enum GameSituation
    {
        LevlUp,
        InGame,
    }
}
