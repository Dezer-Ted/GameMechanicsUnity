using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BirdFlyingBehavior : MonoBehaviour
{
    //Privates
    public enum PlayerState { following, idling, inNest };
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
    private PlayerState currentState;

    private float idleAngle = 0;
    private Vector3 mousePos;
    private Vector3 lastMousePos;

    //Public
    public Vector3 Velocity
    {
        get { return velocity; }
        private set { velocity = value; }
    }
    public PlayerState CurrentState { get; private set; }
    void Start()
    {
        currentState = PlayerState.following;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        target = mousePos;
        switch (currentState)
        {
            case PlayerState.following:
                FollowTarget(RotateToMouse());
                break;

            case PlayerState.idling:
                idleAngle += 1;
                SetIdleTarget();
                FollowTarget(RotateToMouse());
                break;
            case PlayerState.inNest:
                if(Input.GetMouseButtonDown(0))
                {
                    currentState = PlayerState.following;
                }
                RotateToMouse();
                velocity = (Vector3)WindManager.Instance.WindVelocity/1.5f;
                return;

        }
        if (CheckForIdling())
            currentState = PlayerState.idling;
        else
        {
            currentState = PlayerState.following;
            idleAngle = 0;
        }
        lastMousePos = mousePos;
        //transform.position += velocity * Time.deltaTime;

    }

    private void FollowTarget(float angle)
    {
        velocity = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * speed, Mathf.Sin(Mathf.Deg2Rad * angle) * speed) +  (Vector3)WindManager.Instance.WindVelocity;
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

        if ((target - transform.position).magnitude >= idleRange && (currentState != PlayerState.idling))
            return false;

        return true;
    }
    private void SetIdleTarget()
    {

        target += new Vector3(Mathf.Cos(Mathf.Deg2Rad * idleAngle) * idleCirclingRange, Mathf.Sin(Mathf.Deg2Rad * idleAngle) * idleCirclingRange);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Nest"))
        {
            if(gameObject.GetComponent<BirdInventory>().FoodCount != 0)
            {
                GameManager.Instance.Score = gameObject.GetComponent<BirdInventory>().FoodCount * 100;
            }
            currentState = PlayerState.inNest;

        }
        else if (collision.transform.CompareTag("Food"))
        {
            gameObject.GetComponent<BirdInventory>().FoodCount += 1;
            Destroy(collision.transform.gameObject);
        }
    }
} 