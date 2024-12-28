using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GizmosHelper : MonoBehaviour
{
    [Header("Event Node")]
    public Color eventNodeColor = Color.red;
    public float eventNodeRadius;
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
        List<EventNode> eventNodes = transform.GetComponent<Sys>().GetEventNodes();
        foreach (EventNode node in eventNodes) {
            Gizmos.color = eventNodeColor;
            Gizmos.DrawSphere(node.transform.position, eventNodeRadius);
        }
    }

    private void DrawTrace() {
        // TODO: πÏº£œ‘ æ
    }
}
