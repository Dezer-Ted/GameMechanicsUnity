using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestBehavior : MonoBehaviour
{
    [SerializeField]
    RectTransform needle;
    void Update()
    {
        UpdateHomeNeedle();
    }
    //Updates the white compass needle to always point to the player
    private void UpdateHomeNeedle()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Vector3 dir = player.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        needle.transform.rotation = Quaternion.Euler(needle.transform.eulerAngles.x, needle.transform.eulerAngles.y, angle + 90);
    }
}
