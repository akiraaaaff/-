using I2.Loc;
using UnityEngine;

public class LanguageChangeButton : MonoBehaviour
{
    public void SetChineseTraditional()
    {
        LocalizationManager.CurrentLanguage = "繁体字";
    }
    public void SetChineseSimplified()
    {
        LocalizationManager.CurrentLanguage = "国语";
    }
    public void SetJapanese()
    {
        LocalizationManager.CurrentLanguage = "日语";
    }
    public void SetFrench()
    {
        LocalizationManager.CurrentLanguage = "法语";
    }
    public void SetSpanish()
    {
        LocalizationManager.CurrentLanguage = "西班牙语";
    }
    public void SetVietnamese()
    {
        LocalizationManager.CurrentLanguage = "越南";
    }
    public void SetKorean()
    {
        LocalizationManager.CurrentLanguage = "韩语";
    }
    public void SetEnglish()
    {
        LocalizationManager.CurrentLanguage = "英语";
    }
    public void SetThai()
    {
        LocalizationManager.CurrentLanguage = "泰国";
    }
    public void SetRussian()
    {
        LocalizationManager.CurrentLanguage = "俄罗斯";
    }
    public void SetGerman()
    {
        LocalizationManager.CurrentLanguage = "德语";
    }
    public void SetHindi()
    {
        LocalizationManager.CurrentLanguage = "印度";
    }
}
