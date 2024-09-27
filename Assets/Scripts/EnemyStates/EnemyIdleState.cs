using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float timeElapsed;
    [SerializeField]
    private float timeToExitState = 5f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        //Debug.Log("Enemy Idle State");
        enemyTankController.m_tankRigidbody.velocity = Vector3.zero;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        timeElapsed = 0;
    }
        
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed>timeToExitState)
        {
            enemyTankController.ChangeState(enemyTankController.enemyPatrolling);
        }
    }

}
