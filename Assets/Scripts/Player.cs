using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    /* Delegate Declaration */

    public delegate void DeathEvent(uint id);
    public DeathEvent OnDeath;

    /* References */

    [SerializeField]
    private SOPlayerData m_PlayerData;

    [SerializeField]
    private PlayerInput m_PlayerInput;

    [SerializeField]
    [Tooltip("Component that sends the signal to the camera")]
    private CinemachineImpulseSource m_ImpulseSource;

    // List of hittable objects
    private List<GameObject> m_Hittables = new List<GameObject>();

    /* Other Members */

    private uint m_PlayerID = 0u;

    /* Movement Members */

    private float m_Acceleration = 0.0f;
    private Vector3 m_MoveDirection = Vector3.zero;
    private Vector3 m_TargetDirection = Vector3.zero;
    private Vector3 m_BufferedDirection = Vector3.zero;
    private float m_SpeedMultiplier = 1.0f;
    private float m_DashCooldown = 0.0f;

    /* Combat Members */

    private uint m_DamageTaken = 0u;
    private float m_HitboxCooldown = 0.0f;
    private float m_BubbleCooldown = 0.0f;

    /* Timers */

    private float m_HitboxTimer = 0.0f;
    private float m_OxygenTimer = 0.0f;
    private float m_StunnedTimer = 0.0f;
    private float m_InvulnerabilityTimer = 0.0f;
    private float m_DashTimer = 0.0f;

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

    private float BubbleCooldown
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
            return m_PlayerData._bubbleCooldown;
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

    private Vector3 KnockbackDelta
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
            return transform.forward * m_PlayerData._knockbackDistance;
        }
    }

    private float KnockbackDuration
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
            return m_PlayerData._knockbackDuration;
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

    private float OxygenDuration
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
            return m_PlayerData._oxygenDuration;
        }
    }

    private float StunReduction
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
            return m_PlayerData._stunReduction;
        }
    }

    /* Setup Methods */

    public void SetID(uint id)
    {
        m_PlayerID = id;
    }

    /* Events */

    private void OnMove(InputValue value)
    {
        // Reduce stun timer
        if (m_StunnedTimer > 0.0f) { m_StunnedTimer -= StunReduction; }

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
        if (m_BubbleCooldown > 0.0f || Oxygen == 1u) { return; }

        // Spawn the bubble prefab
        Instantiate(BubblePrefab, transform.position, Quaternion.identity, transform);

        // Take damage from the bubble
        m_DamageTaken++;

        // Validate the reference to the player data
        if (m_PlayerData == null)
        {
            Debug.LogError("PlayerData reference not set in Player script");
            return;
        }

        // Set the remaining cooldown
        m_BubbleCooldown = BubbleCooldown;
    }

    private void OnAttack()
    {
        // Set the hitbox timer if it's not on cooldown
        if (m_HitboxCooldown == 0.0f) { m_HitboxTimer = HitDuration; }
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
        // Add the GameObject to the list of hittables
        m_Hittables.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        // Remove the GameObject from the list of hittables
        m_Hittables.Remove(other.gameObject);
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
            }

            // End the dash
            EndDash();
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

    private void EndDash()
    {
        // Load the buffered direction
        m_TargetDirection = m_BufferedDirection;

        // Reset the buffered direction
        m_BufferedDirection = Vector3.zero;

        // Reset the speed multiplier
        m_SpeedMultiplier = 1.0f;
    }

    // Return true if the timer is greater than zero
    private bool TickTimer(ref float timer)
    {
        if (timer > 0.0f)
        {
            // Subtract the time since the last frame from the timer
            timer -= Time.deltaTime;

            // Limit the timer to 0.0f
            if (timer < 0.0f)
            {
                timer = 0.0f;

                return false;
            }

            return true;
        }

        return false;
    }

    private void Move()
    {
        // Move the player in world space
        transform.position += DeltaMove;
    }

    /* Combat Methods */

    private void ScanForHittables()
    {
        // Check if there are no hittables
        if (m_Hittables.Count == 0) { return; }

        // Loop through the hittables list
        foreach (GameObject hittable in m_Hittables)
        {
            // Ignore the player itself
            if (hittable == gameObject) { continue; }

            // Check if the other collider is a player
            if (hittable.CompareTag("Player"))
            {
                // Shake the camera
                ShakeCameraManager.Instance.ShakeCamera(m_ImpulseSource);

                // Get the player script
                Player player = hittable.GetComponent<Player>();

                // Check if the player script is valid
                if (player != null)
                {
                    // Knock the player back as a coroutine
                    StartCoroutine(player.KnockBack(KnockbackDelta, KnockbackDuration, MeleeDamage));
                }

                continue;
            }

            // Check if the other collider is a bubble
            if (hittable.CompareTag("Bubble"))
            {
                // Get the bubble script
                Bubble bubble = hittable.GetComponent<Bubble>();

                // Check if the bubble script is valid
                if (bubble != null)
                {
                    // Randomly determine if the player will parry the bubble or pop it
                    if (Random.value > ParryChance) { Destroy(bubble.gameObject); }

                    else
                    {
                        bubble.Redirect(transform.forward, ParryMultiplier);

                        // Also change ownership of the bubble
                        bubble.SetOwner(gameObject);
                    }
                }
            }
        }

        // Clear the hittables list
        m_Hittables.Clear();
    }

    public void Stun(float seconds)
    {
        m_StunnedTimer = seconds;

        // Clear the hittables list
        m_Hittables.Clear();
    }

    public IEnumerator KnockBack(Vector3 direction, float duration, uint damage = 0u)
    {
        // Check if the player is invulnerable
        if (!TickTimer(ref m_InvulnerabilityTimer))
        {
            float distanceMoved = 0.0f;

            Vector3 frameMove;

            // Continue moving the player in the knockback direction
            while (distanceMoved < direction.magnitude)
            {
                // Calculate the movement for the frame
                frameMove = direction * Time.deltaTime / duration;

                // Move the player in the knockback direction
                transform.position += frameMove;

                // Increase the distance moved
                distanceMoved += frameMove.magnitude;

                // Yield until the next frame
                yield return null;
            }

            // Decrease the player's oxygen
            m_DamageTaken += damage;

            // Become invulnerable for the determined time
            m_InvulnerabilityTimer = InvulnerabilityTime;
        }
    }

    public void Hurt(uint amount)
    {
        // Increase the player's oxygen
        m_DamageTaken += amount;
    }

    public void Kill()
    {
        // Validate the reference to the player data
        if (m_PlayerData == null)
        {
            Debug.LogError("PlayerData reference not set in Player script");
            return;
        }

        // Set the player's oxygen to zero
        m_DamageTaken = m_PlayerData._oxygen;
    }

    public void Heal(uint amount)
    {
        // Increase the player's oxygen
        m_DamageTaken -= amount;
    }

    /* Control Synchronization */

    public void SetPlayerDevice()
    {
        Debug.Log(m_PlayerInput.user.pairedDevices);
    }

    /* Game Loop */

    private void MoveUpdate()
    {
        // If there's no current movement, but there's a target direction,
        // set the move direction to the target direction
        if (m_MoveDirection == Vector3.zero) { m_MoveDirection = m_TargetDirection; }

        // Tick the timers
        TickTimer(ref m_DashTimer);
        if (m_DashCooldown > 0.0f && !TickTimer(ref m_DashCooldown)) { EndDash(); }

        // Directional control variables
        bool sameDirection = m_MoveDirection == m_TargetDirection;
        bool opositeDirection = m_MoveDirection == -m_TargetDirection;
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

    private void CombatUpdate()
    {
        // Tick the hitbox timers and scan for hittables if it's active
        if (TickTimer(ref m_HitboxTimer) && !TickTimer(ref m_HitboxCooldown))
        {
            ScanForHittables();
        }

        TickTimer(ref m_BubbleCooldown);
        TickTimer(ref m_InvulnerabilityTimer);
    }

    private void Start()
    {
        if (m_ImpulseSource is null)
        {
            Debug.LogError("CinemachineImpulseSource reference not set in Player script");
        }

        Debug.Log(m_PlayerInput.user.pairedDevices);

        // Set the oxygen timer
        m_OxygenTimer = OxygenDuration;
    }

    private void FixedUpdate()
    {
        if (TickTimer(ref m_StunnedTimer) || Oxygen == 0u) { return; }

        // Update the movement
        MoveUpdate();

        // Update the attack
        CombatUpdate();

        // Update the player oxygen
        if (Oxygen > 0)
        {
            if (!TickTimer(ref m_OxygenTimer))
            {
                // Reset the timer and take damage
                m_OxygenTimer = OxygenDuration;
                m_DamageTaken++;
            }
        }

        else
        {
            // Send the death signal
            OnDeath?.Invoke(m_PlayerID);
        }
    }
}
