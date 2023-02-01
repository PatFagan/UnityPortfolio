using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float delay;
    public GameObject spawnedObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        Instantiate(spawnedObject);
        yield return new WaitForSeconds(delay);
        StartCoroutine(Spawning());
    }
}