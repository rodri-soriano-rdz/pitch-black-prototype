using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundParticle : MonoBehaviour
{
    private SoundProperties m_soundProperties;

    private float m_timeAlive = 0f;

    public float LifeNormalized => m_timeAlive / m_soundProperties.Lifespan;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
    }

    public void SetSoundProperties(SoundProperties properties)
    {
        m_soundProperties = properties;
    }

    private void FixedUpdate()
    {
        m_timeAlive += Time.fixedDeltaTime;

        if (LifeNormalized >= 1f)
        {
            Destroy(gameObject);
            return;
        }

        var trailRenderer = GetComponent<TrailRenderer>();
        float opacity = 1f - LifeNormalized;

        Color endColor = m_soundProperties.ParticleColor;
        //endColor.r *= opacity;
        //endColor.g *= opacity;
        //endColor.b *= opacity;
        endColor.a *= opacity;

        trailRenderer.widthMultiplier = m_soundProperties.Thickness;

        trailRenderer.startColor = endColor;
        trailRenderer.endColor = endColor;
    }
}
