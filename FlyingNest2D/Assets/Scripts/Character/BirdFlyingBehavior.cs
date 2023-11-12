using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BirdFlyingBehavior : MonoBehaviour
{
    public enum PlayerState { following, idling, inNest };
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 target;
    [SerializeField]
    private float idleRange;
    [SerializeField]
    private float idleCirclingRange;
    [SerializeField]
    private float invincibilityDuration;
    [SerializeField]
    private int staminaPerHit = 20;
    [SerializeField]
    private PlayerState currentState = PlayerState.inNest;
    [SerializeField]
    GameObject staminaBar;
    [SerializeField]
    AudioSource crowSFX;
    [SerializeField]
    AudioSource fruitPickup;
    [SerializeField]
    AudioSource damageSound;

    private float idleAngle = 0;
    private Vector3 mousePos;
    private Vector3 lastMousePos;
    private bool isInvincible = false;

    public Vector3 Velocity
    {
        get { return velocity; }
        private set { velocity = value; }
    }
    public PlayerState CurrentState { get { return currentState; } private set { currentState = value; } }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        target = mousePos;
        switch (currentState)
        {
            case PlayerState.following:
                FollowTarget(RotateToMouse());
                if (Input.GetMouseButtonDown(0))
                    gameObject.GetComponent<BirdInventory>().EatFood();
                break;
            case PlayerState.idling:
                idleAngle += 1;
                SetIdleTarget();
                FollowTarget(RotateToMouse());
                break;
            case PlayerState.inNest:
                if(Input.GetMouseButtonDown(0))
                {
                    StartFlying();
                }
                RotateToMouse();
                velocity = (Vector3)WindManager.Instance.WindVelocity/1.5f;
                return;
        }
        if (CheckForIdling())
            currentState = PlayerState.idling;
        else
        {
            currentState = PlayerState.following;
            idleAngle = 0;
        }
        lastMousePos = mousePos;
    }
    //Player starts the game and leaves nest
    private void StartFlying()
    {
        currentState = PlayerState.following;
        staminaBar.SetActive(true);
        TutorialManager.Instance.HideClickToStart();
        GameManager.Instance.StartTimer();
        if (!TutorialManager.Instance.CompassTutorialFinished)
            StartCoroutine(WaitForCompassTutorial());
    }

    //Changes the velocity to match the angle of the target adjusted to wind 
    private void FollowTarget(float angle)
    {
        velocity = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * speed, Mathf.Sin(Mathf.Deg2Rad * angle) * speed) +  (Vector3)WindManager.Instance.WindVelocity;
    }
    //Rotates the character to the mouse returning the angle in relation to Vector2.right
    private float RotateToMouse()
    {
        Vector2 direction = target - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        if (GameManager.Instance.GamePaused)
            return angle;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);
        return angle;
    }
    //niche interaction: if the player positions the mouse in a way that it is on the character
    //the character will fly loops instead of glitching out.
    //Did not come up once in playtesting
    private bool CheckForIdling()
    {
        if ((Mathf.Abs(mousePos.x - lastMousePos.x) > 1e-9) || (Mathf.Abs(mousePos.y - lastMousePos.y) > 1e-9))
            return false;

        if ((target - transform.position).magnitude >= idleRange && (currentState != PlayerState.idling))
            return false;

        return true;
    }
    private void SetIdleTarget()
    {

        target += new Vector3(Mathf.Cos(Mathf.Deg2Rad * idleAngle) * idleCirclingRange, Mathf.Sin(Mathf.Deg2Rad * idleAngle) * idleCirclingRange);
    }
    //Handles the collision logic for the nest,Food and Enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Nest"))
        {
            EnterNest();
        }
        else if (collision.transform.CompareTag("Food"))
        {
            PickUpFood(collision);
        }
        else if(collision.transform.CompareTag("Enemy"))
        {
            
            DetectPlayer(collision);
        }
        else if(collision.transform.CompareTag("EnemyHit"))
        {
            PlayerHit();
        }
    }

    //Plays damage sound and reduces players stamina
    private void PlayerHit()
    {
        if (GameManager.Instance.GamePaused) return;
        if (isInvincible || CurrentState == PlayerState.inNest)
            return;
        damageSound.Play();
        StartCoroutine(OnHitInvincibility());
        gameObject.GetComponent<BirdStamina>().AddStamina(-staminaPerHit);
    }
    //Notifies enemies that the player is in attack range and plays SFX
    private void DetectPlayer(Collider2D collision)
    {
        if (CurrentState == PlayerState.inNest)
            return;

        if (collision.GetComponent<CrowAi>().GetState() != CrowAi.EnemyState.hunting)
            crowSFX.Play();
        collision.GetComponent<CrowAi>().SetState(CrowAi.EnemyState.hunting);
        if (!TutorialManager.Instance.EnemyTutorialFinished)
            TutorialManager.Instance.ToggleEnemyTutorial(collision.transform.position);
    }
    //Adds food the birds inventory as well as playing SFX
    private void PickUpFood(Collider2D collision)
    {
        fruitPickup.Play();
        gameObject.GetComponent<BirdInventory>().FoodCount += 1;
        Destroy(collision.transform.gameObject);
        if (!TutorialManager.Instance.ScoreTutorialFinished)
            TutorialManager.Instance.ToggleScoreTutorial();
    }
    //Refreshes Stamina, converts food to score
    private void EnterNest()
    {
        if (gameObject.GetComponent<BirdInventory>().FoodCount != 0)
        {
            GameManager.Instance.Score += gameObject.GetComponent<BirdInventory>().FoodCount * 100;
            gameObject.GetComponent<BirdInventory>().FoodCount = 0;
        }
        gameObject.GetComponent<BirdStamina>().ResetStamina();
        currentState = PlayerState.inNest;
        staminaBar.SetActive(false);
        gameObject.GetComponent<BirdInventory>().ResetFoodMultiplier();
    }
    //character gets iframes and starts blinking when hit
    IEnumerator OnHitInvincibility()
    {
        isInvincible = true;
        bool isBlinked = false;
        float blinkDelay = 0.05f;
        float blinkAmount = invincibilityDuration / blinkDelay;
        int currentBlinks = 0;
        while(currentBlinks < blinkAmount)
        {
            currentBlinks++;
            
            if(isBlinked)
            {
                isBlinked = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                for (int index = 0; index < transform.childCount; index++)
                {
                    transform.GetChild(index).GetComponent<SpriteRenderer>().enabled = true;
                }

            }
            else
            {
                isBlinked = true;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                for (int index = 0; index < transform.childCount; index++)
                {
                    transform.GetChild(index).GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            yield return new WaitForSeconds(blinkDelay);
        }
        isInvincible = false;
    }

    //generic wait until playing compass tutorial
    IEnumerator WaitForCompassTutorial()
    {
        yield return new WaitForSeconds(3);
        TutorialManager.Instance.ToggleCompassTutorial();
    }
} 
