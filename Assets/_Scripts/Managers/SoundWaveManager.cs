using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundWaveManager : MonoBehaviour
{
    private static GameObject s_soundParticlePrefab;

    [SerializeField]
    private SoundWaveSettings m_settings;

    private void Awake()
    {
        if (m_settings == null)
        {
            m_settings = new SoundWaveSettings();
        }

        if (s_soundParticlePrefab == null)
        {
            s_soundParticlePrefab = Resources.Load<GameObject>("Prefabs/SoundParticlePrefab");
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

    public void CreateSoundWave(Vector3 position, Vector2 velocity, Loudness loudness)
    {
        // Get sound properties.
        SoundProperties soundPropts = GetSoundProperties(loudness);

        CreateSoundWave(position, velocity, soundPropts);
    }

    public void CreateSoundWave(Vector3 position, Vector2 velocity, SoundProperties properties)
    {
        // Get sound properties.
        var force = Vector2.one * m_settings.GlobalSoundSpeed;
        int particles = properties.ParticleCount;


        float step = 360f / particles;
        float rando = Random.value * properties.AngleOffsetRandomness;
        float offset = step * rando;

        for (float angle = 0f; angle < 360; angle += step)
        {
            float actualAngle = angle + offset;
            GameObject soundParticle = create_sound_particle(position, properties);
            var rigidBody = soundParticle.GetComponent<Rigidbody2D>();

            var sin = Mathf.Sin(actualAngle * Mathf.Deg2Rad);
            var cos = Mathf.Cos(actualAngle * Mathf.Deg2Rad);

            var currentForce = force;
            currentForce.x *= sin;
            currentForce.y *= cos;
            currentForce += velocity * 0.5f;

            rigidBody.AddForce(currentForce, ForceMode2D.Impulse);
        }
    }

    public SoundProperties GetSoundProperties(Loudness loudness)
    {
        switch (loudness)
        {
            case (Loudness.VeryQuiet):
                return m_settings.VeryQuiet;
            case (Loudness.Quiet):
                return m_settings.Quiet;
            case (Loudness.Normal):
                return m_settings.Normal;
            case (Loudness.Loud):
                return m_settings.Loud;
            case (Loudness.VeryLoud):
                return m_settings.VeryLoud;
            default:
                return m_settings.Normal;
        }
    }

    private GameObject create_sound_particle(Vector3 position, SoundProperties properties)
    {
        GameObject soundParticle = Instantiate(s_soundParticlePrefab, position, Quaternion.identity);

        // Get sound properties.
        var soundScript = soundParticle.GetComponent<SoundParticle>();
        soundScript.SetSoundProperties(properties);

        return soundParticle;
    }
}
