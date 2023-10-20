using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestBehavior : MonoBehaviour
{
    [SerializeField]
    RectTransform needle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = player.transform.position - transform.position;
        //dir = player.transform.InverseTransformDirection(dir);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        needle.transform.rotation = Quaternion.Euler(needle.transform.eulerAngles.x, needle.transform.eulerAngles.y, angle+90);
    }
   
}
