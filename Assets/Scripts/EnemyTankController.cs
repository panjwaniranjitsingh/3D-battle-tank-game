using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : MonoBehaviour,IDamageable
{
    [Header("Properties")]
    public float moveSpeed;
    [SerializeField] float health;
    public float damage;
    [SerializeField] Color color;
    public Transform spawnPosition;
    
    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform m_bulletPos;
    [SerializeField] BulletScriptableObject enemyBulletSO;
    [SerializeField] private Transform m_bulletStockPos;
    [SerializeField] int noOfBulletsInStock = 10;
    [SerializeField] int noOfBulletsFired = 0;

    GameObject[] bullets;
    public Rigidbody m_tankRigidbody;
    float FullHealth;
    Color BLUE = new Color32(20, 125, 248, 255);
    Color RED = new Color32(167, 22, 22, 255);
    Color GREEN = new Color32(57, 116, 57, 255);
    Color YELLOW = new Color32(150, 154, 15, 255);

    private EnemyState currentState;
    [Header("EnemyStates")]
    public EnemyIdleState enemyIdleState;
    public EnemyPatrolling enemyPatrolling;
    public EnemyChasing enemyChasing;
    public EnemyAttacking enemyAttacking;

    private void Awake()
    { 
        m_tankRigidbody= GetComponent<Rigidbody>();
        bullets = new GameObject[noOfBulletsInStock];
    }
    
    private void Start()
    {
        ChangeState(enemyIdleState);
    }

    public void ChangeState(EnemyState newState)
    {
        if(currentState!=null)
        {
            currentState.OnExitState();
        }

        currentState = newState;

        currentState.OnEnterState();
    }

    
    
    public void FireBullet()
    {
        bullets[noOfBulletsFired].GetComponent<Bullet>().FireBullet();
        noOfBulletsFired++;
        if (noOfBulletsFired == noOfBulletsInStock)
            noOfBulletsFired = 0;
    }

    public void SetEnemyTank(EnemyTankScriptableObject enemyTSO, BulletScriptableObject bulletSO,Transform spawnPos)
    {
        spawnPosition = spawnPos;
        gameObject.name = enemyTSO.TankName;
        moveSpeed = enemyTSO.Speed;
        health = enemyTSO.Health;
        damage = enemyTSO.Damage;
        enemyBulletSO = bulletSO;
        FullHealth = enemyTSO.Health;
        GameObject TankRenderers = gameObject.transform.GetChild(0).gameObject;
        string plColor = enemyTSO.TankColor.ToString();
        switch (plColor)
        {
            case "Blue":
                color = BLUE;
                break;
            case "Red":
                color = RED;
                break;
            case "Green":
                color = GREEN;
                break;
            case "Yellow":
                color = YELLOW;
                break;
        }
        for (int i = 0; i < TankRenderers.transform.childCount; i++)
        {
            TankRenderers.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = color;
        }
        //Bullet Stock
        for (int i = 0; i < noOfBulletsInStock; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, m_bulletStockPos.position, m_bulletStockPos.rotation) as GameObject;
            bullets[i].GetComponent<Bullet>().SetBullet(color, m_bulletPos, enemyBulletSO, gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("onCollisionEntered with "+other.gameObject.name);
        if (other.gameObject.GetComponent<TankController>()!=null)
        {
            //Player Dies
            other.gameObject.GetComponent<TankController>().TakeDamage(damage);
        }
    }

    void OnDisable()
    {
        health = FullHealth;
        ChangeState(enemyIdleState);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Enemy Dies
            gameObject.SetActive(false);
            TankSpawner.GetInstance().noOfEnemies--;
            TankSpawner.GetInstance().EnableEnemy(gameObject);
        }
    } 
}
