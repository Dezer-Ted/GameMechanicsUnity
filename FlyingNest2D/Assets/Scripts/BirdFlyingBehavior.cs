using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BirdFlyingBehavior : MonoBehaviour
{
    private enum PlayerState { following, idling, stopped};
    // Start is called before the first frame update
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 target;
    [SerializeField]
    private float idleRange;
    [SerializeField]
    private float idleCirclingRange;
    [SerializeField]
    private PlayerState playerState;
    
    private float idleAngle = 0;
    private Vector3 mousePos;
    private Vector3 lastMousePos;
    void Start()
    {
        playerState = PlayerState.following;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        target = mousePos;
        switch (playerState)
        {
            case PlayerState.following:
                FollowTarget(RotateToMouse());
                break;

            case PlayerState.idling:
                idleAngle += 1;
                SetIdleTarget();
                FollowTarget(RotateToMouse());
                break;

        }
        if (CheckForIdling())
            playerState = PlayerState.idling;
        else
        {
            playerState = PlayerState.following;
            idleAngle = 0;
        }
        lastMousePos = mousePos;
        transform.position += velocity * Time.deltaTime;

    }

    private void FollowTarget(float angle)
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
        if ((Mathf.Abs(mousePos.x - lastMousePos.x) > 1e-9) || (Mathf.Abs(mousePos.y - lastMousePos.y) > 1e-9))
            return false;

        if ((target - transform.position).magnitude >= idleRange&&(playerState != PlayerState.idling))
            return false;

        return true;
    }
    private void SetIdleTarget()
    {
        
       target += new Vector3(Mathf.Cos(Mathf.Deg2Rad * idleAngle) * idleCirclingRange, Mathf.Sin(Mathf.Deg2Rad * idleAngle) * idleCirclingRange);
    }
}
