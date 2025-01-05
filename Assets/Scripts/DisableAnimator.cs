using UnityEngine;

public class DisableAnimator : MonoBehaviour
{
    public void DiableOnlyAnimator() {
        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;
        GetComponent<Animator>().enabled = false;
        transform.position = oldPosition;
        transform.rotation = oldRotation;
    }
    public void DisableGameObject() {
        gameObject.SetActive(false);
        GetComponent<Animator>().enabled = true;
    }

}
