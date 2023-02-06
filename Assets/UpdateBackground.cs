using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.backgroundColor = new Color(0f, 0f, 0f, 0f);
    }
}
