using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WindManager : MonoBehaviour
{
    [SerializeField]
    float windSpeed;
    [SerializeField]
    float windAngleDEG;
    [SerializeField]
    Vector2 windVelocity;
    [SerializeField]
    float directionWaitMin, directionWaitMax, windWaitMin, windWaitMax;
    [SerializeField]
    RectTransform windNeedle;
    [SerializeField]
    List<GameObject> winds;
    [SerializeField]
    float turningSpeed;


    private bool isTurning;
    private float desiredAngle;
    private static WindManager instance = null;
    GameObject currentWindVFX;
    Coroutine windRotator;
    public Vector2 WindVelocity
    {
        get { return windVelocity; }
        private set { windVelocity = value; }
    }
    // Start is called before the first frame update

    public static WindManager Instance { get { return instance; } private set { instance = value; } }
    private void Awake()
    {
        if(Instance != null && Instance!=this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {

        //WindAngleDEG = Random.Range(0, 360);
        //windVelocity = new Vector2(Mathf.Cos(WindAngleDEG * Mathf.Deg2Rad) * windSpeed, Mathf.Sin(WindAngleDEG * Mathf.Deg2Rad) * windSpeed);
        RandomizeWindDirection();
        //StartCoroutine(WaitForWindDirection());
        StartCoroutine(WaitForWindVFX());
    }

    // Update is called once per frame
    void Update()
    {
        if(isTurning)
        {
            windAngleDEG = Mathf.MoveTowardsAngle(windAngleDEG,desiredAngle,turningSpeed*Time.deltaTime);
            windVelocity = new Vector2(Mathf.Cos(windAngleDEG * Mathf.Deg2Rad) * windSpeed, Mathf.Sin(windAngleDEG * Mathf.Deg2Rad) * windSpeed);
            windNeedle.transform.rotation = Quaternion.Euler(windNeedle.transform.eulerAngles.x, windNeedle.transform.eulerAngles.y, windAngleDEG - 90);
            if (windAngleDEG<0)
            {
                windAngleDEG = 360 - Mathf.Abs(windAngleDEG);
            }
            if (windAngleDEG == desiredAngle)
                isTurning = false;
        }
    }
    IEnumerator WaitForWindDirection()
    {
        yield return new WaitForSeconds(Random.Range(directionWaitMin, directionWaitMax));
        RandomizeWindDirection();

    }
    IEnumerator WaitForWindVFX()
    {
        yield return new WaitForSeconds(Random.Range(windWaitMin, windWaitMax));
        UpdateWindVFX();
        StartCoroutine(WaitForWindVFX());

    }
    void RandomizeWindDirection()
    {
        desiredAngle = Random.Range(0, 360);
        
        isTurning = true;
        //if (windRotator != null)
        //    StopCoroutine(windRotator);
        //windRotator = StartCoroutine(RotateWind(newAngle));
        StartCoroutine(WaitForWindDirection());
    }
    void UpdateWindVFX()
    {
        Destroy(currentWindVFX);
        Vector3 windPos = new Vector3(
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(Screen.height * (1.0f / 4.0f), 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * (3.0f / 4.0f), 0)).x),
            Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height * (1.0f / 4.0f))).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height*(3.0f/4.0f))).y),
            -3
            );

        if(Random.Range(0,10)>=7)
            currentWindVFX = Instantiate(winds[1], windPos, Quaternion.Euler(new Vector3(0,0,windAngleDEG)));
        else
            currentWindVFX = Instantiate(winds[0], windPos, Quaternion.Euler(new Vector3(0, 0, windAngleDEG)));

    }
}
