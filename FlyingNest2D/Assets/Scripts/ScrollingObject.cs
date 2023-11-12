using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Vector3 scrollingVelocity;

    [SerializeField]
    bool isAffectedByWind;
    protected virtual void Update()
    {
        ApplyBirdMovement();
    }
    //Moves the Object in relation to the player
    //optionally can include the wind velocity
    private void ApplyBirdMovement()
    {
        scrollingVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<BirdFlyingBehavior>().Velocity;
        if(isAffectedByWind)
        {
            transform.position += ((-scrollingVelocity*1.5f) + (Vector3)WindManager.Instance.WindVelocity) *Time.deltaTime;
        }
        else
        {
            transform.position += (-scrollingVelocity * 1.5f) * Time.deltaTime;

        }
    }
}