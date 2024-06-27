using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// クリックをチェックして、相応のスキルを発動させ
/// 押し込みをチェックして、相応のスキルを発動させ
/// ドラッグをチェックして、相応のスキルを発動させ
/// </summary>
public class JoystickSkill : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Image image;
    public Image imageUnAble;
    public Image imageCool;
    public Image imageCharge;
    public Image imageKeep;
    public Image imageSelect;
    public Image imageTimes;
    public Text textTimes;
    public Animator anim;
    [HideInInspector]
    public Skill skill = null;
    public bool isAssDir;
    bool disAble;

    public void Init()
    {
        switch (name)
        {
            case "Handle (1)":
                if (PlayerManager.Instance.Hero.skills.attack != null && PlayerManager.Instance.Hero.skills.attack.skillName != SkillName.no)
                    skill = PlayerManager.Instance.Hero.skills.attack;
                else skill = null;
                break;
            case "Handle (2)":
                if (PlayerManager.Instance.Hero.skills.skill != null && PlayerManager.Instance.Hero.skills.skill.skillName != SkillName.no)
                    skill = PlayerManager.Instance.Hero.skills.skill;
                else skill = null;
                break;
            case "Handle (3)":
                if (PlayerManager.Instance.Hero.skills.ultimate != null && PlayerManager.Instance.Hero.skills.ultimate.skillName != SkillName.no)
                    skill = PlayerManager.Instance.Hero.skills.ultimate;
                else skill = null;
                break;
            case "Handle (4)":
                if (PlayerManager.Instance.Hero.skills.item != null && PlayerManager.Instance.Hero.skills.item.skillName != SkillName.no)
                    skill = PlayerManager.Instance.Hero.skills.item;
                else skill = null;
                break;
            case "Handle (5)":
                if (PlayerManager.Instance.Hero.skills.itemList != null && PlayerManager.Instance.Hero.skills.itemList.Count > 0)
                    skill = PlayerManager.Instance.Hero.skills.itemList[0];
                else skill = null;
                if (LobbyManager.Instance != null) skill = null;
                break;
        }

        if (skill == null || skill.skillName == SkillName.no)
        {
            imageUnAble.enabled = true;
            return;
        }
        if (PlayerManager.Instance.Hero.Mp < skill.cost) imageUnAble.enabled = true;
        if (skill.coolTime == 0) imageCool.fillAmount = 0;
        if (skill.useTimes != 0)
        {
            imageTimes.enabled = true;
            textTimes.enabled = true;
            image.raycastTarget = true;
        }
        else
        {
            imageTimes.enabled = false;
            textTimes.enabled = false;
        }
        if (skill.spell == SpellType.select) imageSelect.enabled = true;
    }

    private void OnDisable()
    {
        if (skill == null) return;
        OnPointerUp(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (imageUnAble.enabled) return;
        if (skill == null ||
            PlayerManager.Instance.Hero.anim.GetCurrentAnimatorStateInfo(0).IsName("gethit") ||
            disAble) return;
        if (!imageSelect.enabled) imageSelect.enabled = true;
        if (skill.canMove && eventData != null) Joystick.Instance.OnPointerDown(eventData);
        skill.isKeeping = true;
        PlayerManager.Instance.Hero.isKeepingSkill = true;
        skill.nowKeepTime = 0;
        if (skill.spell == SpellType.click) return;
        isAssDir = false;
        Spelld();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (skill == null || !skill.isKeeping) return;
        if (skill.spell == SpellType.select)
        {
            if (imageSelect.enabled) imageSelect.enabled = false;
            else imageSelect.enabled = true;
            return;
        }
        if (imageSelect.enabled) imageSelect.enabled = false;
        if (skill.canMove) Joystick.Instance.OnPointerUp(eventData);
        if (!skill.isKeeping) return;
        if (skill.spell == SpellType.click) Spelld();
        Cold();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill == null || !skill.isKeeping) return;
        if (skill.canMove)
        {
            //CheckMove
            isAssDir = true;
            Joystick.Instance.OnDrag(eventData);
        }
    }

    public void ReSet()
    {
        if (skill == null) return;
        imageKeep.fillAmount = 0;
        anim.SetBool("full", false);
        if (skill.spell != SpellType.select) imageSelect.enabled = false;
    }

    private void FixedUpdate()
    {
        if (skill == null || PlayerManager.Instance.Hero == null) return;
        //cold
        if (skill.coolTime > 0 || skill.nowCoolTime > 0 || imageCool.fillAmount > 0) imageCool.fillAmount = skill.nowCoolTime / skill.coolTime;
        //charge
        if (skill.chargePara > 0 || skill.nowChargePara > 0 || imageCharge.fillAmount > 0)
        {
            if (skill.nowChargePara == 0)
            {
                imageCharge.fillAmount = 0;
                if (anim.GetBool("full")) anim.SetBool("full", false);
            }
            else
            {
                imageCharge.fillAmount = skill.nowChargePara / skill.chargePara;
                if (skill.nowChargePara == skill.chargePara) anim.SetBool("full", true);
                else anim.SetBool("full", false);
            }
        }
        //keep
        if (skill.isKeeping && (skill.keepTime > 0 || skill.nowKeepTime > 0 || imageKeep.fillAmount > 0))
        {
            if (imageUnAble.enabled) OnPointerUp(null);
            else imageKeep.fillAmount = skill.nowKeepTime / skill.keepTime;
            if (skill.nowKeepTime >= skill.keepTime)
            {
                anim.SetBool("full", true);
                if (skill.spell == SpellType.loop) OnPointerUp(null);
            }
        }
        //Times
        if (skill.useTimes != 0) textTimes.text = skill.nowUseTimes.ToString();
        //Last
        disAble = skill.disAble;
        if (skill.coolTime == 0 && skill.spell == SpellType.loop &&
            PlayerManager.Instance.Hero.Mp > skill.cost)
            disAble = false;

        imageUnAble.enabled = disAble;
        image.raycastTarget = !disAble;
    }

    private void Cold()
    {
        imageKeep.fillAmount = 0;
        if (anim.GetBool("full")) anim.SetBool("full", false);
        PlayerManager.Instance.Hero.Cold(skill);
    }
    private void Spelld()
    {
        PlayerManager.Instance.Hero.Spelld(skill, isAssDir);
        isAssDir = false;
    }
}
