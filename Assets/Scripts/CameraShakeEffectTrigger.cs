using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeEffectTrigger : GeneralTrigger
{
    public enum NoiseType { Shake }

    public CinemachineBasicMultiChannelPerlin settings;
    private NoiseType noiseType;

    protected override void Start() {
        base.Start();
        // TODO: noiseType
/*        switch (this.noiseType) {
            case NoiseType.Shake:
                settings.NoiseProfile = CinemachineBasicMultiChannelPerlin.Presets.sixdShake;
                break;
        }
*/    }

    protected override void StartEffect() {
        settings.enabled = true;
    }


    protected override void EndEffect() {
        settings.enabled = false;
    }

}
