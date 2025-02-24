using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    public bool isMoving;
    public int wayPointIndex;
    public float moveSpeed;

    void Start()
    {
        StartMoving();
    }

    public void StartMoving()
    {
        wayPointIndex = 0;
        isMoving = true;


    }

    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex].position, Time.deltaTime*moveSpeed);

        var distance = Vector3.Distance(transform.position, wayPoints[wayPointIndex].position);

        if (distance <= 0.5f)
            wayPointIndex++;
    }
}
