using UnityEngine;

public class Flame : MonoBehaviour
{
    const float _ROTATION_SPEED = 50f;

    void Update()
    {
        transform.eulerAngles += Vector3.forward * _ROTATION_SPEED * Time.deltaTime;
    }
}
