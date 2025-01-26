using UnityEngine;

public class Heiser : MonoBehaviour
{
    /*bubble members*/
    [SerializeField]
    private SOHeiserData m_heiserData;

    /* Vertical offset*/
    [SerializeField]
    private float m_verticalOffset = 0.0f;

    //solo metodos de burbujas, / heiser clouster, controla los heiser y sus burbujas
    private GameObject BubblePrefab
    {
        get
        {
            if (m_heiserData == null)
            {
                Debug.LogError("HeiserData reference not set in Heiser script");
                return null;
            }
            return m_heiserData._bubblePrefab;
        }
    }

    public void ShootBubble()
    {
        if (m_heiserData == null)
        {
            Debug.LogError("HeiserData reference not set in Heiser script");
            return;
        }
        Debug.Log("Bubble shot");
        m_verticalOffset = m_heiserData._verticalOffset;
        Instantiate(BubblePrefab, transform.position + new Vector3(0, m_verticalOffset, 0), transform.rotation);
    }
}

