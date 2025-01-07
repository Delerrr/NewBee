using DG.Tweening;
using UnityEngine;

public class EndByScaling : EndGracefully
{
    public bool scaleX = false;
    public bool scaleY = false;
    public bool scaleZ = false;
    public float duration = 1f;
    public Transform target;
    public override void End(GameObject obj) {
        if (target == null) {
            Debug.LogWarning("缩放对象不存在，使用Trigger的对象");
            target = obj.transform;
        }
        Vector3 oldScale = target.localScale;
        Vector3 scaleRes = target.localScale;
        scaleRes.x *= scaleX ? 0 : 1;
        scaleRes.y *= scaleY ? 0 : 1;
        scaleRes.z *= scaleZ ? 0 : 1;
        target.DOScale(scaleRes, duration).onComplete += () => {
            target.localScale = oldScale;
            target.gameObject.SetActive(false);
        };
    }
}
