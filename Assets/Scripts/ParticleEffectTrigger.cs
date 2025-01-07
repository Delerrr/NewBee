using UnityEngine;

public class ParticleEffectTrigger : EffectTrigger
{
    public override void Trigger() {
        Traverse(transform);
    }

    private void Traverse(Transform transform) {
        ParticleSystem particleSystem = transform.GetComponent<ParticleSystem>();
        if (particleSystem != null) {
            particleSystem.Play();
        }
        foreach (Transform child in transform) {
            Traverse(child);
        }
    }
}
