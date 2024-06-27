using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AdModeManager : MonoBehaviour
{
    public static AdModeManager Instance;
    [HideInInspector]
    public GameObject obj;
    [HideInInspector]
    public int roomLv;
    [HideInInspector]
    public int roomLvTotal;
    [HideInInspector]
    public int runeLv;
    [HideInInspector]
    public int runeLvUpExp;

    [SerializeField]
    private ReLifeDialog reLifeDialog;
    public bool IsOnceLife { get; private set; }


    public UnityAction questBattle;
    public UnityAction questNext;
    private QuestStatus questStatus;
    [HideInInspector]
    public RoomBace nowRoom;
    [HideInInspector]
    public RoomBace clearRoom;

    public QuestStatus QuestStatus
    {
        get => questStatus;
        set
        {
            questStatus = value;
            switch (questStatus)
            {
                case QuestStatus.battle:
                    AdSceneManager.Instance.OnBattle();
                    break;
                case QuestStatus.next:
                    AdSceneManager.Instance.OnNext();
                    break;
            }
        }
    }



    /// <summary>
    /// インスタンス自身
    /// </summary>
    private void Awake()
    {
        //唯一初始化
        if (Instance == null)
        {
            Instance = this;
            obj = gameObject;
            DontDestroyOnLoad(gameObject);

            var randomState = Random.state;
            Random.InitState(Random.Range(int.MinValue, int.MaxValue));

            HeroCheck();
            RuneManager.Instance.InitDicRunesCanAdd();

            Random.state = randomState;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HeroCheck()
    {
        if (PlayerManager.Instance.Hero != null) return;

        GameManager.Instance.ClearAllList();

        UnitBace hero = null;
        if (PlayerManager.Instance.heroPrafe != null)
        {
            Transform ts = PoolManager.Instance.GetObj(PlayerManager.Instance.heroPrafe);
            hero = ts.GetComponent<UnitBace>();
        }
        else
        {
            //int i = Random.Range(0, PlayerManager.Instance.heroBaceList.Count);
            int i = 2;
            GameObject go = Instantiate(PlayerManager.Instance.heroBaceList[i].obj, transform.position, transform.rotation);
            hero = go.GetComponent<UnitBace>();
        }
        hero.Init();
        PlayerManager.Instance.Hero = hero;
        DontDestroyOnLoad(PlayerManager.Instance.Hero.obj);
        SkillCheck();
        PlayerManager.Instance.Hero.skills.skill.nowCoolTime = PlayerManager.Instance.Hero.skills.skill.coolTime;
        PlayerManager.Instance.Hero.skills.ultimate.nowCoolTime = PlayerManager.Instance.Hero.skills.ultimate.coolTime;
        PlayerManager.Instance.Hero.Spelld(PlayerManager.Instance.Hero.skills.item, mpDown: 0);
        PlayerManager.Instance.Hero.Cold(PlayerManager.Instance.Hero.skills.item);




        foreach (var net in GameManager.Instance.netHeroList)
            net.OnStartPlay();
    }

    public void SkillCheck()
    {
        PointerEventData data = null;
        Joystick.Instance.OnPointerUp(data);
        if (PlayerManager.Instance.Hero.skills.attack.isKeeping)
            Joystick.Instance.handle1.OnPointerUp(data);
        if (PlayerManager.Instance.Hero.skills.skill.isKeeping)
            Joystick.Instance.handle2.OnPointerUp(data);
        if (PlayerManager.Instance.Hero.skills.ultimate.isKeeping)
            Joystick.Instance.handle3.OnPointerUp(data);
        if (PlayerManager.Instance.Hero.skills.item.isKeeping)
            Joystick.Instance.handle4.OnPointerUp(data);
        if (PlayerManager.Instance.callFriends.isKeeping)
            Joystick.Instance.handle5.OnPointerUp(data);
        Joystick.Instance.handle1.ReSet();
        Joystick.Instance.handle2.ReSet();
        Joystick.Instance.handle3.ReSet();
        Joystick.Instance.handle4.ReSet();
        Joystick.Instance.handle5.ReSet();
    }

    /// <summary>
    /// 能力追加
    /// </summary>
    public void RuneAdd()
    {
        UIManager.Instance.runeModal.Show();
        UIManager.Instance.runeModal.AddRune();
    }

    public void Next()
    {
        QuestStatus = QuestStatus.next;
    }
    public void GameOver(bool isClear = false, bool isCanRelif = false)
    {
        if (isClear)
        {
            QuestStatus = QuestStatus.clear;
            UIManager.Instance.GameOver(isClear);
        }
        else
        {
            if (IsOnceLife|| !isCanRelif)
            {
                QuestStatus = QuestStatus.over;
                UIManager.Instance.GameOver(isClear);
            }
            else
            {
                IsOnceLife = true;
                reLifeDialog.Obj.SetActive(true);
            }
        }
    }
}
