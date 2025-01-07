using DG.Tweening;
using UnityEngine;

public class FadeIn : EndGracefully
{
    public float fadeinDuration = 0.5f;
    public float fadeoutDuration = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Vector3 oldScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(oldScale, fadeoutDuration);
    }
    public override void End(GameObject obj) {
        Vector3 oldScale = transform.localScale;
        transform.DOScale(oldScale * 10f, fadeoutDuration).onComplete += () => {
            transform.localScale = oldScale;
            if (obj == null) {
                obj = gameObject;
            }
            obj.SetActive(false);
        };
    }

}
