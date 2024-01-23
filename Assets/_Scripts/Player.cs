using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public AudioClip[] TiptoeSounds;

    public AudioClip[] WalkSounds;

    public AudioClip[] SprintSounds;

    public AudioClip[] StampSounds;

    public AudioClip[] DriftSounds;

    private Rigidbody2D m_rigidBody2D;

    private AudioSource m_audioSource;

    private WalkMode m_targetWalkMode = WalkMode.Walking;

    private WalkMode m_currentWalkMode = WalkMode.Walking;

    private float m_currentStampCooldown = 0f;

    private float m_distanceSinceLastStep = 0f;

    private float m_timeSinceLastDrift = 0f;

    private float m_currentWalkToIdleTime = 0f;

    private Vector3 m_previousPos = Vector3.zero;

    private bool m_chargingStamp = false;

    private float m_stampChargeTime = 0f;

    private PlayerState m_playerState = PlayerState.Idle;

    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        // Initialize Input Mapping.
        var inputMapping = GameManager.Instance.InputManager.InputMapping;

        // Input event subscriptions.
        inputMapping.Player.Sprint.performed += sprint_started;
        inputMapping.Player.Sprint.canceled += sprint_canceled;

        inputMapping.Player.Tiptoe.performed += tiptoe_started;
        inputMapping.Player.Tiptoe.canceled += tiptoe_canceled;

        inputMapping.Player.Stamp.performed += stamp_performed;
        inputMapping.Player.Stamp.canceled += stamp_canceled;

        // Get components.
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();

        // Send current position to next update.
        m_previousPos = transform.position;
    }

    private void FixedUpdate()
    {
        // Do not update if player does not have a rigid body.
        if (m_rigidBody2D == null)
        {
            return;
        }

        // Short names.
        var playerManager = GameManager.Instance.PlayerManager;
        var inputMapping = GameManager.Instance.InputManager.InputMapping;

        // Drift cooldown.
        if (m_timeSinceLastDrift > 0f)
        {
            m_timeSinceLastDrift -= Time.fixedDeltaTime;
        }

        // Stamp cooldown.
        if (m_currentStampCooldown > 0f)
        {
            m_currentStampCooldown -= Time.fixedDeltaTime;
        }

        // Stamp charging.
        if (m_chargingStamp && m_stampChargeTime < playerManager.StampMaxChargeTime)
        {
            m_stampChargeTime += Time.fixedDeltaTime;
        }
        
        // Perform stamping.
        if (!m_chargingStamp
            && m_stampChargeTime > 0f
            && m_currentStampCooldown <= 0f)
        {
            PerformStamp(m_stampChargeTime);
        }

        // Get velocity to force conversion factor.
        Vector2 velocity = m_rigidBody2D.velocity;
        float linDrag = m_rigidBody2D.drag;
        float mass = m_rigidBody2D.mass;
        float velToForce = linDrag * mass;

        // Calculate current force.
        Vector2 currForce = velocity * velToForce;
        float currForceMag = currForce.magnitude;

        // Get actual walk mode depending on player's velocity.
        m_currentWalkMode = playerManager.GetWalkModeFromForce(currForceMag);

        // Get walk force from targeted walk mode.
        float force = playerManager.GetMovementForce(m_targetWalkMode);

        // Get movement force.
        var movementDirection = inputMapping.Player.Move.ReadValue<Vector2>();
        var movementForce = movementDirection * force;
        var driftForceThreshold = playerManager.GetMovementForce(WalkMode.Walking);

        bool inputMoving = movementDirection != Vector2.zero;

        if (inputMoving)
        {
            // Drifting.
            if (m_timeSinceLastDrift <= 0f
                && currForce.magnitude > driftForceThreshold)
            {
                float forceDiff = Vector2.Distance(movementForce, currForce);

                if (forceDiff > driftForceThreshold * 1.25f)
                {
                    PerformDrift();
                }
            }
        }

        // Step calculations.
        float stepDistance = playerManager.GetStepDistance(m_currentWalkMode);
        var stepLoudness = playerManager.GetStepLoudness(m_currentWalkMode);

        // Calculate walked distance since last frame.
        Vector3 posDiff = m_previousPos - transform.position;
        m_distanceSinceLastStep += posDiff.magnitude;

        // Create step sound waves.
        if (m_distanceSinceLastStep > stepDistance)
        {
            PerformStep(stepLoudness);
        }

        // Apply movement forces.
        if (!m_chargingStamp)
        {
            m_rigidBody2D.AddForce(movementForce, ForceMode2D.Force);
        }

        // Send current position to next update.
        m_previousPos = transform.position;
    }

    private void sprint_started(InputAction.CallbackContext context)
    {
        m_targetWalkMode = WalkMode.Sprinting;
    }

    private void sprint_canceled(InputAction.CallbackContext context)
    {
        if (m_targetWalkMode == WalkMode.Sprinting)
        {
            m_targetWalkMode = WalkMode.Walking;
        }
    }

    private void tiptoe_started(InputAction.CallbackContext context)
    {
        m_targetWalkMode = WalkMode.Tiptoe;
    }

    private void tiptoe_canceled(InputAction.CallbackContext context)
    {
        if (m_targetWalkMode == WalkMode.Tiptoe)
        {
            m_targetWalkMode = WalkMode.Walking;
        }
    }

    private void stamp_performed(InputAction.CallbackContext context)
    {
        m_chargingStamp = true;
    }

    private void stamp_canceled(InputAction.CallbackContext context)
    {
        m_chargingStamp = false;
    }

    private void PerformStamp(float charge)
    {
        var stampSoundProperties = GameManager.Instance.PlayerManager.GetStampProperties(charge);
        var stampCooldown = GameManager.Instance.PlayerManager.StampCooldown;

        GameManager.Instance.SoundWaveManager.CreateSoundWave(
            transform.position,
            m_rigidBody2D.velocity,
            stampSoundProperties
        );

        m_distanceSinceLastStep = 0f;
        m_currentStampCooldown = stampCooldown;
        m_chargingStamp = false;
        m_stampChargeTime = 0f;
        PlayStampSound();
    }

    private void PerformStep(Loudness loudness)
    {
        GameManager.Instance.SoundWaveManager.CreateSoundWave(
                transform.position,
                m_rigidBody2D.velocity,
                loudness
        );

        m_distanceSinceLastStep = 0f;
        PlayStepSound(m_currentWalkMode);
    }

    private void PerformDrift()
    {
        GameManager.Instance.SoundWaveManager.CreateSoundWave(
                transform.position,
                m_rigidBody2D.velocity,
                Loudness.Normal
        );

        m_timeSinceLastDrift = 1f;
        m_distanceSinceLastStep = 0f;
        PlayDriftSound();
    }

    private void PlayStepSound(WalkMode mode)
    {
        switch (mode)
        {
            case WalkMode.Tiptoe:
                PlayTiptoeSound();
                break;
            case WalkMode.Walking:
                PlayWalkSound();
                break;
            case WalkMode.Sprinting:
                PlaySprintSound();
                break;
            default:
                break;
        }
    }

    private void PlayStampSound()
    {
        m_audioSource.volume = 0.7f;
        PlayRandomSound(StampSounds);
    }

    private void PlayTiptoeSound()
    {
        m_audioSource.volume = 0.3f;
        PlayRandomSound(TiptoeSounds);
    }

    private void PlayWalkSound()
    {
        m_audioSource.volume = 0.4f;
        PlayRandomSound(WalkSounds);
    }

    private void PlaySprintSound()
    {
        m_audioSource.volume = 0.55f;
        PlayRandomSound(SprintSounds);
    }

    private void PlayDriftSound()
    {
        m_audioSource.volume = 0.7f;
        PlayRandomSound(DriftSounds);
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        int index = UnityEngine.Random.Range(0, sounds.Length);
        m_audioSource.clip = sounds[index];
        m_audioSource.Play();
    }
}
