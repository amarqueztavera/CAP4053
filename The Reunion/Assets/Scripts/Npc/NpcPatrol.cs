using System.Collections;
using System.Collections.Generic;
using Kinnly;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class DELETE : MonoBehaviour
{

    //list of all the npc waypoints
    [SerializeField] List<Transform> targets = new List<Transform>();
    public int wayPointIndex = 0;
    public bool isWalking = false;

    // references to the navmesh agent, the target wher the npc should walk to, and the player
    NavMeshAgent agent;
    Vector3 target;
    bool targetIsWaypoint = true;
    [SerializeField] Transform player;

    //walls, bool to keep track if player is near navmesh, and a buffer of how far the player can be off the navmesh
    public Tilemap wallTilemap;
    bool playerOnNavMesh = true;
    public float radiusCheck = 1.0f;

    //how far the npc can see
    public float detectionRadius = 10f;

    //reference to the pop up that will show when the plaer gets caught
    public TextMeshProUGUI caughtMessage, notAgainMessage;


    [Header("AI Settings")]
    public float chaseSpeed = 3.5f;
    public float patrolSpeed = 2f;
    public float investigationTime = 5f;
    public float waypointWaitTime = 3f;
    public float waypointArrivalDistance = 1f;
    public float catchDistance = 1f;
    private float timeSinceLastSighting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //select and set the first waypoint to go to
        wayPointIndex = selectWaypointIndex();
        target = targets[wayPointIndex].position;
    }

    void Update()
    {
        //makes sure that the player is always on the navmesh
        IsOnNavMesh();

        //makes npc walk to target
        if (!isWalking)
        {
            StartCoroutine(Walk());
        }
    }

    public void HandleStateUpdate(bool isAlerted, Vector3 lastKnownPosition)
    {
        if (isAlerted)
        {
            target = lastKnownPosition;
            targetIsWaypoint = false;
            agent.speed = chaseSpeed;
            timeSinceLastSighting = 0f;

            if (!isWalking)
            {
                StartCoroutine(Walk());
            }
        }
        else
        {
            agent.speed = patrolSpeed;
        }
    }

    private bool CanChasePlayer()
    {
        // Triple verification:
        return NPCStateManager.Instance.maxSuspicion &&
               !SuspicionManager.Instance.IsInReunionArea &&
               !ReunionArea.PlayerIsInside;
    }


    //IEnumerator Walk()
    //{
    //    isWalking = true;
    //    agent.SetDestination(target);

    //    while (true)
    //    {
    //        float distanceToTarget = Vector3.Distance(transform.position, target);
    //        float distanceToPlayer = Vector3.Distance(transform.position, NPCStateManager.Instance.PlayerTransform.position);

    //        // Skip chasing if player is in reunion area

    //        if (CanChasePlayer() &&
    //        HasLineOfSight(transform.position, player.position, wallTilemap))
    //        {
    //            // Only chase if all conditions are met
    //            target = player.position;
    //            timeSinceLastSighting = 0f;
    //            agent.SetDestination(target);
    //        }
    //        //if (NPCStateManager.Instance.maxSuspicion &&
    //        //    !SuspicionManager.Instance.IsInReunionArea)
    //        //{
    //        //    if (HasLineOfSight(transform.position, player.position, wallTilemap))
    //        //    {
    //        //        //target = player.position;
    //        //        //timeSinceLastSighting = 0f;

    //        //        // Only chase if player is outside reunion area
    //        //        if (!IsPositionInReunionArea(player.position))
    //        //        {
    //        //            target = player.position;

    //        //            agent.SetDestination(target);
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        timeSinceLastSighting += Time.deltaTime;
    //        //    }
    //        //}

    //        // Chase behavior
    //        if (NPCStateManager.Instance.maxSuspicion)
    //        {
    //            if (HasLineOfSight(transform.position, NPCStateManager.Instance.PlayerTransform.position, wallTilemap))
    //            {
    //                target = NPCStateManager.Instance.PlayerTransform.position;
    //                timeSinceLastSighting = 0f;
    //                agent.SetDestination(target);
    //            }
    //            else
    //            {
    //                timeSinceLastSighting += Time.deltaTime;

    //                // Return to patrol after investigation time
    //                if (timeSinceLastSighting > investigationTime)
    //                {
    //                    targetIsWaypoint = true;
    //                    wayPointIndex = selectWaypointIndex();
    //                    target = targets[wayPointIndex].position;
    //                }
    //            }

    //            // Catch player
    //            if (distanceToPlayer <= 1f)
    //            {
    //                CatchPlayer();
    //                yield break;
    //            }
    //        }

    //        // Reached target
    //        if (distanceToTarget <= 1f)
    //        {
    //            if (targetIsWaypoint)
    //            {
    //                yield return new WaitForSeconds(3);
    //                wayPointIndex = selectWaypointIndex();
    //                target = targets[wayPointIndex].position;
    //            }
    //            else
    //            {
    //                targetIsWaypoint = true;
    //                wayPointIndex = selectWaypointIndex();
    //                target = targets[wayPointIndex].position;
    //            }
    //        }

    //        agent.SetDestination(target);
    //        yield return null;
    //    }
    //}

    IEnumerator Walk()
    {
        isWalking = true;
        agent.SetDestination(target);

        while (true)
        {
            Vector3 currentPosition = transform.position;
            float distanceToTarget = Vector3.Distance(currentPosition, target);
            float distanceToPlayer = Vector3.Distance(currentPosition, NPCStateManager.Instance.PlayerTransform.position);

            //---- CHASE LOGIC ----//
            if (ShouldChasePlayer())
            {
                bool canSeePlayer = HasLineOfSight(currentPosition, NPCStateManager.Instance.PlayerTransform.position, wallTilemap);

                if (canSeePlayer)
                {
                    // ACTIVE CHASE
                    target = NPCStateManager.Instance.PlayerTransform.position;
                    timeSinceLastSighting = 0f;
                    agent.speed = chaseSpeed;

                    // CATCH PLAYER
                    if (distanceToPlayer <= catchDistance)
                    {
                        CatchPlayer();
                        yield break;
                    }
                }
                else
                {
                    // INVESTIGATION MODE
                    timeSinceLastSighting += Time.deltaTime;

                    if (timeSinceLastSighting > investigationTime)
                    {
                        ReturnToPatrol();
                    }
                }
            }
            //---- END CHASE LOGIC ----//

            //---- PATROL LOGIC ----//
            if (distanceToTarget <= waypointArrivalDistance)
            {
                if (targetIsWaypoint)
                {
                    // WAIT AT WAYPOINT
                    yield return new WaitForSeconds(waypointWaitTime);
                    ReturnToPatrol();
                }
                else
                {
                    // RETURN TO PATROL AFTER INVESTIGATION
                    ReturnToPatrol();
                }
            }

            agent.SetDestination(target);
            yield return null;
        }
    }

    private bool ShouldChasePlayer()
    {
        return NPCStateManager.Instance.maxSuspicion &&
               !SuspicionManager.Instance.IsInReunionArea &&
               SuspicionManager.Instance.CanSuspicionIncrease;
    }

    private void ReturnToPatrol()
    {
        targetIsWaypoint = true;
        wayPointIndex = selectWaypointIndex();
        target = targets[wayPointIndex].position;
        agent.speed = patrolSpeed;
    }

    //private bool IsPositionInReunionArea(Vector3 position)
    //{
    //    // Implement your reunion area check here
    //    // Example: Check if position is within reunion area bounds
    //    Collider2D[] colliders = Physics2D.OverlapPointAll(position);
    //    foreach (Collider2D collider in colliders)
    //    {
    //        if (collider.CompareTag("ReunionArea"))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    void CatchPlayer()
    {
        Debug.Log("Player caught!");
        NPCStateManager.Instance.PlayerTransform.position = new Vector3(33, -11, 0); // Reunion area position
        NPCStateManager.Instance.SetMaxSuspicion(false);
        //SuspicionManager.Instance.ResetSuspicion();

        // Reset all systems
        NPCStateManager.Instance.ResetAllNPCs();
        SuspicionManager.Instance.ResetFromCaught();

        StartCoroutine(ShowCaughtMessage());
    }


    //checks if the player is in the line of sight of the npc, there cant be walls inbetween
    public bool HasLineOfSight(Vector3 npcWorldPos, Vector3 playerWorldPos, Tilemap wallTilemap)
    {
        Vector3Int npcCell = wallTilemap.WorldToCell(npcWorldPos);
        Vector3Int playerCell = wallTilemap.WorldToCell(playerWorldPos);

        foreach (Vector3Int pos in GetLine(npcCell, playerCell))
        {
            if (wallTilemap.HasTile(pos))
            {
                // Wall in the way
                return false;
            }
        }

        // Clear line of sight
        return true;
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

    //shows us the vision radius of the npcs, and the max distance the player can be off of the navmesh
    void OnDrawGizmos()
    {
        //max distance off navmesh
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(NPCStateManager.Instance.PlayerTransform.position, radiusCheck);

        //npc vision
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    //returns tru if player is close eniugh to the navmesh (aka the npc can still reach the player)
    public void IsOnNavMesh()
    {
        Vector3 position = NPCStateManager.Instance.PlayerTransform.position;
        //Debug.Log("CHECK RADIUS MESH: "+ radiusCheck);
        NavMeshHit hit;
        //Debug.Log(NavMesh.SamplePosition(position, out hit, radiusCheck, NavMesh.AllAreas));
        playerOnNavMesh = NavMesh.SamplePosition(position, out hit, radiusCheck, NavMesh.AllAreas);
    }

    //selects a valid waypoint, must be a waypoint to a room that has been unlocked
    int selectWaypointIndex()
    {
        wayPointIndex = Random.Range(0, targets.Count);
        while (!isValidWaypoint(wayPointIndex))
        {
            wayPointIndex = Random.Range(0, targets.Count);
        }
        return wayPointIndex;
    }

    //check if waypoint is in an unlocked room
    bool isValidWaypoint(int wayPointIndex)
    {

        string tag = targets[wayPointIndex].tag;

        Debug.Log("checking tag: " + tag);

        if (tag == "act1 waypoint")
            return NPCStateManager.Instance.act1;
        if (tag == "act2 waypoint")
            return NPCStateManager.Instance.act2;
        if (tag == "act3 waypoint")
            return NPCStateManager.Instance.act3;

        return false;
    }

    //shows a message when the player gets caught
    IEnumerator ShowCaughtMessage()
    {
        caughtMessage.gameObject.SetActive(true);
        notAgainMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        notAgainMessage.gameObject.SetActive(false);
        caughtMessage.gameObject.SetActive(false);

    }

    public void ResetFromCaught()
    {
        // Reset to nearest valid waypoint
        wayPointIndex = selectWaypointIndex();
        target = targets[wayPointIndex].position;
        targetIsWaypoint = true;

        // Force immediate update
        if (isWalking)
        {
            StopCoroutine(Walk());
        }
        StartCoroutine(Walk());
    }

    public void ResetFromChase()
    {
        // Immediately return to patrol
        targetIsWaypoint = true;
        wayPointIndex = selectWaypointIndex();
        target = targets[wayPointIndex].position;
    
        // Force path update
        if (isWalking)
        {
            StopCoroutine(Walk());
            StartCoroutine(Walk());
        }
    }

    public void ResetToPatrol()
    {
        // Immediate return to patrol
        targetIsWaypoint = true;
        wayPointIndex = selectWaypointIndex();
        target = targets[wayPointIndex].position;

        // Cancel current movement
        if (isWalking)
        {
            StopCoroutine(Walk());
        }

        // Restart patrol
        StartCoroutine(Walk());
    }
}