using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Setup")]
    [SerializeField] int scoreValue = 5;
    [SerializeField] float health = 100f;
    [SerializeField] GameObject destroyedVFX;
    [SerializeField] AudioClip destroyedSFX;
    [Range(0f, 1f)] [SerializeField] float destroyedSFXVolume = 0.5f;
    [Header("Weapon")]
    [SerializeField] GameObject enemyLaserPrefab;
    [Range(10f, 30f)] [SerializeField] float enemyLaserSpeed = 10f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    GameSession gameSession;
    float shotCounter;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject enemyLaser = Instantiate(
                    enemyLaserPrefab,
                    transform.position,
                    Quaternion.identity) as GameObject;//Quarternion.identity means no rotation

        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) //other -> oject that collided with the gameObject
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        TriggerVFX();
        TiggerAudio();
        Destroy(gameObject, 0.1f);
        gameSession.AddToScore(scoreValue);
    }

    private void TiggerAudio()
    {
        AudioSource.PlayClipAtPoint(destroyedSFX, Camera.main.transform.position, destroyedSFXVolume);
    }

    private void TriggerVFX()
    {
        GameObject deathVFX = Instantiate(
                    destroyedVFX,
                    transform.position,
                    Quaternion.identity) as GameObject;
        Destroy(deathVFX, 1f);
    }
}
