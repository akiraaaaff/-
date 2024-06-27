using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 全てのプレハブ
/// </summary>
[CreateAssetMenu(fileName = "GameConf", menuName = "GameConf")]

public class GameConf : ScriptableObject
{
    public List<GameObject> unitList;

    public Bullets bullets = new Bullets();
    public I_Effects i_Effects = new I_Effects();
    public O_Effects o_Effects = new O_Effects();
    public H_Effects h_Effects = new H_Effects();
    public S_Effects s_Effects = new S_Effects();
    public Effects effects = new Effects();
    public EffectsItem effectsItem = new EffectsItem();
    public EffectsKeep effectsKeep = new EffectsKeep();
    public RuneEffects runeEffects = new RuneEffects();
    public Buff buff = new Buff();
    public RuneBuff runeBuff = new RuneBuff();
    public ImageRunes imageRunes = new ImageRunes();
    public Sounds sounds = new Sounds();



    [Header("その他エフェクト")]

    [Tooltip("コイン")]
    public GameObject Gold;
    [Tooltip("NPCバー")]
    public GameObject NpcBar;
    [Tooltip("NPC影")]
    public GameObject NpcShadow;
    [Tooltip("文字ダメッジ")]
    public GameObject TextDamage;
    //------------------------------------------end-----------------------------------------------------------------------------




    [Header("BGM")]

    [Tooltip("冒険BGM")]
    public AudioClip AdventureBGM;
    [Tooltip("勝利")]
    public AudioClip WinMusic;
    [Tooltip("失敗")]
    public AudioClip LoseMusic;
    //------------------------------------------end-----------------------------------------------------------------------------
}

/// <summary>
/// 弾幕
/// </summary>
[System.Serializable]
public class Bullets
{
    [Header("弾幕")]

    [Tooltip("ArrowMagicArrowLight")]
    public GameObject ArrowMagicArrowLight;
    [Tooltip("ダークフレーム")]
    public GameObject DarkFlame;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject ArrowGoblinArcher;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject ArrowGoblinArcherBig;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject ArrowGoblinArcherMagic;
    [Tooltip("爆炎の魔法使い")]
    public GameObject Fireball;
    [Tooltip("ルーキーナイト")]
    public GameObject Bullet_Slash_red;
    [Tooltip("疫病スケメイジ")]
    public GameObject PoisonFlame;
    [Tooltip("GhostAcidClaw")]
    public GameObject Bullet_Slash_Green;
    [Tooltip("RedGuard")]
    public GameObject RedGuardBullet;



    [Tooltip("Enemy")]
    public GameObject MissileCircleRed;
    [Tooltip("Enemy")]
    public GameObject MissileCircleRedTail;
    [Tooltip("Enemy")]
    public GameObject MissileConeRed;
    [Tooltip("Enemy")]
    public GameObject MissileConeRedTail;
    [Tooltip("Enemy")]
    public GameObject MissileStarRed;
    [Tooltip("Enemy")]
    public GameObject MissileStarRedTail;
    [Tooltip("Enemy")]
    public GameObject ArrowCommon;

}

/// <summary>
/// 弾幕
/// </summary>
[System.Serializable]
public class I_Effects
{
    [Header("弾幕")]

    [Tooltip("ダークフレーム")]
    public GameObject i_DarkFlame;
    [Tooltip("疫病スケメイジ")]
    public GameObject i_PoisonFlame;
    [Tooltip("ルーキーナイト")]
    public GameObject i_Slash_red;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject i_ArrowGoblinArcher;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject i_ArrowGoblinArcherBig;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject i_ArrowGoblinArcherBigMagic;

}

/// <summary>
/// 弾幕
/// </summary>
[System.Serializable]
public class O_Effects
{
    [Header("弾幕")]

    [Tooltip("ダークフレーム")]
    public GameObject o_DarkFlame;
    [Tooltip("ダークフレーム")]
    public GameObject o_DarkFlame_small;
    [Tooltip("疫病スケメイジ")]
    public GameObject o_PoisonFlame_small;
    [Tooltip("ルーキーナイト")]
    public GameObject o_Slash_red;
    [Tooltip("Enemy")]
    public GameObject o_MissileRedSmall;
    [Tooltip("Enemy")]
    public GameObject o_MissileRedMiddle;
    [Tooltip("Enemy")]
    public GameObject o_MissileRedLarge;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject o_ArrowGoblinArcher;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject o_ArrowGoblinArcherBig;

}

