using UnityEngine;

[CreateAssetMenu(fileName = "BubbleData", menuName = "Scriptable Objects/Bubble")]
public class SOBubbleData : ScriptableObject
{
    [Header("General Data")]
    [Space(15)]

    [Tooltip("The amount of oxygen the bubble restores if its a health bubble")]
    [Range(0u, 10u)]
    public uint _oxygenRestored;

    [Tooltip("The amount of time in seconds the bubble will stun for")]
    public float _stunDuration;

    [Tooltip("The speed of the bubble in units per second")]
    public float _speed;

    [Tooltip("The range of the bubble in units")]
    public float _range;
}
