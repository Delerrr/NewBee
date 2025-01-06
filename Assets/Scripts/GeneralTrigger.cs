using UnityEngine;

public class GeneralTrigger : EffectTrigger
{
    public GameObject obj;
    [Header("提前触发")]
    public bool hasAdvanceTime = false;
    [Header("延时关闭")]
    public bool hasDelayEndTime = false;
    public float startAdvanceTime;
    public float endDelayTime;
    private TimedTrigger startTimedTrigger;
    private TimedTrigger endTimedTrigger;
    protected override void Start()
    {
        base.Start();
        startTimedTrigger = new();
        endTimedTrigger = new();
        if (hasAdvanceTime) {
            Sys.instance.playEvent += (_, time) => this.TriggerWithAdvanceTime(time);
        }
    }
    public override void End() {
        if (hasDelayEndTime) {
            Invoke(nameof(TryEndGracefully), endDelayTime);
        } else {
            TryEndGracefully();
        }
    }

    public override void Trigger() {
        if (!hasAdvanceTime) {
            obj.SetActive(true);
        }
    }

    private void TriggerWithAdvanceTime(float time) {
        float triggerTime = transform.parent.GetComponent<EventNode>().startTime - startAdvanceTime;
        startTimedTrigger.Trigger(time, triggerTime, () => obj.SetActive(true));
    }
    private void TryEndGracefully() {
        EndGracefully endGracefully = obj.GetComponent<EndGracefully>();
        if (endGracefully != null) {
            endGracefully.End();
        } else {
            obj.SetActive(false);
        }
    }
}
