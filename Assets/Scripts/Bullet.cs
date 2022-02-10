using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string BulletName;
    public float BulletForce;
    public float Damage;
    [SerializeField] Vector3 BulletStockPos;
    private void Awake()
    {
        BulletStockPos = transform.position;
    }
    public void FireBullet(Color color, Transform bulletPos, BulletScriptableObject bulletSO, GameObject FiredFrom)
    {
        BulletName = bulletSO.BulletName;
        BulletForce = bulletSO.BulletForce;
        transform.parent = FiredFrom.transform;
        Damage = FiredFrom.GetComponent<TankController>().damage;
        gameObject.GetComponent<Renderer>().material.color = color;
        gameObject.GetComponent<Rigidbody>().velocity=Vector3.zero;
        gameObject.transform.position = bulletPos.position;
        gameObject.transform.rotation = bulletPos.rotation;
        gameObject.GetComponent<Rigidbody>().AddForce(bulletPos.forward * BulletForce);
        //Destroy(gameObject, 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyTankController>() == null)
            SendBulletToStock();
    }

    public void SendBulletToStock()
    {
        gameObject.transform.position = BulletStockPos;
    }
}
