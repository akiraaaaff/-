using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RuneManager : MonoBehaviour
{
    public static readonly int StartRuneCount = 1;
    public static readonly int MiddleRuneCount = 3;
    public static readonly int LaterRuneCount = 5;
    //ファイルアクセス
    private string folderPath;
    private string filePath;

    public static RuneManager Instance;

    public RuneNpcList HeroRunes;
    public int gotRunesCount;

    public Dictionary<RuneName, Runes> dicRunes;
    public Dictionary<RuneName, int> RunesCount;
    public List<Runes> listRunesCanAdd;
    public Dictionary<RuneName, int> getRunesCount;

    public bool isFirstPlay;

    public void Init()
    {
        //唯一初始化
        if (Instance == null)
        {
            Instance = this;
            folderPath = Application.persistentDataPath + "/Rune";
        }
    }

    /// <summary>
    /// 選択したキャラクターによってデータを読み取り
    /// </summary>
    private void SetFilePath()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            isFirstPlay = true;
        }
        filePath = Path.Combine(folderPath, "Rune" + PlayerManager.Instance.Hero.unitName.ToString() + ".json");
    }

    //hero替えた時に初始化andリセットDicRune
    public void ResetDicRunesWhenHeroChange()
    {
        // 選択したキャラクターによってデータを読み取り
        SetFilePath();
        //読み取りプレイヤーデータ
        LoadPlayerDate();

        //初始化dicRunes
        CreatRunesList();
        //初始化Runes数量と削除MaxのRunes
        GetNpcRunesCount();
        //初始化dicRunesCanAdd
        InitDicRunesCanAdd();
        //初始化andリセットDicRunesCanAddInBattle
        UIManager.Instance.runeModal.runeInBattlePosManger.runeGotList.Clear();
    }

    /// <summary>
    /// プレイヤーデータの読み込み
    /// </summary>
    public void LoadPlayerDate()
    {
        //ファイルなかったら、新しく作る
        if (!File.Exists(filePath))
        {
            HeroRunes = new RuneNpcList();
            string saveData = JsonUtility.ToJson(HeroRunes, true);
            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(saveData);
            sw.Close();
        }
        else
        {
            //ファイル発見したら読み込み
            string dataAsJson = File.ReadAllText(filePath);       //读取所有数据送到json格式的字符串里面。
            HeroRunes = JsonUtility.FromJson<RuneNpcList>(dataAsJson);//直接赋值。FromJson
            //データ合わせ
            if (HeroRunes.StartRunes.Length != StartRuneCount ||
                HeroRunes.MiddleRunes.Length != MiddleRuneCount ||
                HeroRunes.LaterRunes.Length != LaterRuneCount)
            {
                var tempHeroRunes = new RuneNpcList();
                for (int i = 0; i < tempHeroRunes.StartRunes.Length; i++)
                    tempHeroRunes.StartRunes[i] = HeroRunes.StartRunes[i];
                for (int i = 0; i < tempHeroRunes.MiddleRunes.Length; i++)
                    tempHeroRunes.MiddleRunes[i] = HeroRunes.MiddleRunes[i];
                for (int i = 0; i < tempHeroRunes.LaterRunes.Length; i++)
                    tempHeroRunes.LaterRunes[i] = HeroRunes.LaterRunes[i];
                HeroRunes = tempHeroRunes;
                SavePlayerDate();
            }
        }
    }

    /// <summary>
    /// プレイヤーデータの保存
    /// </summary>
    public void SavePlayerDate()
    {
        //获取文件路径。
        //文件名，在Assets/persistentDataPath目录下，如：myData.json。

        File.WriteAllText(filePath, JsonUtility.ToJson(HeroRunes, true));
    }
    /// <summary>
    /// 初始化dicRunes
    /// </summary>
    public void CreatRunesList()
    {
        RunesList runesList = new RunesList();
        dicRunes = new Dictionary<RuneName, Runes>();
        runesList.Init(dicRunes);
    }

    /// <summary>
    /// 初始化Runes数量と削除MaxのRunes
    /// </summary>
    private void GetNpcRunesCount()
    {
        getRunesCount = new Dictionary<RuneName, int>();
        RunesCount = new Dictionary<RuneName, int>();

        // 初心者ボーナス
        if (isFirstPlay)
        {
            var runeName = RuneName.マジックアロー;
            for (int i = 0; i < StartRuneCount + MiddleRuneCount + LaterRuneCount; i++)
            {
                if (i == 0)
                    runeName = RuneName.マジックアロー;
                if (i == 1)
                    runeName = RuneName.深淵の邪眼;
                if (i == 2)
                    runeName = RuneName.攻撃吸血;
                if (i == 3)
                    runeName = RuneName.余震攻撃;
                if (i == 4)
                    runeName = RuneName.蘇生;
                if (i == 5)
                    runeName = RuneName.マジックアロー;
                if (i == 6)
                    runeName = RuneName.攻撃付魔;
                if (i == 7)
                    runeName = RuneName.氷月領域;
                if (i == 8)
                    runeName = RuneName.サンダーボルト;

                int count = 1;
                if (RunesCount.ContainsKey(runeName))
                {
                    count += RunesCount[runeName];
                    RunesCount[runeName] = count;
                }
                else RunesCount.Add(runeName, count);

                Runes rune = CreatRune(dicRunes[runeName]);
                rune.count = count;
                UIManager.Instance.runeModal.runeInBattlePosManger.PresetRuneInit(rune, i);
            }
        }
        // 通常配置
        else
        {
            RuneNpc[] heroRunes = null;
            gotRunesCount = 0;
            for (int ii = 0; ii < 3; ii++)
            {
                switch (ii)
                {
                    case 0:
                        heroRunes = HeroRunes.StartRunes;
                        break;
                    case 1:
                        heroRunes = HeroRunes.MiddleRunes;
                        break;
                    case 2:
                        heroRunes = HeroRunes.LaterRunes;
                        break;
                }

                for (int i = 0; i < heroRunes.Length; i++)
                {
                    if (heroRunes[i].name != 0)
                    {
                        int count = 1;
                        if (RunesCount.ContainsKey(heroRunes[i].name))
                        {
                            count += RunesCount[heroRunes[i].name];
                            RunesCount[heroRunes[i].name] = count;
                        }
                        else RunesCount.Add(heroRunes[i].name, count);

                        Runes rune = CreatRune(dicRunes[heroRunes[i].name]);
                        rune.count = count;
                        UIManager.Instance.runeModal.runeInBattlePosManger.PresetRuneInit(rune, gotRunesCount);
                    }
                    else
                        UIManager.Instance.runeModal.runeInBattlePosManger.PresetRuneInit(null, gotRunesCount);
                    gotRunesCount++;
                }
            }
        }
    }

    /// <summary>
    /// 初始化dicRunesCanAdd
    /// </summary>
    public void InitDicRunesCanAdd()
    {
        listRunesCanAdd = new List<Runes>();
        for (var i = 0; i < dicRunes.Count; i++)
        {
            var rune = dicRunes[(RuneName)i];
            if (RunesCount.ContainsKey(rune.name) && RunesCount[rune.name] >= rune.max)
                continue;
            listRunesCanAdd.Add(rune);
        }
    }

    /// <summary>
    /// ランダムにゲット符文
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    public Runes GetRuneByRandomInBattle()
    {
        var randomState = Random.state;
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));

        int i = Random.Range(0, listRunesCanAdd.Count);

        Random.state = randomState;

        Runes tempRune = CreatRune(listRunesCanAdd[i]);
        if (getRunesCount != null && getRunesCount.ContainsKey(tempRune.name))
        {
            tempRune.count += getRunesCount[tempRune.name];
        }

        listRunesCanAdd.Remove(listRunesCanAdd[i]);
        return tempRune;
    }

    private Runes CreatRune(Runes runeBace)
    {
        Runes newRune = new Runes();
        newRune.name = runeBace.name;
        newRune.image = runeBace.image;
        newRune.max = runeBace.max;
        newRune.count = runeBace.count;
        newRune.upPara = runeBace.upPara;
        newRune.upRank = runeBace.upRank;
        newRune.value = runeBace.value;
        newRune.add = runeBace.add;
        return newRune;
    }

    /// <summary>
    /// Add a rune in RuneManager.Instance.dicRunesCanAddInBattle
    /// </summary>
    /// <param name="rune"></param>
    public void AddRuneInDicRunesCanAdd(Runes rune)
    {
        if (RunesCount.ContainsKey(rune.name) &&
            RunesCount[rune.name] >= rune.max)
            return;
        listRunesCanAdd.Add(rune);
    }

    public void AdSaveHeroRune(RuneNpc rune, int index)
    {
        RuneNpc[] heroRunes = null;
        var HeroRunesIndex = 0;
        if (index == 0)
            heroRunes = HeroRunes.StartRunes;
        else if (index > 0 && index < 4)
        {
            heroRunes = HeroRunes.MiddleRunes;
            HeroRunesIndex = index - 1;
        }
        else if (index > 3)
        {
            heroRunes = HeroRunes.LaterRunes;
            HeroRunesIndex = index - 4;
        }

        heroRunes[HeroRunesIndex] = rune;
        SavePlayerDate();
    }
    public int GetSaveRuneIndex()
    {
        var groupIndex = 0;
        RuneNpc[] heroRunes = null;
        for (int ii = 0; ii < 3; ii++)
        {
            switch (ii)
            {
                case 0:
                    heroRunes = HeroRunes.StartRunes;
                    break;
                case 1:
                    groupIndex = 1;
                    heroRunes = HeroRunes.MiddleRunes;
                    break;
                case 2:
                    groupIndex = 4;
                    heroRunes = HeroRunes.LaterRunes;
                    break;
            }

            for (int i = 0; i < heroRunes.Length; i++)
            {
                if (heroRunes[i].name == 0)
                    return groupIndex + i;
            }
        }
        return 0;
    }
    public bool CheckRuneFull()
    {
        return gotRunesCount == (StartRuneCount +
            MiddleRuneCount + LaterRuneCount);
    }
}
