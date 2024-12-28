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
    public float eventNodeTimeTextSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
    }

    private void OnDrawGizmos() {
        DrawEventNodes();
    }
    private void DrawEventNodes() {
        Sys sys = transform.GetComponent<Sys>();
        List<EventNode> eventNodes = sys.GetEventNodes();
        for (int i = 0; i < eventNodes.Count; i++) {
            Gizmos.color = eventNodeColor;
            Gizmos.DrawSphere(eventNodes[i].transform.position, eventNodeRadius);
            // time
            Handles.color = eventNodeTimeTextColor;
            Handles.Label(eventNodes[i].transform.position + Vector3.up - Vector3.right * eventNodeRadius, sys.GetTime(i).ToString("0.000"));
        }
    }

    private void DrawTrace() {
        // TODO: ¹ì¼£ÏÔÊ¾
    }
}
