using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Vector3 scrollingVelocity;
    
    
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
        transform.position += (-scrollingVelocity+ (Vector3)WindManager.Instance.WindVelocity) * Time.deltaTime;
    }
}