/// <summary>
/// Eff
/// </summary>
[System.Serializable]
public class H_Effects
{
    [Header("EffHit")]

    [Tooltip("h_Hit_block")]
    public GameObject h_Hit_block;
    [Tooltip("ルーキーナイト")]
    public GameObject h_Slash_red;
    [Tooltip("ルーキーナイト")]
    public GameObject h_Hit_blue;
    [Tooltip("ダークフレーム")]
    public GameObject h_DarkFlame;
    [Tooltip("疫病スケメイジ")]
    public GameObject h_PoisonFlame;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject h_ArrowGoblinArcherBigMagic;

}

/// <summary>
/// Mov
/// </summary>
[System.Serializable]
public class S_Effects
{
    [Header("MovHit")]

    [Tooltip("ゴブリンアーチャー")]
    public GameObject s_ArrowGoblinArcher;

}

/// <summary>
/// エフェクト
/// </summary>
[System.Serializable]
public class Effects
{

    [Header("エフェクト")]

    [Tooltip("Hp回復")]
    public GameObject HpRestore;
    [Tooltip("Mp回復")]
    public GameObject MpRestore;
    [Tooltip("colliderPosition")]
    public GameObject DeBUg;


    [Tooltip("ゲットコイン")]
    public GameObject CoinsPickup;
    [Tooltip("レベルアップ")]
    public GameObject LvUp;
    [Tooltip("召喚魔法陳")]
    public GameObject SpawnMagic;
    [Tooltip("召喚魔法陳")]
    public GameObject SpawnMagicShort;
    [Tooltip("召喚魔法陳")]
    public GameObject DieMagic;
    [Tooltip("召喚魔法陳")]
    public GameObject RuneMagic;
    [Tooltip("ソードスラッシュ")]
    public GameObject Slash_blue;
    [Tooltip("ソードスラッシュ")]
    public GameObject Slash_red;


    [Tooltip("ダークメイジ")]
    public GameObject DarkMageUltimate;
    [Tooltip("ダークメイジ")]
    public GameObject DarkMageUltimateBirth;
    [Tooltip("疫病スケメイジ")]
    public GameObject PoisonBlast;
    [Tooltip("SkeletonFireKing")]
    public GameObject FireCleaveRed;
    [Tooltip("SkeletonFireKing")]
    public GameObject FireFieldRed;
    [Tooltip("GhostAcidClaw")]
    public GameObject AcidArea;
    [Tooltip("RedGuard")]
    public GameObject RedGuardPassiveText;
    [Tooltip("RedGuard")]
    public GameObject RedGuardPassiveExplosionWaterBlack;
    [Tooltip("RedGuard")]
    public GameObject RedGuardUltimate;


    [Header("Common")]
    [Tooltip("ExpandAura")]
    public GameObject ExpandAura;
    [Tooltip("FireBlast")]
    public GameObject FireBlast;
    [Tooltip("StarCleave")]
    public GameObject StarCleave;
}

/// </summary>
/// アイテムエフェクト
/// </summary>
[System.Serializable]
public class EffectsItem
{
    [Tooltip("Hp召喚魔法")]
    public GameObject HpSpawn;
    [Tooltip("Hpポイント")]
    public GameObject HPoint;
    [Tooltip("Mp召喚魔法")]
    public GameObject MpSpawn;
    [Tooltip("Mpポイント")]
    public GameObject MPoint;
}

/// <summary>
/// エフェクトKeep
/// </summary>
[System.Serializable]
public class EffectsKeep
{
    [Tooltip("ダークメイジ")]
    public GameObject DarkMageUltimateKeep;
    [Tooltip("RedGuard")]
    public GameObject RedGuardAttackKeep;
}

/// <summary>
/// バフ
/// </summary>
[System.Serializable]
public class Buff
{

    [Header("バフ")]

