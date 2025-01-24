using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [Space(15)]

    [SerializeField]
    private SOPlayerData _playerData;

    /* Movement Members */

    private float m_Acceleration = 0.0f;
    private Vector3 m_MoveDirection = Vector3.zero;
    private Vector3 m_TargetDirection = Vector3.zero;

    /* Accessors */

    public uint Oxygen
    {
        get
        {
            // Validate the reference
            if (_playerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0u;
            }

            // Return the value
            return _playerData._oxygen;
        }
    }

    public float Speed
    {
        get
        {
            // Validate the reference
            if (_playerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }

            // Return the value
            return _playerData._maxSpeed * m_Acceleration;
        }
    }

    private Vector3 DeltaMove
    {
        get
        {
            return m_MoveDirection * Speed * Time.deltaTime;
        }
    }

    private float DirectionalControl
    {
        get
        {
            // Validate the reference
            if (_playerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }
            // Return the value
            return _playerData._directionalControl;
        }
    }

    /* Movement Methods */

    void OnMove(InputValue value)
    {
        // Get the input value
        Vector2 input = value.Get<Vector2>().normalized;

        // Set the target direction
        m_TargetDirection = new Vector3(input.x, 0.0f, input.y);
    }

    void Accelerate()
    {
        // Add the acceleration step to the current acceleration
        m_Acceleration += _playerData._accelerationStep * Time.deltaTime;

        // Clamp the acceleration to the range [0, 1]
        m_Acceleration = Mathf.Clamp(m_Acceleration, 0.0f, 1.0f);
    }

    void Decelerate()
    {
        // Subtract the acceleration step from the current acceleration
        m_Acceleration -= _playerData._accelerationStep * Time.deltaTime;

        // Clamp the acceleration to the range [0, 1]
        m_Acceleration = Mathf.Clamp(m_Acceleration, 0.0f, 1.0f);

        // If the acceleration is zero, make the move direction match the target direction
        if (m_Acceleration == 0.0f)
        {
            m_MoveDirection = m_TargetDirection;
        }
    }

    void LerpDirection()
    {
        m_MoveDirection = Vector3.Lerp(m_MoveDirection, m_TargetDirection, DirectionalControl);
    }

    void MoveUpdate()
    {
        // Compare the move direction with the target direction
        bool sameDirection = m_MoveDirection == m_TargetDirection;

        bool opositeDirection = m_MoveDirection == -m_TargetDirection;

        // Check if there is no target direction
        bool noTarget = m_TargetDirection == Vector3.zero;

        // There isn't movement to be done
        if (noTarget && sameDirection) { return; }

        // Accelerate if the directions are the same
        if (sameDirection) { Accelerate(); }

        // If the directions are opposite to each other, decelerate
        else if (opositeDirection)
        {
            Decelerate();
        }

        // If the directions are different, lerp the direction
        else
        {
            LerpDirection();
        }

        Move();
    }

    void Move()
    {
        // Move the player
        transform.Translate(DeltaMove);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void FixedUpdate()
    {
        // Update the movement
        MoveUpdate();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
