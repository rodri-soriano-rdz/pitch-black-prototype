using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("Settings for this player instance.")]
    [SerializeField]
    private PlayerSettings m_settings;

    /// <summary>
    /// Stamp cooldown, in seconds.
    /// </summary>
    public float StampCooldown => m_settings.StampCooldownTime;

    public float WalkToIdleTime => m_settings.WalkToIdleTime;

    public float StampMaxChargeTime => m_settings.StampMaxChargeTime;

    private void Awake()
    {
        if (m_settings == null)
        {
            m_settings = new PlayerSettings();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Get movement force depending on a given Walk Mode.
    /// </summary>
    public float GetMovementForce(WalkMode mode)
    {
        switch (mode)
        {
            case WalkMode.Tiptoe:
                return m_settings.TiptoeForce;
            case WalkMode.Walking:
                return m_settings.WalkForce;
            case WalkMode.Sprinting:
                return m_settings.SprintForce;
            default: 
                return m_settings.WalkForce;
        }
    }

    /// <summary>
    /// Get step loudness depending on a given Walk Mode.
    /// </summary>
    public Loudness GetStepLoudness(WalkMode mode)
    {
        switch (mode)
        {
            case WalkMode.Tiptoe:
                return m_settings.TiptoeLoudness;
            case WalkMode.Walking:
                return m_settings.WalkLoudness;
            case WalkMode.Sprinting:
                return m_settings.SprintLoudness;
            default:
                return m_settings.WalkLoudness;
        }
    }

    public float GetStepDistance(WalkMode mode)
    {
        switch (mode)
        {
            case WalkMode.Tiptoe:
                return m_settings.TiptoeStepDistance;
            case WalkMode.Walking:
                return m_settings.WalkStepDistance;
            case WalkMode.Sprinting:
                return m_settings.SprintStepDistance;
            default:
                return m_settings.WalkStepDistance;
        }
    }

    public WalkMode GetWalkModeFromForce(float force)
    {
        float sprintThreshold = m_settings.SprintForce * m_settings.SprintForceThreshold;
        float walkThreshold = m_settings.WalkForce * m_settings.WalkForceThreshold;

        if (force >= sprintThreshold)
        {
            return WalkMode.Sprinting;
        }
        else if (force > walkThreshold)
        {
            return WalkMode.Walking;
        }
        else
        {
            return WalkMode.Tiptoe;
        }
    }

    public SoundProperties GetStampProperties(float charge)
    {
        if (charge >= 1f)
        {
            return m_settings.StampLoudSoundProperties;
        }
        else if (charge >= m_settings.NormalStampThreshold)
        {
            return m_settings.StampNormalSoundProperties;
        }
        else
        {
            return m_settings.StampLightSoundProperties;
        }
    }
}
