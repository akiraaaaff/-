using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
/// <summary>
/// どこにも追加しない、全ての符文情報を記録する
/// </summary>
public class RunesList
{
    public static readonly float skillColdPara = 0.3f;
    public static readonly float armorBreakdPara = 0.3f;

    public void Init(Dictionary<RuneName, Runes> dicRunes)
    {
        attack.add = Attack;
        dicRunes.Add(attack.name, attack);

        magic.add = Magic;
        dicRunes.Add(magic.name, magic);

        hp.add = Hp;
        dicRunes.Add(hp.name, hp);

        mp.add = Mp;
        dicRunes.Add(mp.name, mp);

        attackSpeed.add = AttackSpeed;
        dicRunes.Add(attackSpeed.name, attackSpeed);

        armor.add = Armor;
        dicRunes.Add(armor.name, armor);

        restoreHp.add = RestoreHp;
        dicRunes.Add(restoreHp.name, restoreHp);

        critAttack.add = CritAttack;
        dicRunes.Add(critAttack.name, critAttack);

        critDamage.add = CritDamage;
        dicRunes.Add(critDamage.name, critDamage);

        dodge.add = Dodge;
        dicRunes.Add(dodge.name, dodge);

        moveSpeed.add = MoveSpeed;
        dicRunes.Add(moveSpeed.name, moveSpeed);

        restoreMp.add = RestoreMp;
        dicRunes.Add(restoreMp.name, restoreMp);

        coolDown.add = CoolDown;
        dicRunes.Add(coolDown.name, coolDown);

        skillSpeed.add = SkillSpeed;
        dicRunes.Add(skillSpeed.name, skillSpeed);

        armorBreak.add = ArmorBreak;
        dicRunes.Add(armorBreak.name, armorBreak);

        armorThorns.add = ArmorThorns;
        dicRunes.Add(armorThorns.name, armorThorns);

        attackSuck.add = AttackSuck;
        dicRunes.Add(attackSuck.name, attackSuck);

        magicSuck.add = MagicSuck;
        dicRunes.Add(magicSuck.name, magicSuck);
        
        deathControl.add = DeathControl;
        dicRunes.Add(deathControl.name, deathControl);
        
        doppelganger.add = Doppelganger;
        dicRunes.Add(doppelganger.name, doppelganger);
        
        manaAura.add = ManaAura;
        dicRunes.Add(manaAura.name, manaAura);
        
        firendsTimes.add = FirendsTimes;
        dicRunes.Add(firendsTimes.name, firendsTimes);
        
        itemTimes.add = ItemTimes;
        dicRunes.Add(itemTimes.name, itemTimes);

        itemReset.add = ItemReset;
        dicRunes.Add(itemReset.name, itemReset);

        manaMax.add = ManaMax;
        dicRunes.Add(manaMax.name, manaMax);

        battleRhythm.add = BattleRhythm;
        dicRunes.Add(battleRhythm.name, battleRhythm);

        berserker.add = Berserker;
        dicRunes.Add(berserker.name, berserker);

        alert.add = Alert;
        dicRunes.Add(alert.name, alert);
        
        attackAftershock.add = AttackAftershock;
        dicRunes.Add(attackAftershock.name, attackAftershock);
        
        magicArrow.add = MagicArrow;
        dicRunes.Add(magicArrow.name, magicArrow);

        eyesAbyss.add = EyesAbyss;
        dicRunes.Add(eyesAbyss.name, eyesAbyss);

        revival.add = Revival;
        dicRunes.Add(revival.name, revival);

        doubleMagic.add = DoubleMagic;
        dicRunes.Add(doubleMagic.name, doubleMagic);

        sacrifice.add = Sacrifice;
        dicRunes.Add(sacrifice.name, sacrifice);

        attackCoolDown.add = AttackCoolDown;
        dicRunes.Add(attackCoolDown.name, attackCoolDown);
        
        attackEnchant.add = AttackEnchant;
        dicRunes.Add(attackEnchant.name, attackEnchant);
        
        iceArea.add = IceArea;
        dicRunes.Add(iceArea.name, iceArea);

        thunderbolt.add = Thunderbolt;
        dicRunes.Add(thunderbolt.name, thunderbolt);
    }
    public int ValueCheckInt(Runes runes, bool add, int count)
    {
        if (count == 0) return 0;
        if (count > runes.max)
            count = runes.max;
        int value = (int)runes.value;
        if (runes.upRank != 0 & count >= runes.upRank) value *= (int)runes.upPara;
        if (!add) value = -value;
        return value;
    }
    public float ValueCheckFloat(Runes runes, bool add, int count)
    {
        if (count == 0) return 0;
        if (count > runes.max)
            count = runes.max;
        float value = runes.value;
        if (runes.upRank != 0 && count >= runes.upRank) value *= runes.upPara;
        if (!add) value = -value;
        return value;
    }
    //攻撃--------------------------------------------------------------------------------------------
    public Runes attack = new Runes
    {
        name = RuneName.攻撃力,//1
        image = GameManager.Instance.GameConf.imageRunes.attack,//1
        max = 8,
        upRank = 5,
        upPara = 2f,
        value = 20f,
    };
    public void Attack(UnitBace npc, bool add, int count)
    {
        npc.attack += ValueCheckInt(attack, add, count);
    }
    //魔力--------------------------------------------------------------------------------------------
    public Runes magic = new Runes
    {
        name = RuneName.魔力,//1
        image = GameManager.Instance.GameConf.imageRunes.magic,//1
        max = 4,
        upRank = 4,
        upPara = 2f,
        value = 100f,
    };
    public void Magic(UnitBace npc, bool add, int count)
    {
        npc.magic += ValueCheckInt(magic, add, count);
    }
    //HP--------------------------------------------------------------------------------------------
    public Runes hp = new Runes
    {
        name = RuneName.生命力,//1
        image = GameManager.Instance.GameConf.imageRunes.hp,//1
        max = 8,
        upRank = 4,
        upPara = 2f,
        value = 100f,
    };
    public void Hp(UnitBace npc, bool add, int count)
    {
        npc.hpMax += ValueCheckInt(hp, add, count);
        npc.Hp += ValueCheckInt(hp, add, count);
    }
    //Mp--------------------------------------------------------------------------------------------
    public Runes mp = new Runes
    {
        name = RuneName.マナ,//1
        image = GameManager.Instance.GameConf.imageRunes.mp,//1
        max = 3,
        upRank = 0,
        upPara = 0f,
        value = 250f,
    };
    public void Mp(UnitBace npc, bool add, int count)
    {
        npc.mpMax += ValueCheckInt(mp, add, count);
        npc.Mp += ValueCheckInt(mp, add, count);
    }
    //攻撃速度--------------------------------------------------------------------------------------------
    public Runes attackSpeed = new Runes
    {
        name = RuneName.攻撃速度,//1
        image = GameManager.Instance.GameConf.imageRunes.attackSpeed,//1
        max = 5,
        upRank = 0,
        upPara = 0f,
        value = 0.3f,
    };
    public void AttackSpeed(UnitBace npc, bool add, int count)
    {
        npc.AttackSpeed += ValueCheckFloat(attackSpeed, add, count);
    }
    //防御--------------------------------------------------------------------------------------------
    public Runes armor = new Runes
    {
        name = RuneName.防御力,//1
        image = GameManager.Instance.GameConf.imageRunes.armor,//1
        max = 4,
        upRank = 2,
        upPara = 2f,
        value = 10f,
    };
    public void Armor(UnitBace npc, bool add, int count)
    {
        npc.armor += ValueCheckInt(armor, add, count);
    }
    //Hp回復--------------------------------------------------------------------------------------------
    public Runes restoreHp = new Runes
    {
        name = RuneName.生命回復,//1
        image = GameManager.Instance.GameConf.imageRunes.restoreHp,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 40f,
    };
    public void RestoreHp(UnitBace npc, bool add, int count)
    {
        npc.restoreHp += ValueCheckInt(restoreHp, add, count);
    }
    //クリティカル--------------------------------------------------------------------------------------------
    public Runes critAttack = new Runes
    {
        name = RuneName.会心率,//1
        image = GameManager.Instance.GameConf.imageRunes.critAttack,//1
        max = 5,
        upRank = 5,
        upPara = 3f,
        value = 0.1f,
    };
    public void CritAttack(UnitBace npc, bool add, int count)
    {
        npc.critAttack += ValueCheckFloat(critAttack, add, count);
    }
    //critダメッジ--------------------------------------------------------------------------------------------
    public Runes critDamage = new Runes
    {
        name = RuneName.会心ダメッジ,//1
        image = GameManager.Instance.GameConf.imageRunes.critDamage,//1
        max = 4,
        upRank = 4,
        upPara = 2f,
        value = 0.5f,
    };
    public void CritDamage(UnitBace npc, bool add, int count)
    {
        npc.critDamage += ValueCheckFloat(critDamage, add, count);
    }
    //回避--------------------------------------------------------------------------------------------
    public Runes dodge = new Runes
    {
        name = RuneName.回避率,//1
        image = GameManager.Instance.GameConf.imageRunes.dodge,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 0.2f,
    };
    public void Dodge(UnitBace npc, bool add, int count)
    {
        npc.dodge += ValueCheckFloat(dodge, add, count);
    }
    //移動--------------------------------------------------------------------------------------------
    public Runes moveSpeed = new Runes
    {
        name = RuneName.移動速度,//1
        image = GameManager.Instance.GameConf.imageRunes.moveSpeed,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 100f,
    };
    public void MoveSpeed(UnitBace npc, bool add, int count)
    {
        npc.Move += ValueCheckInt(moveSpeed, add, count);
    }
    //マナ回復--------------------------------------------------------------------------------------------
    public Runes restoreMp = new Runes
    {
        name = RuneName.マナ回復,//1
        image = GameManager.Instance.GameConf.imageRunes.restoreMp,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 20f,
    };
    public void RestoreMp(UnitBace npc, bool add, int count)
    {
        npc.restoreMp += ValueCheckInt(restoreMp, add, count);
    }
    //クールダウン--------------------------------------------------------------------------------------------
    public Runes coolDown = new Runes
    {
        name = RuneName.クールダウン,//1
        image = GameManager.Instance.GameConf.imageRunes.coolDown,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = skillColdPara,
    };
    public void CoolDown(UnitBace npc, bool add, int count)
    {
        npc.coolDown += ValueCheckFloat(coolDown, add, count);
    }
    //詠唱速度--------------------------------------------------------------------------------------------
    public Runes skillSpeed = new Runes
    {
        name = RuneName.詠唱速度,//1
        image = GameManager.Instance.GameConf.imageRunes.skillSpeed,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 3f,
    };
    public void SkillSpeed(UnitBace npc, bool add, int count)
    {
        npc.skillSpeed += ValueCheckFloat(skillSpeed, add, count);
    }
    //防御無視--------------------------------------------------------------------------------------------
    public Runes armorBreak = new Runes
    {
        name = RuneName.防御無視,//1
        image = GameManager.Instance.GameConf.imageRunes.armorBreak,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = armorBreakdPara,
    };
    public void ArmorBreak(UnitBace npc, bool add, int count)
    {
        npc.armorBreak += ValueCheckFloat(armorBreak, add, count);
    }
    //弾返防御--------------------------------------------------------------------------------------------
    public Runes armorThorns = new Runes
    {
        name = RuneName.弾返防御,//1
        image = GameManager.Instance.GameConf.imageRunes.armorThorns,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 0.2f,
    };
    public void ArmorThorns(UnitBace npc, bool add, int count)
    {
        npc.armorThorns += ValueCheckFloat(armorThorns, add, count);
    }
    //攻撃吸血--------------------------------------------------------------------------------------------
    public Runes attackSuck = new Runes
    {
        name = RuneName.攻撃吸血,//1
        image = GameManager.Instance.GameConf.imageRunes.attackSuck,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 3f,
    };
    public void AttackSuck(UnitBace npc, bool add, int count)
    {
        npc.attackSuck += ValueCheckInt(attackSuck, add, count);
    }
    //魔法吸血--------------------------------------------------------------------------------------------
    public Runes magicSuck = new Runes
    {
        name = RuneName.魔法吸血,//1
        image = GameManager.Instance.GameConf.imageRunes.magicSuck,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 0.2f,
    };
    public void MagicSuck(UnitBace npc, bool add, int count)
    {
        npc.magicSuck += ValueCheckFloat(magicSuck, add, count);
    }
    //死霊操作--------------------------------------------------------------------------------------------
    public Runes deathControl = new Runes
    {
        name = RuneName.死霊操作,//1
        image = GameManager.Instance.GameConf.imageRunes.deathControl,//1
        max = 3,
        upRank = 0,
        upPara = 0f,
        value = 3f,
    };
    public void DeathControl(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneDeathControl);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: deathControl.max);
    }
    //ゲンガー--------------------------------------------------------------------------------------------
    public Runes doppelganger = new Runes
    {
        name = RuneName.ゲンガー,//1
        image = GameManager.Instance.GameConf.imageRunes.doppelganger,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 1f,
    };
    public void Doppelganger(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneDoppelganger);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: doppelganger.max);
    }
    //マナオーラ--------------------------------------------------------------------------------------------
    public Runes manaAura = new Runes
    {
        name = RuneName.マナオーラ,//1
        image = GameManager.Instance.GameConf.imageRunes.manaAura,//1
        max = 2,
        upRank = 2,
        upPara = 4f,
        value = 5f,
    };
    public void ManaAura(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneManaAura);
        buff.Init(BuffType.canNot, npc, npc, 0f, buffLvMax: manaAura.max, value: ValueCheckInt(manaAura, add, count));
    }
    //従者回数--------------------------------------------------------------------------------------------
    public Runes firendsTimes = new Runes//1
    {
        name = RuneName.従者回数,//1
        image = GameManager.Instance.GameConf.imageRunes.firendsTimes,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 1f,
    };
    public void FirendsTimes(UnitBace npc, bool add, int count)//1
    {
        if (npc.skills.itemList == null)
            return;
        if (npc.skills.itemList.Count == 0)
            return;
        if (npc.skills.itemList[0] == null)
            return;
        npc.skills.itemList[0].useTimes += ValueCheckInt(firendsTimes, add, count);//2
        npc.skills.itemList[0].nowUseTimes += ValueCheckInt(firendsTimes, add, count);//2
    }
    //魔導書回数--------------------------------------------------------------------------------------------
    public Runes itemTimes = new Runes//1
    {
        name = RuneName.魔導書回数,//1
        image = GameManager.Instance.GameConf.imageRunes.itemTimes,//1
        max = 3,
        upRank = 0,
        upPara = 0f,
        value = 3f,
    };
    public void ItemTimes(UnitBace npc, bool add, int count)//1
    {
        npc.skills.item.useTimes += ValueCheckInt(itemTimes, add, count);//2
        npc.skills.item.nowUseTimes += ValueCheckInt(itemTimes, add, count);//2
        npc.itemTimes += ValueCheckInt(itemTimes, add, count);
    }
    //魔導書再構築--------------------------------------------------------------------------------------------
    public Runes itemReset = new Runes//1
    {
        name = RuneName.魔導書再構築,//1
        image = GameManager.Instance.GameConf.imageRunes.itemReset,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 1f,
    };
    public void ItemReset(UnitBace npc, bool add, int count)//1
    {
        PlayerManager.Instance.ResetSkillWhenHeroChange(npc, SkillAni.item);
        npc.skills.item.useTimes = 5 + npc.itemTimes;
        npc.skills.item.nowUseTimes = npc.skills.item.useTimes;
        npc.Spelld(PlayerManager.Instance.Hero.skills.item, mpDown: 0);
        npc.Cold(PlayerManager.Instance.Hero.skills.item, coolTimeSet: 0);
    }
    //魔導の極み--------------------------------------------------------------------------------------------
    public Runes manaMax = new Runes//1
    {
        name = RuneName.魔導の極み,//1
        image = GameManager.Instance.GameConf.imageRunes.manaMax,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 500f,
    };
    public void ManaMax(UnitBace npc, bool add, int count)//1
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneManaMax);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: manaMax.max, value: ValueCheckInt(manaMax, add, count));
    }
    //深淵の邪眼--------------------------------------------------------------------------------------------
    public Runes eyesAbyss = new Runes//1
    {
        name = RuneName.深淵の邪眼,//1
        image = GameManager.Instance.GameConf.imageRunes.eyesAbyss,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 15f,
    };
    public void EyesAbyss(UnitBace npc, bool add, int count)//1
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneEyesAbyss);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: eyesAbyss.max, value: ValueCheckInt(eyesAbyss, add, count));
    }
    //マジックアロー--------------------------------------------------------------------------------------------
    public Runes magicArrow = new Runes
    {
        name = RuneName.マジックアロー,//1
        image = GameManager.Instance.GameConf.imageRunes.magicArrow,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 0.2f,
    };
    public void MagicArrow(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneMagicArrow);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: magicArrow.max, value: magicArrow.value);
    }
    //余震攻撃--------------------------------------------------------------------------------------------
    public Runes attackAftershock = new Runes
    {
        name = RuneName.余震攻撃,//1
        image = GameManager.Instance.GameConf.imageRunes.attackAftershock,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 0.3f,
    };
    public void AttackAftershock(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneAttackAftershock);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: attackAftershock.max, value: ValueCheckFloat(attackAftershock, add, count));
    }
    //蘇生--------------------------------------------------------------------------------------------
    public Runes revival = new Runes
    {
        name = RuneName.蘇生,//1
        image = GameManager.Instance.GameConf.imageRunes.revival,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 0.3f,
    };
    public void Revival(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneRevival);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: revival.max, value: revival.value);
    }
    //ダブルマジック--------------------------------------------------------------------------------------------
    public Runes doubleMagic = new Runes
    {
        name = RuneName.ダブルマジック,//1
        image = GameManager.Instance.GameConf.imageRunes.doubleMagic,//1
        max = 2,
        upRank = 2,
        upPara = 4f,
        value = 1f,
    };
    public void DoubleMagic(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneDoubleMagic);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 0, buffLvMax: doubleMagic.max, value: ValueCheckInt(doubleMagic, add, count));
    }
    //警戒本能--------------------------------------------------------------------------------------------
    public Runes alert = new Runes
    {
        name = RuneName.警戒本能,//1
        image = GameManager.Instance.GameConf.imageRunes.alert,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 1.2f,
    };
    public void Alert(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneAlert);
        buff.Init(BuffType.canNot, npc, npc, 0f, buffLvMax: alert.max, value: ValueCheckFloat(alert, add, count));
    }
    //バーサーカー--------------------------------------------------------------------------------------------
    public Runes berserker = new Runes
    {
        name = RuneName.バーサーカー,//1
        image = GameManager.Instance.GameConf.imageRunes.berserker,//1
        max = 3,
        upRank = 0,
        upPara = 0f,
        value = 0.7f,
    };
    public void Berserker(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneBerserker);
        buff.Init(BuffType.canNot, npc, npc, 0f, buffLvMax: berserker.max, value: berserker.value);
    }
    //戦闘リズム--------------------------------------------------------------------------------------------
    public Runes battleRhythm = new Runes
    {
        name = RuneName.戦闘リズム,//1
        image = GameManager.Instance.GameConf.imageRunes.battleRhythm,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 0.5f,
    };
    public void BattleRhythm(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneBattleRhythm);
        buff.Init(BuffType.canNot, npc, npc, 0f, buffLvMax: battleRhythm.max, value: ValueCheckFloat(battleRhythm, add, count));
    }
    //生贄--------------------------------------------------------------------------------------------
    public Runes sacrifice = new Runes
    {
        name = RuneName.生贄,//1
        image = GameManager.Instance.GameConf.imageRunes.sacrifice,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 5f,
    };
    public void Sacrifice(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneSacrifice);
        buff.Init(BuffType.canNot, npc, npc, 0f, buffLvMax: sacrifice.max, value: ValueCheckInt(sacrifice, add, count));
    }
    //攻撃クールダウン--------------------------------------------------------------------------------------------
    public Runes attackCoolDown = new Runes
    {
        name = RuneName.攻撃クールダウン,//1
        image = GameManager.Instance.GameConf.imageRunes.attackCoolDown,//1
        max = 1,
        upRank = 0,
        upPara = 0f,
        value = 1f,
    };
    public void AttackCoolDown(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneAttackCoolDown);
        buff.Init(BuffType.canNot, npc, npc, 0f, buffLvMax: attackCoolDown.max, value: ValueCheckInt(attackCoolDown, add, count));
    }
    //攻撃付魔--------------------------------------------------------------------------------------------
    public Runes attackEnchant = new Runes
    {
        name = RuneName.攻撃付魔,//1
        image = GameManager.Instance.GameConf.imageRunes.attackEnchant,//1
        max = 2,
        upRank = 0,
        upPara = 0f,
        value = 0.3f,
    };
    public void AttackEnchant(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneAttackEnchant);
        buff.Init(BuffType.canNot, npc, npc, 0f, parent: npc.bulletPosRight, buffLvMax: attackEnchant.max, value: attackEnchant.value);
    }
    //氷月領域--------------------------------------------------------------------------------------------
    public Runes iceArea = new Runes
    {
        name = RuneName.氷月領域,//1
        image = GameManager.Instance.GameConf.imageRunes.iceArea,//1
        max = 2,
        upRank = 2,
        upPara = 2f,
        value = 10f,
    };
    public void IceArea(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneIceArea);
        buff.Init(BuffType.canNot, npc, npc, 0f, buffLvMax: iceArea.max, value: ValueCheckInt(iceArea, add, count));
    }
    //サンダーボルト--------------------------------------------------------------------------------------------
    public Runes thunderbolt = new Runes
    {
        name = RuneName.サンダーボルト,//1
        image = GameManager.Instance.GameConf.imageRunes.thunderbolt,//1
        max = 3,
        upRank = 0,
        upPara = 0f,
        value = 100f,
    };
    public void Thunderbolt(UnitBace npc, bool add, int count)
    {
        BuffBase buff = npc.CheckBuffToAdd(GameManager.Instance.GameConf.runeBuff.RuneThunderbolt);
        buff.Init(BuffType.canNot, npc, npc, 0f, loopTime: 1f, buffLvMax: thunderbolt.max, value: thunderbolt.value);
    }
}
public class Runes
{
    public RuneName name;
    public Sprite image;
    public int count = 1;
    public int max;
    public int upRank;
    public float upPara;
    public float value;
    /// <summary>
    /// 能力追加削除の動作、"UnitBace"誰に追加、"bool"AddかRemove、"int"符文レベル
    /// </summary>
    public UnityAction<UnitBace, bool, int> add;
}
