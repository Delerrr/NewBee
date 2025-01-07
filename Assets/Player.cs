using UnityEngine;

public class Player : MonoBehaviour
{
    public EventNodesParent eventNodesParent;
    public float startTime = -1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        eventNodesParent.playEvent += (pos, time) => {
            if (pos == Vector3.zero) {
                return;
            }
            if (time >= startTime) {
                transform.position = pos;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
