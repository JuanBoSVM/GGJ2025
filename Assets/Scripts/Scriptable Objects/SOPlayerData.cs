using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player")]
public class SOPlayerData : ScriptableObject
{
    [Header("General Data")]
    [Space(15)]
    [Range(0, 10)]
    public uint _oxygen;

    [Space(10)]
    [Header("Movement Data")]
    [Space(15)]

    public float _maxSpeed;

    [Tooltip(
        "The speed gained per second based on the max speed\n" +
        "0 = Never accelerates\n" +
        "1 = Full speed in one second\n" +
        "2 = Full speed in half a second...")]
    public float _accelerationStep;

    [Tooltip(
        "How sharply the player can turn\n" +
        "0 = The player cannot turn\n" +
        "1 = The player instantly turns around")]
    [Range(0.0f, 1.0f)]
    public float _agility;

    [Space(10)]
    [Header("Ranged Combat Data")]
    [Space(15)]

    [Tooltip("The amount of time in seconds the bubble will stun for")]
    public float _stunDuration;

    [Tooltip("The speed of the bubble in units per second")]
    public float _bubbleSpeed;

    [Tooltip("The range of the bubble in units")]
    public float _bubbleRange;


    [Space(10)]
    [Header("Melee Combat Data")]
    [Space(15)]

    [Tooltip("Units to send the opponent flying on a melee hit")]
    public float _knockbackForce;

    [Tooltip("The range of the melee attack in units")]
    public float _meleeRange;

    [Tooltip("The amount of damage the melee attack does")]
    [Range(0, 10)]
    public uint _meleeDamage;
}
