using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTrigger : EffectTrigger
{
    public GameObject obj;
    [Header("ÌáÇ°´¥·¢")]
    public bool hasAdvanceTime = false;
    public float startAdvanceTime;
    public float endAdvanceTime;
    private TimedTrigger startTimedTrigger;
    private TimedTrigger endTimedTrigger;
    protected override void Start()
    {
        base.Start();
        startTimedTrigger = new();
        endTimedTrigger = new();
        if (hasAdvanceTime) {
            Sys.instance.playEvent += (_, time) => this.EndWithAdvanceTime(time);
            Sys.instance.playEvent += (_, time) => this.TriggerWithAdvanceTime(time);
        }
    }
    public override void End() {
        if (!hasAdvanceTime) {
            TryEndGracefully();
        }
    }

    private void EndWithAdvanceTime(float time) {
        float endTime = endNode.startTime - endAdvanceTime;
        endTimedTrigger.Trigger(time, endTime, TryEndGracefully);
    }
    public override void Trigger() {
        if (!hasAdvanceTime) {
            obj.SetActive(true);
        }
    }

    private void TriggerWithAdvanceTime(float time) {
        float triggerTime = transform.parent.GetComponent<EventNode>().startTime - endAdvanceTime;
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
