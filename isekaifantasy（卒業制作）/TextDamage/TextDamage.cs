using UnityEngine;
using UnityEngine.UI;

public class TextDamage : MonoBehaviour
{
    public CanvasGroup LayoutRoot = null;
    public Text m_Text = null;

    [HideInInspector] public Transform trs;
    [HideInInspector] public RectTransform Rect;
    [HideInInspector] public Animation ani;
    [HideInInspector] public Color m_Color;
    [HideInInspector] public TextDamageWay movement;
    [HideInInspector] public float Xcountervail;
    [HideInInspector] public float Ycountervail;
    [HideInInspector] public int m_Size;
    [HideInInspector] public float m_Speed;
    [HideInInspector] public string m_text;
    [HideInInspector] public Transform m_Transform;
    [HideInInspector] public float Yquickness;
    [HideInInspector] public float YquicknessScaleFactor;

    private void Awake()
    {
        trs = transform;
        Rect.GetComponent<RectTransform>();
        ani = GetComponent<Animation>();
    }
}