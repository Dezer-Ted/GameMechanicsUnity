using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BirdInventory : MonoBehaviour
{
    [SerializeField]
    private int foodCount = 0;
    [SerializeField]
    GameObject foodCountText;
    [SerializeField]
    float foodToStaminaRatio =10;

    float foodMultiplier = 1;
    public int FoodCount 
    {
        get { return foodCount; }
        set 
        {
            foodCount = value;
            foodCountText.GetComponent<TextMeshProUGUI>().text = foodCount.ToString();
        }
    }
    //Reduces Food in bag to add to stamina
    //Food effiency starts at 100% and decreases by 10% every time the players eats to a minimum of 10%
    public void EatFood()
    {
        if (GameManager.Instance.GamePaused)
            return;
        if(foodCount > 0)
        {
            --FoodCount;
            gameObject.GetComponent<BirdStamina>().AddStamina((int)(foodToStaminaRatio * foodMultiplier));
            if (foodToStaminaRatio >= foodMultiplier / foodToStaminaRatio) 
            {
                foodMultiplier -= foodMultiplier / foodToStaminaRatio;
            }
        }
    }
    public void ResetFoodMultiplier()
    {
        foodMultiplier = 1;
    }

}
