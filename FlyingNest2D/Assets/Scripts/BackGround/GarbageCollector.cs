using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
