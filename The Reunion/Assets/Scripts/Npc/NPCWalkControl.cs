using UnityEngine;
using UnityEngine.AI;

public class NPCWalkControl : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detect if the NPC is actually moving (has velocity)
        bool isWalking = agent.velocity.sqrMagnitude > 0.01f && !agent.isStopped;

        // Set Animator bool parameter to control the walk animation
        animator.SetBool("isWalking", isWalking);
    }
}
