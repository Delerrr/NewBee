using UnityEngine;

// ��Ҫ��̫�࣬���ܻᵼ����������
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
