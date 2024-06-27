using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using I2.Loc;

public class GameManager : MonoBehaviour
{
    public GameConf GameConf { get; private set; }
    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private RuneManager runeManager;
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private UIManager uiManager;
    public readonly float armorPara = 0.06f;
    public readonly float minMoveSpeed = 0.2f;
    public readonly float minAttackSpeed = 0.2f;
    public readonly float minSkillSpeed = 0.2f;
    public readonly float minUltimateSpeed = 0.2f;
    public readonly float minPassiveSpeed = 0.2f;
    public readonly float invokeLoopTime = 0.1f;
    public readonly float bulletLoopTime = 0.03f;
    public readonly float AiStopDis = 0.5f;
    public readonly float DoubleSkillWaitTime = 0.3f;
    public readonly float ColorRimSize = 0.3f;

    public readonly Color ColorDefult = new Color(1f, 1f, 1f, 1);
    public readonly Color ColorIsDie = new Color(0.6f, 0.4f, 0.4f, 1);
    public readonly Color ColorIsDieLv3 = new Color(0.3f, 0f, 0f, 1);
    public readonly Color ColorIsGangerLv1 = new Color(0f, 0.3f, 1f, 1);
    public readonly Color ColorIsGangerLv2 = new Color(0.3f, 0.8f, 1f, 1);
    public readonly Color ColorInGrass = new Color(0.5f, 0.5f, 0.5f, 1);
    public readonly Color ColorIsLock = new Color(0.2f, 0.2f, 0.2f, 1);
    //保存用データのゲーム内変数---------------------------------

    private string folderPath;
    private string filePath;

    //以上まで-----------------------------------------------


    //使いやすい変数
    public static GameManager Instance;
    public List<Collider> enemyList = new List<Collider>();
    public List<Collider> friendsList = new List<Collider>();
    public List<UnitBace> enemyBaceList = new List<UnitBace>();
    public List<UnitBace> friendsBaceList = new List<UnitBace>();
    public List<Collider> enemyBulletsList = new List<Collider>();
    public List<Collider> friendsBulletsList = new List<Collider>();
    public List<Collider> friendsCanAtkList = new List<Collider>();
    public List<Collider> canNotAtkList = new List<Collider>();
    public List<UnitBace> dieBaceList = new List<UnitBace>();


    public void AddNpcList(List<Collider> list, Collider col, UnitBace unit)
    {
        if (list == enemyList)
        {
            enemyList.Add(col);
            enemyBaceList.Add(unit);
        }
        else if (list == friendsList)
        {
            friendsList.Add(col);
            friendsBaceList.Add(unit);
        }
    }
    public void RemoveNpcList(List<Collider> list, Collider col, UnitBace unit)
    {
        if (list == enemyList)
        {
            enemyList.Remove(col);
            enemyBaceList.Remove(unit);
        }
        else if (list == friendsList)
        {
            friendsList.Remove(col);
            friendsBaceList.Remove(unit);
        }
        if (friendsCanAtkList.Contains(col))
            friendsCanAtkList.Remove(col);
        if (canNotAtkList.Contains(col))
            canNotAtkList.Remove(col);
    }
    public List<UnitBace> ChangeNpcList(List<Collider> colList)
    {
        List<UnitBace> baceList = enemyBaceList;
        if (colList == friendsList)
            baceList = friendsBaceList;
        return baceList;
    }
    public void ClearAllList()
    {
        foreach (var unit in enemyBaceList)
        {
            if (unit != null)
                Destroy(unit.obj);
        }
        foreach (var unit in friendsBaceList)
        {
            if (unit != null)
                Destroy(unit.obj);
        }
        foreach (var unit in dieBaceList)
        {
            if (unit != null)
                Destroy(unit.obj);
        }

        enemyList.Clear();
        friendsList.Clear();
        enemyBaceList.Clear();
        friendsBaceList.Clear();
        enemyBulletsList.Clear();
        friendsBulletsList.Clear();
        friendsCanAtkList.Clear();
        canNotAtkList.Clear();
        dieBaceList.Clear();
    }


    private void Awake()
    {
        //唯一初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);

            GameConf = Resources.Load<GameConf>("GameConf");

            uiManager.Init();
            playerManager.Init();
            runeManager.Init();
            audioManager.Init();
            LoadPlayerDate();
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void Start()
    {


        // 言語
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Japanese:
                LocalizationManager.CurrentLanguage = "日语";
                break;
            case SystemLanguage.English:
                LocalizationManager.CurrentLanguage = "英语";
                break;
            case SystemLanguage.Chinese:
                LocalizationManager.CurrentLanguage = "国语";
                break;
            case SystemLanguage.ChineseSimplified:
                LocalizationManager.CurrentLanguage = "国语";
                break;
            case SystemLanguage.ChineseTraditional:
                LocalizationManager.CurrentLanguage = "繁体字";
                break;
            case SystemLanguage.Korean:
                LocalizationManager.CurrentLanguage = "韩语";
                break;
            case SystemLanguage.Vietnamese:
                LocalizationManager.CurrentLanguage = "越南";
                break;
            case SystemLanguage.Spanish:
                LocalizationManager.CurrentLanguage = "西班牙语";
                break;
            case SystemLanguage.French:
                LocalizationManager.CurrentLanguage = "法语";
                break;
            case SystemLanguage.German:
                LocalizationManager.CurrentLanguage = "德语";
                break;
            case SystemLanguage.Russian:
                LocalizationManager.CurrentLanguage = "俄罗斯";
                break;
            case SystemLanguage.Thai:
                LocalizationManager.CurrentLanguage = "泰国";
                break;
            default:
                LocalizationManager.CurrentLanguage = "英语";
                break;
        }
    }

