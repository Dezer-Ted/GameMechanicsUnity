using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TutorialManager : MonoBehaviour
{
    //Singleton Class
    //This class turns off/on the necessary tutorial parts
    //When a tutorial is shown it will be ended by clicking in this class update
    private static TutorialManager instance;
    public static TutorialManager Instance { get { return instance; } private set { instance = value; } }


    [SerializeField]
    GameObject tutorialBackdrop;
    [SerializeField]
    GameObject clickToStart;
    [SerializeField]
    GameObject clickToContinue;
    [SerializeField]
    GameObject staminaMask;
    [SerializeField]
    GameObject staminaText;
    [SerializeField]
    GameObject staminaArrow;
    [SerializeField]
    GameObject compassMask;
    [SerializeField]
    GameObject compassText;
    [SerializeField]
    GameObject compassArrow;
    [SerializeField]
    GameObject inventoryMask;
    [SerializeField]
    GameObject inventoryText;
    [SerializeField]
    GameObject inventoryArrow;
    [SerializeField]
    GameObject scoreMask;
    [SerializeField]
    GameObject scoreText;
    [SerializeField]
    GameObject scoreArrow;
    [SerializeField]
    GameObject enemyMask;
    [SerializeField]
    GameObject enemyText;
    [SerializeField]
    GameObject foodText;

    [SerializeField]
    Camera mainCam;
    public bool StaminaTutorialFinished { get; private set; }
    public bool CompassTutorialFinished { get; private set; }
    public bool ScoreTutorialFinished { get; private set; }
    public bool EnemyTutorialFinished { get; private set; }
    public bool FoodTutorialFinished { get; private set; }
    

    bool activeTutorial = false;
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            if (PersistentDataHolder.Instance == null)
                return;
            CompassTutorialFinished = PersistentDataHolder.Instance.CompassTutorial;
            ScoreTutorialFinished = PersistentDataHolder.Instance.ScoreTutorial;
            EnemyTutorialFinished = PersistentDataHolder.Instance.EnemyTutorial;
            FoodTutorialFinished = PersistentDataHolder.Instance.FoodTutorial;
            StaminaTutorialFinished = PersistentDataHolder.Instance.StaminaTutorial;
        }
    }
    private void Update()
    {
        if (activeTutorial && Input.GetMouseButtonDown(0))
        {
            if (staminaText.activeSelf)
            {
                ToggleStaminaTutorial();
            }
            else if (compassMask.activeSelf)
            {
                ToggleCompassTutorial();
            }
            else if (inventoryText.activeSelf)
            {
                ToggleScoreTutorial();
            }
            else if(enemyMask.activeSelf)
            {
                ToggleEnemyTutorial(new Vector2(0,0));
            }
            else if(foodText.activeSelf)
            {
                ToggleFoodTutorial();
            }
        }
    }
    public void HideClickToStart()
    {
        clickToStart.SetActive(false);
    }
    public void ToggleClickToContinue()
    {
        if(clickToContinue.activeSelf)
        {
            clickToContinue.SetActive(false);
        }
        else
        {
            clickToContinue.SetActive(true);
        }
    }
    public void ToggleStaminaTutorial()
    {
        if(staminaMask.activeSelf)
        {
            activeTutorial = false;
            ToggleTutorialBackdrop();
            ToggleClickToContinue();
            staminaMask.SetActive(false);
            staminaText.SetActive(false);
            staminaArrow.SetActive(false);
            Time.timeScale = 1;
            GameManager.Instance.GamePaused = false;
            PersistentDataHolder.Instance.StaminaTutorial = true;
            GameManager.Instance.StartTimer();

        }
        else
        {
            StaminaTutorialFinished = true;
            activeTutorial = true;
            ToggleClickToContinue();
            ToggleTutorialBackdrop();
            staminaMask.SetActive(true);
            staminaText.SetActive(true);
            staminaArrow.SetActive(true);
            Time.timeScale = 0;
            GameManager.Instance.GamePaused = true;
            GameManager.Instance.StopTimer();

        }
    }
    public void ToggleTutorialBackdrop()
    {
        if(tutorialBackdrop.activeSelf)
            tutorialBackdrop.SetActive(false);
        else
            tutorialBackdrop.SetActive(true);
    }
    public void ToggleCompassTutorial()
    {
        if (compassMask.activeSelf)
        {
            activeTutorial = false;
            ToggleTutorialBackdrop();
            ToggleClickToContinue();
            compassMask.SetActive(false);
            compassArrow.SetActive(false);
            compassText.SetActive(false);
            Time.timeScale = 1;
            GameManager.Instance.GamePaused = false;
            PersistentDataHolder.Instance.CompassTutorial = true;
            GameManager.Instance.StartTimer();


        }
        else
        {
            CompassTutorialFinished = true;
            activeTutorial = true;
            ToggleClickToContinue();
            ToggleTutorialBackdrop();
            compassArrow.SetActive(true);
            compassMask.SetActive(true);
            compassText.SetActive(true);
            Time.timeScale = 0;
            GameManager.Instance.GamePaused = true;
            GameManager.Instance.StopTimer();

        }
    }
    public void ToggleScoreTutorial()
    {
        if (inventoryMask.activeSelf)
        {
            activeTutorial = false;
            ToggleTutorialBackdrop();
            ToggleClickToContinue();
            inventoryMask.SetActive(false);
            inventoryText.SetActive(false);
            inventoryArrow.SetActive(false);
            scoreMask.SetActive(false);
            scoreText.SetActive(false);
            scoreArrow.SetActive(false);

            Time.timeScale = 1;
            GameManager.Instance.GamePaused = false;
            PersistentDataHolder.Instance.ScoreTutorial = true;
            GameManager.Instance.StartTimer();

        }
        else
        {
            ScoreTutorialFinished = true;
            activeTutorial = true;
            ToggleClickToContinue();
            ToggleTutorialBackdrop();
            inventoryMask.SetActive(true);
            inventoryText.SetActive(true);
            inventoryArrow.SetActive(true);
            scoreMask.SetActive(true);
            scoreText.SetActive(true);
            scoreArrow.SetActive(true);
            Time.timeScale = 0;
            GameManager.Instance.GamePaused = true;
            GameManager.Instance.StopTimer();

        }
    }
    public void ToggleEnemyTutorial(Vector3 enemyPos)
    {
        if (enemyMask.activeSelf)
        {
            activeTutorial = false;
            ToggleTutorialBackdrop();
            ToggleClickToContinue();
            enemyMask.SetActive(false);
            enemyText.SetActive(false);
            Time.timeScale = 1;
            GameManager.Instance.GamePaused = false;
            PersistentDataHolder.Instance.EnemyTutorial = true;
            GameManager.Instance.StartTimer();

        }
        else
        {
            EnemyTutorialFinished = true;

            Vector3 screenPos = mainCam.WorldToScreenPoint(enemyPos);
            float xPos = screenPos.x - (Screen.width / 2);
            float yPos = screenPos.y - (Screen.height / 2);
            enemyMask.GetComponent<RectTransform>().localPosition = new Vector2(xPos,yPos);

            activeTutorial = true;
            ToggleClickToContinue();
            ToggleTutorialBackdrop();
            enemyMask.SetActive(true);
            enemyText.SetActive(true);
            Time.timeScale = 0;
            GameManager.Instance.GamePaused = true;
            GameManager.Instance.StopTimer();

        }
    }
    public void ToggleFoodTutorial()
    {
        if (foodText.activeSelf)
        {
            activeTutorial = false;
            ToggleTutorialBackdrop();
            ToggleClickToContinue();
            inventoryMask.SetActive(false);
            foodText.SetActive(false);
            staminaMask.SetActive(false);
            PersistentDataHolder.Instance.FoodTutorial = true;
            Time.timeScale = 1;
            GameManager.Instance.GamePaused = false;
            GameManager.Instance.StartTimer();

        }
        else
        {
            FoodTutorialFinished = true;
            activeTutorial = true;
            ToggleClickToContinue();
            ToggleTutorialBackdrop();
            inventoryMask.SetActive(true);
            foodText.SetActive(true);
            staminaMask.SetActive(true);

            Time.timeScale = 0;
            GameManager.Instance.GamePaused = true;
            GameManager.Instance.StopTimer();

        }
    }
}
