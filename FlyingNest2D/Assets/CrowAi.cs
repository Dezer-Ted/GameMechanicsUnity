using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAi : MonoBehaviour
{
    // Start is called before the first frame update
    public enum EnemyState
    {
        wandering,
        eating,
        hunting
    }
    EnemyState currentState = EnemyState.wandering;
    Vector2 targetPos;
    [SerializeField]
    float movementSpeed = 10;
    void Start()
    {
        targetPos = new Vector2 (0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }
    void MoveToTarget()
    {
        Vector3 direction = (Vector3)targetPos - transform.position;
        direction.Normalize();
        LookAtTarget(direction);
        transform.position += movementSpeed * Time.deltaTime * direction;
    }
    void LookAtTarget(Vector3 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3(0, 0, angle-90);
    }
}
