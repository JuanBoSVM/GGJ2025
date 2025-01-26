using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private Color m_P1HighlightColor;

    [SerializeField]
    private Color m_P2HighlightColor;

    [SerializeField]
    private List<GameObject> m_ButtonObjects;
    private List<Button> m_ButtonComponents = new List<Button>();

    [SerializeField]
    private PlayerInput m_P1PlayerInput;
    [SerializeField]
    private PlayerInput m_P2PlayerInput;

    public void SelectPlayer(int id)
    {
        // Change the highlight color
        ChangeHighlightColor(m_P2HighlightColor);

        // Send the player ID to the GameManager
        GameManager.Instance.m_PlayerSkinsID.Add((uint)id);
    }

    private void ChangeHighlightColor(Color color)
    {
        // Validate the list of buttons
        if (m_ButtonComponents.Count == 0)
        {
            return;
        }

        // Change the highlighted color
        ColorBlock colors = m_ButtonComponents[0].colors;
        colors.highlightedColor = color;

        // Set it to all the buttons
        foreach (Button button in m_ButtonComponents)
        {
            button.colors = colors;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        // Load the button components
        foreach (GameObject button in m_ButtonObjects)
        {
            m_ButtonComponents.Add(button.GetComponent<Button>());
        }

        ChangeHighlightColor(m_P1HighlightColor);
    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
