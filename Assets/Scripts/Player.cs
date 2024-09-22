using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour,IDamagable
{
    public GameObject laserPrefab;
    [SerializeField] float thrust,rotSpeed;
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] float fireDelay;
    private float horizontalScreenLimit = 10f;
    private float verticalScreenLimit = 6f;
    private bool canShoot = true;
    [SerializeField]
    float health;
    PlayerInput playerInput;
    InputAction moveAction;
    Rigidbody2D rb;

    public AudioSource audioSource;
    public AudioClip shootSound;

    CinemachineImpulseSource impulse;
    [SerializeField]
    LineRenderer jet;
    [SerializeField]
    ParticleSystem jetParticle;

    public static Action PlayerDied;
    private void Awake()
    {
        impulse=this.GetComponent<CinemachineImpulseSource>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        moveAction.performed += OnMove;
        playerInput.actions["Fire"].performed += Shooting;
        rb = GetComponent<Rigidbody2D>();
       
    }
    private void OnDestroy()
    {
        moveAction.performed -= OnMove;
        playerInput.actions["Fire"].performed -= Shooting;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        horizontalScreenLimit = Camera.main.orthographicSize* Camera.main.aspect+1;
        verticalScreenLimit = Camera.main.orthographicSize +1;
    }

    // Update is called once per frame
    void Update()
    {
       
        //Shooting();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        //transform.Translate(moveAction.ReadValue<Vector2>() * Time.deltaTime * maxSpeed);
        float fwdInput = Mathf.Clamp(moveAction.ReadValue<Vector2>().y,-0.25f,1);
        float offest = (fwdInput) / jet.positionCount;
        for (int i = 1; i < jet.positionCount; i++)
        {

            jet.SetPosition(i, Vector3.down * i * offest);
        }
        float rot = moveAction.ReadValue<Vector2>().x;
        // rb.AddForce(fwdThrust * this.transform.up*rb.mass,ForceMode2D.Force);
        rb.velocity += ((Vector2)this.transform.up * (fwdInput*thrust * Time.fixedDeltaTime));
        //rb.velocity=Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        this.transform.Rotate(Vector3.forward * rot*-rotSpeed * Time.fixedDeltaTime);

        if (transform.position.x > horizontalScreenLimit || transform.position.x <= -horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1f, transform.position.y, 0);
        }
        if (transform.position.y > verticalScreenLimit || transform.position.y <= -verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }

    void Shooting(InputAction.CallbackContext context)
    {
        if (!canShoot)
        {
            return;
        }
        Instantiate(laserPrefab, transform.position + this.transform.forward, this.transform.rotation);
        audioSource.PlayOneShot(shootSound, 1.0f);
        canShoot = false;
        StartCoroutine("Cooldown");

    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(fireDelay);
        canShoot = true;
    }

    public void TakeDamage(int damage = 1)
    {

        health -= 1;
        if (health <= 0)
        {
            impulse.GenerateImpulse();
            PlayerDied?.Invoke();
            Destroy(this.gameObject);
            
        }
    }
    void OnMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y>0)
        {
            jetParticle.Play();
        }
        else
        {
            jetParticle.Stop();
        }
    }
}
