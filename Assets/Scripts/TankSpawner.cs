using UnityEngine;

public class TankSpawner : SingletonDemo<TankSpawner>
{
    [SerializeField] GameObject playerTank;
    [SerializeField] GameObject enemyTankPrefab;
    [SerializeField] Transform[] spawnPositions;

    public int noOfEnemies;
    [SerializeField] TankScriptableObjectList allPlayerTanks;
    [SerializeField] EnemyTankScriptableObjectList allEnemyTanks;
    [SerializeField] BulletScriptableObjectList allBullets;
    void Start()
    {
        int selectPlayerType = UnityEngine.Random.Range(0, allPlayerTanks.Tanks.Length);
        TankScriptableObject playerTSO = allPlayerTanks.Tanks[selectPlayerType];
        int selectBulletType = UnityEngine.Random.Range(0, allBullets.Bullets.Length);
        playerTank.GetComponent<TankController>().SetPlayerTank(playerTSO,allBullets.Bullets[selectBulletType]);
        for(int i=0;i<spawnPositions.Length;i++)
        {
            GameObject enemyTank = Instantiate(enemyTankPrefab, spawnPositions[i].position, Quaternion.identity) as GameObject;
            int selectEnemyType= UnityEngine.Random.Range(0, allEnemyTanks.EnemyTanks.Length);
            selectBulletType = UnityEngine.Random.Range(0, allBullets.Bullets.Length);
            enemyTank.GetComponent<EnemyTankController>().SetEnemyTank(allEnemyTanks.EnemyTanks[selectEnemyType],allBullets.Bullets[selectBulletType],spawnPositions[i]);
        }
        noOfEnemies = spawnPositions.Length;
    }

    private void Update()
    {
        if(noOfEnemies==0)
        {
            //Debug.Log("Player Won");
        }
    }
  
}
