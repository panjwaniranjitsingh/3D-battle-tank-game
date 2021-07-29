﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TankController : SingletonDemo<TankController>,IDamageable
{
    [Header("Properties")]
    [SerializeField] Joystick joystick;
    [SerializeField] float horizontal,vertical;
    [SerializeField] float moveSpeed;
    [SerializeField] float health;
    [SerializeField] float currentHealth;
    [SerializeField] int score;
    public float damage;
    [SerializeField] Color color;
    [SerializeField] GameObject TankExplosion;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI ScoreText;

    [Header("Bullet")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform m_bulletPos;
    [SerializeField] BulletScriptableObject playerBulletSO;
    [SerializeField] private Transform m_bulletStockPos;
    GameObject[] bullets;
    [SerializeField] int noOfBulletsInStock = 20;
    [SerializeField] int noOfBulletsFired = 0;

    const float TURNSPEED = 50f;
    private Rigidbody m_tankRigidbody;
    const string HORIZONTAL = "Horizontal1";
    const string VERTICAL = "Vertical1";
    readonly Color BLUE = new Color32(20, 125, 248, 255);
    readonly Color RED = new Color32(167, 22, 22, 255);
    readonly Color GREEN = new Color32(57, 116, 57, 255);
    readonly Color YELLOW = new Color32(150, 154, 15, 255);

    void Awake()
    {
        score = 0;
        DisplayScore();
        m_tankRigidbody = GetComponent<Rigidbody>();
        bullets = new GameObject[noOfBulletsInStock];
    }

    private void DisplayScore()
    {
        ScoreText.text = "Score-"+score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = joystick.Horizontal;
        
        vertical = joystick.Vertical;

        PlayerMovement(horizontal, vertical);
    }
   
    private void PlayerMovement(float horizontal, float vertical)
    {
        //Move player oldway 
        /*  Vector3 position = transform.position;
          position.x += horizontal * speed * Time.deltaTime;
          position.y = 0;
          position.z += vertical * speed * Time.deltaTime;
          transform.position = position;

          Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
          float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
          if(targetAngle!=0)
              headOfTank.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);*/
       Vector3 Movement= transform.forward * vertical * moveSpeed * Time.deltaTime;
       m_tankRigidbody.AddForce(Movement);

       float turn = horizontal * TURNSPEED * Time.deltaTime;
       Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_tankRigidbody.MoveRotation(m_tankRigidbody.rotation * turnRotation);
    }

    public void FireBullet()
    { 
        ServiceEvents.GetInstance().InvokeBulletFiredEvent();
        bullets[noOfBulletsFired].GetComponent<Bullet>().FireBullet();
        noOfBulletsFired++;
        if (noOfBulletsFired == noOfBulletsInStock)
            noOfBulletsFired = 0;
    }

    public void SetPlayerTank(TankScriptableObject tankScriptableObject,BulletScriptableObject bulletSO)
    {
        gameObject.name = tankScriptableObject.TankName;
        moveSpeed = tankScriptableObject.Speed;
        health = tankScriptableObject.Health;
        currentHealth = health;
        damage = tankScriptableObject.Damage;
        playerBulletSO = bulletSO;
        GameObject TankRenderers = gameObject.transform.GetChild(0).gameObject;
        string plColor = tankScriptableObject.TankColor.ToString();
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
        healthBar.color = color;
        //Bullet Stock
        for (int i = 0; i < noOfBulletsInStock; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, m_bulletStockPos.position, m_bulletStockPos.rotation) as GameObject;
            //Debug.Log("Bullet added to stock");
            bullets[i].GetComponent<Bullet>().SetBullet(color, m_bulletPos, playerBulletSO, gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            //Player Dies
            GameObject explosionEffect = Instantiate(TankExplosion, transform.position, transform.rotation);
            Destroy(explosionEffect, 1f);
            Debug.Log("Player is Dead");
            TankSpawner.GetInstance().StartDestruction();
            gameObject.SetActive(false);
        }
    }

    public void AddScore(int addToScore)
    {
        score += addToScore;
        DisplayScore();
    }
}
