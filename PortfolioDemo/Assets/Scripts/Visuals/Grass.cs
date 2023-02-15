using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sources
// UnityCity: https://www.youtube.com/watch?v=Kwh4TkQqqf8&t=33s 

public class Grass : MonoBehaviour
{
    public float stiffness = 1f; // magnitude of vector between old and new vector positions
    public float damping = 0.75f; // if < 1, velocity decreases at that rate
    public float intensity = 1f; // speed of lerp between jiggles

    public float xOffset, yOffset, zOffset;

    private Mesh OriginalMesh, MeshClone;
    private GrassVertex[] gelatinVertices;
    private Vector3[] vertexArray;

    public float timeBetweenVertices;

    private float scalar = 1f;

    //TranslateMovement movementScript;

    void Start()
    {
        //movementScript = GetComponent<TranslateMovement>(); // get movement script

        // get two meshes (one to edit, then one to set to the edited version)
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;

        // get list of mesh vertices
        gelatinVertices = new GrassVertex[MeshClone.vertices.Length];

        // create each vertex with an id and position
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            gelatinVertices[i] = new GrassVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }

        Vector3 grassMov = new Vector3(xOffset, yOffset, zOffset);
        StartCoroutine(MoveGrass(grassMov));
    }

    void OnTriggerEnter(Collider collided)
    {
        if (collided.gameObject.tag == "Player")
        {
            scalar *= 3f;
        }
    }
    void OnTriggerExit(Collider collided)
    {
        if (collided.gameObject.tag == "Player")
        {
            scalar /= 3f;
        }
    }

    IEnumerator MoveGrass(Vector3 grassMov)
    {   
        float randMove = Random.Range(1, 1.05f);
        vertexArray = OriginalMesh.vertices; // set the vertex array to the original mesh, to be edited
        for (int i = 4; i < gelatinVertices.Length; i++) // loop through all vertices in the mesh
        {
            Vector3 target = transform.TransformPoint(vertexArray[i]);
            target += grassMov * (randMove) * scalar; // get current vertex pos in world space
            gelatinVertices[i].Jiggle(target, stiffness, damping); // jiggle the current vertex
            target = transform.InverseTransformPoint(gelatinVertices[i].position); // get pos of new vertex
            // set new vertex positions to the array
            vertexArray[gelatinVertices[i].ID] = Vector3.Lerp(vertexArray[gelatinVertices[i].ID], target, intensity);
            
            MeshClone.vertices = vertexArray; // set the clone mesh equal to the edited array of vertices

            float randTime = Random.Range(0, .05f);
            yield return new WaitForSeconds(timeBetweenVertices + randTime);
            
        }
        for (int i = 4; i < gelatinVertices.Length; i++) // loop through all vertices in the mesh
        {
            Vector3 target = transform.TransformPoint(vertexArray[i]);
            target -= grassMov * (randMove) * scalar; // get current vertex pos in world space
            gelatinVertices[i].Jiggle(target, stiffness, damping); // jiggle the current vertex
            target = transform.InverseTransformPoint(gelatinVertices[i].position); // get pos of new vertex
            // set new vertex positions to the array
            vertexArray[gelatinVertices[i].ID] = Vector3.Lerp(vertexArray[gelatinVertices[i].ID], target, intensity);
            
            MeshClone.vertices = vertexArray; // set the clone mesh equal to the edited array of vertices

            float randTime = Random.Range(0, .05f);
            yield return new WaitForSeconds(timeBetweenVertices + randTime);

        }

        // stop moving while player stands on grass
        yield return new WaitUntil(() => scalar == 1);
        
        StartCoroutine(MoveGrass(grassMov));
    }

    public class GrassVertex
    {
        public int ID;
        public Vector3 position;
        Vector3 velocity;

        // gelatin vertex constructor
        public GrassVertex(int customId, Vector3 customPos)
        {
            ID = customId;
            position = customPos;
        }

        // jiggle each point
        public void Jiggle(Vector3 target, float stiffness, float dampening)
        {
            Vector3 force = (target - position) * stiffness;
            velocity = (velocity + force) * dampening;
            position += velocity;
        }

    }
}