using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Sys : MonoBehaviour {
    public static Sys instance => _instance;
    private static Sys _instance;
    public GameObject cinemachieCamera;
    public enum MoveType { Jump, Straight };
    public Action<float> playAtTimeAction;
    public Action restartAction;
    public Action initAction;
    public Transform player;

    private void Awake() {
        if (_instance != null) {
            Debug.LogError(this.GetType() + ":单例模式不能出现多个实例");
            return;
        }
        _instance = this;
        /**
         * 要在Awake里Init，因为有的组件(比如<see cref="NootNoot"/> 在Start里就要获取EventNodes了
         */
        Init();
    }

    void Start() {
        Restart();
    }

/*    public int GetIdx(EventNode target, List<EventNode> eventNodes) {
        for (int i = 0; i < eventNodes.Count; i++) {
            if (eventNodes[i] == target) {
                return i;
            }
        }
        return -1;
    }
*/    
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
        initAction?.Invoke();
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
*/    


    private void Restart() {
        if (!Application.isPlaying) {
            return;
        }
        restartAction?.Invoke();
        AudioManager.instance.GetAudioSource().PlayScheduled(0f);
        playAtTimeAction?.Invoke(0f);
        timePre = AudioSettings.dspTime;
    }
    double timePre = -1;
    // Update is called once per frame
    void LateUpdate() {
        if (Application.isPlaying) {
            if (Input.GetKeyDown(KeyCode.K)) {
                Restart();
            } else {
                playAtTimeAction?.Invoke((float)(AudioSettings.dspTime - timePre));
            }
        }
    }
}
