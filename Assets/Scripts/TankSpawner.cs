using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class TankSpawner : SingletonDemo<TankSpawner>
{
    [SerializeField] GameObject playerTank;
    [SerializeField] GameObject enemyTankPrefab;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject EnemyTanks;
    [SerializeField] GameObject Environment;
    [SerializeField] CinemachineVirtualCamera cinemachineVC;
    [SerializeField] GameObject TankExplosion;

    public int noOfEnemies;
    [SerializeField] TankScriptableObjectList allPlayerTanks;
    [SerializeField] EnemyTankScriptableObjectList allEnemyTanks;
    [SerializeField] BulletScriptableObjectList allBullets;

    [SerializeField] float timeToEnableEnemies = 100f;

    void Start()
    {
        int selectPlayerType = UnityEngine.Random.Range(0, allPlayerTanks.Tanks.Length);
        TankScriptableObject playerTSO = allPlayerTanks.Tanks[selectPlayerType];
        int selectBulletType = UnityEngine.Random.Range(0, allBullets.Bullets.Length);
        playerTank.GetComponent<TankController>().SetPlayerTank(playerTSO, allBullets.Bullets[selectBulletType]);
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            GameObject enemyTank = Instantiate(enemyTankPrefab, spawnPositions[i].position, Quaternion.identity) as GameObject;
            int selectEnemyType = UnityEngine.Random.Range(0, allEnemyTanks.EnemyTanks.Length);
            selectBulletType = UnityEngine.Random.Range(0, allBullets.Bullets.Length);
            enemyTank.GetComponent<EnemyTankController>().SetEnemyTank(allEnemyTanks.EnemyTanks[selectEnemyType], allBullets.Bullets[selectBulletType], spawnPositions[i]);
            enemyTank.transform.parent = EnemyTanks.transform;
        }
        noOfEnemies = spawnPositions.Length;
    }

    private void Update()
    {
        if (noOfEnemies == 0)
        {
            Debug.Log("Player Won");
        }
        GameObject player = TankController.GetInstance().gameObject;
        if (!player.activeSelf)
        {
            //StartCoroutine(EnableGameObject(player, 30f));
        }
    }

    public void EnableEnemy(GameObject enemy)
    {
        StartCoroutine(EnableGameObject(enemy, timeToEnableEnemies));
    }

    IEnumerator EnableGameObject(GameObject myGameObject, float time)
    {
        yield return new WaitForSeconds(time);
        noOfEnemies++;
        myGameObject.SetActive(true);
    }
    
    public IEnumerator DestroyAll()
    {
        cinemachineVC.enabled = true;
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(DestroyEnemies());
       
        yield return StartCoroutine(CheckChildren(Environment));

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator DestroyEnemies()
    {
        for (int i = 0; i < EnemyTanks.transform.childCount; i++)
        {
            GameObject target = EnemyTanks.transform.GetChild(i).gameObject;
            EnemyTankController targetTankController = target.GetComponent<EnemyTankController>();
            targetTankController.ChangeState(targetTankController.enemyIdleState);
            cinemachineVC.Follow = target.transform;
            yield return new WaitForSeconds(2f);
            target.SetActive(false);
            GameObject explosionEffect = Instantiate(TankExplosion, target.transform.position, target.transform.rotation);
            Destroy(explosionEffect, 1f);
            yield return new WaitForSeconds(2f);
        }
    }
   
    IEnumerator CheckChildren(GameObject gameObject)
    {
        Debug.Log("Gameobject checked is " + gameObject.name);
        if (gameObject.transform.childCount == 0)
            yield return StartCoroutine(DestroyGameObject(gameObject));
        else
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                yield return StartCoroutine(CheckChildren(gameObject.transform.GetChild(i).gameObject));
            }
        }
    }

    IEnumerator DestroyGameObject(GameObject target)
    {
        if (target.GetComponent<Renderer>() != null)
        {
            Debug.Log("Gameobject destroyed is " + target.name);
            cinemachineVC.Follow = target.transform;
            yield return new WaitForSeconds(1f);
            target.SetActive(false);
            GameObject explosionEffect = Instantiate(TankExplosion, target.transform.position, target.transform.rotation);
            Destroy(explosionEffect, 1f);
            yield return new WaitForSeconds(1f);
        }
    }

    internal void StartDestruction()
    {
        StartCoroutine(DestroyAll());
    }
}
