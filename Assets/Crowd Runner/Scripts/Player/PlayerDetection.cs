using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDetection : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private CrowdSystem crowdSystem;

    [Header("Events")]
    public static Action onDoorsHit;
    public static Action onCoinsHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.IsGameState())
            DetectColliders();
    }

    private void DetectColliders()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, crowdSystem.GetCrowdRadius());

        for (int i = 0; i < detectedColliders.Length; i++)
        {
            if(detectedColliders[i].TryGetComponent(out Doors doors))
            {
                Debug.Log("We hit some doors");

                int bonusAmount = doors.GetBonusAmount(transform.position.x);
                BonusType bonusType = doors.GetBonusType(transform.position.x);

                doors.Disable();
                onDoorsHit?.Invoke();

                crowdSystem.ApplyBonus(bonusType, bonusAmount);
            }
        
            else if(detectedColliders[i].tag == "Finish")
            {
                Debug.Log("We've hit the finish line");

                PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);

                GameManager.instance.SetGameState(GameManager.GameState.LevelComplete);

                //SceneManager.LoadScene(0);
            }
            
            else if (detectedColliders[i].tag == "Coin")
            {
                Destroy(detectedColliders[i].gameObject);
                onCoinsHit?.Invoke();

                DataManager.instance.AddCoins(1);
            }
        }
    }
}
