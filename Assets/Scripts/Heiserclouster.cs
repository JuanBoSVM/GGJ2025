using UnityEngine;

public class Heiserclouster : MonoBehaviour
{
    [SerializeField]
    private SOHeiserData m_heiserData;

    //List of heisers
    private GameObject[] m_heisers;

    //local variables
    private float m_timeToShoot = 0.0f;

    private void StartTimer ()
    {
        if (m_heiserData == null)
        {
            Debug.LogError("HeiserData reference not set in Heiser script");
            return;
        }
        m_timeToShoot = m_heiserData._bubbleCooldown;
    }

    private void Start()
    {
        if (m_heiserData == null)
        {
            Debug.LogError("HeiserData reference not set in Heiser script");
            return;
        }
        m_heisers = GameObject.FindGameObjectsWithTag("Heiser");
        Debug.Log("Heisers found: " + m_heisers.Length);
        StartTimer();
    }

    //Randomize which heiser shoots a bubble 
    private void HeiserControl()
    {
        if (m_timeToShoot <= 0.0f)
        {
            if (m_heiserData == null)
            {
                Debug.LogError("HeiserData reference not set in Heiser script");
                return;
            }

            m_timeToShoot = m_heiserData._bubbleCooldown;

            if (m_heisers.Length == 0)
            {
                Debug.LogError("No heisers found");
                return;
            }

            if (m_heisers.Length > 0)
            {
                int randomHeiser = Random.Range(0, m_heisers.Length);
                Debug.Log("Heiser " + randomHeiser + " shooting");
                m_heisers[randomHeiser].GetComponent<Heiser>().ShootBubble();
            }
        }
        else
        {
            m_timeToShoot -= Time.deltaTime;
        }
    }

    private void Update()
    {
        HeiserControl();
    }
}
