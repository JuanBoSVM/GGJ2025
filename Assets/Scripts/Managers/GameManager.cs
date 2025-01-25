using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Instance of the GameManager
    private static GameManager _instance;

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
