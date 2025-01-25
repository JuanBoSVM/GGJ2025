using UnityEngine;

public class Bubble : MonoBehaviour
{
    /* References */
    [SerializeField]
    private SOBubbleData m_BubbleData;

    [SerializeField]
    private SphereCollider m_Collider;

    private GameObject m_Shooter;

    /* Movement Members */
    private float m_TraveledDistance = 0.0f;
    private Vector3 m_MoveDirection = Vector3.zero;

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

    public void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player
        if (other.gameObject.CompareTag("Player") && other.gameObject != m_Shooter)
        {
            // Get the player script
            Player player = other.gameObject.GetComponent<Player>();

            // Check if the player script is valid
            if (player != null)
            {
                // Validate the reference to the bubble data
                if (m_BubbleData == null)
                {
                    Debug.LogError("BubbleData reference not set in Bubble script");
                    Destroy(gameObject);
                    return;
                }

                // Stun the player
                player.Stun(m_BubbleData._stunDuration);
            }
        }

        // Destroy the bubble if it hits anything other than the shooter
        if (other.gameObject != m_Shooter) { Destroy(gameObject); }
    }

    private void Start()
    {
        // Save a reference to the shooter
        m_Shooter = transform.parent.gameObject;

        // Remove the bubble from its parent
        transform.parent = null;

        // Match the rotation of the shooter
        m_MoveDirection = m_Shooter.transform.forward;
    }

    private void FixedUpdate()
    {
        // Move the bubble until it reaches the range
        if (m_TraveledDistance < m_BubbleData._range)
        {
            transform.position += DeltaMove;
            m_TraveledDistance += DeltaMove.magnitude;

            return;
        }

        // Destroy the bubble
        Destroy(gameObject);
    }
}
