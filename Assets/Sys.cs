using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Sys : MonoBehaviour {
    public static Sys instance => _instance;
    private static Sys _instance;
    public enum MoveType { Jump, Straight };

    public Transform player;
    private Sequence sq;
    private List<EventNode> eventNodes = new();
    private List<VelocityNode> velocityNodes = new();

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

    public List<EventNode> GetEventNodes() {
        if (eventNodes.Count == 0) {
            InitEventNodes();
        }
        return eventNodes;
    }

    private void ResetPlayer() {
        if (eventNodes.Count > 0) {
            player.transform.position = eventNodes[0].transform.position;
        }
    }
    private void Play() {
        ResetPlayer();
        sq.Restart();
    }

    public void Init() {
        sq = DOTween.Sequence();
        sq.SetAutoKill(false);
        sq.Pause();

        InitEventNodes();
        ResetPlayer();

        for (int i = 0; i < eventNodes.Count - 1; i++) {
            EventNode eventNode = eventNodes[i];
            Tween tween = GetTween(player, eventNode.moveType, GetTime(i + 1) - GetTime(i), eventNodes[i + 1].transform.position, eventNode.jumpPower);
            eventNode.SetTween(tween);
            sq.Append(tween);
        }

    }

    private void InitEventNodes() {
        eventNodes.Clear();
        foreach (Transform child in transform) {
            EventNode eventNode = child.GetComponent<EventNode>();
            if (eventNode != null) {
                eventNodes.Add(eventNode);
            }

            VelocityNode velocityNode = child.GetComponent<VelocityNode>();
            if (velocityNode != null) {
                velocityNodes.Add(velocityNode);
            }
        }
    }

    private float GetTime(int idx) {
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
