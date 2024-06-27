using UnityEngine;

public class DoorMoveNone : DoorBace
{
    public override void OpenDoor()
    {
        isOpen = false;
        isClose = false;
    }
    public override void CloseDoor()
    {
        isClose = false;
        isOpen = false;
    }
}
