using UnityEngine;

public class Bubble : MonoBehaviour
{
    /* References */
    [SerializeField]
    private SOBubbleData m_BubbleData;

    [SerializeField]
    private SphereCollider m_Collider;

    private GameObject m_Owner;

    /* Movement Members */
    private float m_TraveledDistance = 0.0f;
    private Vector3 m_MoveDirection = Vector3.zero;

    /* Type Members */

    private bool m_IsHealthBubble = false;

    /* Accessors */

    private Vector3 DeltaMove
    {
        get
        {
            // Validate the reference to the bubble data
            if (m_BubbleData == null)
            {
                Debug.LogError("BubbleData reference not set in Bubble script");
                return Vector3.zero;
            }

            // Return the delta move
            return m_MoveDirection * m_BubbleData._speed * Time.fixedDeltaTime;
        }
    }

    private float Speed
    {
        get
        {
            // Validate the reference to the bubble data
            if (m_BubbleData == null)
            {
                Debug.LogError("BubbleData reference not set in Bubble script");
                return 0.0f;
            }
            // Return the speed

            return m_BubbleData._speed;
        }
    }

    private float Range
    {
        get
        {
            // Validate the reference to the bubble data
            if (m_BubbleData == null)
            {
                Debug.LogError("BubbleData reference not set in Bubble script");
                return 0.0f;
            }

            // Return the range
            return m_BubbleData._range;
        }
    }

    private float StunDuration
    {
        get
        {
            // Validate the reference to the bubble data
            if (m_BubbleData == null)
            {
                Debug.LogError("BubbleData reference not set in Bubble script");
                return 0.0f;
            }

            // Return the stun duration
            return m_BubbleData._stunDuration;
        }
    }

    private uint OxygenRestored
    {
        get
        {
            // Validate the reference to the bubble data
            if (m_BubbleData == null)
            {
                Debug.LogError("BubbleData reference not set in Bubble script");
                return 0u;
            }

            // Return the oxygen restored
            return m_BubbleData._oxygenRestored;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player
        if (other.gameObject.CompareTag("Player") && other.gameObject != m_Owner)
        {
            // Get the player script
            Player player = other.gameObject.GetComponent<Player>();

            // Check if the player script is valid
            if (player != null)
            {
                if (m_IsHealthBubble)
                {
                    // Restore the player's oxygen
                    player.Heal(OxygenRestored);
                }

                else
                {
                    // Stun the player
                    player.Stun(StunDuration);
                }
            }
        }

        // Destroy the bubble if it hits anything other than the shooter
        if (other.gameObject != m_Owner) { Destroy(gameObject); }
    }

    public void Purify()
    {
        m_IsHealthBubble = true;
    }

    public void Corrupt()
    {
        m_IsHealthBubble = false;
    }

    private void Start()
    {
        // Determine if the bubble has a parent
        if (transform.parent == null)
        {
            Debug.LogError("Bubble has no parent");
            Destroy(gameObject);
            return;
        }

        // Save a reference to the shooter
        m_Owner = transform.parent.gameObject;

        // Remove the bubble from its parent
        transform.parent = null;

        // Determine if the parent is a player
        if (transform.parent.gameObject.CompareTag("Player"))
        {
            // Match the rotation of the shooter
            m_MoveDirection = m_Owner.transform.forward;
        }

        // If the parent is not a player, the bubble will not move
        else { m_MoveDirection = Vector3.zero; }
    }

    private void FixedUpdate()
    {
        // Move the bubble until it reaches the range
        if (m_TraveledDistance < Range)
        {
            transform.position += DeltaMove;
            m_TraveledDistance += DeltaMove.magnitude;

            return;
        }

        // Destroy the bubble
        Destroy(gameObject);
    }
}
