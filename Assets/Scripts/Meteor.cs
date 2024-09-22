using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    [SerializeField] ScreenShake screenShakeCall;
    [SerializeField] float shakeAmount, shakeTimer; // The amount to change the Virtual Camera's amplitude by, and for how long.

    [SerializeField] AudioClip explosion;
    static public Action<AudioClip> OnMeteorDestroyed;

    // Awake is called on the first active frame
    void Awake()
    {
        screenShakeCall = GameObject.FindGameObjectWithTag("VCam").GetComponent<ScreenShake>();
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
            GameObject.Find("GameManager").GetComponent<GameManager>().gameOver = true;
            screenShakeCall.CamShake(shakeAmount, shakeTimer);
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        } else if (whatIHit.tag == "Laser")
        {
            OnMeteorDestroyed?.Invoke(explosion);
            screenShakeCall.CamShake(shakeAmount, shakeTimer);
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }
}
