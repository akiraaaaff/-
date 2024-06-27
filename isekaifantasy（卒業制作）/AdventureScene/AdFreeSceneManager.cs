using System.Collections;
using UnityEngine;

public class AdFreeSceneManager : AdSceneManager
{
    public static new AdFreeSceneManager Instance;
    [SerializeField]
    private LoadNextScene loadNextScene;


    public override void DoorSetBattle(RoomBace room)
    {
        AdModeManager.Instance.nowRoom = room;
    }
    public override void OnBattle()
    {
        AdModeManager.Instance.roomLv++;
        AdModeManager.Instance.roomLvTotal++;
        PlayerManager.Instance.Hero.nav.areaMask = GameManager.Instance.GetAreaMaskIndex();
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
    public override void OnNext()
    {
        AdModeManager.Instance.clearRoom = AdModeManager.Instance.nowRoom;
        AdModeManager.Instance.nowRoom.DisableItems();
        AdModeManager.Instance.questNext?.Invoke();
        if (AdModeManager.Instance.roomLvTotal == 24)
            AdModeManager.Instance.GameOver(true);
        else
        {
            if (AdModeManager.Instance.roomLvTotal == 8 ||
                AdModeManager.Instance.roomLvTotal == 16)
                ViewTransPort();
            else
                ToBattle();
            if (AdModeManager.Instance.roomLv == 4)
            {
                AdModeManager.Instance.roomLv = 0;
                AdModeManager.Instance.RuneAdd();
            }
        }
    }

    protected override void EnemyInit(UnitBace enemy, SpawnEnemDate enemyData)
    {
        base.EnemyInit(enemy, enemyData);
        StopCoroutine(animeIEnumerator);
        enemy.SetGroup(GameManager.Instance.enemyList);
        if (enemy.Target != null)
            enemy.LookAtPos(enemy.Target.trs.position);
        enemy.nav.enabled = true;
        enemy.ai.Init(enemyData);
        var effTrs = lastEffTrs;
        StartCoroutine(SetEffIE(effTrs, enemy));
    }
    private IEnumerator SetEffIE(Transform trs, UnitBace enemy)
    {
        yield return new WaitForSeconds(0);
        trs.position = enemy.trs.position;
    }

    /// <summary>
    /// インスタンス自身
    /// </summary>
    protected override void Start()
    {
        if (Instance != null) return;

        Instance = this;
        base.Start();
        camFollow.InMap();
        effPfb = GameManager.Instance.GameConf.effects.SpawnMagicShort;
    }

    private void ToBattle()
    {
        AdModeManager.Instance.QuestStatus = QuestStatus.battle;
    }

    public new void ViewTransPort()
    {
        transPort.position = PlayerManager.Instance.Hero.trs.position;
        transPortObj.SetActive(true);
        Invoke(nameof(Trans), 1f);
    }
    private void Trans()
    {
        if (transPortObj.activeSelf)
            loadNextScene.Trans();
    }
}
