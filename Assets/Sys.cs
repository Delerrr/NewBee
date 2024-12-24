using DG.Tweening;
using UnityEngine;

public class Sys : MonoBehaviour
{
    public Transform player;
    private Sequence sq;
    public float del = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();   
    }

    private void Play()
    {
        sq.Restart();
    }
    private void Init()
    {
        sq = DOTween.Sequence();
        sq.SetAutoKill(false);
        sq.Pause(); 
        foreach(Transform child in transform)
        {
             sq.Append(player.transform.DOJump(child.transform.position, 1, 1, del)); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Play();
        }
    }
}
