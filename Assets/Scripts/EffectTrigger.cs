using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    public EventNode endNode;
    protected virtual void Start() {
        transform.parent.GetComponent<EventNode>().triggerNodeStart += () => this.Trigger();
        if (endNode != null) {
            endNode.triggerNodeStart += () => this.End();
        }
    }
    public virtual void Trigger() { } 
    public virtual void End() { }
}
