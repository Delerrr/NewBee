using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum EffectType { Normal, UnderWater }
    public static AudioManager instance => _instance;
    private static AudioManager _instance;
    private AudioSource audioSource;
    public AudioMixerGroup normalGroup;
    public AudioMixerGroup underWaterGroup;
    private void Awake() {
        if (_instance != null) {
            Debug.LogError(this.GetType() + ":单例模式不能出现多个实例");
            return;
        }
        _instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public AudioSource GetAudioSource() {
        return audioSource;
    }
    
    public void SwitchEffect(EffectType effectType) {
        switch (effectType) {
            case EffectType.Normal:
                audioSource.outputAudioMixerGroup = normalGroup;
                break;
            case EffectType.UnderWater:
                audioSource.outputAudioMixerGroup = underWaterGroup;
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
