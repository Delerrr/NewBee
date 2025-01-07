using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    public EventNode endNode;
    protected EventNodesParent eventNodesParent;
    protected virtual void Start() {
        EventNode eventNode = transform.parent.GetComponent<EventNode>();
        eventNodesParent = eventNode.GetParent();
        eventNode.triggerNodeStart += () => this.Trigger();
        if (endNode != null) {
            endNode.triggerNodeStart += () => this.End();
        }
    }
    public virtual void Trigger() { }
    public virtual void End() { }
}
