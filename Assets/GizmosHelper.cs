using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GizmosHelper : MonoBehaviour
{
    [Header("Event Node")]
    public Color eventNodeColor;
    public float eventNodeRadius;
    public Color eventNodeTimeTextColor;
    public Color eventNodeVelocityTextColor;
    //public float eventNodeTimeTextSize;
    [Header("Refresh")]
    public bool refresh = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
    }

    private void OnDrawGizmos() {
        if (refresh) {
            Refresh();
            refresh = false;
        }
        DrawEventNodes();
    }

    private void Refresh() {
        Sys sys = transform.GetComponent<Sys>();
        sys.Init();
    }

    private void DrawEventNodes() {
        Sys sys = transform.GetComponent<Sys>();
        List<EventNode> eventNodes = sys.GetEventNodes();
        for (int i = 0; i < eventNodes.Count; i++) {
            Gizmos.color = eventNodeColor;
            Gizmos.DrawSphere(eventNodes[i].transform.position, eventNodeRadius);
            // time
            GUIStyle style = new();
            style.normal.textColor = eventNodeTimeTextColor;
            Handles.Label(eventNodes[i].transform.position + Vector3.up * (1f + eventNodeRadius) - Vector3.right * eventNodeRadius, 
                "t: " + sys.GetTime(i).ToString("0.000"), style);
            if (eventNodes[i].transform.GetComponent<VelocityNode>() != null) {
                style.normal.textColor = eventNodeVelocityTextColor;
                Handles.Label(eventNodes[i].transform.position + Vector3.down * (1f + eventNodeRadius) - Vector3.right * eventNodeRadius,
                    "v: " + eventNodes[i].transform.GetComponent<VelocityNode>().velocity, style);
            }
        }
    }

    private void DrawTrace() {
        // TODO: πÏº£œ‘ æ
    }
}
