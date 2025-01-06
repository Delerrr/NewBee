using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class NootNoot : MonoBehaviour
{
    public float alpha = 0.886f;
    public float appearTimeAdvance = 0.5f;
    public float disappearDuration = 0.5f;
    [Header("����")]
    public List<EventNodeSection> sections;
    [Header("Noot����ʱ���")]
    public List<EventNode> eventNodes;
    private Animator animator;
    private float nootAnimDuration;
    private float pivotRatio = 0.6f;
    // ��������������ᵼ��TimedTrigger�������Զ���������֪��Ϊʲô
    private List<TimedTrigger> timedTriggers = new();
    private void Start() {
        animator = GetComponent<Animator>();
        InitNootAnimaDuration();
        InitAnimEvents();
    }

    private void InitAnimEvents() {
        // ע������Trigger
        foreach(EventNodeSection section in sections) {
            TimedTrigger enterTimedTrigger = new TimedTrigger();
            TimedTrigger exitTimedTrigger = new TimedTrigger();
            timedTriggers.Add(enterTimedTrigger);
            timedTriggers.Add(exitTimedTrigger);
            Sys.instance.playEvent += (_, time) => enterTimedTrigger.Trigger(
                    time,
                    section[0].startTime - appearTimeAdvance,
                    Appear
            );
            Sys.instance.playEvent += (_, time) => exitTimedTrigger.Trigger(
                    time,
                    section[1].startTime,
                    Disappear
            );
        }

        // ע��Noot����Trigger
        for (int i = 0; i < eventNodes.Count; i++) {
            float duration = nootAnimDuration;
            if (i > 0) {
                duration = Mathf.Min(duration, eventNodes[i].startTime - eventNodes[i - 1].startTime);
            }
            if (i < eventNodes.Count - 1) {
                duration = Mathf.Min(duration, eventNodes[i + 1].startTime - eventNodes[i].startTime);
            }
            float animStartTime = eventNodes[i].startTime - duration * pivotRatio;
            TimedTrigger timedTrigger = new TimedTrigger();
            timedTriggers.Add(timedTrigger);
            Sys.instance.playEvent += (_, time) => timedTrigger.Trigger(
                time,
                animStartTime,
                () => {
                    animator.SetFloat("Speed", duration / nootAnimDuration);
                    animator.Play("NootNoot");
                }
            );
        }
    }
    private void InitNootAnimaDuration() {
        AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip animationClip in animationClips) {
            if (animationClip.name == "NootNoot") {
                nootAnimDuration = animationClip.length;
                return;
            }
        }
        Debug.LogError("û���ҵ�NootNoot�Ķ���ʱ��");
    }

    private void Appear() {
        Debug.Log("Appear");
        Vector3 oldScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(oldScale, appearTimeAdvance);
        GetComponent<SpriteRenderer>().DOFade(alpha, appearTimeAdvance);
    }

    public void Disappear() {
        Debug.Log("Disappear");
        Vector3 oldScale = transform.localScale;
        transform.DOScale(oldScale * 2f, disappearDuration).onComplete += () => transform.localScale = oldScale;
        GetComponent<SpriteRenderer>().DOFade(0f, disappearDuration);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class EventNodeSection
{
    public EventNode[] Array = new EventNode[2];
    public EventNode this[int index] {
        get {
            return Array[index];
        }
    }
}
