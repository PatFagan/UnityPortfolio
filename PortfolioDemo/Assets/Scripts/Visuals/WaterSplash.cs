using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sources
// UnityCity: https://www.youtube.com/watch?v=Kwh4TkQqqf8&t=33s 

public class WaterSplash : MonoBehaviour
{
    public float stiffness = 1f; // magnitude of vector between old and new vector positions
    public float damping = 0.75f; // if < 1, velocity decreases at that rate
    public float intensity = 1f; // speed of lerp between jiggles

    public float xOffset, yOffset, zOffset;

    private Mesh OriginalMesh, MeshClone;
    private WaterVertex[] gelatinVertices;
    private Vector3[] vertexArray;

    public float timeBetweenVertices;

    //TranslateMovement movementScript;

    void OnTriggerEnter(Collider collidedObject)
    {
        if (collidedObject.gameObject.tag == "Player")
        {
            // find vertex closest to player pos
            // create a splash at that position

            vertexArray = OriginalMesh.vertices; // set the vertex array to the original mesh, to be edited
            for (int i = 0; i < gelatinVertices.Length; i++) // loop through all vertices in the mesh
            {
                Vector3 target = collidedObject.gameObject.transform.position;//transform.TransformPoint(vertexArray[i]);
                gelatinVertices[i].Jiggle(target, stiffness, damping); // jiggle the current vertex
                target = transform.InverseTransformPoint(gelatinVertices[i].position); // get pos of new vertex
                // set new vertex positions to the array
                vertexArray[gelatinVertices[i].ID] = Vector3.Lerp(vertexArray[gelatinVertices[i].ID], target, intensity);
                
            }
            MeshClone.vertices = vertexArray;
        }
    }

    void Start()
    {
        //movementScript = GetComponent<TranslateMovement>(); // get movement script

        // get two meshes (one to edit, then one to set to the edited version)
        OriginalMesh = GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        GetComponent<MeshFilter>().sharedMesh = MeshClone;

        // get list of mesh vertices
        gelatinVertices = new WaterVertex[MeshClone.vertices.Length];

        // create each vertex with an id and position
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            gelatinVertices[i] = new WaterVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }

        Vector3 waterMov = new Vector3(xOffset, yOffset, zOffset);
        StartCoroutine(MoveWater(waterMov));
    }

    IEnumerator MoveWater(Vector3 waterMov)
    {   
        vertexArray = OriginalMesh.vertices; // set the vertex array to the original mesh, to be edited
        for (int i = 0; i < gelatinVertices.Length; i++) // loop through all vertices in the mesh
        {
            Vector3 target = transform.TransformPoint(vertexArray[i]);
            target += waterMov; // get current vertex pos in world space
            gelatinVertices[i].Jiggle(target, stiffness, damping); // jiggle the current vertex
            target = transform.InverseTransformPoint(gelatinVertices[i].position); // get pos of new vertex
            // set new vertex positions to the array
            vertexArray[gelatinVertices[i].ID] = Vector3.Lerp(vertexArray[gelatinVertices[i].ID], target, intensity);
            
            MeshClone.vertices = vertexArray;
            float randTime = Random.Range(0, .01f);

            yield return new WaitForSeconds(timeBetweenVertices + randTime);
        }
        for (int i = 0; i < gelatinVertices.Length; i++) // loop through all vertices in the mesh
        {
            Vector3 target = transform.TransformPoint(vertexArray[i]);
            target -= waterMov; // get current vertex pos in world space
            gelatinVertices[i].Jiggle(target, stiffness, damping); // jiggle the current vertex
            target = transform.InverseTransformPoint(gelatinVertices[i].position); // get pos of new vertex
            // set new vertex positions to the array
            vertexArray[gelatinVertices[i].ID] = Vector3.Lerp(vertexArray[gelatinVertices[i].ID], target, intensity);
            
            MeshClone.vertices = vertexArray;

            float randTime = Random.Range(0, .01f);
            yield return new WaitForSeconds(timeBetweenVertices + randTime);
        }
        //MeshClone.vertices = vertexArray; // set the clone mesh equal to the edited array of vertices

        //yield return new WaitForSeconds(.5f);
        
        StartCoroutine(MoveWater(waterMov));
    }

    public class WaterVertex
    {
        public int ID;
        public Vector3 position;
        Vector3 velocity;

        // gelatin vertex constructor
        public WaterVertex(int customId, Vector3 customPos)
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