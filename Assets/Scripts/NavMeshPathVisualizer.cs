using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Draws a glowing ribbon along the current NavMesh path between <see cref="player"/> and <see cref="goal"/>.
/// Attach this to an empty GameObject that also has a Line Renderer.
/// • Set Line Renderer → Alignment = **Transform Z**
/// • Rotate the GameObject so its local Z-axis points up (e.g. X = 90°, Y = 0, Z = 0)
/// That makes the ribbon lie flat on the floor.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class NavMeshPathVisualizer : MonoBehaviour
{
    [Tooltip("Transform whose position represents the player (e.g. PlayerAnchor under XR Origin).")]
    public Transform player;

    [Tooltip("Transform at the centre of the EndZone (e.g. EndZoneTarget).")]
    public Transform goal;

    [Tooltip("Seconds between path recalculations.")]
    public float refreshInterval = 0.20f;

    [Tooltip("Height (metres) the ribbon floats above the mesh to avoid z-fighting.")]
    public float pathHeight = 0.01f;

    private LineRenderer lr;
    private NavMeshPath navPath;
    private WaitForSeconds wait;

    void Awake()
    {
        lr       = GetComponent<LineRenderer>();
        navPath  = new NavMeshPath();
        wait     = new WaitForSeconds(refreshInterval);

        lr.useWorldSpace = true;             // keep vertices in world coordinates
        lr.alignment     = LineAlignment.TransformZ;  // face local Z, not the camera

        StartCoroutine(UpdatePathLoop());
    }

    IEnumerator UpdatePathLoop()
    {
        while (true)
        {
            UpdatePath();
            yield return wait;
        }
    }

    /// <summary>
    /// Rebuild the line from the current NavMesh path.
    /// </summary>
    private void UpdatePath()
    {
        bool gotStart = NavMesh.SamplePosition(player.position, out NavMeshHit sHit, 1f, NavMesh.AllAreas);
        bool gotGoal  = NavMesh.SamplePosition(goal.position,   out NavMeshHit gHit, 1f, NavMesh.AllAreas);

        if (gotStart && gotGoal &&
            NavMesh.CalculatePath(sHit.position, gHit.position, NavMesh.AllAreas, navPath) &&
            navPath.status == NavMeshPathStatus.PathComplete)
        {
            int count = navPath.corners.Length;
            lr.positionCount = count;

            // copy corners and raise them slightly
            for (int i = 0; i < count; i++)
            {
                Vector3 p = navPath.corners[i];
                p.y += pathHeight;
                lr.SetPosition(i, p);
            }
        }
        else
        {
            lr.positionCount = 0;  // hide if no valid path
        }
    }
}
