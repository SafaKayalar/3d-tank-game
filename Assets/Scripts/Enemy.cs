using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int healthPoint;
    public float speed;
    public bool isDead;
    public NavMeshAgent navMeshAgent;
    public Transform player;
    [SerializeField] private float bulletSpeed = 15f;
    public float attackRange = 10f;
   
    public GameObject bulletPrefab;
    public float attackSpeed = 2f;
    public float fireDelay = 2f;
    public int damage = 20;
    public Image healthBar;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Player'� bul
    }
    public void StartEnemy() 
    { 

    }
    private void Update()
    {
        EnemyMove();
        Die();
        EnemyFire();
        UpdateHealthBar();
    }

    private void EnemyMove()
    {
        navMeshAgent.speed = speed;
        navMeshAgent.destination = player.position;
    }

    public void Die()
    {
        if (healthPoint <= 0)
        {
            isDead = true;
        } 
    }
    private void EnemyFire()
    {
        fireDelay += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange && fireDelay >= attackSpeed)
        {
            fireDelay = 0f;
            Vector3 spawnOffset = new Vector3(0f, 0.3f, 1.8f);  // Merminin ��kaca�� yerin ofseti
            Vector3 spawnPosition = transform.position + transform.forward * spawnOffset.z + transform.right * spawnOffset.x + transform.up * spawnOffset.y;
            GameObject newBullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
            Bullet bulletInfo = newBullet.AddComponent<Bullet>();
            bulletInfo.playerBulletDamage = damage;
            Rigidbody rbBullet = newBullet.GetComponent<Rigidbody>();
            rbBullet.linearVelocity = transform.forward * bulletSpeed;
            Destroy(newBullet, 10f);
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)healthPoint / 100;
    }

}
