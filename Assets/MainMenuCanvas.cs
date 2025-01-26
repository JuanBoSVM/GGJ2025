using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    /* References */
    [SerializeField]
    private List<GameObject> m_ButtonObjects;
    private List<Button> m_ButtonComponents = new List<Button>();

    [SerializeField]
    private PlayerInput m_PlayerInput;

    private uint m_HighlightedButton = 0u;

    public void OnSubmit()
    {
        // Disable the player input
        m_PlayerInput.enabled = false;

        // Click the highlighted button
        m_ButtonComponents[(int)m_HighlightedButton].onClick.Invoke();
    }

    public void OnNavigate(InputValue value)
    {
        // Get the value of the input
        Vector2 input = value.Get<Vector2>();

        // If the input is not zero change the highlighted button
        if (input != null)
        {
            // If the input is up
            if (input.y > 0)
            {
                // If the highlighted button is not the first one
                if (m_HighlightedButton > 0)
                {
                    // Change the highlighted button
                    m_ButtonComponents[(int)m_HighlightedButton].OnDeselect(null);
                    m_HighlightedButton--;
                    m_ButtonComponents[(int)m_HighlightedButton].OnSelect(null);
                }

                // Loop the highlighted button
                else
                {
                    // Change the highlighted button
                    m_ButtonComponents[(int)m_HighlightedButton].OnDeselect(null);
                    m_HighlightedButton = (uint)(m_ButtonComponents.Count - 1);
                    m_ButtonComponents[(int)m_HighlightedButton].OnSelect(null);
                }
            }

            // If the input is down
            else if (input.y < 0)
            {
                // If the highlighted button is not the last one
                if (m_HighlightedButton < m_ButtonComponents.Count - 1)
                {
                    // Change the highlighted button
                    m_ButtonComponents[(int)m_HighlightedButton].OnDeselect(null);
                    m_HighlightedButton++;
                    m_ButtonComponents[(int)m_HighlightedButton].OnSelect(null);
                }

                // Loop the highlighted button
                else
                {
                    // Change the highlighted button
                    m_ButtonComponents[(int)m_HighlightedButton].OnDeselect(null);
                    m_HighlightedButton = 0u;
                    m_ButtonComponents[(int)m_HighlightedButton].OnSelect(null);
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load the button components
        foreach (GameObject button in m_ButtonObjects)
        {
            m_ButtonComponents.Add(button.GetComponent<Button>());
        }

        // Highlight the first button
        m_ButtonComponents[(int)m_HighlightedButton].OnSelect(null);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
