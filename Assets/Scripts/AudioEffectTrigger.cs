using UnityEngine;

public class AudioEffectTrigger : EffectTrigger {
    public AudioManager.EffectType effectType; 
    public override void End() {
        AudioManager.instance.SwitchEffect(AudioManager.EffectType.Normal);
    }

    public override void Trigger() {
        AudioManager.instance.SwitchEffect(effectType);
    }
}
