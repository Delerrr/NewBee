using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    public EventNode endNode;
    private void Start() {
        transform.parent.GetComponent<EventNode>().triggerNodeStart += () => this.Trigger();
        endNode.triggerNodeStart += () => this.End();
    }
    public virtual void Trigger() { } 
    public virtual void End() { }
}
