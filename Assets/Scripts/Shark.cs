using Unity.VisualScripting;
using UnityEngine;

public class Shark : MonoBehaviour
{
    /*shark members*/
    [SerializeField]
    private SOshark m_sharkData;

    /* Shark speed */
    private float m_sharkSpeed = 0.0f;

    /* Shark damage*/
    private uint m_sharkDamage = 0;

    /* Shark direction */
    private Vector3 m_sharkDirection = Vector3.zero;

    /* Shark knockback */
    private float m_sharkKnockbackForce = 0.0f;

    /* Shark knockback direction */
    private Vector3[] m_knockbackDirection;

    /*shark position*/
    private Vector3 m_sharkPosition = Vector3.zero;

    private Player[] m_knockedPlayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (m_sharkData == null)
        {
            Debug.LogError("SharkData reference not set in Shark script");
            return;
        }
        m_sharkSpeed = m_sharkData._sharkSpeed;
        m_sharkDamage = m_sharkData._sharkDamage;
        m_sharkKnockbackForce = m_sharkData.SharkKnockback;
    }

    //Get shark position
    private void GetSharkPosition()
    {
        m_sharkPosition = transform.position;
    }

    private Vector3 CalculateKnockbackDirection(Vector3 playerPosition)
    {
        Vector3 knockbackDirection = playerPosition - m_sharkPosition;
        knockbackDirection.Normalize();
        return knockbackDirection;
    }

    //Start moving the shark to a direction
    public void Move()
    {
        transform.position += m_sharkDirection * m_sharkSpeed * Time.deltaTime;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        Move();

        for (int i = 0; i < m_knockedPlayers.Length; i++)
        {
            if (m_knockedPlayers[i] != null)
            {
                StartCoroutine(m_knockedPlayers[i].KnockBack(m_knockbackDirection[i], m_sharkKnockbackForce, m_sharkDamage));
            }
        }
    }
    public void OnTriggerEnter(Collider colider)
    {
        Debug.Log("Shark collided with something");

        if (colider.gameObject.CompareTag("Player"))
        {
            // Handle collision with player
            Debug.Log("Shark collided with player");

            Player player = colider.gameObject.GetComponent<Player>();
            if (player == null)
            {
                Debug.Log("Null reference player ");
            }
            GetSharkPosition();

            // Add the player to the knockback list
            m_knockedPlayers[m_knockedPlayers.Length] = player;

            // Calculate the knockback direction
            Vector3 knockDir = CalculateKnockbackDirection(player.transform.position);

            // Add the knockback to the knockback list
            m_knockbackDirection[m_knockbackDirection.Length] = knockDir;
        }
    }
}
