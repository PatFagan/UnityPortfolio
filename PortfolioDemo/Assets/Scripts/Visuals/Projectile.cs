using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;
    public Vector3 direction;
    public float speed;
    bool lockYAxis = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        float scaleScalar = .5f;
        //transform.localScale += new Vector3(scaleScalar, scaleScalar, scaleScalar);

        if (!lockYAxis)
        {
            transform.Translate(direction * speed);
        }

        if (lockYAxis)
        {
            transform.Translate(new Vector3(direction.x, 0f, direction.z) * speed);
        }
    }
}
