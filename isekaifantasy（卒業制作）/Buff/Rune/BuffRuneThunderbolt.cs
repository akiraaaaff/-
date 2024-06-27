using UnityEngine;

public class BuffRuneThunderbolt : BuffBase
{
    private float[] time = new float[] { 0, 0, 0 };


    protected override void BuffAdd()
    {
        base.BuffAdd();
        time = new float[] { 0, 0, 0 };
    }

    protected override void BuffLoop()
    {
        if (AdModeManager.Instance == null || AdModeManager.Instance.QuestStatus != QuestStatus.battle)
            return;
        for (int i = 0; i < buffLv; i++)
        {
            time[i] -= loopTime;
            if (time[i] <= 0)
            {
                time[i] = 6;
                var index = 0;
                if (buffLv == 1)
                    index = Random.Range(0, AdModeManager.Instance.nowRoom.spawnList.Count);
                else if (buffLv == 2)
                {
                    if (i == 0)
                        index = Random.Range(0, AdModeManager.Instance.nowRoom.spawnList.Count / 2 + 1);
                    else
                    if (i == 1)
                        index = Random.Range(AdModeManager.Instance.nowRoom.spawnList.Count / 2 - 1, AdModeManager.Instance.nowRoom.spawnList.Count);
                }
                else if (buffLv == 3)
                {
                    if (i == 0)
                        index = Random.Range(0, AdModeManager.Instance.nowRoom.spawnList.Count / 3 + 1);
                    else
                    if (i == 1)
                        index = Random.Range(AdModeManager.Instance.nowRoom.spawnList.Count / 3 - 1, AdModeManager.Instance.nowRoom.spawnList.Count / 3 * 2 + 1);
                    else
                    if (i == 1)
                        index = Random.Range(AdModeManager.Instance.nowRoom.spawnList.Count / 3 * 2 - 1, AdModeManager.Instance.nowRoom.spawnList.Count);
                }

                var childTs = AdModeManager.Instance.nowRoom.spawnList[index];
                var ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.runeEffects.Thunderbolt);

                float tempY = childTs.position.y;
                Vector3 pos = childTs.position + Random.insideUnitSphere * 0.5f;
                pos.y = tempY;

                ts.position = pos;
                ts.rotation = childTs.rotation;
                var eff = ts.GetComponent<EffectsBase>();
                eff.Init(my, attack: value + my.magic * 0.5f, damageType: DamageType.magic
                    , clip: GameManager.Instance.GameConf.sounds.MagicBurst
                    , reSetDamage: GetDamage);
                break;
            }
        }
    }

    private ReSetDamage GetDamage(UnitBace attacker, UnitBace target)
    {
        var data = new ReSetDamage();
        data.isDamage = true;
        data.damageType = DamageType.magic;

        if (target == null)
            data.damage = value + attacker.magic * 0.5f;
        else
            data.damage = value * (1 + (target.hpMax - target.Hp)*3
                / (float)target.hpMax) + attacker.magic * 0.5f;



        return data;
    }
}
