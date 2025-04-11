using System.Collections;
using System.Collections.Generic;
using Kinnly;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DELETE : MonoBehaviour
{

    [SerializeField] List<Transform> targets = new List<Transform>();
    public int wayPointIndex=0;

    public bool isWalking = false;

    NavMeshAgent agent;


    [SerializeField] Transform player;
    [SerializeField] float visionRange = 10f;
    [SerializeField] float visionAngle = 45f;
    public LayerMask playerLayer;
    public float detectionRadius = 5f;


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

        isWalking = true;
        agent.SetDestination(targets[wayPointIndex].position);

        Vector3 currentPosition = transform.position;
        var distance = Vector3.Distance(currentPosition, targets[wayPointIndex].position);

        //Debug.Log("NPC POS: " + currentPosition + "Target pos:" + targets[wayPointIndex].position + "Distance:" + distance);

        if (PlayerInSight())
        {
            Debug.Log("chase");
            agent.SetDestination(player.position);
        }
        if (distance <= 1.0f)
        {
            wayPointIndex = selectWaypointIndex();
            yield return new WaitForSeconds(3);
        }

        isWalking = false;

        yield return null;


    }

    bool PlayerInSight()
    {
        Debug.Log("LOOKING FOR PLAYER");
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, player.position);

        // Check if the player is within a certain distance
        if (distance <= 5.0f)
        {
            // Perform a raycast from NPC to Player to check for obstacles with the "Wall" tag
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, (player.position - currentPosition).normalized, distance);

            // If the ray hits something within the distance
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                // Check if the object hit by the ray has the "Wall" tag
           
                // An object with the "Wall" tag is blocking the view
                Debug.Log("Player is blocked by a wall");
                return false;  // Return false if the player is blocked by a wall
                
            }

            // If no wall is detected, return true
            return true;
        }
        else
        {
            // If the player is too far, return false
            return false;
        }
    }


    //// Create a sphere to detect objects in the player layer
    //Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

    //// If there are any objects detected in the radius, we assume the player is detected
    //foreach (var hitCollider in hitColliders)
    //{
    //        //player = hitCollider.transform; // Set the player reference
    //        return true; // Player is in sight
    //}



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
            return NPCStateManager.Instance.act1;
        if (tag == "act2 waypoint" )
            return NPCStateManager.Instance.act2;
        if (tag == "act3 waypoint") 
            return NPCStateManager.Instance.act3;

        return false;
    }

}
