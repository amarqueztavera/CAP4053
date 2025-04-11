using System.Collections;
using System.Collections.Generic;
using Kinnly;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class DELETE : MonoBehaviour
{

    [SerializeField] List<Transform> targets = new List<Transform>();
    public int wayPointIndex=0;

    public bool isWalking = false;

    NavMeshAgent agent;


    public Tilemap wallTilemap;

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

        var distaceToPlayer = Vector3.Distance(currentPosition, player.position);

        //Debug.Log("NPC POS: " + currentPosition + "Target pos:" + targets[wayPointIndex].position + "Distance:" + distance);

        if (HasLineOfSight( transform.position, player.position,  wallTilemap) && distaceToPlayer<=5)
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

    public bool HasLineOfSight(Vector3 npcWorldPos, Vector3 playerWorldPos, Tilemap wallTilemap)
    {
        Vector3Int npcCell = wallTilemap.WorldToCell(npcWorldPos);
        Vector3Int playerCell = wallTilemap.WorldToCell(playerWorldPos);

        foreach (Vector3Int pos in GetLine(npcCell, playerCell))
        {
            if (wallTilemap.HasTile(pos)) // You can fine-tune what counts as a wall
            {
                return false; // Wall in the way
            }
        }
        return true; // Clear line of sight
    }

    public IEnumerable<Vector3Int> GetLine(Vector3Int from, Vector3Int to)
    {
        int x0 = from.x, y0 = from.y;
        int x1 = to.x, y1 = to.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            yield return new Vector3Int(x0, y0, from.z);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
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
