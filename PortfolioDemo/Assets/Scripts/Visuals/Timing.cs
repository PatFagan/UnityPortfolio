using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timing : MonoBehaviour
{
    public float timingOffset;

    // Start is called before the first frame update
    void Start()
    {
        timingOffset = Random.Range(0.1f, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
