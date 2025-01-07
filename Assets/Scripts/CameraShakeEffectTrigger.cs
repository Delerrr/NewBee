using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeEffectTrigger : GeneralTrigger
{
    public enum NoiseType { Shake }

    public CinemachineBasicMultiChannelPerlin settings;
    private NoiseType noiseType;
    public float power = 0.05f;

    protected override void Start() {
        if (endNode == null) {
            endNode = transform.parent.GetComponent<EventNode>();
        }
        base.Start();
        if (settings == null) {
            settings = Sys.instance.cinemachieCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
        // TODO: noiseType
/*        switch (this.noiseType) {
            case NoiseType.Shake:
                settings.NoiseProfile = CinemachineBasicMultiChannelPerlin.Presets.sixdShake;
                break;
        }
*/    }

    protected override void StartEffect() {
        settings.AmplitudeGain = power;
        settings.enabled = true;
    }


    protected override void EndEffect() {
        settings.enabled = false;
    }

}
