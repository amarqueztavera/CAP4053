using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DELETE : MonoBehaviour
{
    public bool act1 = true;
    public bool act2 = false;
    public bool act3 = false;


    [SerializeField] List<Transform> targets = new List<Transform>();
    public int wayPointIndex=0;

    public bool isWalking = false;

    //[SerializeField] Transform target;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation =  false;
        agent.updateUpAxis = false;

        wayPointIndex = selectWaypointIndex();
    }

    void Update()
    {

        if (!isWalking) 
        {
            StartCoroutine(Walk());
        }
    }

    IEnumerator Walk()
    {
        isWalking= true;
        agent.SetDestination(targets[wayPointIndex].position);

        Vector3 currentPosition = transform.position;
        var distance = Vector3.Distance(currentPosition, targets[wayPointIndex].position);

        //Debug.Log("NPC POS: " + currentPosition + "Target pos:" + targets[wayPointIndex].position + "Distance:" + distance);

        if (distance <= 1.0f)
        {
            wayPointIndex = selectWaypointIndex();
            yield return new WaitForSeconds(3);
        }

        isWalking = false;

        yield return null;


    }

    int selectWaypointIndex()
    {
        wayPointIndex = Random.Range(0, targets.Count);
        while (!isValidWaypoint(wayPointIndex))
        {
            wayPointIndex = Random.Range(0, targets.Count);
        }
        return wayPointIndex;
    }

    bool isValidWaypoint( int wayPointIndex)
    {
        string tag = targets[wayPointIndex].tag;

        Debug.Log("checking tag: "+ tag);

        if (tag == "act1 waypoint")
            return act1;
        if (tag == "act2 waypoint" )
            return act2;
        if (tag == "act3 waypoint") 
            return act3;

        return false;
    }

}
