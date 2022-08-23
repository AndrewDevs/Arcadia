using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class mob : NetworkBehaviour
{
    public NavMeshAgent agent;
    public GameObject chunk;
    public Collider collider;
    public GameObject mobObject;

    int mobSeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move(GameObject chunk)
    {
        collider = chunk.GetComponent<Collider>();
        //agent.destination 
        mobSeed = Mathf.RoundToInt(transform.position.x);
        Random.InitState(mobSeed);
        mobObject.transform.position = new Vector3(UnityEngine.Random.Range(collider.bounds.max.x, collider.bounds.min.x), transform.position.y, UnityEngine.Random.Range(collider.bounds.max.z, collider.bounds.min.z));

    }
}
