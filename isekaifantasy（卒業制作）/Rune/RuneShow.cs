using UnityEngine;
using UnityEngine.UI;

public class RuneShow : MonoBehaviour
{
    public RectTransform trs;
    [SerializeField] private Image m_Slot;
    [SerializeField] private GameObject m_SlotObj;
    [SerializeField] private Text m_CountText;
    [SerializeField] private Text m_MaxText;
    [SerializeField] private GameObject m_Overlay;
    public Runes rune;


    public void Init(Runes rune=null,bool isPreset=false,bool presetOn=false)
    {
        if (rune != null)
        {
            this.rune = rune;
        }
        if (this.m_CountText != null) this.m_CountText.text = this.rune.count.ToString();
        if (this.m_MaxText != null) this.m_MaxText.text = this.rune.max.ToString();
        if (this.m_Slot != null)
        {
            this.m_Slot.sprite = this.rune.image;
            m_SlotObj.SetActive(true);
            if(!presetOn&& isPreset) m_Overlay.SetActive(false);
            else m_Overlay.SetActive(true);
        }
    }

    public void Exit()
    {
        rune = null;
        this.m_SlotObj.SetActive(false);
        this.m_Overlay.SetActive(false);
        this.m_Slot.sprite = null;
        this.m_CountText.text = "0";
        this.m_MaxText.text = "0";
    }
}
