using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class WindManager : MonoBehaviour
{
    //Singleton class
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
    [SerializeField]
    float warningLength;
    [SerializeField]
    GameObject warningText;
    [SerializeField]
    AudioSource windSound;
    private bool isTurning;
    private float desiredAngle;
    private static WindManager instance = null;
    GameObject currentWindVFX;
    public Vector2 WindVelocity
    {
        get { return windVelocity; }
        private set { windVelocity = value; }
    }

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
        RandomizeWindDirection();
        StartCoroutine(WaitForWindVFX());
    }

    void Update()
    {
        TurnWindSlowly();
    }
    //Lerps the current wind angle to the wanted wind angle while playing a wind soundeffect
    private void TurnWindSlowly()
    {
        if (isTurning)
        {
            windAngleDEG = Mathf.MoveTowardsAngle(windAngleDEG, desiredAngle, turningSpeed * Time.deltaTime);
            windVelocity = new Vector2(Mathf.Cos(windAngleDEG * Mathf.Deg2Rad) * windSpeed, Mathf.Sin(windAngleDEG * Mathf.Deg2Rad) * windSpeed);
            windNeedle.transform.rotation = Quaternion.Euler(windNeedle.transform.eulerAngles.x, windNeedle.transform.eulerAngles.y, windAngleDEG - 90);
            if (windAngleDEG < 0)
            {
                windAngleDEG = 360 - Mathf.Abs(windAngleDEG);
            }
            if (windAngleDEG == desiredAngle)
            {
                isTurning = false;
                windSound.Stop();
            }
        }
    }

    IEnumerator WaitForWindDirection()
    {
        yield return new WaitForSeconds(Random.Range(directionWaitMin, directionWaitMax)-warningLength);
        StartCoroutine(WindChangeWarning());

    }
    IEnumerator WindChangeWarning()
    {
        float timer = 0;
        warningText.SetActive(true);
        while(timer <warningLength)
        {
            timer+= Time.deltaTime;
            warningText.GetComponent<TextMeshProUGUI>().text = "WIND CHANGING IN " + Mathf.Floor(warningLength-timer).ToString();
            yield return null;
        }
        RandomizeWindDirection();
        warningText.SetActive(false);
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
        windSound.Play();
        StartCoroutine(WaitForWindDirection());
    }
    //Creates WindVFX that always points in the wind direction and deleting those that dont.
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
