using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player")]
public class SOPlayerData : ScriptableObject
{
    [Header("General Data")]
    [Space(15)]
    [Range(0u, 10u)]
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
    public float _directionalControl;

    [Tooltip("The speed multiplier during the dash")]
    public float _dashSpeed;

    [Tooltip("The duration of the dash in seconds")]
    public float _dashDuration;

    [Tooltip("The cooldown of the dash in seconds")]
    public float _dashCooldown;

    [Space(10)]
    [Header("Ranged Combat Data")]
    [Space(15)]

    [Tooltip("How many charge phases will the projectile have")]
    public uint _chargePhases;

    [Tooltip("The time in seconds the player has to charge the shot phase")]
    public float _phaseChargeTime;

    [Tooltip("Cooldown between shots in seconds")]
    public float _bubbleCooldown;

    [Tooltip("Prefab of the bubble object")]
    public GameObject _bubblePrefab;


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

    [Tooltip("The speed multiplier for the bubble when attacking it")]
    public float _parryMultiplier;

    [Tooltip("Chance to parry the bubble")]
    [Range(0.0f, 1.0f)]
    public float _parryChance;

    [Tooltip("The amount of time in seconds the player is invulnerable after being hit")]
    public float _invulnerabilityDuration;

    [Tooltip("The active time of the hitbox in seconds")]
    public float _hitboxDuration;

    [Tooltip("The cooldown time of the hitbox in seconds")]
    public float _hitboxCooldown;
}
