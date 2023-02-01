using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    public float distanceChange;
    float currentMovement;
    
    void Start()
    {
        currentMovement = distanceChange;
        StartCoroutine(MoveWaterLevel());
    }

    IEnumerator MoveWaterLevel()
    {
        currentMovement = distanceChange;
        yield return new WaitForSeconds(2f);
        currentMovement = -distanceChange;
        yield return new WaitForSeconds(2f);
        StartCoroutine(MoveWaterLevel());
    }

    void Update()
    {
        transform.position += new Vector3(0f, currentMovement, 0f);
    }
}