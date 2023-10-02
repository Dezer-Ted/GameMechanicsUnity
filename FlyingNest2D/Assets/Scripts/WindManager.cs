using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    [SerializeField]
    float windSpeed;
    [SerializeField]
    float WindAngleDEG;
    [SerializeField]
    Vector2 windVelocity;
    [SerializeField]
    float waitMin, waitMax;
    [SerializeField]
    RectTransform windNeedle;
    public Vector2 WindVelocity
    {
        get { return windVelocity; }
        private set { windVelocity = value; }
    }
    // Start is called before the first frame update
    void Start()
    {

        //WindAngleDEG = Random.Range(0, 360);
        //windVelocity = new Vector2(Mathf.Cos(WindAngleDEG * Mathf.Deg2Rad) * windSpeed, Mathf.Sin(WindAngleDEG * Mathf.Deg2Rad) * windSpeed);
        RandomizeWindDirection();
        StartCoroutine(WaitForFunction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForFunction()
    {
        yield return new WaitForSeconds(Random.Range(waitMin, waitMax));
        RandomizeWindDirection();
        StartCoroutine(WaitForFunction());

    }
    void RandomizeWindDirection()
    {
        WindAngleDEG = Random.Range(0, 360);
        windVelocity = new Vector2(Mathf.Cos(WindAngleDEG * Mathf.Deg2Rad) * windSpeed, Mathf.Sin(WindAngleDEG * Mathf.Deg2Rad) * windSpeed);
        windNeedle.transform.rotation = Quaternion.Euler(windNeedle.transform.eulerAngles.x, windNeedle.transform.eulerAngles.y, WindAngleDEG-90);
    }
    
}