    [Tooltip("スピードダウン")]
    public GameObject Slow;
    [Tooltip("ArmorBreak")]
    public GameObject ArmorBreak;


    [Tooltip("スケルトン")]
    public GameObject AttackUp;


    [Tooltip("ルーキーナイト")]
    public GameObject RookieNightSkill;
    [Tooltip("ルーキーナイト")]
    public GameObject RookieNightPassive;
    [Tooltip("ルーキーナイト")]
    public GameObject RookieNightUltimate;
    [Tooltip("ダークメイジ")]
    public GameObject DarkMagePassive;
    [Tooltip("ダークメイジ")]
    public GameObject DarkAuraSlow;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject GoblinArcherUltimate;
    [Tooltip("ゴブリンアーチャー")]
    public GameObject GoblinArcherPassive;
    [Tooltip("SkeletonFireKing")]
    public GameObject SkeletonFireKingPassive;
    [Tooltip("爆炎の魔法使い")]
    public GameObject FirebustSparkles;
    [Tooltip("疫病スケメイジ")]
    public GameObject SkeletonPoisonMagePassivePoison;
    [Tooltip("疫病スケメイジ")]
    public GameObject SkeletonPoisonMagePassive;
    [Tooltip("疫病スケメイジ")]
    public GameObject SkeletonPoisonMageUltimate;
    [Tooltip("GhostAcidClaw")]
    public GameObject GhostAcidClawPassive;
    [Tooltip("GhostAcidClaw")]
    public GameObject GhostAcidClawSkill;
    [Tooltip("RedGuard")]
    public GameObject RedGuardPassive;
    [Tooltip("RedGuard")]
    public GameObject RedGuardPassiveTar;
}

/// <summary>
/// 符文Effects
/// </summary>
[System.Serializable]
public class RuneEffects
{

    [Header("符文Effects")]

    [Tooltip("AftershockAura")]
    public GameObject AftershockAura;
    [Tooltip("DarkBlast")]
    public GameObject DarkBlast;
    [Tooltip("RevivalOK")]
    public GameObject RevivalOK;
    [Tooltip("SacrificeMagic")]
    public GameObject SacrificeMagic;
    [Tooltip("AttackEnchant")]
    public GameObject AttackEnchant;
    [Tooltip("IceArea")]
    public GameObject IceArea;
    [Tooltip("Thunderbolt")]
    public GameObject Thunderbolt;
}

/// <summary>
/// 符文バフ
/// </summary>
[System.Serializable]
public class RuneBuff
{

    [Header("符文バフ")]

    [Tooltip("RuneManaAura")]
    public GameObject RuneManaAura;
    [Tooltip("RuneDeathControl")]
    public GameObject RuneDeathControl;
    [Tooltip("RuneDoppelganger")]
    public GameObject RuneDoppelganger;
    [Tooltip("RuneManaMax")]
    public GameObject RuneManaMax;
    [Tooltip("RuneBattleRhythm")]
    public GameObject RuneBattleRhythm;
    [Tooltip("RuneBerserker")]
    public GameObject RuneBerserker;
    [Tooltip("RuneAlert")]
    public GameObject RuneAlert;
    [Tooltip("RuneAttackAftershock")]
    public GameObject RuneAttackAftershock;
    [Tooltip("RuneMagicArrow")]
    public GameObject RuneMagicArrow;
    [Tooltip("RuneEyesAbyss")]
    public GameObject RuneEyesAbyss;
    [Tooltip("RuneEyesTime")]
    public GameObject RuneEyesTime;
    [Tooltip("RuneRevival")]
    public GameObject RuneRevival;
    [Tooltip("RuneDoubleMagic")]
    public GameObject RuneDoubleMagic;
    [Tooltip("RuneSacrifice")]
    public GameObject RuneSacrifice;
    [Tooltip("RuneAttackCoolDown")]
    public GameObject RuneAttackCoolDown;
    [Tooltip("RuneAttackEnchant")]
    public GameObject RuneAttackEnchant;
    [Tooltip("RuneIceArea")]
    public GameObject RuneIceArea;
    [Tooltip("RuneIceAreaSlow")]
    public GameObject RuneIceAreaSlow;
    [Tooltip("RuneThunderbolt")]
    public GameObject RuneThunderbolt;
}


