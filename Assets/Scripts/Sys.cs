using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Sys : MonoBehaviour {
    public static Sys instance => _instance;
    private static Sys _instance;
    public enum MoveType { Jump, Straight };

    public Transform player;
    private Sequence sq;

    private void Awake() {
        if (_instance != null) {
            Debug.LogError("单例模式不能出现多个实例");
            return;
        }
        _instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        NodeTimeParser nodeTimeParser = GetComponent<NodeTimeParser>();
        for (int i = eventNodes.Count - 1; i >= 0; i--) {
            eventNodes[i].time = nodeTimeParser.GetTime(i);
            if (i < eventNodes.Count - 1) {
                eventNodes[i].velocity = Vector3.Distance(eventNodes[i].transform.position, eventNodes[i + 1].transform.position) / GetDuration(i, eventNodes);
            }
        }
        return eventNodes;
    }

    private void ResetPlayer(List<EventNode> eventNodes) {
        if (eventNodes.Count > 0) {
            player.transform.position = eventNodes[0].transform.position;
        }
    }
    private void Play() {
        ResetPlayer(GetEventNodes());
        sq.Restart();
        GetComponent<AudioSource>().Play();
    }

    public void Init() {
        sq = DOTween.Sequence();
        sq.SetAutoKill(false);
        sq.Pause();

        List<EventNode> eventNodes = GetEventNodes();
        ResetPlayer(eventNodes);

        for (int i = 0; i < eventNodes.Count - 1; i++) {
            EventNode eventNode = eventNodes[i];
            Tween tween = GetTween(player, eventNode.moveType, GetDuration(i, eventNodes), eventNodes[i + 1].transform.position, eventNode.jumpPower);
            sq.Append(tween);
        }
    }

    public float GetDuration(int idx, List<EventNode> eventNodes) {
        return eventNodes[idx + 1].time - eventNodes[idx].time;
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
*/    private Tween GetTween(Transform transform, MoveType moveType, float duration, Vector3 destination, float jumpPower) {
        switch (moveType) {
            case MoveType.Jump:
                return transform.DOJump(destination, jumpPower, 1, duration);
            case MoveType.Straight:
                return transform.DOMove(destination, duration).SetEase(Ease.Linear);
            default:
                Debug.LogError("MoveType 不合法: " + moveType);
                return null;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            Play();
        }
    }
}
