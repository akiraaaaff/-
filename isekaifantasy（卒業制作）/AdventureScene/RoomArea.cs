using UnityEngine;

public class RoomArea : MonoBehaviour
{
    public RoomBace room;

    public void SetRoom(RoomBace room) => this.room = room;

    private void OnTriggerEnter(Collider other)
    {
        if (AdModeManager.Instance.nowRoom == room)
            return;
        if (other == PlayerManager.Instance.Hero.col &&
            !AdSceneManager.Instance.transPortObj.activeSelf)
            AdSceneManager.Instance.DoorSetBattle(room);
    }
}
