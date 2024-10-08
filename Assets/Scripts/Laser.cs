using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    float lifeSpan;
    float lifeTime;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 8f);
        //lifeTime += Time.deltaTime;
        //if (lifeTime > lifeSpan)
        //{
        //    Destroy(this.gameObject);
        //}
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }
        collision.gameObject.GetComponent<IDamagable>()?.TakeDamage();
        Destroy(this.gameObject);
    }
}
