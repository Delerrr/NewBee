
using System;
using UnityEngine;

public class TimedTrigger {
    private bool flag = false;

    public TimedTrigger() {
        Sys.instance.restartAction += () => flag = false;
    }
    public bool Trigger(float time, float triggerTime, Action successCallback) {
        if (triggerTime < 0) {
            Debug.LogError("TriggerTime不能小于0");
            return false;
        }
        if (this.flag) {
            return false;
        }

        if (time - triggerTime >= 0) {
            this.flag = true;
            successCallback?.Invoke();
            return true;
        }
        return false;
    }

}
