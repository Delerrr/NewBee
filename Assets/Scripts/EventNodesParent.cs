using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EventNodesParent : MonoBehaviour
{
    private NodeTimeParser nodeTimeParser;
    public delegate void Play(Vector3 position, float time);
    public Play playEvent;
    [Header("Jump")]
    public float gravity = -9.8f;

    private EventNode currentEventNode;
    private void Awake() {
        nodeTimeParser = GetComponent<NodeTimeParser>();
        Sys.initAction += () => GetEventNodes();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Sys.instance.playAtTimeAction += PlayAtTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PlayAtTime(float t) {
        this.playEvent?.Invoke(CalcPosition(t), t);
    }
    private Vector3 CalcPosition(float t) {
        List<EventNode> eventNodes = GetEventNodes();
        if (t < 0 || t > nodeTimeParser.GetTime(eventNodes.Count - 1)) {
            //Debug.LogError("����λ��ʱ��ʱ��Ƿ�");
            return Vector3.zero;
        }

        for (int i = 0; i < eventNodes.Count - 1; i++) {
            if (eventNodes[i].startTime <= t && eventNodes[i + 1].startTime > t) {
                // TODO: ���ﲻ��CalcPosition�Ĺ��ܣ�Ӧ�ð���Ų�������ط�
                // TODO: ������currentEventNode���²��ܴ�����ʱ��㿪ʼ���ţ�
                if (currentEventNode != eventNodes[i]) {
                    currentEventNode = eventNodes[i];
                    eventNodes[i].TriggerEffects();
                }
                return eventNodes[i].GetPointAtTime(t - eventNodes[i].startTime, eventNodes[i + 1]);
            }
        }
        Debug.LogError("����λ��ʱ��û�ҵ���Ӧ��Event Node");
        return Vector3.zero;
    }
    public List<EventNode> GetEventNodes() {
        List<EventNode> eventNodes = new();
        foreach (Transform child in transform) {
            EventNode eventNode = child.GetComponent<EventNode>();
            if (eventNode != null) {
                eventNodes.Add(eventNode);
            }
        }
        int n = eventNodes.Count;
        eventNodes[n - 1].InitStartTimeAndVelocity(nodeTimeParser.GetTime(n - 1), null);
        for (int i = n - 2; i >= 0; i--) {
            eventNodes[i].InitStartTimeAndVelocity(nodeTimeParser.GetTime(i), eventNodes[i + 1]);
        }
        return eventNodes;
    }

}
