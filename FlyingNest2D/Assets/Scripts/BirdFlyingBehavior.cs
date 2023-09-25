using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BirdFlyingBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private Vector3 target;
    [SerializeField]
    private float idleRange;

    private enum PlayerState { following, idling, stopped};
    private PlayerState playerState;
    private float idleAngle = 0;

    private Vector3 lastTarget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;
        float angle = RotateToMouse();
        switch (playerState)
        {
            case PlayerState.following:
                FollowCursor(angle);
                break;

            case PlayerState.idling:

                break;

        }
        lastTarget = target;
        if (CheckForIdling())
            playerState = PlayerState.idling;

        transform.position += velocity * Time.deltaTime;

    }

    private void FollowCursor(float angle)
    {
        velocity = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * speed, Mathf.Sin(Mathf.Deg2Rad * angle) * speed);
    }

    private float RotateToMouse()
    {
        Vector2 direction = target - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle - 90);
        return angle;
    }
    private bool CheckForIdling()
    {
        if (target != lastTarget)
            return false;
        if ((target - transform.position).magnitude <= idleRange)
            return false;

        return true;
    }
    private void SetIdleTarget()
    {
        Vector3 currentPos  = transform.position;
        currentPos += Vector3()
    }
}
