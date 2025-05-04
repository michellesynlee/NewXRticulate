using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class NavMeshPathVisualizer : MonoBehaviour
{
    public Transform player;     // PlayerAnchor
    public Transform goal;       // EndZoneTarget
    public float refreshInterval = 0.2f;

    LineRenderer lr;
    NavMeshPath path;
    WaitForSeconds wait;

    void Awake()
    {
        lr   = GetComponent<LineRenderer>();
        path = new NavMeshPath();
        wait = new WaitForSeconds(refreshInterval);
        StartCoroutine(UpdateLoop());
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            if (NavMesh.SamplePosition(player.position, out var sHit, 1f, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(goal.position,   out var gHit, 1f, NavMesh.AllAreas) &&
                NavMesh.CalculatePath(sHit.position, gHit.position, NavMesh.AllAreas, path) &&
                path.status == NavMeshPathStatus.PathComplete)
            {
                lr.positionCount = path.corners.Length;
                lr.SetPositions(path.corners);
            }
            else
            {
                lr.positionCount = 0;   // hide if no path
            }
            yield return wait;
        }
    }
}