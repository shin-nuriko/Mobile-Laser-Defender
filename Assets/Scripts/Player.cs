using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [Header("Player Aircraft")] 
    [SerializeField] float health = 100f;
    [SerializeField] AudioClip onGettingHitSFX;
    [Range(0f, 1f)] [SerializeField] float onGettingHitSFXVolume = 0.5f;
    [SerializeField] GameObject destroyedVFX;
    [SerializeField] AudioClip destroyedSFX;
    [Range(0f, 1f)] [SerializeField] float destroyedSFXVolume = 0.5f;
    [Header("Player Weapon")]
    [SerializeField] GameObject laserPrefab;
    [Range(10f, 30f)] [SerializeField] float laserSpeed = 10f;
    [Range(1f, 10f)][SerializeField] float moveSpeed = 5f;
    [Range(0.1f, 3f)] [SerializeField] float laserRefireSeconds = 0.1f;
    
        
    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float xPadding = 0.5f;
    float yMin;
    float yMax;
    float yPadding = 0.5f;
    float maxHealth;

    private void Awake()
    {
        maxHealth = health; //capture this before player sustains any damage
    }
    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        maxHealth = health; //capture this before player sustains any damage
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other) //other -> oject that collided with the gameObject
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return;  }
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)  {  Die(); } 
        else 
        {
            AudioSource.PlayClipAtPoint(
                onGettingHitSFX, 
                Camera.main.transform.position, 
                onGettingHitSFXVolume);
        }
        Debug.Log("Player Health: " + health +  " / " + maxHealth);
        Debug.Log("COLLISION DETECTED");

    }

    private void Die()
    {
        GameObject deathVFX = Instantiate(
                    destroyedVFX,
                    transform.position,
                    Quaternion.identity) as GameObject;
        Destroy(deathVFX, 1f); 
        AudioSource.PlayClipAtPoint(destroyedSFX, Camera.main.transform.position, destroyedSFXVolume);
        Debug.Log("DIE");
        //StartCoroutine(GameOver());
        InstantGameOver();
    }

    private void InstantGameOver()
    {        
        //hide player out of sight for a while before destroying the game object so we can see the VFX
        //var outOfSight = new Vector3(transform.position.x, -1000f, transform.position.z);
        //transform.position = outOfSight;
        FindObjectOfType<Level>().LoadGameOver();// Object Level will handle screen display even after this gameobject is destroyed
        Destroy(gameObject, 0.1f);  
    }
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinue());        
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinue()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                    laserPrefab,
                    transform.position,
                    Quaternion.identity) as GameObject;//Quarternion.identity means no rotation

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            yield return new WaitForSeconds(laserRefireSeconds);
        }   
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;//make game framerate independent
        float newXPos = transform.position.x + deltaX;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;//make game framerate independent
        float newYPos = transform.position.y + deltaY;
        //clamp make sure it does not go out of camera view
        newXPos = Mathf.Clamp(newXPos, xMin + xPadding, xMax - xPadding);
        newYPos = Mathf.Clamp(newYPos, yMin + yPadding, yMax - yPadding);
        transform.position = new Vector2(newXPos, newYPos); 
    }
    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        //get the world space value at given camera x,y,z position
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }
}
