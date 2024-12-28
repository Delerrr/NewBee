using DG.Tweening;
using UnityEngine;

public class EventNode : MonoBehaviour
{
    public Sys.MoveType moveType = Sys.MoveType.Straight;
    [Header("Jump")]
    public float jumpPower = 1f;
    private Tween tween;

    public void SetTween(Tween tween) {
        this.tween = tween;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
