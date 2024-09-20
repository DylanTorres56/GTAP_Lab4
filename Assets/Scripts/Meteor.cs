using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    [SerializeField] ScreenShake screenShakeCall;
    [SerializeField] float shakeAmount, shakeTimer; // The amount to change the Virtual Camera's amplitude by, and for how long.

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
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
            screenShakeCall.CamShake(shakeAmount, shakeTimer);
        } else if (whatIHit.tag == "Laser")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().meteorCount++;
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
            screenShakeCall.CamShake(shakeAmount, shakeTimer);
        }
    }
}
