using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        currentDirection = - transform.position.normalized; //Locks onto player coordinates so the enemy doesnt just leave
        currentTimeToChange = maxTimeToChange;
        chargeDistance = transform.GetComponent<CircleCollider2D>().radius;
    }
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.wandering:
                ChangeDirection();
                DirectionalMovement();
                break;
            case EnemyState.hunting:
                Hunting();
                break;
        }
    }
    //Charges the player in a pattern of multiple attacks
    //The crow saves the player location and changes its direction to that point while gaining some speed.
    //The crow charges the player three times before losing interest.
    private void Hunting()
    {
        ChargePlayer();
        DirectionalMovement();
        if (traveledDistance.sqrMagnitude > chargeDistance * chargeDistance)
        {
            isCharging = false;
            currentCharges++;
            traveledDistance = new Vector2(0, 0);
        }
        if (currentCharges >= maxCharges)
        {
            SetState(EnemyState.wandering);
            currentCharges = 0;
        }
    }
    
    void LookAtTarget()
    {
        float angle = Vector2.SignedAngle(Vector2.right, currentDirection);
        transform.eulerAngles = new Vector3(0, 0, angle-90);
    }
    //This Movement follows the currentDirection with the currentSpeed
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
            GetNewAngle();
        }
    }
    //Get A Random angle in a 180 degrees window while staying in 360°
    void GetNewAngle()
    {
        currentAngle = currentAngle + Random.Range(-90, 90);
        if (currentAngle < 0)
            currentAngle = 360 + currentAngle;
        if (currentAngle > 360)
            currentAngle = currentAngle - 360;
        currentDirection = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
    }
    //Sets the state and adjusts speed accordingly
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
        Vector3 newDirection = (  player.transform.position- transform.position);
        newDirection.Normalize();
        currentDirection = newDirection;
        isCharging = true;
    }
    public EnemyState GetState()
    {
        return currentState;
    }
}
