using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Instance of the GameManager
    private static GameManager _instance;

    // List of the player game objects and its scores
    private List<Player> m_Players = new List<Player>();
    private List<uint> m_Scores = new List<uint>();

    // Public list not serialized
    [System.NonSerialized]
    public List<uint> m_PlayerSkinsID = new List<uint>();

    [SerializeField]
    GameObject m_P1Spawn;

    [SerializeField]
    GameObject m_P2Spawn;

    private bool m_P1Spawned = false;

    public uint GetPlayerSkinID(uint playerID)
    {
        if (playerID > m_PlayerSkinsID.Count)
        {
            Debug.LogError("Player ID out of range");
            return 0;
        }

        return m_PlayerSkinsID[(int)playerID];
    }

    public uint GetScore(uint playerID)
    {
        if (playerID > m_Scores.Count)
        {
            Debug.LogError("Player ID out of range");
            return 0;
        }

        return m_Scores[(int)playerID];
    }

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
        // Check the action map its using
        if (playerInput.defaultActionMap != "Player") { return; }

        // Move it to the spawn point
        if (!m_P1Spawned)
        {
            playerInput.transform.position = m_P1Spawn.transform.position;
            m_P1Spawned = true;
        }

        else
        {
            playerInput.transform.position = m_P2Spawn.transform.position;
        }

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
        player.SetID((uint)m_Players.Count - 1);

        // Change the game object's name
        player.gameObject.name = "Player " + m_Players.Count;

        // Subscribe to the player's death event
        player.OnDeath += OnPlayerDeath;
    }

    void OnPlayerDeath(uint id)
    {
        m_P1Spawned = false;

        // Increase the score of the other player
        m_Scores[(int)(id == 1 ? 0 : 1)]++;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find all the other game managers in the scene
        GameManager[] gameManagers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);

        foreach (GameManager gameManager in gameManagers)
        {
            // Delete all non original managers
            if (gameManager != Instance)
            {
                Destroy(gameManager.gameObject);
            }
        }

        // Initialize the scores
        m_Scores.Add(0);
        m_Scores.Add(0);

        // Make the GameManager persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
