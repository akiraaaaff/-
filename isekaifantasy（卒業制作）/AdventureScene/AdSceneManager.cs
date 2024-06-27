using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AdSceneManager : MonoBehaviour
{
    public static AdSceneManager Instance;
    [SerializeField]
    protected string mapName;
    [SerializeField]
    protected Transform roomGroup;
    [HideInInspector]
    public List<RoomBace> roomList = new List<RoomBace>();
    [SerializeField]
    protected Transform roomAreaGroup;
    protected List<RoomArea> roomAreaList = new List<RoomArea>();
    [SerializeField]
    protected Transform doorColliderGroup;
    protected List<DoorBace> doorList = new List<DoorBace>();
    public Transform transPort;
    [HideInInspector] public GameObject transPortObj;

    [HideInInspector]
    public FollowPlayerAdventureScene camFollow;
    public float CameraZMinRoom;
    public float CameraZMaxRoom;
    public float CameraZMinMap;
    public float CameraZMaxMap;
    public float CameraXMinRoom;
    public float CameraXMaxRoom;
    public float CameraXMinMap;
    public float CameraXMaxMap;

    [SerializeField, Range(2, 9)]
    protected int enemyMaxCount = 5;
    [SerializeField]
    protected float hpPara = 1.0f;
    [SerializeField]
    protected float attackPara = 1.0f;
    [SerializeField]
    protected float atSpeedPara = 0.3f;
    [SerializeField]
    protected int reHpDown = 10;
    [SerializeField]
    protected int reMpDown = 10;
    [SerializeField]
    protected float roateSpeedDownPara = 3.0f;
    [SerializeField]
    protected int moveSpeedDown = 100;
    public List<SpawnEnemDate> enemDate = new List<SpawnEnemDate>();
    public List<SpawnEnemDate> enemDateBoss = new List<SpawnEnemDate>();
    protected List<SpawnEnemDate> enemDateTemp;
    [HideInInspector]
    public List<SpawnEnemDate> enemyTypeListThis = new List<SpawnEnemDate>();
    [HideInInspector]
    public int runeExp;

    protected int countEnemyAll;
    protected int countBossAll;
    protected IEnumerator animeIEnumerator;

    public virtual void DoorSetBattle(RoomBace room)
    {
        if (AdModeManager.Instance.QuestStatus != QuestStatus.next)
            return;
        netSetBattle?.Invoke(roomList.FindIndex(x => x == room));
        AdModeManager.Instance.nowRoom = room;
        AdModeManager.Instance.QuestStatus = QuestStatus.battle;
    }
    public UnityAction<int> netSetBattle;

    public virtual void OnBattle()
    {
        AdModeManager.Instance.roomLv++;
        AdModeManager.Instance.roomLvTotal++;
        PlayerManager.Instance.Hero.nav.areaMask = GameManager.Instance.GetAreaMaskIndex();
        camFollow.InRoom();
        if (AdModeManager.Instance.roomLvTotal == 1 ||
            AdModeManager.Instance.roomLv == 3 ||
            AdModeManager.Instance.roomLvTotal == 8 ||
            AdModeManager.Instance.roomLvTotal == 16 ||
            AdModeManager.Instance.roomLvTotal == 24)
            AdModeManager.Instance.RuneAdd();
        if (AdModeManager.Instance.clearRoom != null)
            AdModeManager.Instance.clearRoom.CloseDoor();
        AdModeManager.Instance.questBattle?.Invoke();
        AdModeManager.Instance.nowRoom.AbleItems();
        PlayerManager.Instance.FriendsFllowHero();
        Spawn();
    }
    public virtual void OnNext()
    {
        AdModeManager.Instance.clearRoom = AdModeManager.Instance.nowRoom;
        AdModeManager.Instance.nowRoom.DisableItems();
        AdModeManager.Instance.questNext?.Invoke();
        if (AdModeManager.Instance.roomLvTotal == 24)
            AdModeManager.Instance.GameOver(true);
        else
        {
            ShowTitle();
            if (AdModeManager.Instance.roomLv == 4)
            {
                AdModeManager.Instance.roomLv = 0;
                AdModeManager.Instance.RuneAdd();
                ViewTransPort();
            }
            else
            {
                camFollow.InMap();
                AdModeManager.Instance.nowRoom.OpenDoor();
                PlayerManager.Instance.FriendsSetNavAllArea();
            }
        }
    }


    /// <summary>
    /// インスタンス自身
    /// </summary>
    protected virtual void Start()
    {
        if (Instance != null) return;
        Instance = this;
        effPfb = GameManager.Instance.GameConf.effects.SpawnMagic;
        camFollow = Camera.main.GetComponent<FollowPlayerAdventureScene>();
        foreach (Transform room in roomGroup) roomList.Add(room.GetComponent<RoomBace>());
        foreach (Transform area in roomAreaGroup) roomAreaList.Add(area.GetComponent<RoomArea>());
        foreach (Transform door in doorColliderGroup) doorList.Add(door.GetComponent<DoorBace>());
        SetDoorAndAreaOfRoom();
        transPortObj = transPort.gameObject;
        enemDateTemp = enemDate;
        foreach (SpawnEnemDate enemy in enemDate)
            countEnemyAll += enemy.count;
        if (enemDateBoss != null)
        {
            foreach (SpawnEnemDate enemy in enemDateBoss)
                countBossAll += enemy.count;
        }
        AdModeManager.Instance.SkillCheck();

        ResetRoom();
    }

    private void HideName()
    {
        UIManager.Instance.tips.Hide();
    }

    public void ResetRoom()
    {
        int id = Random.Range(0, roomList.Count);
        //int id = 0;
        AdModeManager.Instance.nowRoom = roomList[id];
        PlayerManager.Instance.Hero.nav.enabled = false;
        PlayerManager.Instance.Hero.trs.position = AdModeManager.Instance.nowRoom.trs.position;
        PlayerManager.Instance.Hero.trs.rotation = AdModeManager.Instance.nowRoom.trs.rotation;
        PlayerManager.Instance.Hero.nav.areaMask = NavMesh.AllAreas;
        PlayerManager.Instance.Hero.nav.enabled = true;
        camFollow.InRoom();
        camFollow.Init();
        AdModeManager.Instance.QuestStatus = QuestStatus.battle;
    }
    public void ViewTransPort()
    {
        transPort.position = AdModeManager.Instance.nowRoom.trs.position;
        transPortObj.SetActive(true);
    }
    public void ShowTitle()
    {
        UIManager.Instance.SpawnShowRow.Show();
    }

    private SpawnEnemDate CountChangSpawnEnemDate(int id)
    {
        SpawnEnemDate spawnEnemDate = enemDateTemp[0];
        int i = 0;
        int iBefor = 0;
        foreach (SpawnEnemDate enemy in enemDateTemp)
        {
            i += enemy.count;
            if (id >= iBefor && id < i)
                return enemy;
            iBefor += enemy.count;
        }
        return spawnEnemDate;
    }

    public void Spawn()
    {
        Invoke(nameof(PlayVoice), 0.1f);
        UIManager.Instance.SpawnShowRow.Hide();
        List<Transform> tempSpawnList = new List<Transform>();
        foreach (Transform child in AdModeManager.Instance.nowRoom.spawnList)
            tempSpawnList.Add(child);


        //get---------------------------------------------------
        var enemyListThis = new List<UnitBace>();
        int enmeyCount = AdModeManager.Instance.roomLv + 1;
        if (AdModeManager.Instance.roomLvTotal >= 4)
            enmeyCount = Random.Range(2, enemyMaxCount);
        if (AdModeManager.Instance.roomLvTotal == 4 ||
            AdModeManager.Instance.roomLvTotal == 12 ||
            AdModeManager.Instance.roomLvTotal == 20)
        {
            enmeyCount = 8;
        }
        int tempCountEnemyAll = countEnemyAll;
        if (AdModeManager.Instance.roomLvTotal == 8 ||
            AdModeManager.Instance.roomLvTotal == 16 ||
            AdModeManager.Instance.roomLvTotal == 24)
        {
            enemDateTemp = enemDateBoss;
            enmeyCount = 0;
            tempCountEnemyAll = countBossAll;
        }

        for (int i = 0; i <= enmeyCount; i++)
        {
            int id = Random.Range(0, tempCountEnemyAll);
            enemyTypeListThis.Add(CountChangSpawnEnemDate(id));

            UnitBace newEnemy = PoolManager.Instance.GetObj(CountChangSpawnEnemDate(id).enemy).GetComponent<UnitBace>();
            newEnemy.Init();
            newEnemy.nav.enabled = false;
            enemyListThis.Add(newEnemy);
        }
        //-------------------------------------------------


        for (int i = 0; i < enemyTypeListThis.Count; i++)
        {
            int id = Random.Range(0, tempSpawnList.Count - 1);
            enemyListThis[i].trs.position = tempSpawnList[id].position;
            enemyListThis[i].trs.rotation = tempSpawnList[id].rotation;
            tempSpawnList.Remove(tempSpawnList[id]);
            EnemyInit(enemyListThis[i], enemyTypeListThis[i]);
        }
        enemyTypeListThis.Clear();
        //name
        if (AdModeManager.Instance.roomLv == 1)
        {
            UIManager.Instance.tips.Show(
                LocalizationManager.GetTranslation(mapName));
            Invoke(nameof(HideName), 10f);
        }
    }
    private void PlayVoice()
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.MagicStay);
    }

    protected GameObject effPfb;
    protected Transform lastEffTrs;
    protected virtual void EnemyInit(UnitBace enemy, SpawnEnemDate enemyData)
    {
        lastEffTrs = PoolManager.Instance.GetObj(effPfb);
        lastEffTrs.position = enemy.trs.position;
        if (AdModeManager.Instance.roomLvTotal >= 4)
        {
            int hpPlus = 150 * AdModeManager.Instance.roomLvTotal / enemyTypeListThis.Count;
            if (AdModeManager.Instance.roomLvTotal == 4 ||
                AdModeManager.Instance.roomLvTotal == 8 ||
                AdModeManager.Instance.roomLvTotal == 12 ||
                AdModeManager.Instance.roomLvTotal == 16 ||
                AdModeManager.Instance.roomLvTotal == 20 ||
                AdModeManager.Instance.roomLvTotal == 24)
                hpPlus += 150 * AdModeManager.Instance.roomLvTotal /
                    enemyTypeListThis.Count;

            enemy.hpMax += hpPlus;
            enemy.Hp = enemy.hpMax;
        }
        if (AdModeManager.Instance.roomLvTotal == 8 ||
            AdModeManager.Instance.roomLvTotal == 16 ||
            AdModeManager.Instance.roomLvTotal == 24)
        {
            if (AdModeManager.Instance.roomLvTotal >= 8)
                enemy.attack += 10;
            if (AdModeManager.Instance.roomLvTotal >= 16)
                enemy.attack += 10;
            if (AdModeManager.Instance.roomLvTotal >= 24)
                enemy.attack += 10;

            enemy.restoreMp += reMpDown;
            enemy.mpMax += 100;
            enemy.Mp = enemy.mpMax;
            enemy.isBoss = true;
            enemy.trs.position = AdModeManager.Instance.nowRoom.trs.position;
            lastEffTrs.position = enemy.trs.position;
        }
        else
        {
            enemy.AttackSpeed = enemy.AttackSpeed * atSpeedPara;
        }
        enemy.hpMax = (int)(enemy.hpMax * hpPara);
        enemy.Hp = enemy.hpMax;
        enemy.attack = (int)(enemy.attack * attackPara);
        enemy.restoreHp -= reHpDown;
        enemy.Move -= moveSpeedDown;
        enemy.rotateSpeed *= roateSpeedDownPara;
        animeIEnumerator = EnemyRun(enemy, enemyData);
        StartCoroutine(animeIEnumerator);
    }

    protected virtual IEnumerator EnemyRun(UnitBace enemy, SpawnEnemDate enemyData)
    {
        int i = 0;
        Vector3 tarPos = enemy.trs.position;
        enemy.trs.position = new Vector3(enemy.trs.position.x, enemy.trs.position.y - enemy.capsuleCol.height, enemy.trs.position.z);
        float loopTime = 0.01f;
        float aniSpeed = 2f;
        while (enemy.Hp > 0)
        {
            yield return new WaitForSeconds(loopTime);
            {
                if (enemy.Hp > 0)
                {
                    i += 1;
                    enemy.trs.position = Vector3.Lerp(enemy.trs.position, tarPos, loopTime * aniSpeed);
                    if (i == 80)
                    {
                        enemy.SetGroup(GameManager.Instance.enemyList);
                        if (enemy.Target != null) enemy.LookAtPos(enemy.Target.trs.position);
                    }
                    if (i == 160)
                    {
                        enemy.trs.position = tarPos;
                        enemy.nav.enabled = true;
                        enemy.ai.Init(enemyData);
                        break;
                    }
                }
            }
        }
    }

    protected void SetDoorAndAreaOfRoom()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            roomAreaList[i].SetRoom(roomList[i]);
            if (roomList[i].doorList != null && roomList[i].doorList.Count == 0)
            {
                if (i <= 2)
                {
                    roomList[i].doorList.Add(doorList[i]);
                    if (i == 0)
                        roomList[i].doorList.Add(doorList[i + 1]);
                    roomList[i].doorList.Add(doorList[i + 2]);
                }
                else if (i <= 5)
                {
                    roomList[i].doorList.Add(doorList[i + 2]);
                    roomList[i].doorList.Add(doorList[i + 4]);
                    if (i == 3)
                    {
                        roomList[i].doorList.Add(doorList[i - 3]);
                        roomList[i].doorList.Add(doorList[i + 3]);
                    }
                    else
                    {
                        roomList[i].doorList.Add(doorList[i - 1]);
                    }
                }
                else if (i <= 8)
                {
                    roomList[i].doorList.Add(doorList[i + 4]);
                    roomList[i].doorList.Add(doorList[i + 6]);
                    if (i == 6)
                    {
                        roomList[i].doorList.Add(doorList[i - 1]);
                        roomList[i].doorList.Add(doorList[i + 5]);
                    }
                    else
                    {
                        roomList[i].doorList.Add(doorList[i + 1]);
                    }
                }
                else if (i <= 11)
                {
                    if (i == 9)
                    {
                        roomList[i].doorList.Add(doorList[i + 1]);
                        roomList[i].doorList.Add(doorList[i + 6]);
                        roomList[i].doorList.Add(doorList[i + 7]);
                    }
                    else
                    {
                        roomList[i].doorList.Add(doorList[i + 3]);
                        roomList[i].doorList.Add(doorList[i + 5]);
                    }
                }
            }
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            var room = roomList[i];
            room.Init(i);
        }
    }
}

[System.Serializable]
public class SpawnEnemDate
{
    public GameObject enemy;
    public int count = 1;
    public float skillColdPlus = 2f;
    public bool attackAble = true;
    public AiType attackAi = AiType.no;
    public bool skillAble = false;
    public AiType skillAi = AiType.no;
    public bool ultimateAble = false;
    public AiType ultimateAi = AiType.no;
    public bool itemAble = false;
    public AiType itemAi = AiType.no;
    public List<SkillName> addSkills;
}
