using UnityEngine;
using System.Collections.Generic;

public class TextDamageManager : MonoBehaviour
{
    /// <summary>
    /// The Canvas Root of scene.
    /// </summary>
    private Transform CanvasParent;
    static readonly float FadeSpeed = 150;
    static readonly float FloatingSpeed = 2;
    static readonly float HideDistance = 80;
    static readonly float MaxViewAngle = 180;

    //Privates
    private static List<TextDamage> texts = new List<TextDamage>();
    private Camera MCamera = null;

    public static TextDamageManager Instance;

    void Start()
    {
        Instance = this;
        CanvasParent = UIManager.Instance.transform;
    }

    Camera m_Cam
    {
        get
        {
            if (MCamera == null)
            {
                MCamera = (Camera.main != null) ? Camera.main : Camera.current;
            }

            return MCamera;
        }
    }


    void OnGUI()
    {
        if (m_Cam == null)
        {
            return;
        }

        if (Event.current.type == EventType.Repaint)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                //when target is destroyed then remove it from list.
                if (texts[i].m_Transform == null)
                {
                    //When player / Object death, destroy all last text.
                    if (texts[i] != null)
                    {
                        PoolManager.Instance.PushObj(texts[i].trs);
                    }

                    texts[i] = null;
                    texts.Remove(texts[i]);
                    return;
                }
                TextDamage temporal = texts[i];
                //fade text
                temporal.m_Color -= new Color(0f, 0f, 0f, (Time.deltaTime * FadeSpeed) / 100f);
                //if Text have more than a target graphic
                //add a canvas group in the root for fade all
                if (texts[i].LayoutRoot != null)
                {
                    texts[i].LayoutRoot.alpha = texts[i].m_Color.a;
                }
                //if complete fade remove and destroy text
                if (texts[i].m_Color.a <= 0f)
                {
                    if (texts[i] != null)
                    {
                        PoolManager.Instance.PushObj(texts[i].trs);
                        texts[i] = null;
                    }
                    texts.Remove(texts[i]);
                }
                else//if UI visible
                {
                    //Convert Word Position in screen position for UI
                    int mov = ScreenPosition(texts[i].m_Transform);

                    TextDamage m_Text = texts[i];
                    m_Text.Yquickness += Time.deltaTime * texts[i].YquicknessScaleFactor; ;
                    switch (texts[i].movement)
                    {
                        case TextDamageWay.Up:
                            m_Text.Ycountervail += Time.deltaTime * FloatingSpeed * mov * 0.15f * texts[i].Yquickness;
                            break;
                        case TextDamageWay.Left:
                            m_Text.Ycountervail += Time.deltaTime * FloatingSpeed * mov * 0.1f * texts[i].Yquickness;
                            m_Text.Xcountervail -= Time.deltaTime * FloatingSpeed * mov * 0.06f * texts[i].Yquickness;
                            break;
                        case TextDamageWay.Right:
                            m_Text.Ycountervail += Time.deltaTime * FloatingSpeed * mov * 0.1f * texts[i].Yquickness;
                            m_Text.Xcountervail += Time.deltaTime * FloatingSpeed * mov * 0.06f * texts[i].Yquickness;
                            break;
                        case TextDamageWay.LeftUp:
                            m_Text.Ycountervail += Time.deltaTime * FloatingSpeed * mov * 0.12f * texts[i].Yquickness;
                            m_Text.Xcountervail += Time.deltaTime * FloatingSpeed * mov * 0.03f * texts[i].Yquickness;
                            break;
                        case TextDamageWay.RightUp:
                            m_Text.Ycountervail += Time.deltaTime * FloatingSpeed * mov * 0.12f * texts[i].Yquickness;
                            m_Text.Xcountervail -= Time.deltaTime * FloatingSpeed * mov * 0.03f * texts[i].Yquickness;
                            break;
                        case TextDamageWay.Down:
                            m_Text.Ycountervail += Time.deltaTime * FloatingSpeed * mov * -0.15f * texts[i].Yquickness;
                            break;
                    }

                    //Get center up of target
                    Vector3 position = texts[i].m_Transform.position + (((Vector3.up) * 0.5f));
                    Vector3 front = position - MCamera.transform.position;
                    //its in camera view
                    if ((front.magnitude <= HideDistance) && (Vector3.Angle(MCamera.transform.forward, position - MCamera.transform.position) <= MaxViewAngle))
                    {
                        //Convert position to view port
                        Vector2 v = MCamera.WorldToViewportPoint(position);
                        //configure each text
                        texts[i].m_Text.fontSize = (mov / 2 * 1 + texts[i].m_Size) / 2;
                        texts[i].m_Text.text = texts[i].m_text;
                        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(texts[i].m_text));

                        //Calculate the movement 
                        Vector2 v2 = new Vector2((v.x - size.x * 0.5f) + texts[i].Xcountervail, -((v.y - size.y) - texts[i].Ycountervail));
                        //Apply to Text
                        texts[i].Rect.anchorMax = v;
                        texts[i].Rect.anchorMin = v;

                        texts[i].Rect.anchoredPosition = v2;
                        texts[i].m_Text.color = texts[i].m_Color;
                    }
                }
            }
        }
    }

    /// <summary>
    /// send a new event, to create a new floating text
    /// </summary>
    public void NewText(string text, Transform trans, Color color, int size, int way = -1)
    {
        Transform t = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.TextDamage);
        //Create new text info to instatiate 
        TextDamage item = t.GetComponent<TextDamage>();

        item.m_Speed = 2;
        item.m_Color = color;
        item.m_Transform = trans;
        item.m_text = text;
        item.m_Size = size;
        int tempWay = way;
        if (tempWay == -1)
            tempWay = Random.Range(0, 5);
        item.movement = (TextDamageWay)tempWay;
        item.Yquickness = 10;
        item.YquicknessScaleFactor = 1.5f;
        item.Xcountervail = 0;
        item.Ycountervail = 2f;

        t.SetParent(CanvasParent, false);
        t.SetSiblingIndex(0);
        item.Rect.anchoredPosition = Vector2.zero;
        item.Rect.localScale = Vector3.one;
        item.ani.Play();

        texts.Add(item);
    }

    private int ScreenPosition(Transform t)
    {
        int p = (int)(m_Cam.WorldToScreenPoint(t.position + (Vector3.up * 1f)).y - m_Cam.WorldToScreenPoint(t.position - (Vector3.up * 1f)).y);

        return p;
    }
}