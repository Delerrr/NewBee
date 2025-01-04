using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Sys : MonoBehaviour {
    public static Sys instance => _instance;
    private static Sys _instance;
    public enum MoveType { Jump, Straight };
    public delegate void Play(Vector3 position);
    public Play playEvent;
    public Transform player;
    [Header("Jump")]
    public float gravity = -9.8f;
    private NodeTimeParser nodeTimeParser;
    private AudioSource music;

    private void Awake() {
        if (_instance != null) {
            Debug.LogError("单例模式不能出现多个实例");
            return;
        }
        _instance = this;
        nodeTimeParser = GetComponent<NodeTimeParser>();
        music = GetComponent<AudioSource>();
    }

    void Start() {
        Init();
    }

    public int GetIdx(EventNode target, List<EventNode> eventNodes) {
        for (int i = 0; i < eventNodes.Count; i++) {
            if (eventNodes[i] == target) {
                return i;
            }
        }
        return -1;
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

    public EventNode GetDestination(EventNode eventNode, List<EventNode> eventNodes) {
        int idx = eventNodes.FindIndex(e => e == eventNode);
        if (idx >= 0 && idx < eventNodes.Count - 1) {
            return eventNodes[idx + 1];
        }
        return null;
    }

/*    private void ResetPlayer(List<EventNode> eventNodes) {
        if (eventNodes.Count > 0) {
            player.transform.position = eventNodes[0].transform.position;
        }
    }
    private void Play() {
        ResetPlayer(GetEventNodes());
        sq.Restart();
        GetComponent<AudioSource>().Play();
    }
*/
    public void Init() {
        GetEventNodes();
    }

    public float GetDuration(int idx, List<EventNode> eventNodes) {
        return eventNodes[idx + 1].startTime - eventNodes[idx].startTime;
    }

/*    public float GetTime(int idx) {
        List<EventNode> eventNodes = GetEventNodes(); 
        if (idx >= eventNodes.Count) {
            Debug.LogError("idx 不合法");
            return -1;
        }
        float res = 0;
        float velocity = velocityNodes[0].velocity;
        for (int i = 0; i < idx; i++) {
            EventNode eventNode = eventNodes[i];
            if (eventNode.transform.GetComponent<VelocityNode>() != null) {
                velocity = eventNode.transform.GetComponent<VelocityNode>().velocity;
            }
            res += Vector3.Distance(eventNodes[i + 1].transform.position, eventNodes[i].transform.position) / velocity;
        }
        return res;
    }
    private Tween GetTween(Transform transform, MoveType moveType, float duration, Vector3 destination, float jumpPower) {
        switch (moveType) {
            case MoveType.Jump:
                return transform.DOJump(destination, jumpPower, 1, duration).SetEase(jumpCurve);
            case MoveType.Straight:
                return transform.DOMove(destination, duration).SetEase(Ease.Linear);
            default:
                Debug.LogError("MoveType 不合法: " + moveType);
                return null;
        }
    }
*/    private Vector3 CalcPosition(float t) {
        List<EventNode> eventNodes = GetEventNodes();
        if (t < 0 || t > nodeTimeParser.GetTime(eventNodes.Count - 1)) {
            Debug.LogError("计算位置时，时间非法");
            return Vector3.zero;
        }

        for (int i = 0; i < eventNodes.Count - 1; i++) {
            if (eventNodes[i].startTime <= t && eventNodes[i + 1].startTime > t) {
                // TODO: 这里不是CalcPosition的功能，应该把它挪到其他地方
                if (currentEventNode != eventNodes[i]) {
                    currentEventNode = eventNodes[i];
                    eventNodes[i].TriggerEffects();
                }
                return eventNodes[i].GetPointAtTime(t - eventNodes[i].startTime, eventNodes[i + 1]);
            }
        }
        Debug.LogError("计算位置时，没找到对应的Event Node");
        return Vector3.zero;
    }

    private void PlayAtTime(float t) {
        this.playEvent.Invoke(CalcPosition(t));
    }

    double timePre = -1;
    private EventNode currentEventNode;
    // Update is called once per frame
    void LateUpdate() {
        if (Application.isPlaying) {
            if (Input.GetKeyDown(KeyCode.K)) {
                music.PlayScheduled(0f);
                PlayAtTime(0f);
                timePre = AudioSettings.dspTime;
            }
            PlayAtTime((float)(AudioSettings.dspTime - timePre));
        }
    }
}
