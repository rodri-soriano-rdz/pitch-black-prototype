using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoudnessSettings", menuName = "Scriptables/Sound Properties", order = 3)]
public class SoundProperties : ScriptableObject
{
    [Tooltip("Color of sound particles.")]
    public Color ParticleColor = Color.white;

    [Tooltip("Number of sound particles.")]
    [Range(0, 360)]
    public int ParticleCount = 20;

    [Tooltip("Lifespan of sound particles.")]
    [Min(0f)]
    public float Lifespan = 1f;

    [Tooltip("Thickness of sound particles' trails.")]
    [Range(0f, 1f)]
    public float Thickness = 0.2f;

    [Tooltip("Angle offset randomness of every wave generated.")]
    [Range(0f, 1f)]
    public float AngleOffsetRandomness = 1f;
}
