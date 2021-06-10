using UnityEngine;
[RequireComponent(typeof(EnemyTankController))]
public class EnemyState : MonoBehaviour
{
    protected EnemyTankController enemyTankController;

    protected virtual void Awake()
    {
        enemyTankController = GetComponent<EnemyTankController>();
    }

    public virtual void OnEnterState()
    {
        this.enabled = true;
    }

    public virtual void OnExitState()
    {
        this.enabled = false;
    }

        // public virtual void Tick() { } //same as Update()
}

