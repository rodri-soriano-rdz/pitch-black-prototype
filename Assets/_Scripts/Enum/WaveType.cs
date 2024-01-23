using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    [Tooltip("Particle trail duration is infinite.")]
    Focal = 0,
    [Tooltip("Particle trail duration is defined by Max Trail Duration.")]
    Expansive = 1
}
