public enum UnitName
{
    no=0,
    //Hero--------------------------------------------------------------------------------------------
    RookieNight,
    DarkMage,
    GoblinArcher,
    CatMagician,
    BakuenMahotukai,
    GhostAcidClaw,
    //Head--------------------------------------------------------------------------------------------
    DragonHead,
    GhostHead,
    MummyHead,
    OrcHead,
    SkeletonHead,
    WerewolfHead,
    ZombieHead,
    //Skeleton--------------------------------------------------------------------------------------------
    SkeletonAngry,
    SkeletonBigHead,
    SkeletonPoisonMage,
    SkeletonIceWarrior,
    SkeletonLithingArcher,
    SkeletonFireKing,
    //Unit--------------------------------------------------------------------------------------------
    EggGolden,
    GolemRock,
    BatThirstyBlock,
    DeathKnightBlock,
    GhostPhantomBlock,
    FatBatUndead,
    FatGhostSoul,
    FatSpiderPosion,
    DarkTreant,
    OneEyedBatDark,
    PlantOneHead,
    ScarecrowBlue,
    TreasureChestMonster,
    //Kingdom--------------------------------------------------------------------------------------------
    KingdomArcher,
    KingdomCommander,
    KingdomCrossbowman,
    KingdomHalberdier,
    KingdomHeavyCavalry,
    KingdomHeavyInfantry,
    KingdomHeavySwordman,
    KingdomHighPriest,
    KingdomHolyFather,
    KingdomKnight,
    KingdomLightCavalry,
    KingdomLightInfantry,
    KingdomMage,
    KingdomMountedCrossbowman,
    KingdomMountedMage,
    KingdomMountedPaladin,
    KingdomMountedPriest,
    KingdomMountedScout,
    KingdomPaladin,
    KingdomPeasant,
    KingdomPriest,
    KingdomPrince,
    KingdomScout,
    KingdomSpearman,
    KingdomSwordman,
    //Farm--------------------------------------------------------------------------------------------
    Burrow,
    Chick,
    Chicken,
    RabbitRed,
    //Animals--------------------------------------------------------------------------------------------
    Bear,
    Boar,
    CougarCub,
    Deer,
    FoxCub,
    Wolf,
    WolfCub,
    //Next--------------------------------------------------------------------------------------------

    RedGuard,
}

public enum RuneName
{
    攻撃力=0,
    魔力,
    生命力,
    マナ,
    攻撃速度,
    防御力,
    生命回復,
    会心率,
    会心ダメッジ,
    回避率,
    移動速度,
    マナ回復,
    クールダウン,
    詠唱速度,
    防御無視,
    弾返防御,
    攻撃吸血,
    魔法吸血,
    死霊操作,
    ゲンガー,
    マナオーラ,
    従者回数,
    魔導書回数,
    魔導書再構築,
    魔導の極み,
    深淵の邪眼,
    マジックアロー,
    余震攻撃,
    蘇生,
    ダブルマジック,
    警戒本能,
    バーサーカー,
    戦闘リズム,
    生贄,
    攻撃クールダウン,
    攻撃付魔,
    氷月領域,
    サンダーボルト,
}

public enum SkillName
{
    no=0,
    //
    不滅意志,
    斬撃,
    防御,
    連斬,
    //
    ダークオーラ,
    ダークフレーム,
    ダーク連射,
    ダークマキシマム,
    //
    燃焼の魂,
    炎斬,
    炎爆,
    炎骨大葬,
    //
    ダブル射撃,
    射て,
    射撃ノックバック,
    マジック矢,
    //
    毒ガス,
    毒玉,
    スケルトン召喚,
    毒爆発,
    //
    爪傷,
    爪撃,
    背瞬,
    腐蝕,
    //
    死亡游戏,
    扫射,
    翻滚,
    大号铅弹,
    //
    仲間召喚
}

public enum SpellType
{
    no,
    click,
    keep,
    loop,
    select
}

public enum SkillAni
{
    passive,
    attack,
    skill,
    ultimate,
    item
}

public enum SkillType
{
    attackSolo,
    aoe,
    heal,
    buff,
    deBuff,
    defend
}

public enum TextDamageWay
{
    Up,
    Left,
    Right,
    LeftUp,
    RightUp,
    Down
}

public enum DamageRange
{
    no,
    cylinder,
    hemisphere,
    cone
}

public enum DamageType
{
    no,
    self,
    attack,
    attackAftershock,
    magicArrow,
    magic,
    heal,
    poison
}

public enum BulletType
{
    Solo,
    Aoe,
    Line
}

public enum BuffType
{
    plus,
    minus,
    stun,
    slow,
    canNot
}

public enum BuffState
{
    start=0,
    wait=10,
    cold =20
}

public enum QuestStatus
{
    battle = 0,
    next,
    clear,
    over
}

public enum AiType
{
    no=0,
    chase,
    random,
    runAway,
    pingPlr,
    pingPud,
    around,
    keepHome,
    friendlyFllow=100
}

public enum ItemType
{
    Hp = 0,
    Mp
}

public enum Tags
{
    obj = 0,
    canBreakObj,
    unit
}