/// <summary>
/// スキルアイコン
/// </summary>
[System.Serializable]
public class ImageRunes
{
    [Header("スキルアイコン")]

    [Tooltip("攻撃力")]
    public Sprite attack;
    [Tooltip("魔力")]
    public Sprite magic;
    [Tooltip("生命力")]
    public Sprite hp;
    [Tooltip("マナ")]
    public Sprite mp;
    [Tooltip("攻撃速度")]
    public Sprite attackSpeed;
    [Tooltip("防御")]
    public Sprite armor;
    [Tooltip("Hp回復")]
    public Sprite restoreHp;
    [Tooltip("クリティカル")]
    public Sprite critAttack;
    [Tooltip("critダメッジ")]
    public Sprite critDamage;
    [Tooltip("回避")]
    public Sprite dodge;
    [Tooltip("移動")]
    public Sprite moveSpeed;
    [Tooltip("Mp回復")]
    public Sprite restoreMp;
    [Tooltip("クールダウン")]
    public Sprite coolDown;
    [Tooltip("詠唱速度")]
    public Sprite skillSpeed;
    [Tooltip("防御破壊")]
    public Sprite armorBreak;
    [Tooltip("防御弾返")]
    public Sprite armorThorns;
    [Tooltip("攻撃飢餓")]
    public Sprite attackSuck;
    [Tooltip("魔法飢餓")]
    public Sprite magicSuck;
    [Tooltip("死霊操作")]
    public Sprite deathControl;
    [Tooltip("ドッペルゲンガー")]
    public Sprite doppelganger;
    [Tooltip("マナオーラ")]
    public Sprite manaAura;
    [Tooltip("従者回数")]
    public Sprite firendsTimes;
    [Tooltip("魔法書回数")]
    public Sprite itemTimes;
    [Tooltip("魔法書再構築")]
    public Sprite itemReset;
    [Tooltip("魔導の極み")]
    public Sprite manaMax;
    [Tooltip("深淵の邪眼")]
    public Sprite eyesAbyss;
    [Tooltip("マジックアロー")]
    public Sprite magicArrow;
    [Tooltip("余震攻撃")]
    public Sprite attackAftershock;
    [Tooltip("蘇生")]
    public Sprite revival;
    [Tooltip("ダブルマジック")]
    public Sprite doubleMagic;
    [Tooltip("警戒本能")]
    public Sprite alert;
    [Tooltip("バーサーカー")]
    public Sprite berserker;
    [Tooltip("戦闘リズム")]
    public Sprite battleRhythm;
    [Tooltip("生贄")]
    public Sprite sacrifice;
    [Tooltip("攻撃クールダウン")]
    public Sprite attackCoolDown;
    [Tooltip("攻撃付魔")]
    public Sprite attackEnchant;
    [Tooltip("氷月領域")]
    public Sprite iceArea;
    [Tooltip("サンダーボルト")]
    public Sprite thunderbolt;
}

/// <summary>
/// サウンド
/// </summary>
[System.Serializable]
public class Sounds
{
    [Header("サウンド")]

    [Tooltip("エフェクトプレヤー")]
    public GameObject EFAudio;
    [Tooltip("ボタンクリック")]
    public AudioClip ButtonClick;
    [Tooltip("ゲットコイン")]
    public AudioClip CoinsPickup;
    [Tooltip("ダメージ食らう")]
    public AudioClip DamageHit;
    [Tooltip("剣撃")]
    public AudioClip SwordSound;
    [Tooltip("魔法継続")]
    public AudioClip MagicStay;
    [Tooltip("魔法波多")]
    public AudioClip MagicBulltsMany;
    [Tooltip("魔法爆裂")]
    public AudioClip MagicBurst;
    [Tooltip("水産出")]
    public AudioClip WaterSpawn;
    [Tooltip("水回復")]
    public AudioClip WaterHeal;
    [Tooltip("水落ち")]
    public AudioClip WaterDown;
    [Tooltip("ドアOPEN CLOSE")]
    public AudioClip DoorOC;
}