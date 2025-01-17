using UnityEngine;

public class UI_Billboard : MonoBehaviour
{
    public Transform cam;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);

        //Better way?
        //[Reference to UI health bar gameobject].transform.position = Camera.main.WorldToScreenPoint([Reference to target].transform.position + posOffset);
        // posOffset is public Vector3 posOffset;
    }
}
