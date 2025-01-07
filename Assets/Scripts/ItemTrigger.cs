using UnityEngine;

public class ItemTrigger : EffectTrigger {
    public GameObject item;
    protected override void Start() {
        base.Start();
        if (item == null) {
            item = transform.GetChild(0).gameObject;
        }
        if (item == null) {
            Debug.LogError("初始化失败！Item不能为null");
            return;
        }
        Sys.instance.restartEvent += () => item.SetActive(true);
    }
    public override void Trigger() {
        item.SetActive(false);
    }

}
