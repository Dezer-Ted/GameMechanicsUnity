using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BirdStamina : MonoBehaviour
{
    [SerializeField]
    GameObject staminaBar;
    [SerializeField]
    GameObject staminaFill;
    [SerializeField]
    float maxStamina;
    [SerializeField,]
    float currentStamina;
    [SerializeField]
    float staminaDrainPerSecond;
    [SerializeField]
    AudioSource staminaWarningSound;

    bool wasWarned = false;
    
    void Start()
    {
        currentStamina = maxStamina;
    }
    void Update()
    {
        if(gameObject.GetComponent<BirdFlyingBehavior>().CurrentState != BirdFlyingBehavior.PlayerState.inNest)
        {
            
            HandleStaminaEvents(CalculateStamina());
        }
    }
    //Checks if stamina is below a certain threshhold and triggers the fitting events
    private void HandleStaminaEvents(float staminaPercentage)
    {
        if (currentStamina <= 0 && !GameManager.Instance.GamePaused)
            GameManager.Instance.GameOver();
        else if (staminaPercentage < 0.3f)
        {
            if (!wasWarned)
            {
                staminaWarningSound.Play();
                wasWarned = true;
            }
            staminaFill.GetComponent<Image>().color = new Color(0xE7 / 255f, 0x2E / 255f, 0x2D / 255f);
            if (!TutorialManager.Instance.FoodTutorialFinished)
                TutorialManager.Instance.ToggleFoodTutorial();
        }
        else if (staminaPercentage < 0.5f)
        {
            wasWarned = false;
            staminaFill.GetComponent<Image>().color = new Color(0xBE / 255f, 0xA1 / 255f, 0x23 / 255f);
            if (!TutorialManager.Instance.StaminaTutorialFinished)
                TutorialManager.Instance.ToggleStaminaTutorial();
        }
        else if (staminaPercentage > 0.5f)
        {
            wasWarned = false;
            staminaFill.GetComponent<Image>().color = new Color(0x77 / 255f, 0xBE / 255f, 0x23 / 255f);
        }
    }
    //Calculate the current stamina according to the drain rate. Updates the visualisation
    private float CalculateStamina()
    {
        currentStamina -= staminaDrainPerSecond * Time.deltaTime;
        float staminaPercentage = currentStamina / maxStamina;
        staminaBar.GetComponent<Slider>().value = staminaPercentage;
        return staminaPercentage;
    }

    public void ResetStamina()
    {
        currentStamina = maxStamina;
    }
    public void AddStamina(int amount)
    {
        currentStamina += amount;
    }
}
