using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの全て属性を記録する
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [HideInInspector]
    public List<UnitBace> unitBaceList;
    //[HideInInspector]
    public List<UnitBace> heroBaceList;
    [HideInInspector]
    public Dictionary<UnitName, GameObject> dicAllUnitName = new Dictionary<UnitName, GameObject>();

    [HideInInspector]
    public Dictionary<UnitName, SkillUnit> dicAllSkills;
    public List<RuneNpc> gotRuneInBattle = new List<RuneNpc>();
    [HideInInspector]
    public Skill callFriends;
    [HideInInspector]
    public GameObject friendPrafe1;
    [HideInInspector]
    public GameObject friendPrafe2;

    private List<UnitBace> enemyBaceList;
    private List<UnitBace> friendsBaceList;
    private UnitBace hero = null;
    [HideInInspector]
    public GameObject heroPrafe;

    //AiAuto------------------------------------------------------------------------------
    //[HideInInspector]
    public bool attackAuto = true;
    private bool attackAutoBefor;
    //[HideInInspector]
    public bool battleAuto = true;
    //[HideInInspector]
    public bool allAuto = true;
    public float isPlayerMove;
    private RoomBace nextRoom;
    public float noTouchTime;
    private bool isnoTouch;
    private bool isTransPort;
    //------------------------------------------------------------------------------




    /// <summary>
    /// メインキャラクター変換
    /// </summary>
    public UnitBace Hero
    {
        get => hero;
        set
        {
            hero = value;
            if (hero != null)
            {
                hero.Move += 100;
                hero.critAttack += 0.1f;
                hero.dodge += 0.1f;
                SetHeroPrafe(hero);
                hero.SetGroup(GameManager.Instance.friendsList);
                //リセットSkill
                if (LobbyManager.Instance == null)
                {
                    ResetSkillWhenHeroChange(hero, SkillAni.item);
                    hero.skills.item.useTimes = 4;
                    hero.skills.item.nowUseTimes = hero.skills.item.useTimes;
                    AddFirendsSkill();
                }
                //リセットDicRune
                RuneManager.Instance.ResetDicRunesWhenHeroChange();
                //リセットUI
                UIManager.Instance.buttonMaster.SetActive(true);
                UIManager.Instance.buttonAuto.SetActive(true);
                if (LobbyManager.Instance != null)
                    LobbyManager.Instance.gachaButton.SetActive(true);

                Joystick.Instance.gameObject.SetActive(true);
                Joystick.Instance.handle1.Init();
                Joystick.Instance.handle2.Init();
                Joystick.Instance.handle3.Init();
                Joystick.Instance.handle4.Init();
                Joystick.Instance.handle5.Init();
                InitPlayerBar(value);

                // 初心者ボーナス
                if (RuneManager.Instance.isFirstPlay)
                {
                    hero.attack += 50;
                    hero.AttackSpeed += 0.8f;
                    hero.hpMax += 200;
                    hero.Hp = hero.hpMax;
                    hero.mpMax += 500;
                    hero.Mp = hero.mpMax;
                    hero.restoreMp += 20;
                }
            }
            else
            {
                DestroyPlayerBarByPool();
                DestroyTargetBarByPool();
            }
        }
    }

    private UnitBace target = null;
    public UnitBace Target
    {
        get => target;
        set
        {
            if (value == null)
                DestroyTargetBarByPool();
            else if (target != value) InitTargetBar(value);
            if (Hero != null)
                Hero.Target = value;
            target = value;
        }
    }

    //一番近い敵検索
    float distance_min = 10000;

    //Playerに追加し、hpとmpを頭の上で表示する
    public NpcBarBase playerBar;

    //Targetに追加し、hpとmpを頭の上で表示する
    public NpcBarBase targetBar;



    public void Init()
    {
        //唯一初始化
        if (Instance == null)
        {
            Instance = this;
            //InitUnitAndSkill
            InitUnitInfo();
            enemyBaceList = GameManager.Instance.enemyBaceList;
            friendsBaceList = GameManager.Instance.friendsBaceList;
            callFriends = new Skill
            {
                skillAni = SkillAni.item,
                skillName = SkillName.仲間召喚,
                spell = SpellType.click,
                aiType = AiType.runAway,
                coolTime = 3f,
                useTimes = 2,
                nowUseTimes = 2,
                effList = new List<Transform>(),
            };
            callFriends.skill = CallFriends;
        }
    }
    public void SetHeroPrafe(UnitBace unit)
    {
        heroPrafe = dicAllUnitName[unit.unitName];
    }
    public void CallFriends(UnitBace my, UnitBace target, Skill skill, SkillPara skillPara)
    {
        AudioManager.Instance.PlayEFAudio(my.weaponAudio);
        for (int i = 0; i <= 1; i++)
        {
            GameObject firendObj = friendPrafe1;
            if (i == 1) firendObj = friendPrafe2;
            if (firendObj != null)
            {
                var haveBuff = false;
                var effPos = Vector3.zero;
                if (my.CheckBuff(GameManager.Instance.GameConf.runeBuff.RuneSacrifice.name))
                {
                    effPos = my.BuffDic["RuneSacrifice"].GetComponent<BuffRuneSacrifice>().position;
                    haveBuff = true;
                }
                Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.SpawnMagicShort);
                Vector3 position = my.trs.position + Random.insideUnitSphere * 3;
                position.y = 0;
                if (haveBuff)
                {
                    if (Vector3.Distance(position, effPos) > 1.8f)
                    {
                        float tempY = position.y;
                        Vector3 pos = effPos + Random.insideUnitSphere * 1.5f;
                        pos.y = tempY;
                        position = pos;
                    }
                }
                ts.position = position;

                ts = PoolManager.Instance.GetObj(firendObj);
                UnitBace friend = ts.GetComponent<UnitBace>();
                friend.Init();
                friend.nav.enabled = false;
                friend.trs.position = position;
                if (haveBuff)
                {
                    friend.trs.LookAt(effPos);
                    friend.trs.localEulerAngles = new Vector3(0, friend.trs.localEulerAngles.y, 0);
                }
                friend.SetGroup(GameManager.Instance.friendsList);
                friend.Owner = my;

                if (!haveBuff)
                    friend.ai.Init();
                DontDestroyOnLoad(friend);

                friend.swpanSkill = skill;
                if (skill.effList.Count >= 4)
                    skill.effList[0].GetComponent<UnitBace>().ToDeathUseEff();
                skill.effList.Add(ts);
            }
        }
    }
    public void SetFriendsPrafe(UnitBace unit, int friendsNum)
    {
        GameObject friend = dicAllUnitName[unit.unitName];
        if (friendsNum == 0)
        {
            friendPrafe1 = friend;
        }
        else if (friendsNum == 1)
        {
            friendPrafe2 = friend;
        }
    }
    public int GetFriendsRandom(int lastNum = -1)
    {
        var randomState = Random.state;
        Random.InitState(Random.Range(int.MinValue,int.MaxValue));

        int num = Random.Range(0, LobbyManager.Instance.friendsGroup.childCount);
        if (lastNum != -1 && num == lastNum)
        {
            num = Random.Range(0, lastNum);
            if (num == lastNum)
            {
                num = Random.Range(lastNum + 1, LobbyManager.Instance.friendsGroup.childCount);
            }
        }
        LobbyManager.Instance.friendsGroup.GetChild(num).GetComponent<UnitChoise>().ChoseThis();

        Random.state = randomState;
        return num;
    }

    /// <summary>
    /// InitUnit
    /// </summary>
    public void InitUnitInfo()
    {
        dicAllSkills = new Dictionary<UnitName, SkillUnit>();
        //キャラクター
        foreach (GameObject obj in GameManager.Instance.GameConf.unitList)
        {
            if (obj != null)
            {
                UnitBace npcBace = obj.GetComponent<UnitBace>();
                NewSkillsDataInDec(npcBace);
                npcBace.InitBefor();
                if (npcBace.isUnit) unitBaceList.Add(npcBace);
                if (npcBace.isHero) heroBaceList.Add(npcBace);
                if (npcBace.unitName != UnitName.no) dicAllUnitName.Add(npcBace.unitName, npcBace.obj);
            }
        }
    }

    /// <summary>
    /// ランダムスキルのセット
    /// </summary>
    public void ResetSkillWhenHeroChange(UnitBace unit, SkillAni skillPos, int index = -1, int ranSkillType = 0)
    {
        if (ranSkillType == 0)
            ranSkillType = Random.Range(1, 3);
        int ranUnit = Random.Range(0, heroBaceList.Count);
        //ranSkillType = 1;
        //ranUnit = 0;
        switch (ranSkillType)
        {
            case 1:
                unit.ChangSkill(dicAllSkills[heroBaceList[ranUnit].unitName].skill, skillPos, index: index);
                break;
            case 2:
                unit.ChangSkill(dicAllSkills[heroBaceList[ranUnit].unitName].ultimate, skillPos, index: index);
                break;
        }
        //Hero.ChangSkill(dicAllSkills[NpcName.RookieNight].attack, SkillType.passive);
        //Hero.ChangSkill(dicAllSkills[NpcName.RookieNight].attack, SkillType.attack);
        //Hero.ChangSkill(dicAllSkills[NpcName.RookieNight].attack, SkillType.skill);
        //Hero.ChangSkill(dicAllSkills[NpcName.RookieNight].attack, SkillType.ultimate);
    }
    private void AddFirendsSkill()
    {
        if (friendPrafe1 != null || friendPrafe2 != null)
            Hero.ChangSkill(callFriends, SkillAni.item, true);
    }
    private void NewSkillsDataInDec(UnitBace npcBace)
    {
        SkillUnit skills = new SkillUnit();
        npcBace.InitSkill(skills);
        dicAllSkills.Add(npcBace.unitName, skills);
    }

    //Player、hpとmp表示する
    public void InitPlayerBar(UnitBace tar)
    {
        if (playerBar == null)
        {
            var barTrs = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.NpcBar);
            DontDestroyOnLoad(barTrs);
            playerBar = barTrs.GetComponent<NpcBarBase>();
        }
        playerBar.InitTarget(tar);
    }

    //Target、hpとmp表示する
    private void InitTargetBar(UnitBace tar)
    {
        if (targetBar == null)
            targetBar = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.NpcBar).GetComponent<NpcBarBase>();
        targetBar.InitTarget(tar);
    }

    /// <summary>
    /// 一番近い敵検索
    /// </summary>
    private void FindTarget()
    {
        Target = SetTargetForUnit(Hero);
    }
    private void SetTargetForEnemy()
    {
        if (enemyBaceList != null && enemyBaceList.Count > 0)
        {
            for (int i = 0; i < enemyBaceList.Count; i++)
            {
                if (enemyBaceList[i].Target == null || enemyBaceList[i].Target.Hp <= 0 ||
                    enemyBaceList[i].Target.isInsideGrass ||
                    !enemyBaceList[i].enemyGroup.Contains(enemyBaceList[i].Target.col) ||
                    GameManager.Instance.canNotAtkList.Contains(enemyBaceList[i].Target.col))
                    enemyBaceList[i].Target = SetTargetForUnit(enemyBaceList[i]);
            }
        }
    }
    private void SetTargetForFriendly()
    {
        if (friendsBaceList != null && friendsBaceList.Count > 1)
        {
            for (int i = 0; i < friendsBaceList.Count; i++)
            {
                if (friendsBaceList[i].Target == null || friendsBaceList[i].Target.Hp <= 0 ||
                    friendsBaceList[i].Target.isInsideGrass ||
                    !friendsBaceList[i].enemyGroup.Contains(friendsBaceList[i].Target.col) ||
                    GameManager.Instance.canNotAtkList.Contains(friendsBaceList[i].Target.col))
                {
                    if (friendsBaceList[i] != Hero)
                        friendsBaceList[i].Target = SetTargetForUnit(friendsBaceList[i]);
                }
            }
        }
    }
    public UnitBace SetTargetForUnit(UnitBace unit)
    {
        if (Hero == null) return null;
        UnitBace target = null;
        if (unit == Hero)
        {
            if (enemyBaceList != null && enemyBaceList.Count > 0)
            {
                for (int i = 0; i < enemyBaceList.Count; i++)
                {
                    if (!GameManager.Instance.canNotAtkList.Contains(enemyBaceList[i].col))
                    {
                        float distance = Vector3.Distance(unit.trs.position, enemyBaceList[i].trs.position);
                        if (distance < distance_min)
                        {
                            distance_min = distance;
                            target = GameManager.Instance.enemyBaceList[i];
                        }
                    }
                }
                distance_min = 10000;
            }
        }
        else if (enemyBaceList.Contains(unit))
        {
            if (!Hero.isInsideGrass && Hero.Hp > 0 && Hero != unit && !GameManager.Instance.canNotAtkList.Contains(Hero.col))
                target = Hero;
            else
            {
                if (friendsBaceList != null && friendsBaceList.Count > 1)
                {
                    for (int i = 0; i < friendsBaceList.Count; i++)
                    {
                        if (!friendsBaceList[i].isInsideGrass && friendsBaceList[i].Hp > 0 &&
                            !GameManager.Instance.canNotAtkList.Contains(friendsBaceList[i].col))
                        {
                            target = friendsBaceList[i];
                            break;
                        }
                    }
                }
            }
        }
        else if (friendsBaceList.Contains(unit))
        {
            if (Target != null && !Target.isInsideGrass && Target.Hp > 0 && Target != unit && !GameManager.Instance.canNotAtkList.Contains(Target.col))
                target = Target;
            else
            {
                if (enemyBaceList != null && enemyBaceList.Count > 1)
                {
                    for (int i = 0; i < enemyBaceList.Count; i++)
                    {
                        if (!enemyBaceList[i].isInsideGrass && enemyBaceList[i].Hp > 0 &&
                            !GameManager.Instance.canNotAtkList.Contains(enemyBaceList[i].col))
                        {
                            target = enemyBaceList[i];
                            break;
                        }
                    }
                }
            }
        }
        return target;
    }
    public void FriendsFllowHero()
    {
        if (friendsBaceList != null && friendsBaceList.Count > 1)
        {
            foreach (UnitBace friends in friendsBaceList)
            {
                if (friends != Hero)
                {
                    if (friends.isTower)
                        continue;

                    if (friends.Owner == null)
                    {
                        friends.Owner = Hero;
                        friends.isOwnerDead = true;
                    }
                    friends.nav.enabled = false;
                    friends.nav.areaMask = GameManager.Instance.GetAreaMaskIndex();
                    friends.trs.position = friends.Owner.trs.position;
                    friends.ai.backPos = friends.trs.position;
                    friends.nav.enabled = true;
                }
            }
        }
    }
    public void FriendsSetNavAllArea()
    {
        if (friendsBaceList != null && friendsBaceList.Count > 0)
        {
            foreach (UnitBace friends in friendsBaceList)
                friends.nav.areaMask = NavMesh.AllAreas;
        }
    }
    //AiAuto------------------------------------------------------------------------------
    private RoomBace GetRoomRandom()
    {
        RoomBace room = null;
        int num = Random.Range(0, AdModeManager.Instance.nowRoom.doorList.Count);
        if (AdModeManager.Instance.nowRoom != AdModeManager.Instance.nowRoom.doorList[num].room1)
            room = AdModeManager.Instance.nowRoom.doorList[num].room1;
        else room = AdModeManager.Instance.nowRoom.doorList[num].room2;
        return room;
    }
    private void GetHeroRandom()
    {
        int num = Random.Range(0, LobbyManager.Instance.HeroGroup.childCount);
        LobbyManager.Instance.HeroGroup.GetChild(num).GetComponent<UnitChoise>().ChoseThis();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            noTouchTime = 0;
            if (isnoTouch)
            {
                if (!IsBotton())
                    SetNoTouch(false);
                else isnoTouch = false;
            }
        }
        noTouchTime += Time.unscaledDeltaTime;
        if (!battleAuto && noTouchTime >= 30)
        {
            SetNoTouch(true);
        }
    }
    private void SetNoTouch(bool isOn)
    {
        isnoTouch = isOn;
        battleAuto = isnoTouch;
        allAuto = isnoTouch;
        UIManager.Instance.aiAutoButton.CheckImageBattle.SetActive(isnoTouch);
        UIManager.Instance.aiAutoButton.CheckImageAll.SetActive(isnoTouch);
        if (isOn)
        {
            attackAutoBefor = attackAuto;
        }
        else
        {
            isOn = attackAutoBefor;
            attackAutoBefor = false;
        }
        attackAuto = isOn;
        UIManager.Instance.aiAutoButton.CheckImageAttack.SetActive(isOn);
    }

    private bool IsBotton()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.pressPosition = Input.mousePosition;
        data.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        //GraphicRaycaster只会响应UI
        GraphicRaycaster raycaster = FindObjectOfType<GraphicRaycaster>();
        raycaster.Raycast(data, results);
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "AttackAuto" ||
                    result.gameObject.name == "BattleAuto" ||
                    result.gameObject.name == "AllAuto")
                {
                    return true;
                }
            }
        }
        return false;
    }
    //------------------------------------------------------------------------------
    private void FixedUpdate()
    {
        if (Hero == null)
        {
            //AiAuto------------------------------------------------------------------------------
            if (LobbyManager.Instance != null && allAuto)
            {
                GetHeroRandom();
                int lastNum = GetFriendsRandom();
                GetFriendsRandom(lastNum);
            }
            //------------------------------------------------------------------------------
            return;
        }

        FindTarget();
        SetTargetForEnemy();
        SetTargetForFriendly();
        //AiAuto------------------------------------------------------------------------------
        if (LobbyManager.Instance != null && allAuto && (LobbyManager.Instance.friend1 == null || LobbyManager.Instance.friend2 == null))
        {
            if (LobbyManager.Instance.friend1 == null && LobbyManager.Instance.friend2 == null)
            {
                int lastNum = GetFriendsRandom();
                GetFriendsRandom(lastNum);
            }
            else
                GetFriendsRandom();
        }
        if (allAuto || (attackAuto &&
            AdModeManager.Instance != null &&
            !Hero.inPool))
        {
            if (Hero.ai.my == null) Hero.ai.Init();
            else
            {
                if (Hero.ai.onlyAttack)
                {
                    if (battleAuto || allAuto) Hero.ai.onlyAttack = false;
                }
                else if (!battleAuto && !allAuto) Hero.ai.SetOnlyAttack();
            }
            if (AdModeManager.Instance != null)
            {
                if (AdModeManager.Instance.QuestStatus == QuestStatus.next)
                {
                    if (AdSceneManager.Instance.transPortObj.activeSelf)
                    {
                        Hero.ai.backPos = AdSceneManager.Instance.transPort.position;
                        isTransPort = true;
                    }
                    else
                    {
                        if (!isTransPort)
                        {
                            if (nextRoom == null) nextRoom = GetRoomRandom();
                            Hero.ai.backPos = nextRoom.trs.position;
                        }
                        else if (!AdSceneManager.Instance.transPortObj.activeSelf)
                            Hero.ai.backPos = Hero.trs.position;
                    }
                }
                else
                {
                    isTransPort = false;
                    Hero.ai.backPos = AdModeManager.Instance.nowRoom.trs.position;
                    if (nextRoom != null) nextRoom = null;
                }
            }
            else if (LobbyManager.Instance != null)
            {
                if (allAuto)
                {
                    Hero.ai.backPos = LobbyManager.Instance.transPort.position;
                }
            }
        }
        else
        {
            if (Hero.ai.my != null) Hero.ai.ExitAi();
        }
        if (isPlayerMove > 0f)
        {
            isPlayerMove -= Time.deltaTime;
            if (isPlayerMove < 0f) isPlayerMove = 0f;
        }
        //------------------------------------------------------------------------------
        if (Hero.anim != null && Joystick.Instance.isActiveAndEnabled && !Hero.anim.GetBool("getHit")) PlayerMove();
    }

    /// <summary>
    /// プレイヤー操作によるキャラクターの移動
    /// </summary>
    private void PlayerMove()
    {
        if (Joystick.Instance.handle.localPosition.x != 0 || Joystick.Instance.handle.localPosition.y != 0)
        {
            Hero.PlayAnime("run", true, true);
            Vector3 targetPos = new Vector3(Joystick.Instance.handle.localPosition.x, transform.position.y, Joystick.Instance.handle.localPosition.y);
            Hero.trs.position = Vector3.MoveTowards(Hero.trs.position, targetPos, Hero.Move / 100 * Time.deltaTime * Hero.anim.GetFloat("moveSpeed"));
            isPlayerMove = 0.1f;
            if (Hero.isSkilling && Hero.runningSkill.isLookAt && target != null)
                return;
            Hero.trs.LookAt(targetPos);
        }
        else
        {
            if (Hero.ai.my == null || isPlayerMove > 0) Hero.PlayAnime("run", true, false);
            if (Hero.ai.my != null && isPlayerMove > 0)
            {
                isPlayerMove = 0;
                if (Hero.Hp > 0)
                    Hero.ai.Run();
            }
        }
    }

    /// <summary>
    /// 不表示時にPlayerBarを消す
    /// </summary>
    public void DestroyPlayerBarByPool()
    {
        if (playerBar == null) return;
        playerBar.DestroyNpcBarByPool();
        playerBar = null;
    }

    /// <summary>
    /// 不表示時にTargetBarを消す
    /// </summary>
    public void DestroyTargetBarByPool()
    {
        if (targetBar == null) return;
        targetBar.DestroyNpcBarByPool();
        targetBar = null;
    }
}
