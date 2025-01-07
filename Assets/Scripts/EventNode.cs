using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EventNode : MonoBehaviour
{
    public delegate void TriggerNodeStart();
    public TriggerNodeStart triggerNodeStart;
    public Sys.MoveType moveType = Sys.MoveType.Straight;
    public float startTime;
    public float velocity = 1f;
    [Header("Jump")]
    public bool customGravity = false;
    public float gravity = -9.6f;

    private void Start() {
    }

    public EventNodesParent GetParent() {
        return transform.parent.GetComponent<EventNodesParent>();
    }
    public void TriggerEffects() {
        this.triggerNodeStart?.Invoke();  
    }

    public void InitStartTimeAndVelocity(float startTime, EventNode destination) {
        this.startTime = startTime;
        if (destination != null) {
            float duration = destination.startTime - startTime;
            switch (this.moveType) {
                case Sys.MoveType.Jump:
                    this.velocity = (destination.transform.position.x - transform.position.x) / duration;
                    break;
                case Sys.MoveType.Straight:
                    this.velocity = Vector3.Distance(destination.transform.position, transform.position) / duration;
                    break;
                default:
                    Debug.LogError($"movetype 不合法, {moveType}");
                    break;
            }
        }
    }
    public List<Vector3> GetPathPoints(int count, List<EventNode> eventNodes) {
        List<Vector3> pathPoints = new();
        EventNode destination = Sys.instance.GetDestination(this, eventNodes);
        if (destination == null) {
            return pathPoints;
        }
        float duration = destination.startTime - startTime;
        for (int i = 0; i <= count; i++) {
            float t = duration / count * i;
            pathPoints.Add(GetPointAtTime(t, destination));
        }
        return pathPoints;
    }
    public Vector3 GetPointAtTime(float t, EventNode destination) {
        return GetPointAtTime(t, destination.startTime - startTime, destination.transform.position);
    }

    private Vector3 GetPointAtTime(float t, float duration, Vector3 destination) {
        Vector3 res = transform.position;
        if (t == 0) {
            return res;
        }
        switch (this.moveType) {
            case Sys.MoveType.Jump:
                res.x += velocity * t;
                if (!customGravity) {
                    gravity = GetParent().gravity;
                }
                // x = vt + 1/2 a t^2
                float verticalVelocity = ((destination.y - res.y) - 0.5f * gravity * duration * duration) / duration;
                res.y += verticalVelocity * t + 0.5f * gravity * t * t;
                break;
            case Sys.MoveType.Straight:
                res = Vector3.Lerp(res, destination, t / duration);
                break;
            default:
                Debug.LogError($"movetype 不合法, {moveType}");
                break;
        }
        return res;
    }

/*    public Tween UpdatePathPoints(Sys sys, GizmosHelper gizmosHelper) {
        if (isUpdatingPathPoints) {
            return null;
        }

        List<EventNode> eventNodes = sys.GetEventNodes();
        int idx = sys.GetIdx(this, eventNodes);
        if (idx == -1 || idx == eventNodes.Count - 1) {
            return null;
        }

        isUpdatingPathPoints = true;

        Tween ret = null;
        List<Vector3> newPathPoints = new();
        if (moveType == Sys.MoveType.Jump) {
            emptyGameObject.position = transform.position;
            ret = emptyGameObject.DOJump(eventNodes[idx + 1].transform.position, jumpPower, 1, gizmosHelper.tweenDuration);
            ret.onUpdate += () => newPathPoints.Add(emptyGameObject.transform.position);
            ret.onComplete += () => {
                isUpdatingPathPoints = false;
                pathPoints = newPathPoints;
            };
        }
        return ret;
    }
*/    // Update is called once per frame
    void Update() {

    }
}
