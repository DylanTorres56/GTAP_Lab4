using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour,IDamagable
{
    [SerializeField]int health = 1;
    //[SerializeField] float shakeAmount, shakeTimer; // The amount to change the Virtual Camera's amplitude by, and for how long.

    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip tookDamage;
    static public Action<AudioClip> OnMeteorDestroyed;
    static public Action<AudioClip> OnMeteorDamaged;
    CinemachineImpulseSource impulse;

    private void OnValidate()
    {
        if (health < 1)
        {
            health = 1; 
        }
        this.transform.localScale = Vector3.one * health;
    }
    // Awake is called on the first active frame
    void Awake()
    {
        this.transform.localScale = Vector3.one*health;
        impulse = GetComponent<CinemachineImpulseSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * 2f);

        if (transform.position.y < -11f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            whatIHit.gameObject.GetComponent<IDamagable>().TakeDamage(health);
            TakeDamage(health);
        }
    }


    public void TakeDamage(int damage = 1)
    {
        health -= damage;
        if (health <= 0)
        {
            OnMeteorDestroyed?.Invoke(explosion);
            impulse.GenerateImpulse();
            Destroy(this.gameObject);
        }
        else
        {
            OnMeteorDamaged?.Invoke(tookDamage);
        }
    }
}
