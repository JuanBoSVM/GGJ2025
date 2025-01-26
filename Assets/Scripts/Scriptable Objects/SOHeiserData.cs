using UnityEngine;

[CreateAssetMenu(fileName = "HeiserData", menuName = "Heiser Data")]
public class SOHeiserData : ScriptableObject
{
    [Header ("Bubble cooldown")]
    [Space(10)]
    [Range(0.0f, 10f)]
    [Tooltip("Bubble cooldown in seconds")]
    //randomize cooldown
    public float _bubbleCooldown;

    [Tooltip("Bubble prefab")]
    public GameObject _bubblePrefab;

    [Header ("Bubble Spawn")]
    [Space(10)]
    [Range(0.0f, 10f)]
    [Tooltip("Vertical offset for the bubble spawn")]
    public float _verticalOffset;
}
