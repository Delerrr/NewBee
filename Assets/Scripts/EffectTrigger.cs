using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    public void Trigger() {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null) {
            particleSystem.Play();
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
