using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /* References */

    [SerializeField]
    private SOPlayerData m_PlayerData;

    [SerializeField]
    private PlayerInput m_PlayerInput;

    [SerializeField]
    [Tooltip("The collider component of the player")]
    private CapsuleCollider m_Collider;

    [SerializeField]
    [Tooltip("The collider component of the melee attack")]
    private BoxCollider m_HitBox;

    /* Movement Members */

    private float m_Acceleration = 0.0f;
    private Vector3 m_MoveDirection = Vector3.zero;
    private Vector3 m_TargetDirection = Vector3.zero;

    /* Combat Members */

    private float m_HitboxActiveTime = 0.0f;
    private float m_HitboxRemainingCooldown = 0.0f;

    /* Accessors */

    public uint Oxygen
    {
        get
        {
            // Validate the reference
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0u;
            }

            // Return the value
            return m_PlayerData._oxygen;
        }
    }

    public float Speed
    {
        get
        {
            // Validate the reference
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }

            // Return the value
            return m_PlayerData._maxSpeed * m_Acceleration;
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
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }
            // Return the value
            return m_PlayerData._directionalControl;
        }
    }

    private GameObject BubblePrefab
    {
        get
        {
            // Validate the reference
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return null;
            }

            // Return the value
            return m_PlayerData._bubblePrefab;
        }
    }

    private float HitDuration
    {
        get
        {
            // Validate the reference
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }

            // Return the value
            return m_PlayerData._hitboxDuration;
        }
    }

    private float HitCooldown
    {
        get
        {
            // Validate the reference
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }

            // Return the value
            return m_PlayerData._hitboxCooldown;
        }
    }

    /* Input Methods */

    void OnMove(InputValue value)
    {
        // Get the input value
        Vector2 input = value.Get<Vector2>().normalized;

        // Set the target direction
        m_TargetDirection = new Vector3(input.x, 0.0f, input.y);
    }

    void OnFire()
    {
        // Spawn the bubble prefab
        Instantiate(BubblePrefab, transform.position, Quaternion.identity, transform);
    }

    void OnAttack()
    {
        // Enable the hit box
        m_HitBox.enabled = true;
    }

    /* Movement Methods */

    void Accelerate()
    {
        // Add the acceleration step to the current acceleration
        m_Acceleration += m_PlayerData._accelerationStep * Time.deltaTime;

        // Clamp the acceleration to the range [0, 1]
        m_Acceleration = Mathf.Clamp(m_Acceleration, 0.0f, 1.0f);
    }

    void Decelerate()
    {
        // Subtract the acceleration step from the current acceleration
        m_Acceleration -= m_PlayerData._accelerationStep * Time.deltaTime;

        // Clamp the acceleration to the range [0, 1]
        m_Acceleration = Mathf.Clamp(m_Acceleration, 0.0f, 1.0f);
    }

    void LerpDirection()
    {
        m_MoveDirection = Vector3.Lerp(m_MoveDirection, m_TargetDirection, DirectionalControl);
    }

    void MoveUpdate()
    {
        // If there's no current movement, but there's a target direction,
        // set the move direction to the target direction
        if (m_MoveDirection == Vector3.zero) { m_MoveDirection = m_TargetDirection; }

        // Compare the move direction with the target direction
        bool sameDirection = m_MoveDirection == m_TargetDirection;
        bool opositeDirection = m_MoveDirection == -m_TargetDirection;

        // Check if there is no target direction
        bool noTarget = m_TargetDirection == Vector3.zero;

        // There isn't movement to be done
        if (noTarget && sameDirection) { return; }

        // If the directions are opposite to each other, decelerate
        if (opositeDirection || noTarget)
        {
            Decelerate();
        }

        // If the directions are different, lerp the direction
        else
        {
            Accelerate();
            LerpDirection();
        }

        if (DeltaMove != Vector3.zero) { Move(); }
    }

    void AtackUpdate()
    {
        // Check if the hitbox is active
        if (m_HitBox.enabled)
        {
            // Add the time since the last frame to the active time
            m_HitboxActiveTime += Time.deltaTime;

            // Check if the active time is greater than the hit duration
            if (m_HitboxActiveTime >= HitDuration)
            {
                // Disable the hitbox
                m_HitBox.enabled = false;

                // Reset the active time
                m_HitboxActiveTime = 0.0f;

                // Set the remaining cooldown
                m_HitboxRemainingCooldown = HitCooldown;
            }
        }

        // Check if the hitbox is on cooldown
        if (m_HitboxRemainingCooldown > 0.0f)
        {
            // Subtract the time since the last frame from the remaining cooldown
            m_HitboxRemainingCooldown -= Time.deltaTime;
        }
    }

    void Move()
    {
        // Move the player
        transform.Translate(DeltaMove);
    }

    void FixedUpdate()
    {
        // Update the movement
        MoveUpdate();

        // Update the attack
        AtackUpdate();
    }
}
