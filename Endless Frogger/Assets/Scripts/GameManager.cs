using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // The static reference to the GameManager, ensuring it's a singleton
    public static GameManager instance;
    private int coinCollection;

    void Awake()
    {
        // Singleton pattern - ensure only one GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this GameObject persists across scenes
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy this one
        }
    }

    public void CoinCollectionIncrease(int count)
    {
        coinCollection += count;
    }

    public int getCoinCollection() { return coinCollection; }
}
