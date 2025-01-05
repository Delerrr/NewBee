using UnityEngine;

public class GeneralTrigger : EffectTrigger
{
    public GameObject obj;
    [Header("��ǰ����")]
    public bool hasAdvanceTime = false;
    public float startAdvanceTime;
    public float endAdvanceTime;
    private bool startFlag = false;
    private bool endFlag = false;
    protected override void Start()
    {
        base.Start();
        if (hasAdvanceTime) {
            Sys.instance.playEvent += (_, time) => this.EndWithAdvanceTime(time);
            Sys.instance.playEvent += (_, time) => this.TriggerWithAdvanceTime(time);
        }
    }
    public override void End() {
        if (!hasAdvanceTime) {
            TryEndGraceFully();
        } else {
        }
    }

    private void EndWithAdvanceTime(float time) {
        float endTime = endNode.startTime - endAdvanceTime;
        if (endFlag) {
            // ���¿�ʼ����Ϸ
            if (time - endTime < 0) {
                endFlag = false;
            }
            return;
        }

        if (time - endTime >= 0) {
            if (time - endTime < Time.deltaTime * 2) {
                TryEndGraceFully();
                endFlag = true;
            } else {
                Debug.LogError("û����������Trigger!!!");
            }
        }
    }
    public override void Trigger() {
        if (!hasAdvanceTime) {
            obj.SetActive(true);
        }
    }

    private void TriggerWithAdvanceTime(float time) {
        float triggerTime = endNode.startTime - endAdvanceTime;
        if (startFlag) {
            // ���¿�ʼ����Ϸ
            if (time - triggerTime < 0) {
                startFlag = false;
            }
            return;
        }

        if (time - triggerTime >= 0) {
            if (time - triggerTime < Time.deltaTime * 2) {
                obj.SetActive(true);
                startFlag = true;
            } else {
                Debug.LogError("û����������Trigger!!!");
            }
        }
    }
    private void TryEndGraceFully() {
        EndGracefully endGracefully = obj.GetComponent<EndGracefully>();
        if (endGracefully != null) {
            endGracefully.End();
        } else {
            obj.SetActive(false);
        }
    }
}
