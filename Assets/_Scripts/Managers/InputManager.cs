using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMapping m_inputMapping;

    public InputMapping InputMapping => m_inputMapping;

    void Awake()
    {
        // Initialize Input Mapping.
        m_inputMapping = new InputMapping();
        m_inputMapping.Player.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
