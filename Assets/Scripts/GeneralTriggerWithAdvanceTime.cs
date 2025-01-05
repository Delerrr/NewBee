using UnityEngine;

// 不要用太多，可能会导致性能问题
public class GeneralTriggerWithAdvanceTime : GeneralTrigger
{
    public GameObject obj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Trigger(float time) {
    }

    private void TriggerSuccessfully() {
        obj.SetActive(true);
    }
}
