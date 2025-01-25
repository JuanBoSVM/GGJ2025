using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

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

    [SerializeField]
    [Tooltip("Component that sends the signal to the camera")]
    private CinemachineImpulseSource m_ImpulseSource;

    /* Movement Members */

    private float m_Acceleration = 0.0f;
    private Vector3 m_MoveDirection = Vector3.zero;
    private Vector3 m_TargetDirection = Vector3.zero;
    private Vector3 m_BufferedDirection = Vector3.zero;
    private float m_SpeedMultiplier = 1.0f;
    private float m_DashTimer = 0.0f;
    private float m_DashCooldown = 0.0f;

    /* Combat Members */

    private uint m_DamageTaken = 0u;
    private float m_HitboxActiveTime = 0.0f;
    private float m_HitboxCooldown = 0.0f;
    private float m_BubbleCooldown = 0.0f;
    private float m_StunnedTimer = 0.0f;
    private float m_InvulnerabilityTimer = 0.0f;

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
            return m_PlayerData._oxygen - m_DamageTaken;
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
            return m_PlayerData._maxSpeed * m_Acceleration * m_SpeedMultiplier;
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

    private uint MeleeDamage
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
            return m_PlayerData._meleeDamage;
        }
    }

    private float ParryMultiplier
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
            return m_PlayerData._parryMultiplier;
        }
    }

    private float ParryChance
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
            return m_PlayerData._parryChance;
        }
    }

    private Vector3 KnockbackDirection
    {
        get
        {
            // Validate the reference
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return Vector3.zero;
            }

            // Return the value
            return transform.forward * m_PlayerData._knockbackForce;
        }
    }

    private float InvulnerabilityTime
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
            return m_PlayerData._invulnerabilityDuration;
        }
    }

    private float DashDuration
    {
        get
        {
            // Validate the reference to the player data
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }

            // Return the value
            return m_PlayerData._dashDuration;
        }
    }

    private float DashSpeed
    {
        get
        {
            // Validate the reference to the player data
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }

            // Return the value
            return m_PlayerData._dashSpeed;
        }
    }

    private float DashCooldown
    {
        get
        {
            // Validate the reference to the player data
            if (m_PlayerData == null)
            {
                Debug.LogError("PlayerData reference not set in Player script");
                return 0.0f;
            }

            // Return the value
            return m_PlayerData._dashCooldown;
        }
    }

    /* Events */

    private void OnMove(InputValue value)
    {
        // Buffer inputs during the dash
        if (m_DashTimer > 0.0f)
        {
            m_BufferedDirection = value.Get<Vector2>().normalized;
            return;
        }

        // Get the input value
        Vector2 input = value.Get<Vector2>().normalized;

        // Set the target direction
        m_TargetDirection = new Vector3(input.x, 0.0f, input.y);
    }

    private void OnFire()
    {
        if (m_BubbleCooldown > 0.0f) { return; }

        // Spawn the bubble prefab
        Instantiate(BubblePrefab, transform.position, Quaternion.identity, transform);

        // Validate the reference to the player data
        if (m_PlayerData == null)
        {
            Debug.LogError("PlayerData reference not set in Player script");
            return;
        }

        // Set the remaining cooldown
        m_BubbleCooldown = m_PlayerData._bubbleCooldown;
    }

    private void OnAttack()
    {
        // Enable the hit box
        m_HitBox.enabled = true;
    }

    private void OnLook(InputValue value)
    {
        // Determine if the input scheme is a mouse or a gamepad
        bool isGamepad = m_PlayerInput.currentControlScheme == "Gamepad";

        // Get the input value
        Vector2 input = value.Get<Vector2>().normalized;

        if (!isGamepad)
        {
            // TODO: Implement mouse look
        }

        // Rotate the player to look at the target direction
        transform.LookAt(transform.position + new Vector3(input.x, 0.0f, input.y));
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if the other collider is a player
        if (other.gameObject.CompareTag("Player"))
        {
            // Shake the camera
            ShakeCameraManager.Instance.ShakeCamera(m_ImpulseSource);

            // Get the player script
            Player player = other.gameObject.GetComponent<Player>();

            // Check if the player script is valid
            if (player != null)
            {
                // Validate the reference to the player data
                if (m_PlayerData == null)
                {
                    Debug.LogError("PlayerData reference not set in Player script");
                    return;
                }

                // Knock the player back
                player.KnockBack(KnockbackDirection, MeleeDamage);
            }
        }

        // Check if the other collider is a bubble
        else if (other.gameObject.CompareTag("Bubble"))
        {
            // Get the bubble script
            Bubble bubble = other.gameObject.GetComponent<Bubble>();

            // Check if the bubble script is valid
            if (bubble != null)
            {
                // Validate the reference to the player data
                if (m_PlayerData == null)
                {
                    Debug.LogError("Bubble script component missing");
                    return;
                }

                // Randomly determine if the player will parry the bubble or pop it
                if (Random.value > ParryChance) { Destroy(bubble.gameObject); }
                else { bubble.Parry(transform.forward, ParryMultiplier); }
            }
        }
    }

    private void OnDash(InputValue value)
    {
        // Check if the input was a press or a release
        if (value.isPressed)
        {
            // Check if the dash is on cooldown
            if (m_DashCooldown > 0.0f) { return; }

            // Set a target direction for the dash if there isn't one
            if (m_TargetDirection == Vector3.zero) { m_TargetDirection = transform.forward; }

            // Set the dash timer
            m_DashTimer = DashDuration;

            // Set the speed multiplier
            m_SpeedMultiplier = DashSpeed;
        }

        else
        {
            // Check if the dash was interrupted
            if (m_DashTimer > 0.0f)
            {
                // Reduce the dash timer to zero
                m_DashTimer = 0.0f;

                // Reset the speed multiplier
                m_SpeedMultiplier = 1.0f;
            }
        }
    }

    /* Movement Methods */

    private void Accelerate()
    {
        // Add the acceleration step to the current acceleration
        m_Acceleration += m_PlayerData._accelerationStep * Time.deltaTime;

        // Clamp the acceleration to the range [0, 1]
        m_Acceleration = Mathf.Clamp(m_Acceleration, 0.0f, 1.0f);
    }

    private void Decelerate()
    {
        // Subtract the acceleration step from the current acceleration
        m_Acceleration -= m_PlayerData._accelerationStep * Time.deltaTime;

        // Clamp the acceleration to the range [0, 1]
        m_Acceleration = Mathf.Clamp(m_Acceleration, 0.0f, 1.0f);

        // If the acceleration is zero, reset the move direction
        if (m_Acceleration == 0.0f) { m_MoveDirection = Vector3.zero; }
    }

    private void LerpDirection()
    {
        m_MoveDirection = Vector3.Lerp(m_MoveDirection, m_TargetDirection, DirectionalControl);
    }

    private void MoveUpdate()
    {
        // If there's no current movement, but there's a target direction,
        // set the move direction to the target direction
        if (m_MoveDirection == Vector3.zero) { m_MoveDirection = m_TargetDirection; }

        // Check if the dash is active
        if (m_DashTimer > 0.0f)
        {
            // Subtract the time since the last frame from the dash timer
            m_DashTimer -= Time.deltaTime;

            // Clamp the dash timer to the range [0, DashDuration]
            m_DashTimer = Mathf.Clamp(m_DashTimer, 0.0f, DashDuration);

            // Check if the dash timer is zero
            if (m_DashTimer == 0.0f)
            {
                // Load the buffered direction if there's one
                if (m_BufferedDirection != Vector3.zero)
                {
                    m_TargetDirection = m_BufferedDirection;

                    // Reset the buffered direction
                    m_BufferedDirection = Vector3.zero;
                }

                // Reset the speed multiplier
                m_SpeedMultiplier = 1.0f;
            }
        }

        // Check if the dash is on cooldown
        if (m_DashCooldown > 0.0f)
        {
            // Subtract the time since the last frame from the dash cooldown
            m_DashCooldown -= Time.deltaTime;

            // Clamp the dash cooldown to the range [0, DashCooldown]
            m_DashCooldown = Mathf.Clamp(m_DashCooldown, 0.0f, DashCooldown);
        }

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

    private void AtackUpdate()
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
                m_HitboxCooldown = HitCooldown;
            }
        }

        // Check if the hitbox is on cooldown
        if (m_HitboxCooldown > 0.0f)
        {
            // Subtract the time since the last frame from the remaining cooldown
            m_HitboxCooldown -= Time.deltaTime;
        }

        // Check if the bubble is on cooldown
        if (m_BubbleCooldown > 0.0f)
        {
            // Subtract the time since the last frame from the remaining cooldown
            m_BubbleCooldown -= Time.deltaTime;
        }
    }

    private void Move()
    {
        // Move the player in world space
        transform.position += DeltaMove;
    }

    public void Stun(float seconds)
    {
        m_StunnedTimer = seconds;
    }

    public void KnockBack(Vector3 direction, uint damage = 0u)
    {
        // Check if the player is invulnerable
        if (m_InvulnerabilityTimer > 0.0f) { return; }

        // Move the player in the knockback direction
        transform.position += direction;

        // Decrease the player's oxygen
        m_DamageTaken += damage;

        // Become invulnerable for the determined time
        m_InvulnerabilityTimer = InvulnerabilityTime;
    }

    public void Heal(uint amount)
    {
        // Increase the player's oxygen
        m_DamageTaken -= amount;
    }

    private void Start()
    {
        if (m_ImpulseSource is null)
        {
            Debug.LogError("CinemachineImpulseSource reference not set in Player script");
        }
    }

    private void FixedUpdate()
    {
        // Check if the player is stunned
        if (m_StunnedTimer > 0.0f)
        {
            // Subtract the time since the last frame from the stunned timer
            m_StunnedTimer -= Time.deltaTime;

            // If the stunned timer is less than or equal to zero, reset the timer
            if (m_StunnedTimer <= 0.0f) { m_StunnedTimer = 0.0f; }
            else { return; }
        }

        // Update the movement
        MoveUpdate();

        // Update the attack
        AtackUpdate();
    }
}
