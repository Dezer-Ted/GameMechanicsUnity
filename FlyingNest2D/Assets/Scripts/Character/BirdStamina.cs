using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdStamina : MonoBehaviour
{
    [SerializeField]
    GameObject staminaBar;

    [SerializeField]
    float maxStamina;
    [SerializeField,]
    float currentStamina;
    [SerializeField]
    float staminaDrainPerSecond;
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<BirdFlyingBehavior>().CurrentState != BirdFlyingBehavior.PlayerState.inNest)
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            float staminaPercentage = currentStamina / maxStamina;
            staminaBar.GetComponent<Slider>().value = staminaPercentage;
        }
    }

    public void ResetStamina()
    {
        currentStamina = maxStamina;
    }
}
