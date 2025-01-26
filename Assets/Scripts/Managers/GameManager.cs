using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Instance of the GameManager
    private static GameManager _instance;

    // List of the player game objects
    private List<Player> m_Players = new List<Player>();

    // Accessor for the GameManager instance
    public static GameManager Instance
    {
        get
        {
            // Validate that the instance is not null
            if (_instance == null)
            {
                // Attempt to find the instance in the scene
                _instance = FindFirstObjectByType<GameManager>();

                // If the instance is still null,
                // create a new GameObject and add the GameManager component
                if (_instance == null)
                {
                    GameObject gm = new GameObject("GameManager");
                    _instance = gm.AddComponent<GameManager>();
                }
            }

            // Return the instance
            return _instance;
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Get the player component
        Player player = playerInput.gameObject.GetComponent<Player>();

        // Validate it
        if (player == null)
        {
            Debug.LogError("Player component not found in player object");
            return;
        }

        // Add the player to the list of players
        m_Players.Add(player);

        // Set the player's ID
        player.SetID((uint)m_Players.Count);

        // Change the game object's name
        player.gameObject.name = "Player " + m_Players.Count;

        // Subscribe to the player's death event
        player.OnDeath += OnPlayerDeath;
    }

    void OnPlayerDeath(uint id)
    {
        Debug.Log("Player " + id + " has died");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make the GameManager persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
