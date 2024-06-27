using UnityEngine.UI;
using UnityEngine;
using I2.Loc;

public class UISpawnShow : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject obj;

    public void Show()
    {
        if (AdModeManager.Instance.roomLvTotal == 0)
            return;
        if (AdModeManager.Instance.roomLvTotal == 4)
            return;
        if (AdModeManager.Instance.roomLvTotal == 8)
            return;
        if (AdModeManager.Instance.roomLvTotal == 12)
            return;
        if (AdModeManager.Instance.roomLvTotal == 20)
            return;
        if (AdModeManager.Instance.roomLvTotal == 16)
            return;
        if (AdModeManager.Instance.roomLvTotal == 23) text.text =
                LocalizationManager.GetTranslation("UI/UI4");
        else if (AdModeManager.Instance.roomLvTotal == 3 ||
                 AdModeManager.Instance.roomLvTotal == 7 ||
                 AdModeManager.Instance.roomLvTotal == 11 ||
                 AdModeManager.Instance.roomLvTotal == 15 ||
                 AdModeManager.Instance.roomLvTotal == 19)
            text.text = LocalizationManager.GetTranslation("UI/UI3");
        else text.text = LocalizationManager.GetTranslation("UI/UI2");
        if (!obj.activeSelf)
        {
            obj.SetActive(true);
        }
    }
    public void Hide()
    {
        if (obj != null && obj.activeSelf)
        {
            obj.SetActive(false);
        }
    }
}
