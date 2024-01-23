using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptables/Player Settings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    [Header("Forces")]

    [Tooltip("Movement force when tiptoe walking.")]
    [Min(0f)]
    public float TiptoeForce = 10f;

    [Tooltip("Movement force when walking.")]
    [Min(0f)]
    public float WalkForce = 15f;

    [Tooltip("Movement force when sprinting.")]
    [Min(0f)]
    public float SprintForce = 30f;

    [Space]
    [Header("Thresholds")]

    [Tooltip("Minimum normalized walk force required to start walking. Anything under this is considered tiptoe walking.")]
    [Range(0f, 1f)]
    public float WalkForceThreshold = 1f;

    [Tooltip("Minimum normalized walk force required to start sprinting.")]
    [Range(0f, 1f)]
    public float SprintForceThreshold = 1f;

    [Tooltip("Minimum normalized charge time required to perform a normal stamp. Anything under this will be a light stamp, and charging up to Max Charge Time will be a loud stamp.")]
    [Range(0f, 1f)]
    public float NormalStampThreshold = 0.5f;

    [Space]
    [Header("Cooldown Times")]

    [Tooltip("Stamp max charge time, in seconds.")]
    [Min(0f)]
    public float StampMaxChargeTime = 1.5f;

    [Tooltip("Stamp cooldown time, in seconds.")]
    [Min(0f)]
    public float StampCooldownTime = 0.1f;

    [Tooltip("Time it takes to go from walk to idle.")]
    [Min(0f)]
    public float WalkToIdleTime = 1f;

    [Space]
    [Header("Sound Waves")]

    [Tooltip("How loud steps are when tiptoe walking.")]
    public Loudness TiptoeLoudness = Loudness.VeryQuiet;

    [Tooltip("How loud steps are when walking.")]
    public Loudness WalkLoudness = Loudness.Quiet;

    [Tooltip("How loud steps are when sprinting.")]
    public Loudness SprintLoudness = Loudness.Normal;

    [Tooltip("Sound properties of light stamps.")]
    public SoundProperties StampLightSoundProperties;

    [Tooltip("Sound properties of normal stamps.")]
    public SoundProperties StampNormalSoundProperties;

    [Tooltip("Sound properties of loud stamps.")]
    public SoundProperties StampLoudSoundProperties;

    [Space]
    [Header("Step Distance")]

    [Tooltip("Distance between each step when tiptoe walking.")]
    [Min(0f)]
    public float TiptoeStepDistance = 0.7f;

    [Tooltip("Distance between each step when walking.")]
    [Min(0f)]
    public float WalkStepDistance = 1f;

    [Tooltip("Distance between each step when sprinting.")]
    [Min(0f)]
    public float SprintStepDistance = 1.3f;
}
