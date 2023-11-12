using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScrollingObject :  MonoBehaviour
{
    private Vector3 scrollingVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateScrolledDistance();

    }
    //Updates Scrolls object and calculates the distance
    private void CalculateScrolledDistance()
    {
        // This is not good Code! Im doing twice the calculations but I dont think it'll impact performance
        Vector3 traveledDistance = new Vector3();
        scrollingVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<BirdFlyingBehavior>().Velocity;
        traveledDistance = scrollingVelocity * Time.deltaTime;
        transform.position += -scrollingVelocity * 1.5f * Time.deltaTime;
        gameObject.GetComponentInParent<RepeatingBackground>().ScrolledDistance += (Vector2)traveledDistance;
    }
}
