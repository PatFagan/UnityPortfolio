using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    Rigidbody rigidbody;
    float speed = 130f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Wind")
        {
            Vector3 randomForce = new Vector3(Random.Range(-1, 1), Random.Range(1, 3), Random.Range(-1, 1));
            Vector3 leafForce = (collider.gameObject.GetComponent<Projectile>().direction + (.5f * randomForce)) * speed;
            rigidbody.AddForce(leafForce);
            //print("blow me " + leafForce);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Ground")
        {
            Vector3 randomForce = new Vector3(0f, 10f, 0f);
            //rigidbody.AddForce(randomForce);
            //print("UP " + randomForce);
        }
    }
}
