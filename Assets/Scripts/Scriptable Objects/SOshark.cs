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

    [Header("Shark knockback")]
    [Tooltip("shark knockback")]
    public float SharkKnockback;
}
