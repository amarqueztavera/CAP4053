using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DELETE : MonoBehaviour
{
    [SerializeField] List<Transform> targets = new List<Transform>();
    public int wayPointIndex=0;

    //[SerializeField] Transform target;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation =  false;
        agent.updateUpAxis = false;
        
    }

    void Update()
    {

        agent.SetDestination(targets[wayPointIndex].position);

        Vector3 currentPosition = transform.position;
        var distance = Vector3.Distance(currentPosition, targets[wayPointIndex].position);

        Debug.Log("NPC POS: " + currentPosition + "Target pos:" + targets[wayPointIndex].position +"Distance:"+ distance);

        if (distance <= 1.0f)
            wayPointIndex++;
    }
}
