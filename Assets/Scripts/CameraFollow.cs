using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(-10f, 10f, -10f); 
    public Vector3 fixedEulerAngles = new Vector3(30f, 45f, 0f); 

    void LateUpdate()
    {
        if (target != null)
        {
            
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Euler(fixedEulerAngles);
        }
    }
}
