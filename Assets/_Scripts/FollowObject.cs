using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject Follow;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Follow != null)
        {
            Vector3 pos = transform.position;
            pos.x = Follow.transform.position.x;
            pos.y = Follow.transform.position.y;

            transform.position = pos;
        }
    }
}
