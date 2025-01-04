using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sys.instance.playEvent += (pos) => transform.position = pos; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
