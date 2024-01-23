using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    public static GameManager Instance
    {
        get {
            // Singleton.
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();

                if (m_instance == null)
                {
                    var prefab = Resources.Load<GameObject>("Prefabs/Managers/GameManager");
                    var gameManager = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    m_instance = gameManager.GetComponent<GameManager>();
                }
            }

            return m_instance;
        }
    }

    public InputManager InputManager { get; private set; }

    public AudioManager AudioManager { get; private set; }

    public SoundWaveManager SoundWaveManager { get; private set; }

    public PlayerManager PlayerManager { get; private set; }

    public GameObject Player => m_player;

    [SerializeField]
    [Tooltip("Reference to the player of the scene.")]
    private GameObject m_player;

    private void Awake()
    {
        // Destroy duplicate instances.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            m_instance = this;

            return;
        }

        // Initialize Input manager.
        InputManager = GetComponent<InputManager>();

        if (InputManager == null)
        {
            gameObject.AddComponent<InputManager>();
            InputManager = gameObject.AddComponent<InputManager>();
        }

        AudioManager = GetComponent<AudioManager>();

        // Initialize Sound Wave Manager.
        SoundWaveManager = GetComponent<SoundWaveManager>();

        if (SoundWaveManager == null)
        {
            gameObject.gameObject.AddComponent<SoundWaveManager>();
        }

        // Initialize Player Manager.
        PlayerManager = GetComponent<PlayerManager>();

        if (PlayerManager == null)
        {
            gameObject.AddComponent<PlayerManager>();
            PlayerManager = GetComponent<PlayerManager>();
        }

        // Make manager persistent over scenes.
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update.
    void Start()
    {
        // Create player.
        if (Player == null)
        {
            var playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerPrefab");
            m_player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
