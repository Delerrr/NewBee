using UnityEngine;

public class Player : MonoBehaviour
{
    public EventNodesParent eventNodesParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventNodesParent.playEvent += (pos, _) => transform.position = pos; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
