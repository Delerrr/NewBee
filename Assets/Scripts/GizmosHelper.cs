using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GizmosHelper : MonoBehaviour
{
    public static GizmosHelper instance => _instance;
    private static GizmosHelper _instance;
    [Header("Event Node")]
    public Color eventNodeColor;
    public Color eventNodeBoundsColor;
    public float eventNodeRadius;
    public Color eventNodeTimeTextColor;
    public Color eventNodeVelocityTextColor;
    [Header("Trace")]
    public Color traceColor;
    public int tracePointsCountPerEventNode = 30;

    public delegate void UpdatePathPointsLoop();
    public UpdatePathPointsLoop updatePathPointsLoop;
    public List<EventNode> GetEventNodes() {
        return GetSys().GetEventNodes();
    }

    public Sys GetSys() {
        return GetComponent<Sys>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        if (_instance != null) {
            Debug.LogError("单例模式不能出现多个实例");
            return;
        }
        _instance = this;
    }
    void Start() {
    }

    public void StartUpdatePathPointsLoopMethod() {
        InvokeRepeating(nameof(UpdatePathPointsLoopMethod), 0, 1f);
    }

    private void UpdatePathPointsLoopMethod() {
        updatePathPointsLoop?.Invoke();
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnDrawGizmos() {
        DrawEventNodes();
    }

    private void Refresh() {
        Sys sys = transform.GetComponent<Sys>();
        sys.Init();
    }

    private void DrawEventNodes() {
        Sys sys = GetSys();
        List<EventNode> eventNodes = sys.GetEventNodes();
        for (int i = 0; i < eventNodes.Count; i++) {
            DrawNodePosition(eventNodes[i]);
            DrawNodeBounds(eventNodes[i]);
            DrawEventNodeTime(eventNodes[i], i);
            DrawEventNodeVelocity(eventNodes[i]);
            if (i < eventNodes.Count - 1) {
                Vector3 to = eventNodes[i + 1].transform.position;
                DrawNodeTrace(to, eventNodes[i], eventNodes);
            }
        }
    }
    private void DrawNodeBounds(EventNode eventNode) {
        Gizmos.color = eventNodeBoundsColor;
        BoxCollider2D boxCollider2D = Sys.instance.player.GetComponent<BoxCollider2D>();
        Bounds bounds = boxCollider2D.bounds;
        float z = eventNode.transform.position.z;
        Vector3 min = bounds.min - bounds.center + eventNode.transform.position;
        Vector3 max = bounds.max - bounds.center + eventNode.transform.position;
        Vector3[] points = new Vector3[8] {
            new Vector3(min.x, min.y, z),
            new Vector3(min.x, max.y, z),
            new Vector3(min.x, max.y, z),
            new Vector3(max.x, max.y, z),
            new Vector3(max.x, max.y, z),
            new Vector3(max.x, min.y, z),
            new Vector3(max.x, min.y, z),
            new Vector3(min.x, min.y, z)
        };
        Gizmos.DrawLineList(points);
    }
    private void DrawEventNodeVelocity(EventNode eventNode) {
        GUIStyle style = new();
        style.normal.textColor = eventNodeVelocityTextColor;
        Handles.Label(eventNode.transform.position + Vector3.down * (1f + eventNodeRadius) - Vector3.right * eventNodeRadius,
            "v: " + eventNode.velocity, style);
    }

    private void DrawEventNodeTime(EventNode eventNode, int idx) {
        GUIStyle style = new();
        style.normal.textColor = eventNodeTimeTextColor;
        Handles.Label(eventNode.transform.position + Vector3.up * (1f + eventNodeRadius) - Vector3.right * eventNodeRadius,
           "idx: " + idx + ", t: " + eventNode.startTime, style);
    }

    private void DrawNodePosition(EventNode eventNode) {
        Gizmos.color = eventNodeColor;
        Gizmos.DrawSphere(eventNode.transform.position, eventNodeRadius);
    }
    private void DrawNodeTrace(Vector3 to, EventNode eventNode, List<EventNode> eventNodes) {
        Sys.MoveType moveType = eventNode.moveType;
        Vector3 from = eventNode.transform.position;
        Gizmos.color = traceColor;
        switch (moveType) {
            case Sys.MoveType.Jump:
                DrawJumpTrace(eventNode, eventNodes);
                break;
            case Sys.MoveType.Straight:
                DrawStraightTrace(from, to);
                break;
        }
    }

    private void DrawStraightTrace(Vector3 from, Vector3 to) {
        Gizmos.DrawLine(from, to);
    }

    private void DrawJumpTrace(EventNode eventNode, List<EventNode> eventNodes) {
        List<Vector3> pathPoints = eventNode.GetPathPoints(tracePointsCountPerEventNode, eventNodes);
        if (pathPoints == null || pathPoints.Count < 2) {
            return;
        }
        Vector3 pre = pathPoints[0];
        for (int i = 1; i < pathPoints.Count; i++) {
            Gizmos.DrawLine(pre, pathPoints[i]);
            pre = pathPoints[i];
        }
    }
}
