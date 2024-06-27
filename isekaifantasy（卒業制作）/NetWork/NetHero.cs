using Unity.Netcode;
using UnityEngine;

public class NetHero : NetworkBehaviour
{

    private NetworkVariable<Vector2> InputPos = new();
    private UnitBace unit;


    public override void OnNetworkSpawn()
    {
        DontDestroyOnLoad(gameObject);
        GameManager.Instance.netHeroList.Add(this);
    }

    public void ChangeScence()
    {
        if (NetworkManager.Singleton.IsServer && IsOwner)
        {
            var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            ChangeScenceClientRpc(seed);
        }
    }

    [ClientRpc]
    void ChangeScenceClientRpc(int seed)
    {
        UnityEngine.Random.InitState(seed);
        GameManager.Instance.loadNextScene.Trans();
    }

    public void OnStartPlay()
    {
        if (IsOwner)
        {
            unit = PlayerManager.Instance.Hero;
            unit.netSpelld = Spelld;


            if (NetworkManager.Singleton.IsServer)
            {
                CreateSameHeroClientRpc(PlayerManager.Instance.heroBaceList
                    .FindIndex(x => x.unitName == PlayerManager.Instance.Hero.unitName));
            }
            else
            {
                CreateSameHeroServerRpc(PlayerManager.Instance.heroBaceList
                    .FindIndex(x => x.unitName == PlayerManager.Instance.Hero.unitName));
            }
        }
    }

    [ClientRpc]
    void CreateSameHeroClientRpc(int index)
    {
        if (IsOwner)
            return;
        GameObject go = Instantiate(PlayerManager.Instance.heroBaceList[index].obj, PlayerManager.Instance.Hero.trs.position, PlayerManager.Instance.Hero.trs.rotation);
        unit = go.GetComponent<UnitBace>();
        unit.Init();
        unit.SetGroup(PlayerManager.Instance.Hero.friendsGroup);


        AdSceneManager.Instance.netSetBattle = SetBattle;
    }
    [ServerRpc]
    void CreateSameHeroServerRpc(int index)
    {
        if (IsOwner)
            return;
        GameObject go = Instantiate(PlayerManager.Instance.heroBaceList[index].obj, PlayerManager.Instance.Hero.trs.position, PlayerManager.Instance.Hero.trs.rotation);
        unit = go.GetComponent<UnitBace>();
        unit.Init();
        unit.SetGroup(PlayerManager.Instance.Hero.friendsGroup);
    }

    public void SetBattle(int index)
    {
        var room = AdSceneManager.Instance.roomList[index];
        AdSceneManager.Instance.DoorSetBattle(room);
    }

    void Spelld(Skill skill, bool isAssDir)
    {
        if (IsOwner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                if (skill == unit.skills.attack)
                    Spelld1ClientRpc(isAssDir);
                else if (skill == unit.skills.skill)
                    Spelld2ClientRpc(isAssDir);
                else if (skill == unit.skills.ultimate)
                    Spelld3ClientRpc(isAssDir);
                else if (skill == unit.skills.item)
                    Spelld4ClientRpc(isAssDir);
                else if (skill == unit.skills.itemList[0])
                    Spelld5ClientRpc(isAssDir);
            }
            else
            {
                if (skill == unit.skills.attack)
                {
                    Spelld1ClientRpc(isAssDir);
                    Spelld1ServerRpc(isAssDir);
                }
                else if (skill == unit.skills.skill)
                {
                    Spelld2ClientRpc(isAssDir);
                    Spelld2ServerRpc(isAssDir);
                }
                else if (skill == unit.skills.ultimate)
                {
                    Spelld3ClientRpc(isAssDir);
                    Spelld3ServerRpc(isAssDir);
                }
                else if (skill == unit.skills.item)
                {
                    Spelld4ClientRpc(isAssDir);
                    Spelld4ServerRpc(isAssDir);
                }
                else if (skill == unit.skills.itemList[0])
                {
                    Spelld5ClientRpc(isAssDir);
                    Spelld5ServerRpc(isAssDir);
                }
            }
        }
    }

    [ServerRpc]
    void Spelld1ServerRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.attack, isAssDir);
            unit.Cold(unit.skills.attack);
        }
    }

    [ClientRpc]
    void Spelld1ClientRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.attack, isAssDir);
            unit.Cold(unit.skills.attack);
        }
    }

    [ServerRpc]
    void Spelld2ServerRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.skill, isAssDir);
            unit.Cold(unit.skills.skill);
        }
    }

    [ClientRpc]
    void Spelld2ClientRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.skill, isAssDir);
            unit.Cold(unit.skills.skill);
        }
    }

    [ServerRpc]
    void Spelld3ServerRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.ultimate, isAssDir);
            unit.Cold(unit.skills.ultimate);
        }
    }

    [ClientRpc]
    void Spelld3ClientRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.ultimate, isAssDir);
            unit.Cold(unit.skills.ultimate);
        }
    }

    [ServerRpc]
    void Spelld4ServerRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.item, isAssDir);
            unit.Cold(unit.skills.item);
        }
    }

    [ClientRpc]
    void Spelld4ClientRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.item, isAssDir);
            unit.Cold(unit.skills.item);
        }
    }

    [ServerRpc]
    void Spelld5ServerRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.itemList[0], isAssDir);
            unit.Cold(unit.skills.itemList[0]);
        }
    }

    [ClientRpc]
    void Spelld5ClientRpc(bool isAssDir)
    {
        if (!IsOwner)
        {
            unit.Spelld(unit.skills.itemList[0], isAssDir);
            unit.Cold(unit.skills.itemList[0]);
        }
    }



    [ServerRpc]
    void SubmitPositionRequestServerRpc(Vector2 pos)
    {
        InputPos.Value = pos;
    }

    float isMove;
    void Update()
    {
        if (isMove > 0f)
        {
            isMove -= Time.deltaTime;
            if (isMove < 0f) isMove = 0f;
        }

        if (!IsOwner)
        {
            if (InputPos.Value.x != 0 || InputPos.Value.y != 0)
            {
                unit.PlayAnime("run", true, true);
                isMove = 0.1f;
                Vector3 targetPos = new Vector3(InputPos.Value.x, transform.position.y, InputPos.Value.y);
                unit.trs.position = Vector3.MoveTowards(unit.trs.position, targetPos, unit.Move / 100 * Time.deltaTime * unit.anim.GetFloat("moveSpeed"));
                if (unit.isSkilling && unit.runningSkill.isLookAt && unit.Target != null)
                {

                }
                else
                    unit.trs.LookAt(targetPos);
            }
            else
            {
                if (unit.ai.my == null || isMove > 0) unit.PlayAnime("run", true, false);
                if (unit.ai.my != null && isMove > 0)
                {
                    isMove = 0;
                    if (unit.Hp > 0)
                        unit.ai.Run();
                }
            }
        }
        else
        {
            if (NetworkManager.Singleton.IsServer)
            {
                InputPos.Value = Joystick.Instance.handle.localPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc(Joystick.Instance.handle.localPosition);
            }
        }
    }
}
