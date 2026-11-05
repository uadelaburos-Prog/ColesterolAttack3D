using UnityEngine;
using Demonics;
using ED262C;
using Unity.VisualScripting;
using System.IO;
public class AStarMovement : MonoBehaviour
{
    private AStar<Vector3> aStar = new();
    private ISimpleList<Vector3> path;
    private int pathIndex;
    private float repathTimer;

    [SerializeField] private Transform playerPosition;

    private void Awake()
    {
        playerPosition = GameObject.Find("Player").transform;
    }

    private void CalculatePath()
    {
        WaypointGraph wg = WaypointGraph.Instance;

        GraphNode<Vector3> origin = wg.GetClosestNode(transform.position);
        GraphNode<Vector3> destination = wg.GetClosestNode(playerPosition.position);

        path = aStar.GetShortesPath(origin, destination, wg.Graph, (a, b) => Mathf.RoundToInt(Vector3.Distance(a.Value, b.Value) * WaypointGraph.CostScale));

        pathIndex = 0;
    }

    public void FollowPath(Vector3 directDirection, float speed)
    {
        repathTimer -= Time.deltaTime;

        if (repathTimer < 0)
        {
            repathTimer = 0.5f;
            CalculatePath();
        }

        if (path == null || pathIndex >= path.Count)
        {
            MoveTowardsPos(directDirection, speed);
            return;
        }

        Vector3 target = path[pathIndex];
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir.magnitude < 0.6f)
        {
            pathIndex++;
            return;
        }

        MoveTowardsPos(dir.normalized, speed);
    }
    
    public void ResetPath()
    {
        path = null;
        pathIndex = 0;
        repathTimer = 0f;
    }

    private void MoveTowardsPos(Vector3 direction, float actualVelocity)
    {
        transform.position += direction * actualVelocity * Time.deltaTime;
    }

}
