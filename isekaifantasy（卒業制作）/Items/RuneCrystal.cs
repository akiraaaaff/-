using UnityEngine;

public class RuneCrystal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8)   //Layer 10 - GrassDetector
            return;

        UnitBace unit = other.GetComponentInParent<UnitBace>();
        if (unit == PlayerManager.Instance.Hero)
        {
            AdModeManager.Instance.RuneAdd();

            AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.sounds.WaterSpawn);
            Transform ts = PoolManager.Instance.GetObj(GameManager.Instance.GameConf.effects.RuneMagic);
            Vector3 position = unit.trs.position;
            position.y = 0;
            ts.position = position;

            Destroy(gameObject);
        }
    }
}
