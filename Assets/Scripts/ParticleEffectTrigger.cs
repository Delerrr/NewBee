using UnityEngine;

public class ParticleEffectTrigger : EffectTrigger
{
    public override void Trigger() {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null) {
            particleSystem.Play();
        }
    }
}
