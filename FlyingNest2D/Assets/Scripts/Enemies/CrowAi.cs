using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrowAi : MonoBehaviour
{
    // Start is called before the first frame update
    public enum EnemyState
    {
        wandering,
        eating,
        hunting
    }
    [SerializeField]
    EnemyState currentState = EnemyState.wandering;
    Vector2 targetPos;
    [SerializeField]
    float currentSpeed = 5;
    [SerializeField]
    float wanderSpeed = 5;
    [SerializeField]
    float huntingSpeed = 8;
    [SerializeField]
    Vector3 currentDirection;
    [SerializeField]
    float maxTimeToChange;
    [SerializeField]
    int maxCharges = 3;
    int currentCharges = 0;
    float currentTimeToChange;
    float currentAngle;
    float chargeDistance;
    Vector2 traveledDistance = Vector2.zero;
    bool isCharging = false;
    void Start()
    {
        targetPos = new Vector2 (0, 0);
        currentDirection = transform.forward;
        //StartCoroutine(WanderDirection());
        currentTimeToChange = maxTimeToChange;
        chargeDistance = transform.GetComponent<CircleCollider2D>().radius;
        NewAngle();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.wandering:
                ChangeDirection();
                DirectionalMovement();
                break;
            case EnemyState.hunting:
                ChargePlayer();
                DirectionalMovement();
                if (traveledDistance.sqrMagnitude > chargeDistance * chargeDistance)
                {
                    isCharging = false;
                    currentCharges++;
                    traveledDistance = new Vector2 (0, 0);
                }
                if (currentCharges >= maxCharges)
                {
                    SetState(EnemyState.wandering);
                    currentCharges = 0;
                }
                break;
        }
    }
    void MoveToTarget()
    {
        currentDirection = (Vector3)targetPos - transform.position;
        currentDirection.Normalize();
        LookAtTarget();
        transform.position += currentSpeed * Time.deltaTime * currentDirection;
    }
    void LookAtTarget()
    {
        float angle = Vector2.SignedAngle(Vector2.right, currentDirection);
        transform.eulerAngles = new Vector3(0, 0, angle-90);
    }

    void DirectionalMovement()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (isCharging)
        {
            traveledDistance += (Vector2)(currentSpeed * Time.deltaTime * currentDirection);
        }
        transform.position += currentSpeed * Time.deltaTime * currentDirection;
        LookAtTarget();
    } 
    void ChangeDirection()
    {
        currentTimeToChange -=Time.deltaTime;
        if(currentTimeToChange < 0)
        {
            currentTimeToChange = maxTimeToChange;
            NewAngle();
        }
    }
    void NewAngle()
    {
        currentAngle = currentAngle + Random.Range(-90, 90);
        if (currentAngle < 0)
            currentAngle = 360 + currentAngle;
        if (currentAngle > 360)
            currentAngle = currentAngle - 360;
        currentDirection = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
    }

    public void SetState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.hunting:
                currentSpeed = huntingSpeed;
                currentState = EnemyState.hunting;
                break;
            case EnemyState.wandering:
                currentSpeed = wanderSpeed;
                currentState = EnemyState.wandering;
                break;
        }
    }
    void ChargePlayer()
    {
        if (isCharging)
            return;

        var player = GameObject.FindGameObjectWithTag("Player");
        Vector3 newDirection = (transform.position - player.transform.position);
        newDirection.Normalize();
        currentDirection = newDirection;
        isCharging = true;
    }
}
