using System.Collections;
using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI AchievementText;
    [SerializeField] int enemyAchievement;

    [SerializeField] int bulletAchievement;

    [SerializeField] int GamesPlayedAchievement;
    
    int enemiesKilled = 0,noOfEnemyAchievements=1;
    int bulletsFired = 0,noOfBulletAchievements=1;
    int noOfGamesPlayed = 0, noOfGameAchievements = 1;

    bool enemiesKilledUnlocked = false;
    bool bulletsFiredUnlocked = false;
    bool noOfGamesPlayedUnlocked = false;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        enemiesKilled = PlayerPrefs.GetInt("enemiesKilled", 0);
        bulletsFired = PlayerPrefs.GetInt("bulletsFired", 0);
        noOfGamesPlayed = PlayerPrefs.GetInt("noOfGamesPlayed", 0);
        enemiesKilledUnlocked = enemiesKilled > enemyAchievement;
        bulletsFiredUnlocked = bulletsFired > bulletAchievement;
        noOfGamesPlayedUnlocked = noOfGamesPlayed > GamesPlayedAchievement;
        noOfBulletAchievements = PlayerPrefs.GetInt("noOfBulletAchievements", 1);
        noOfEnemyAchievements = PlayerPrefs.GetInt("noOfEnemyAchievements", 1);
        noOfGameAchievements = PlayerPrefs.GetInt("noOfGameAchievements", 1);
    }

    private void OnEnable()
    {
        ServiceEvents.GetInstance().EnemyKilled += RegisterEnemyKill;
        ServiceEvents.GetInstance().BulletFired += RegisterBulletFired;
        ServiceEvents.GetInstance().GameStarted += RegisterGamePlayed;
    }
    
   /* private void OnDestroy()
    {
        ServiceEvents.GetInstance().EnemyKilled -= RegisterEnemyKill;
        ServiceEvents.GetInstance().BulletFired -= RegisterBulletFired;
        ServiceEvents.GetInstance().GameStarted -= RegisterGamePlayed;
    }
    */
    private void RegisterGamePlayed()
    {
        noOfGamesPlayed++;
        PlayerPrefs.SetInt("noOfGamesPlayed", noOfGamesPlayed);
        CheckForAchievements();
    }

    private void RegisterBulletFired()
    {
        bulletsFired++;
        PlayerPrefs.SetInt("bulletsFired", bulletsFired);
        CheckForAchievements();
    }
    

    private void RegisterEnemyKill()
    {
        enemiesKilled++;
        PlayerPrefs.SetInt("enemiesKilled", enemiesKilled);
        CheckForAchievements();
    }

    private void CheckForAchievements()
    {
        CheckForBulletsFiredAchievement();
        CheckForEnemyKillAchievement();
        CheckForGamesPlayedAchievement();
    }
    
    private void CheckForBulletsFiredAchievement()
    {
        if (bulletsFired >= bulletAchievement && !bulletsFiredUnlocked)
        {
            bulletsFiredUnlocked = true;
        }
        if(bulletsFiredUnlocked)
        {
            int newAchievement = noOfBulletAchievements * bulletAchievement;
            if (bulletsFired == newAchievement)
            {
                StartCoroutine(DisplayAchievement(newAchievement + " Bullets Fired"));
                noOfBulletAchievements++;
                PlayerPrefs.SetInt("noOfBulletAchievements", noOfBulletAchievements);
            }
        }
    }

    private void CheckForEnemyKillAchievement()
    {
        if (enemiesKilled >= enemyAchievement && !enemiesKilledUnlocked)
        {
            enemiesKilledUnlocked = true;
        }
        if (enemiesKilledUnlocked)
        {
            int newAchievement = noOfEnemyAchievements * enemyAchievement;
            if (enemiesKilled == newAchievement)
            {
                StartCoroutine(DisplayAchievement(newAchievement + " Enemies Killed"));
                noOfEnemyAchievements++;
                PlayerPrefs.SetInt("noOfEnemyAchievements", noOfEnemyAchievements);
            }
        }
    }

    private void CheckForGamesPlayedAchievement()
    {
        if (noOfGamesPlayed >= GamesPlayedAchievement && !noOfGamesPlayedUnlocked)
        {
            noOfGamesPlayedUnlocked = true;
        }
        if (noOfGamesPlayedUnlocked)
        {
            int newAchievement = noOfGameAchievements * GamesPlayedAchievement;
            if (noOfGamesPlayed == newAchievement)
            {
                StartCoroutine(DisplayAchievement(newAchievement + " Games Played"));
                noOfGameAchievements++;
                PlayerPrefs.SetInt("noOfGameAchievements", noOfGameAchievements);
            }
        }
    }

    IEnumerator DisplayAchievement(string message)
    {
        AchievementText.text = message;
        AchievementText.transform.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        AchievementText.transform.gameObject.SetActive(false);
    }
}
