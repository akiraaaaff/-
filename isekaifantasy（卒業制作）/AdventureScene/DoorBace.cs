using UnityEngine;

public class DoorBace : MonoBehaviour
{
    [SerializeField]
    protected Transform door;
    public RoomBace room1;
    public RoomBace room2;
    [SerializeField]
    protected float speed = 1f;

    protected BoxCollider box;
    protected Vector3 defultPos;
    protected bool isOpen;
    protected bool isClose;

    private void Awake()
    {
        if (door == null)
            return;
        box = door.GetComponent<BoxCollider>();
        defultPos = door.position;
    }

    public void SetRoom(RoomBace room)
    {
        if (room1 == null) room1 = room;
        else room2 = room;
    }

    public virtual void OpenDoor()
    {
        isOpen = true;
        isClose = false;
    }
    public virtual void CloseDoor()
    {
        isClose = true;
        isOpen = false;
    }
}
