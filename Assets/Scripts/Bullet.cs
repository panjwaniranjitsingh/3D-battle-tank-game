using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float BulletForce;
    public float Damage;
    private Transform bulletFirePos;
    [SerializeField] Vector3 BulletStockPos;
    [SerializeField] GameObject BulletExplosion;
    private void Awake()
    {
        BulletStockPos = transform.position;
    }
    public void FireBullet()
    {
        gameObject.transform.position = bulletFirePos.position;
        gameObject.transform.rotation = bulletFirePos.rotation;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().AddForce(bulletFirePos.forward * BulletForce);
    }

    public void SetBullet(Color color, Transform bulletPos, BulletScriptableObject bulletSO, GameObject FiredFrom)
    {
        //Debug.Log(FiredFrom.name);
        gameObject.name = bulletSO.BulletName;
        BulletForce = bulletSO.BulletForce;
        transform.parent = FiredFrom.transform;
        if(FiredFrom.GetComponent<TankController>()!=null)
            Damage = FiredFrom.GetComponent<TankController>().damage;
        else
            Damage = FiredFrom.GetComponent<EnemyTankController>().damage;
        gameObject.GetComponent<Renderer>().material.color = color;
        bulletFirePos = bulletPos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject bulletEffect= Instantiate(BulletExplosion, transform.position, transform.rotation);
        Destroy(bulletEffect, 1f);
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if(damageable!=null)
        {
            damageable.TakeDamage(Damage);
        } 
        SendBulletToStock();
    }

    public void SendBulletToStock()
    {
        gameObject.transform.position = BulletStockPos;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
