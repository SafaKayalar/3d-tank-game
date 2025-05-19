using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gameManager;
    public List<GameObject> enemyList;
    public Transform player;
    public float minDistance = 15f; 

    public void SpawnEnemy()
    {
        for (int i = 0; i < gameManager.wave; ++i)
        {
            float xPosition, zPosition;
            Vector3 spawnPosition;
            do
            {
                xPosition = Random.Range(-30, 30);
                zPosition = Random.Range(-30, 30);
                spawnPosition = new Vector3(xPosition, 1, zPosition);
            }
            while (Vector3.Distance(spawnPosition, player.position) < minDistance);

            GameObject selectedEnemy = enemyList[Random.Range(0, enemyList.Count)];
            GameObject e = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
            Enemy enemyscript = e.GetComponent<Enemy>();
            gameManager.enemies.Add(enemyscript);
        }
    }
}
