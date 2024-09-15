using UnityEngine;

public class EnemyPatrolling : EnemyState
{
    int enemyTarget = 0;
    bool canMove = false;
    Transform targetPos;
    [SerializeField]Vector3[] PatrolPos;

    void Start()
    {
        targetPos = enemyTankController.spawnPosition;
        PatrolPos = new Vector3[targetPos.transform.childCount];
        for (int i = 0; i < targetPos.transform.childCount; i++)
        {
            PatrolPos[i] = targetPos.GetChild(i).gameObject.transform.position;
        }
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        Debug.Log("Enemy Patrolling State");
        canMove = true;
    }

    

    public override void OnExitState()
    {
        base.OnExitState();
        enemyTankController.m_tankRigidbody.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (canMove)
            EnemyMove();
        Vector3 playerPos = TankController.GetInstance().gameObject.transform.position;
        if (Vector3.Distance(transform.position,playerPos)<20f)
            enemyTankController.ChangeState(enemyTankController.enemyChasing);
        
    }
    private void EnemyMove()
    {
        Vector3 playerPos = TankController.GetInstance().gameObject.transform.position;
        Debug.Log("Distance="+ Vector3.Distance(transform.position, playerPos), gameObject);
        Vector3 nextPos = PatrolPos[enemyTarget];
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
            if (Mathf.Abs(transform.position.x - nextPos.x) < 0.1 && enemyTarget < PatrolPos.Length)
            {
                enemyTarget++;
                if (enemyTarget == PatrolPos.Length)
                    enemyTarget = 0;
                enemyTankController.m_tankRigidbody.velocity = Vector3.zero;
            }
        }
    }
}
