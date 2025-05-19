using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float sensitivity = 5f;
    private float rotationY = 0f; 
    private float fireWait = 0f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    public float attackSpeed = 2f;
    public int damage = 20;
    public int maxHealthPoint = 100;
    public int currentHealthPoint;
    public float speed = 5f;
    private Animator animator;



    public AudioSource fireAudio;



    public void StartPlayer()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        attackSpeed = 1.5f;
        damage = 20;
        maxHealthPoint = 100;
        currentHealthPoint = maxHealthPoint;
        speed = 5f;

    }

    void Update()
    {
        PlayerMove();
        PlayerFire();
    }

    private void PlayerFire()
    {
        fireWait += Time.deltaTime;
        if(Input.GetMouseButton(0) && fireWait >= attackSpeed)
        {
            
            fireWait = 0f;
            Vector3 spawnOffset = new Vector3(-0.25f, 0.3f, 2.3f);  // Merminin ��kaca�� yerin ofseti
            Vector3 spawnPosition = transform.position + transform.forward * spawnOffset.z + transform.right * spawnOffset.x + transform.up * spawnOffset.y;  
            GameObject newBullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
            Bullet bulletInfo = newBullet.AddComponent<Bullet>();
            bulletInfo.playerBulletDamage = damage;
            Rigidbody rbBullet = newBullet.GetComponent<Rigidbody>();
            rbBullet.linearVelocity = transform.forward * bulletSpeed;
            Destroy(newBullet, 10f);
            fireAudio.Play();
            animator.ResetTrigger("RecoilTrigger");
            animator.SetTrigger("isFired");
        }
    }

    private void PlayerMove()
    {
        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction -= transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += transform.right;
        }
        rb.linearVelocity = direction.normalized * speed;

        
        
        float mouseX = Input.GetAxis("Mouse X") * sensitivity; // Mouse'un X eksenindeki hareketini al
        rotationY += mouseX; // Mouse hareketini Y ekseni a��s�na ekle
        Quaternion newRotation = Quaternion.Euler(0f, rotationY, 0f); // Sadece Y ekseninde d�nd�r
        rb.MoveRotation(newRotation); // Rigidbody'yi d�nd�r
    }
}
