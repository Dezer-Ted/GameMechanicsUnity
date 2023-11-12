using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    
    //Destroys anyobject that enters the trigger to keep the performance up
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
        }
        else if(collision.transform.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
