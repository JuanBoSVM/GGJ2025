using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SOshark", menuName = "Scriptable Objects/SOshark")]
public class SOshark : ScriptableObject
{
    [Header("SharkDJ genal data")]
    //velocidad, daño
    [Tooltip("shark speed")]
    public float _sharkSpeed;

    [Tooltip("shark damage")]
    public uint _sharkDamage;

    [Header("Shark prefab")]
    [Tooltip("The shark prefab")]
    public GameObject _sharkPrefab;

    [Header("Player prefab")]
    [Tooltip ("player prefab")]
    public GameObject _playerPrefab;

    [Header("Shark collider")]
    [Tooltip ("shark´s collider component")]
    public BoxCollider _hitBox;
}
