using UnityEngine;

public class EndAnimGracefully : EndGracefully
{
    public override void End() {
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("End");
    }
}
