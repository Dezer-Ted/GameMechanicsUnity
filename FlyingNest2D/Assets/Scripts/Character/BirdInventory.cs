using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BirdInventory : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int foodCount = 0;

    [SerializeField]
    GameObject foodCountText;
    public int FoodCount 
    {
        get { return foodCount; }
        set 
        {
            foodCount = value;
            foodCountText.GetComponent<TextMeshProUGUI>().text = foodCount.ToString();
        }
    }

}
