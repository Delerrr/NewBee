using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EventNode : MonoBehaviour
{
    public Sys.MoveType moveType = Sys.MoveType.Straight;
    [Header("Jump")]
    public float jumpPower = 1f;

    private List<Vector3> pathPoints = new();

    private bool isUpdatingPathPoints = false;
    private Transform emptyGameObject;

    private void Start() {
        emptyGameObject = transform.Find("Empty(Clone)");
        if (emptyGameObject == null) {
            emptyGameObject = Instantiate(GizmosHelper.instance.emptyGameObject, transform).transform;
        }
    }

    public List<Vector3> GetPathPoints() {
        if (isUpdatingPathPoints) {
            return null;
        }
        if (Application.isPlaying) {
            if (pathPoints.Count == 0) {
                UpdatePathPoints(Sys.instance);
            }
        }
        return pathPoints;
    }

    public Tween UpdatePathPoints(Sys sys) {
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
        pathPoints.Clear();
        if (moveType == Sys.MoveType.Jump) {
            emptyGameObject.position = transform.position;
            ret = emptyGameObject.DOJump(eventNodes[idx + 1].transform.position, jumpPower, 1, 1f);
            ret.onUpdate += () => pathPoints.Add(emptyGameObject.transform.position);
            ret.onComplete += () => isUpdatingPathPoints = false;
        }
        return ret;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
