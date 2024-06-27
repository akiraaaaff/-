using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ドアに追加、プレイヤーが入ったらシーンをリセット
/// </summary>
public class LoadNextScene : MonoBehaviour
{
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private GameObject obj;

    private void OnEnable()
    {
        Invoke("On", 1.5f);
    }

    private void On()
    {
        boxCollider.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == PlayerManager.Instance.Hero.col)
        {
            //foreach (var net in GameManager.Instance.netHeroList)
                //net.ChangeScence();




            Trans();
        }
    }

    private void Awake()
    {
        GameManager.Instance.loadNextScene = this;
    }

    public void Trans()
    {
        PlayerManager.Instance.Hero.ClearGrass();

        if (LobbyManager.Instance != null || AdModeManager.Instance.roomLvTotal == 8 || AdModeManager.Instance.roomLvTotal == 16)
        {
            //------------------------------------------------------------------------------------------------------
            //ランダムゲット従者
            if (LobbyManager.Instance != null && (LobbyManager.Instance.friend1 == null || LobbyManager.Instance.friend2 == null))
            {
                if (LobbyManager.Instance.friend1 == null && LobbyManager.Instance.friend2 == null)
                {
                    int lastNum = PlayerManager.Instance.GetFriendsRandom();
                    PlayerManager.Instance.GetFriendsRandom(lastNum);
                }
                else
                    PlayerManager.Instance.GetFriendsRandom();
            }
            //------------------------------------------------------------------------------------------------------

            int id = SceneManager.GetActiveScene().buildIndex;
            if (id == 0)
            {
                var index = Random.Range(0, 4);
                /*
                switch (index)
                {
                    case 0:
                        id = 8;
                        break;
                    case 1:
                        id = 9;
                        break;
                    case 2:
                        id = 10;
                        break;
                }
                */
                switch (index)
                {
                    case 0:
                        id = 1;
                        break;
                    case 1:
                        id = 4;
                        break;
                    case 2:
                        id = 5;
                        break;
                    case 3:
                        id = 9;
                        break;
                }
            }
            else if (id == 1 || id == 4 || id == 5 || id == 9)
            {
                var index = Random.Range(0, 4);
                switch (index)
                {
                    case 0:
                        id = 2;
                        break;
                    case 1:
                        id = 6;
                        break;
                    case 2:
                        id = 8;
                        break;
                    case 3:
                        id = 10;
                        break;
                }
            }
            else if (id == 2 || id == 6 || id == 8 || id == 10)
            {
                var index = Random.Range(0, 2);
                switch (index)
                {
                    case 0:
                        id = 3;
                        break;
                    case 1:
                        id = 7;
                        break;
                }
            }
            SceneManager.LoadSceneAsync(id);
            boxCollider.enabled = false;
            obj.SetActive(false);
        }
        else
        {
            AdSceneManager.Instance.ResetRoom();
            boxCollider.enabled = false;
            obj.SetActive(false);
        }
    }
}
