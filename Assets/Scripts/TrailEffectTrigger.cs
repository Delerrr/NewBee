using UnityEngine;

public class GeneralTrigger : EffectTrigger
{
    public GameObject obj;
    public override void End() {
        obj.SetActive(false);
    }
    public override void Trigger() {
        obj.SetActive(true);
    }
}
