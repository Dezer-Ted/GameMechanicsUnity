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
    float directionWaitMin, directionWaitMax, windWaitMin, windWaitMax;
    [SerializeField]
    RectTransform windNeedle;
    [SerializeField]
    List<GameObject> winds;

    private static WindManager instance = null;
    GameObject currentWindVFX;
    public Vector2 WindVelocity
    {
        get { return windVelocity; }
        private set { windVelocity = value; }
    }
    // Start is called before the first frame update

    public static WindManager Instance { get; private set; }
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
        StartCoroutine(WaitForWindDirection());
        StartCoroutine(WaitForWindVFX());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForWindDirection()
    {
        yield return new WaitForSeconds(Random.Range(directionWaitMin, directionWaitMax));
        RandomizeWindDirection();
        StartCoroutine(WaitForWindDirection());

    }
    IEnumerator WaitForWindVFX()
    {
        yield return new WaitForSeconds(Random.Range(windWaitMin, windWaitMax));
        UpdateWindVFX();
        StartCoroutine(WaitForWindVFX());

    }
    void RandomizeWindDirection()
    {
        WindAngleDEG = Random.Range(0, 360);
        windVelocity = new Vector2(Mathf.Cos(WindAngleDEG * Mathf.Deg2Rad) * windSpeed, Mathf.Sin(WindAngleDEG * Mathf.Deg2Rad) * windSpeed);
        windNeedle.transform.rotation = Quaternion.Euler(windNeedle.transform.eulerAngles.x, windNeedle.transform.eulerAngles.y, WindAngleDEG-90);
        UpdateWindVFX();
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
            currentWindVFX = Instantiate(winds[1], windPos, Quaternion.Euler(new Vector3(0,0,WindAngleDEG)));
        else
            currentWindVFX = Instantiate(winds[0], windPos, Quaternion.Euler(new Vector3(0, 0, WindAngleDEG)));

    }
}
