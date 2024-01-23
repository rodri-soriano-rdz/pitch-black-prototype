using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundWaveSettings", menuName = "Scriptables/Sound Wave Settings", order = 2)]
public class SoundWaveSettings : ScriptableObject
{
    [Header("Sound Lifespan")]
    [Tooltip("Properties of a very quiet sound wave.")]
    public SoundProperties VeryQuiet;

    [Tooltip("Properties of a quiet sound wave.")]
    public SoundProperties Quiet;

    [Tooltip("Properties of a normal sound wave.")]
    public SoundProperties Normal;

    [Tooltip("Properties of a loud sound wave.")]
    public SoundProperties Loud;

    [Tooltip("Properties of a very loud sound wave.")]
    public SoundProperties VeryLoud;

    [Tooltip("Global speed of sound.")]
    public float GlobalSoundSpeed = 2f;
}
