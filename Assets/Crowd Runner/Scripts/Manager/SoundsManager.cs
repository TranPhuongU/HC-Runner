using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{

    [Header("Sounds")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource doorHitSound;
    [SerializeField] private AudioSource runnerDieSound;
    [SerializeField] private AudioSource levelComplateSound;
    [SerializeField] private AudioSource gameoverSound;
    [SerializeField] private AudioSource coinSound;
    // Start is called before the first frame update
    void Start()
    {
        PlayerDetection.onDoorsHit += PlayDoorHitSound;

        GameManager.onGameStateChanged += GameStateChangedCallback;

        Enemy.onRunnerDied += PlayRunnerDieSound;

        PlayerDetection.onCoinsHit += PlayerHitCoinSound;
    }

    private void OnDestroy()
    {
        PlayerDetection.onDoorsHit -= PlayDoorHitSound;

        PlayerDetection.onCoinsHit -= PlayerHitCoinSound;

        GameManager.onGameStateChanged -= GameStateChangedCallback;

        Enemy.onRunnerDied -= PlayRunnerDieSound;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayDoorHitSound()
    {
        doorHitSound.Play();
    }

    private void GameStateChangedCallback(GameManager.GameState gameState)
    {
        if(gameState == GameManager.GameState.LevelComplete)
            levelComplateSound.Play();
        else if(gameState == GameManager.GameState.Gameover)
            gameoverSound.Play();
    }

    private void PlayRunnerDieSound()
    {
        runnerDieSound.Play();
    }

    private void PlayerHitCoinSound()
    {
        coinSound.Play();
    }

    public void DisableSounds()
    {
        doorHitSound.volume = 0;
        runnerDieSound.volume = 0;
        levelComplateSound.volume = 0;
        gameoverSound.volume = 0;
        buttonSound.volume = 0;
        coinSound.volume = 0;
    }

    public void EnableSounds()
    {
        doorHitSound.volume = 1;
        runnerDieSound.volume = 1;
        levelComplateSound.volume = 1;
        gameoverSound.volume = 1;
        buttonSound.volume = 1;
        coinSound.volume = 1;
    }
}
