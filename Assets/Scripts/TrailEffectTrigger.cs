using UnityEngine;

public class TrailEffectTrigger : EffectTrigger
{
    public enum TriggerType { Enable, Disable }
    public TriggerType triggerType;
    private Transform oriParent;
    private void Start() {
    }
    public void SetOriParent() {
        if (oriParent == null) {
            Debug.LogError("原来的父物体不能为null");
            return;
        }
        transform.SetParent(oriParent, false);
    }
    public override void Trigger() {
        Transform player = Sys.instance.player;
        switch (this.triggerType) {
            case TriggerType.Enable:
                oriParent = transform.parent;
                transform.SetParent(player, false);
                gameObject.SetActive(true);
                break;
            case TriggerType.Disable:
                TrailEffectTrigger effectTrigger = player.GetComponentInChildren<TrailEffectTrigger>();
                if (effectTrigger != null) {
                    effectTrigger.SetOriParent();
                    effectTrigger.gameObject.SetActive(false);
                }
                break;
        }
    }
}
