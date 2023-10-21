using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Vector3 scrollingVelocity;

    [SerializeField]
    bool isAffectedByWind;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ApplyBirdMovement();


    }

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