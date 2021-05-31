using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    const float TURNSPEED=50f;
    private Rigidbody m_tankRigidbody;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform m_bulletPos;
    [SerializeField] BulletScriptableObject enemyBulletSO;
    [SerializeField] float health;
    [SerializeField] float damage;
    [SerializeField] Color color;
    Color BLUE = new Color32(20, 125, 248, 255);
    Color RED = new Color32(167, 22, 22, 255);
    Color GREEN = new Color32(57, 116, 57, 255);
    Color YELLOW = new Color32(150, 154, 15, 255);
    [SerializeField]Vector3[] EnemytargetPos;
    [SerializeField]bool isAlive = false;
    int enemyTarget = 0;
    // Start is called before the first frame update
    void Start()
    { 
        m_tankRigidbody= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
            EnemyMove();
        
    }
    private void EnemyMove()
    {
        //isAlive = false;
            
          //  Vector3 dir = (EnemytargetPos[enemyTarget] - transform.position).normalized * moveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position,EnemytargetPos[enemyTarget],Time.deltaTime);
            if (Mathf.Abs(transform.position.x-EnemytargetPos[enemyTarget].x)<0.1)
            {
                m_tankRigidbody.velocity = Vector3.zero;
                if (enemyTarget < EnemytargetPos.Length)
                    enemyTarget++;
                if (enemyTarget == EnemytargetPos.Length)
                    enemyTarget = 0;
                Debug.Log("enemyTarget=" + enemyTarget,gameObject);
            }
        
    }

    public void FireBullet()
    {
        //Debug.Log("Bullet at "+m_bulletPos.position);
        GameObject bullet = Instantiate(bulletPrefab, m_bulletPos.position, m_bulletPos.rotation);
        bullet.GetComponent<Bullet>().FireBullet(color, m_bulletPos, enemyBulletSO, gameObject);
    }

    public void SetEnemyTank(EnemyTankScriptableObject enemyTSO, BulletScriptableObject bulletSO,Transform targetPos)
    {
        EnemytargetPos = new Vector3[targetPos.transform.childCount];
        for(int i=0;i<targetPos.transform.childCount;i++)
        {
            EnemytargetPos[i]=targetPos.GetChild(i).gameObject.transform.position;
        }
        //EnemyMove();
        isAlive = true;
        gameObject.name = enemyTSO.TankName;
        moveSpeed = enemyTSO.Speed;
        health = enemyTSO.Health;
        damage = enemyTSO.Damage;
        enemyBulletSO = bulletSO;
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
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("onCollisionEntered with "+other.gameObject.name);
        if (other.gameObject.transform.parent.GetComponent<TankController>()!=null)
        {
            var bulletScript = other.gameObject.GetComponent<Bullet>();
            health -=bulletScript.Damage;
            bulletScript.SendBulletToStock();
            if (health<0)
            {
                //Enemy Dies
                gameObject.SetActive(false);
                TankSpawner.GetInstance().noOfEnemies--;
            }
        }
    }
}
