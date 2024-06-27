using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// スクリーンのタッチを受け入れて、移動向量に変える
/// </summary>
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static Joystick Instance;
    public JoystickSkill handle1;
    public JoystickSkill handle2;
    public JoystickSkill handle3;
    public JoystickSkill handle4;
    public JoystickSkill handle5;
    public RectTransform handle;

    [SerializeField] private RectTransform skillHandle;
    [SerializeField] private RectTransform background;

    private Vector2 input = Vector2.zero;
    private Vector2 touchMovePosition;
    private RectTransform baseRect;
    private Canvas canvas;


    public void Init()
    {
        //唯一初始化
        if (Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false);
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            background.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        skillHandle.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        skillHandle.gameObject.SetActive(false);
        OnPointerUp(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerManager.Instance.Hero == null || PlayerManager.Instance.Hero.anim.GetCurrentAnimatorStateInfo(0).IsName("getHit")) return;
        touchMovePosition = eventData.position;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        Instance.input = input;
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (PlayerManager.Instance.Hero == null || PlayerManager.Instance.Hero.anim.GetCurrentAnimatorStateInfo(0).IsName("getHit")) return;
        Vector2 radius = background.sizeDelta / 2;
        input = (eventData.position - touchMovePosition) / (radius * canvas.scaleFactor);
        Instance.input = input;
        HandleInput(input.magnitude, input.normalized);
        handle.anchoredPosition = input * radius;
    }




    private void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > 0)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;
    }

    private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, null, out localPoint))
        {
            return localPoint - (background.anchorMax * baseRect.sizeDelta);
        }
        return Vector2.zero;
    }
}