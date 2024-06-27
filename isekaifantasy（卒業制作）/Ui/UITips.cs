using UnityEngine.UI;
using UnityEngine;
using I2.Loc;

public class UITips : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject obj;

    public void Show(string str = null)
    {
        if (str != null) text.text = str;
        else text.text =
                LocalizationManager.GetTranslation("UI/UI1");
        if (!obj.activeSelf) obj.SetActive(true);
    }
    public void Hide()
    {
        if (obj != null && obj.activeSelf) obj.SetActive(false);
    }
}
