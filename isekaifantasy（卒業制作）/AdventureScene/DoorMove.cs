using UnityEngine;

public class DoorMove : DoorBace
{
    [SerializeField]
    private MoveWay moveWay = MoveWay.down;

    private Vector3 targetPos;


    public enum MoveWay
    {
        down = 0,
        left,
        right,
    }

    private void Start()
    {
        if (moveWay == MoveWay.down)
            targetPos = new Vector3(defultPos.x, defultPos.y - Mathf.Abs(box.size.y) - 0.1f, defultPos.z);
    }

    private void FixedUpdate()
    {
        if (isOpen)
        {
            float distance = Vector3.Distance(door.position, targetPos);
            if (distance > 3f) door.position = Vector3.Lerp(door.position, targetPos, Time.deltaTime * speed);
            else
            {
                door.position = targetPos;
                isOpen = false;
            }
        }
        else
        if (isClose)
        {
            float distance = Vector3.Distance(door.position, defultPos);
            if (distance > 0.02f) door.position = Vector3.Lerp(door.position, defultPos, Time.deltaTime * speed);
            else
            {
                door.position = defultPos;
                isClose = false;
            }
        }
    }
}
