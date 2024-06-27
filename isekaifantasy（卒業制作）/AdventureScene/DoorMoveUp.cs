using UnityEngine;

public class DoorMoveUp : DoorBace
{
    [SerializeField]
    private int targetY = 1;
    private Vector3 targetPos;


    private void Start()
    {
        targetPos = new Vector3(defultPos.x, defultPos.y - targetY, defultPos.z);
        door.position = targetPos;
    }

    private void FixedUpdate()
    {
        if (isOpen)
        {
            float distance = Vector3.Distance(door.position, defultPos);
            if (distance > 0.8f) door.position = Vector3.Lerp(door.position, defultPos, Time.deltaTime * speed);
            else
            {
                door.position = defultPos;
                isOpen = false;
            }
        }
        else
        if (isClose)
        {
            float distance = Vector3.Distance(door.position, targetPos);
            if (distance > 0.02f) door.position = Vector3.Lerp(door.position, targetPos, Time.deltaTime * speed);
            else
            {
                door.position = targetPos;
                isClose = false;
            }
        }
    }
}
