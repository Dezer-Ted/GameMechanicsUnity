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
    void Update()
    {
        Vector3 traveledDistance = new Vector3();
        scrollingVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<BirdFlyingBehavior>().Velocity;
        traveledDistance = scrollingVelocity * Time.deltaTime;
        gameObject.GetComponentInParent<RepeatingBackground>().ScrolledDistance += (Vector2)traveledDistance;
        transform.position += -scrollingVelocity * 1.5f * Time.deltaTime;
        

    }
}
