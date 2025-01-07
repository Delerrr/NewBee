using UnityEngine;

public class EndAnimGracefully : EndGracefully
{
    public override void End(GameObject _) {
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("End");
    }
}
