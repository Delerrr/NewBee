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
    public float eventNodeRadius;
    public Color eventNodeTimeTextColor;
    public Color eventNodeVelocityTextColor;
    [Header("Trace")]
    public GameObject emptyGameObject;
    public Color traceColor;
    public float tweenDuration = 0.5f;

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
            DrawEventNodeTime(eventNodes[i], i, sys);
            if (eventNodes[i].transform.GetComponent<VelocityNode>() != null) {
                DrawEventNodeVelocity(eventNodes[i]);
            }
            if (i < eventNodes.Count - 1) {
                Vector3 to = eventNodes[i + 1].transform.position;
                DrawNodeTrace(to, eventNodes[i]);
            }
        }
    }
    private void DrawEventNodeVelocity(EventNode eventNode) {
        GUIStyle style = new();
        style.normal.textColor = eventNodeVelocityTextColor;
        Handles.Label(eventNode.transform.position + Vector3.down * (1f + eventNodeRadius) - Vector3.right * eventNodeRadius,
            "v: " + eventNode.transform.GetComponent<VelocityNode>().velocity, style);
    }

    private void DrawEventNodeTime(EventNode eventNode, int idx, Sys sys) {
        GUIStyle style = new();
        style.normal.textColor = eventNodeTimeTextColor;
        Handles.Label(eventNode.transform.position + Vector3.up * (1f + eventNodeRadius) - Vector3.right * eventNodeRadius,
            "t: " + sys.GetTime(idx).ToString("0.000"), style);
    }

    private void DrawNodePosition(EventNode eventNode) {
        Gizmos.color = eventNodeColor;
        Gizmos.DrawSphere(eventNode.transform.position, eventNodeRadius);
    }
    private void DrawNodeTrace(Vector3 to, EventNode eventNode) {
        Sys.MoveType moveType = eventNode.moveType;
        Vector3 from = eventNode.transform.position;
        Gizmos.color = traceColor;
        switch (moveType) {
            case Sys.MoveType.Jump:
                DrawJumpTrace(eventNode);
                break;
            case Sys.MoveType.Straight:
                DrawStraightTrace(from, to);
                break;
        }
    }

    private void DrawStraightTrace(Vector3 from, Vector3 to) {
        Gizmos.DrawLine(from, to);
    }

    private void DrawJumpTrace(EventNode eventNode) {
        List<Vector3> pathPoints = eventNode.GetPathPoints();
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
