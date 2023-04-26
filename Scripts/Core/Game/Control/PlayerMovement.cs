using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private ParticleSystem playerTrace;
    [SerializeField] private float speedStart;
    [SerializeField] private float speedMax;
    private float speed;
    [SerializeField] private float accelerationPerSecond;
    private int moveMultiplier;
    private float speedMultiplier = 1f;
    bool isMoving;
    LevelManager levelManager;
    private float freezeTimer;
    private Coroutine freezeCoroutine;
    [SerializeField] private SpriteRenderer frozenSprite;
    

    
    
    private void Awake() 
    {
        levelManager = FindObjectOfType<LevelManager>();
        frozenSprite.enabled = false;
    }

    private void Start() 
    {
        transform.position -= new Vector3(0, 8, 0) * ResolutionScaler.GetScaler();
        speedStart *= ResolutionScaler.GetScaler();
        speedMax *= ResolutionScaler.GetScaler();
        accelerationPerSecond *= ResolutionScaler.GetScaler();
        speed = speedStart;
        SetPlayerPosition();
        levelManager.RegisterSpecialCollisionEvent(ExtraCollision.Frozen, Freeze);
    }

    private void SetPlayerPosition()
    {
        float width = Screen.width;
        float height = Screen.height;
        Vector2 screenPoint = new Vector2 (width / 2, height / 20);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        transform.position = new Vector3 (worldPoint.x, worldPoint.y, 0);
    }

    public void DecreaseMultiplyer()
    {
        moveMultiplier--;
    }

    public void IncreaseMultiplyer()
    {
        moveMultiplier++;
    }

    private void Update() 
    {
        if (moveMultiplier == 0) 
        {
            if (isMoving)
            {
                isMoving = false;
                speed = speedStart;
                playerTrace.gameObject.SetActive(false);
            }

            return;
        }
        if (!isMoving)
        {
            isMoving = true;
            playerTrace.gameObject.SetActive(true);
        }
        var force = playerTrace.forceOverLifetime;
        force.x = -80 * speed / speedMax * moveMultiplier;
        Move();
        speed += accelerationPerSecond * Time.unscaledDeltaTime;
        speed = Mathf.Clamp(speed, speedStart, speedMax);
    }

    private void Move()
    {
        Vector3 newPos = transform.position + Vector3.right * speed 
            * moveMultiplier * Time.unscaledDeltaTime * speedMultiplier;
        transform.position = CheckCameraBoundaries(newPos);
    }

    private Vector3 CheckCameraBoundaries(Vector3 newPos)
    {
        Vector3 screenBounds = levelManager.ScreenBounds;
        newPos.x = Mathf.Clamp(newPos.x, screenBounds.x * -1 + 1, screenBounds.x - 1);
        newPos.y = Mathf.Clamp(newPos.y, screenBounds.y * -1 + 1, screenBounds.y - 1);
        return newPos;
    }

    private void OnMoveLeft(InputValue value)
    {
        if (value.isPressed)
        {
            DecreaseMultiplyer();
            return;
        }
        IncreaseMultiplyer();
    }

    private void OnMoveRight(InputValue value)
    {
        if (value.isPressed)
        {
            IncreaseMultiplyer();
            return;
        }
        DecreaseMultiplyer();
    }

    private void Freeze()
    {
        speedMultiplier = 0.5f;
        freezeTimer = levelManager.FreezeTimerMax;
        if (freezeCoroutine is null)
        {
            freezeCoroutine = StartCoroutine(FreezeCoroutine());
            frozenSprite.enabled = true;
        }
            
    }

    IEnumerator FreezeCoroutine()
    {
        while (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            yield return null;
        }
        speedMultiplier = 1f;
        frozenSprite.enabled = false;
        freezeCoroutine = null;
    }
}