    /// <summary>
    /// プレイヤーデータの読み込み
    /// </summary>
    public void LoadPlayerDate()
    {
        folderPath = Application.persistentDataPath + "/Option";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        filePath = Path.Combine(folderPath, "Settings" + ".json");

        //ファイルなかったら、新しく作る
        if (!File.Exists(filePath))
        {
            var newSettings = new OptionSettings();

            /*
            //クオリティ
            int systemMemory = SystemInfo.systemMemorySize;
            int graphicsMemory = SystemInfo.graphicsMemorySize;
            if (systemMemory < 5000 || graphicsMemory < 2000)
            {
                newSettings.qualityLevel = 1;
            }
            else
            {
                newSettings.qualityLevel = 5;
            }
            */

            //ボリューム
            newSettings.bgmVolume = 1;
            newSettings.seVolume = 1;

            string saveData = JsonUtility.ToJson(newSettings, true);
            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(saveData);
            sw.Close();
        }

        //ファイル発見したら読み込み
        string dataAsJson = File.ReadAllText(filePath);       //读取所有数据送到json格式的字符串里面。
        var settings = JsonUtility.FromJson<OptionSettings>(dataAsJson);//直接赋值。FromJson

        /*
        //クオリティ
        UIManager.Instance.optionModal.qualityHeightToggele.onValueChanged.AddListener(UIManager.Instance.optionModal.SetHeightQuality);
        UIManager.Instance.optionModal.qualityLowToggele.onValueChanged.AddListener(UIManager.Instance.optionModal.SetLowQuality);
        if (settings.qualityLevel == 5)
        {
            UIManager.Instance.optionModal.qualityHeightToggele.isOn = true;
        }
        else
        {
            UIManager.Instance.optionModal.qualityLowToggele.isOn = true;
        }
        */

        //ボリューム
        UIManager.Instance.optionModal.bgmSlider.onValueChanged.AddListener(SaveBGM);
        UIManager.Instance.optionModal.seSlider.onValueChanged.AddListener(SaveSE);
        UIManager.Instance.optionModal.bgmSlider.value = settings.bgmVolume;
        UIManager.Instance.optionModal.seSlider.value = settings.seVolume;
        SaveBGM(settings.bgmVolume);
        SaveSE(settings.seVolume);
    }

    /// <summary>
    /// Qualityの保存
    /// </summary>
    public void SaveQuality(int lv)
    {
        /*
        if (QualitySettings.GetQualityLevel() == lv)
            return;
        QualitySettings.SetQualityLevel(lv, true);
        string dataAsJson = File.ReadAllText(filePath);
        var settings = JsonUtility.FromJson<OptionSettings>(dataAsJson);
        settings.qualityLevel = lv;
        File.WriteAllText(filePath, JsonUtility.ToJson(settings, true));
        */
    }
    /// <summary>
    /// BGMの保存
    /// </summary>
    public void SaveBGM(float volume)
    {
        AudioManager.Instance.SetBGM(volume);
        string dataAsJson = File.ReadAllText(filePath);
        var settings = JsonUtility.FromJson<OptionSettings>(dataAsJson);
        settings.bgmVolume = volume;
        File.WriteAllText(filePath, JsonUtility.ToJson(settings, true));
    }
    /// <summary>
    /// SEの保存
    /// </summary>
    public void SaveSE(float volume)
    {
        AudioManager.Instance.SetSE(volume);
        string dataAsJson = File.ReadAllText(filePath);
        var settings = JsonUtility.FromJson<OptionSettings>(dataAsJson);
        settings.seVolume = volume;
        File.WriteAllText(filePath, JsonUtility.ToJson(settings, true));
    }
    public int GetRoomAreaMaskIndex()
    {
        var index = -1;
        if (AdModeManager.Instance != null &&
            AdModeManager.Instance.QuestStatus == QuestStatus.battle &&
            AdModeManager.Instance.nowRoom != null&&
            AdFreeSceneManager.Instance == null)
            index = AdModeManager.Instance.nowRoom.index;
        var AreaIndex = 0;
        switch (index)
        {
            case 0:
                AreaIndex = 8;
                break;
            case 1:
                AreaIndex = 16;
                break;
            case 2:
                AreaIndex = 32;
                break;
            case 3:
                AreaIndex = 64;
                break;
            case 4:
                AreaIndex = 128;
                break;
            case 5:
                AreaIndex = 256;
                break;
            case 6:
                AreaIndex = 512;
                break;
            case 7:
                AreaIndex = 1024;
                break;
            case 8:
                AreaIndex = 2048;
                break;
            case 9:
                AreaIndex = 4096;
                break;
            case 10:
                AreaIndex = 8192;
                break;
            case 11:
                AreaIndex = 16384;
                break;
            default:
                return NavMesh.AllAreas;
        }
        return AreaIndex;
    }
    public int GetAreaMaskIndex()
    {
        int index = GetRoomAreaMaskIndex();
        if (index != NavMesh.AllAreas)
            index += 229380;
        return index;
    }

    public readonly List<NetHero> netHeroList = new List<NetHero>();
    public LoadNextScene loadNextScene;
}

public class OptionSettings
{
    public int qualityLevel;
    public float seVolume;
    public float bgmVolume;
}