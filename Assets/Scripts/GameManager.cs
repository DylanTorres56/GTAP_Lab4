using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject meteorPrefab;
    public GameObject bigMeteorPrefab;
    public bool gameOver = false;

    public int meteorCount = 0;
    [SerializeField]
    InputAction restartAction;

    public AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        InvokeRepeating("SpawnMeteor", 1f, 2f);
        restartAction.performed += Restart;
        restartAction.Enable();

        Meteor.OnMeteorDestroyed += MeteorDestroyed;
        Meteor.OnMeteorDamaged += PlaySound;
        Player.PlayerDied += PlayerDied;
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    void PlayerDied()
    {
        gameOver = true;
        CancelInvoke();

    }
    void Restart(InputAction.CallbackContext context)
    {
        if (gameOver)
        {
            Meteor.OnMeteorDestroyed -= MeteorDestroyed;
            Meteor.OnMeteorDamaged -= PlaySound;
            Player.PlayerDied -= PlayerDied;
            SceneManager.LoadScene("Week5Lab");
        }
    }
    void SpawnMeteor()
    {
        Instantiate(meteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.Euler(0, 0, Random.Range(-75.0f, 75.0f)));
    }

    void BigMeteor()
    {
        meteorCount = 0;
        Instantiate(bigMeteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }
    void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip, 1.0f);
    }
    void MeteorDestroyed(AudioClip audioClip)
    {
        PlaySound(audioClip);
        meteorCount++;
        if (meteorCount == 5)
        {
            BigMeteor();
        }
    }
}
