using UnityEngine;

public class ItemTrigger : EffectTrigger {
    public GameObject item;
    protected override void Start() {
        base.Start();
        if (item == null) {
            item = transform.GetChild(0).gameObject;
        }
        if (item == null) {
            Debug.LogError("��ʼ��ʧ�ܣ�Item����Ϊnull");
            return;
        }
        Sys.instance.restartAction += () => item.SetActive(true);
    }
    public override void Trigger() {
        item.SetActive(false);
    }

}
