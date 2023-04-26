using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem collisionEffectPrefab { get; private set; }
    [field: SerializeField] public Color collisionEffectColor { get; private set; }
    [field: SerializeField] public AudioClip collisionSound { get; private set; }

    public int ScoreBonus => scoreBonus;
    public int TimeBonus => timeBonus;
    public int DoubleScoreTime => doubleScoreTime;
    
    private int scoreBonus;
    private int timeBonus;
    private int doubleScoreTime;
    private Rigidbody2D rigidBody;
    [field: SerializeField] public bool IsNegative { get; private set; }
    private List<Item> spawnedItems;
    private Vector2 baseVelocity;
    public float destroyBoundY;
    public ExtraCollision extraCollision;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (transform.position.y < destroyBoundY)
        {
            Destroy(gameObject);
        }
    }

    private bool ShieldCheck()
    {
        int shieldAmount = GameManager.Instance.ShieldAmount;
        if (shieldAmount > 0)
        {
            // GameManager.Instance.SetShieldAmount(shieldAmount - 1);
            // UIManager.Instance.ScoreFlyingText("Блок", transform.position, Color.blue);
            return true;
        }
        return false;
    }
    
    
    // public void IncreaseSpeed(float value)
    // {
    //     _rigidBody.velocity += Vector2.down * value * ResolutionScaler.GetScaler();
    // }

    public void SetBaseVelocity(float value)
    {
        rigidBody.velocity = Vector2.down * value * ResolutionScaler.GetScaler();
        baseVelocity = rigidBody.velocity;
    }

    public void MultiplyVelocity(float value)
    {
        rigidBody.velocity *= value;
    }

    public void ResetVelocityToBase()
    {
        rigidBody.velocity = baseVelocity;
    }

    // public void DivideSpeed(float value)
    // {
    //     _rigidBody.velocity /= value;
    // }

    private void OnDestroy() 
    {
        spawnedItems.Remove(this);
    }

    public void MakeItemNegative()
    {
        IsNegative = true;
    }

    public void SetBonuses( int scoreBonus, int timeBonus, int doubleScoreTime)
    {
        this.timeBonus = timeBonus;
        this.scoreBonus = scoreBonus;
        this.doubleScoreTime = doubleScoreTime;
    }

    public void AddToSpawnList(List<Item> list)
    {
        list.Add(this);
        spawnedItems = list;
    }
}