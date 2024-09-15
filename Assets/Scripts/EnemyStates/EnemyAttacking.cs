using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : EnemyState
{
    Vector3 playerPos;
    private float timeElapsed;
    [SerializeField]
    private float bulletFireCounter = 1f;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        Debug.Log("Enemy Chasing State");
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }

    void FixedUpdate()
    {
        playerPos = TankController.GetInstance().gameObject.transform.position;
        EnemyMove();
        timeElapsed += Time.deltaTime;
        if (timeElapsed > bulletFireCounter)
        {
            bulletFireCounter = 0;
            enemyTankController.FireBullet();
        }
        if (Vector3.Distance(transform.position, playerPos) > 10f)
            enemyTankController.ChangeState(enemyTankController.enemyChasing);
    }

    private void EnemyMove()
    {
        Vector3 nextPos = playerPos;
        var localTarget = transform.InverseTransformPoint(nextPos);
        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        Vector3 dir = (nextPos - transform.position).normalized * enemyTankController.moveSpeed * Time.deltaTime;
        if (deltaRotation != Quaternion.identity && enemyTankController.m_tankRigidbody.velocity == Vector3.zero)
        {
            //Rotate Enemy to NextPos
            enemyTankController.m_tankRigidbody.MoveRotation(enemyTankController.m_tankRigidbody.rotation * deltaRotation);
        }
        else if (deltaRotation == Quaternion.identity)
        {
            //transform.position = Vector3.Lerp(transform.position,EnemytargetPos[enemyTarget],Time.deltaTime);
            enemyTankController.m_tankRigidbody.velocity = dir;
        }
    }
}
