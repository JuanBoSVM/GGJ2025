using UnityEngine;

public class Shark : MonoBehaviour
{ 
    /*shark members*/
    [SerializeField]
    private SOshark m_sharkData;

    /* Shark speed */
    [SerializeField]
    private float m_sharkSpeed = 0.0f;

    /* Shark damage*/
    [SerializeField]
    private uint m_sharkDamage = 0;

    /* Shark direction */
    [SerializeField]
    private Vector3 m_sharkDirection = Vector3.zero;

    /*player reference*/
    [SerializeField]
    private GameObject m_player;

    [SerializeField]
    [Tooltip("Shark´s collider component")]
    private BoxCollider m_hitbox;

    private Collision m_collision;

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

        //onAttack();
    }



    private void onAttack()
    {
        
    }

    //Collision with player
    private void OnCollision()
    {

    }

    public void Move ()
    {
        transform.position += m_sharkDirection * m_sharkSpeed * Time.deltaTime;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        Move();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == m_player)
        {
            // Handle collision with player
            Debug.Log("Shark collided with player");
        }
    }
}
