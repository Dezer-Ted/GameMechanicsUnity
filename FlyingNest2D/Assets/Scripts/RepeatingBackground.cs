using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> backgrounds = new List<GameObject>();
    [SerializeField]
    Vector2 scrolledDistance = new Vector2(0, 0);
    Vector2 centerStartPos = new Vector2();
    float width, height;
    public Vector2 ScrolledDistance
    {
        get { return scrolledDistance; }
        set { scrolledDistance = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        width = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        height = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            backgrounds.Add(gameObject.transform.GetChild(i).gameObject);
        }
        centerStartPos = backgrounds[4].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOutOfBounds();
    }
    bool CheckIfOutOfBounds()
    {
        if (backgrounds[4].transform.position.y - centerStartPos.y < -height)
        {
            ExpandVertically(true);
            return true;
        }
        else if (backgrounds[4].transform.position.y - centerStartPos.y > width)
        {
            ExpandVertically(false);
            return true;
        }
        if (backgrounds[4].transform.position.x - centerStartPos.x < -width)
        {
            ExpandHorizontally(true);
        }
        else if (backgrounds[4].transform.position.x - centerStartPos.x > width)
        {
            ExpandHorizontally(false);
        }
        return false;
    }

    //Hardcoded position swapping to keep the array easy to read
    //  0/1/2
    //  3/4/5
    //  6/7/8
    void ExpandVertically(bool isExpandingUp)
    {
        List<GameObject> tempList = new List<GameObject>();

        if (isExpandingUp)
        {
            ExpandUp(tempList);
        }
        else
        {
            ExpandDown(tempList);
        }
    }

    private void ExpandUp(List<GameObject> tempList)
    {
        tempList.Add(backgrounds[0]);
        tempList.Add(backgrounds[1]);
        tempList.Add(backgrounds[2]);
        tempList.Add(backgrounds[3]);
        tempList.Add(backgrounds[4]);
        tempList.Add(backgrounds[5]);

        backgrounds[0] = backgrounds[6];
        backgrounds[1] = backgrounds[7];
        backgrounds[2] = backgrounds[8];

        backgrounds[3] = tempList[0];
        backgrounds[4] = tempList[1];
        backgrounds[5] = tempList[2];
        backgrounds[6] = tempList[3];
        backgrounds[7] = tempList[4];
        backgrounds[8] = tempList[5];

        for (int i = 0; i < 3; ++i)
        {
            backgrounds[i].transform.position += new Vector3(0, height * 3, 0);
        }
        centerStartPos.y = backgrounds[4].transform.position.y;

    }
    private void ExpandDown(List<GameObject> tempList)
    {
        tempList.Add(backgrounds[3]);
        tempList.Add(backgrounds[4]);
        tempList.Add(backgrounds[5]);
        tempList.Add(backgrounds[6]);
        tempList.Add(backgrounds[7]);
        tempList.Add(backgrounds[8]);

        backgrounds[6] = backgrounds[0];
        backgrounds[7] = backgrounds[1];
        backgrounds[8] = backgrounds[2];

        backgrounds[0] = tempList[0];
        backgrounds[1] = tempList[1];
        backgrounds[2] = tempList[2];
        backgrounds[3] = tempList[3];
        backgrounds[4] = tempList[4];
        backgrounds[5] = tempList[5];

        for (int i = 0; i < 3; ++i)
        {
            backgrounds[6 + i].transform.position -= new Vector3(0, height * 3, 0);
        }
        centerStartPos.y = backgrounds[4].transform.position.y;

    }
    private void ExpandHorizontally(bool isExpandingRight)
    {
        List<GameObject> tempList = new List<GameObject>();

        if (isExpandingRight)
        {
            ExpandRight(tempList);
        }
        else
        {
            ExpandLeft(tempList);
        }
    }
    //Hardcoded position swapping to keep the array easy to read
    //  2/1/0
    //  5/4/3
    //  8/7/6
    private void ExpandRight(List<GameObject> tempList)
    {
        tempList.Add(backgrounds[2]);
        tempList.Add(backgrounds[5]);
        tempList.Add(backgrounds[8]);
        tempList.Add(backgrounds[1]);
        tempList.Add(backgrounds[4]);
        tempList.Add(backgrounds[7]);

        for (int i = 0; i < 3; ++i)
        {
            backgrounds[i*3].transform.position += new Vector3(width * 3, 0, 0);
        }

        backgrounds[2] = backgrounds[0];
        backgrounds[5] = backgrounds[3];
        backgrounds[8] = backgrounds[6];

        backgrounds[1] = tempList[0];
        backgrounds[4] = tempList[1];
        backgrounds[7] = tempList[2];
        backgrounds[0] = tempList[3];
        backgrounds[3] = tempList[4];
        backgrounds[6] = tempList[5];


        centerStartPos.x = backgrounds[4].transform.position.x;

    }
    private void ExpandLeft(List<GameObject> tempList)
    {
        tempList.Add(backgrounds[0]);
        tempList.Add(backgrounds[3]);
        tempList.Add(backgrounds[6]);
        tempList.Add(backgrounds[1]);
        tempList.Add(backgrounds[4]);
        tempList.Add(backgrounds[7]);

        for (int i = 0; i < 3; ++i)
        {
            backgrounds[i * 3+2].transform.position -= new Vector3(width * 3, 0, 0);
        }

        backgrounds[0] = backgrounds[2];
        backgrounds[3] = backgrounds[5];
        backgrounds[6] = backgrounds[8];

        backgrounds[1] = tempList[0];
        backgrounds[4] = tempList[1];
        backgrounds[7] = tempList[2];
        backgrounds[2] = tempList[3];
        backgrounds[5] = tempList[4];
        backgrounds[8] = tempList[5];


        centerStartPos.x = backgrounds[4].transform.position.x;
    }
}