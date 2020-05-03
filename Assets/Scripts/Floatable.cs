using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Floatable class
 * ------------------------------------------------
 */

public class Floatable : MonoBehaviour
{
    private float _defaultY;
    private float _floatSpeed = .5f;
    private Vector3 _lastDir;

    private const float _MIN_HEIGHT = 0f;
    private const float _MAX_HEIGHT = .5f;

    private void Start()
    {
        _defaultY = transform.position.y;
        transform.position = new Vector3(transform.position.x, _defaultY + _MIN_HEIGHT, transform.position.z);
        _lastDir = Vector3.up;
    }

    private void Update()
    {
        Vector3 dir = transform.position.y > _defaultY + _MAX_HEIGHT ? Vector3.down : transform.position.y < _defaultY + _MIN_HEIGHT ? Vector3.up : _lastDir;
        transform.position += dir * Time.deltaTime * _floatSpeed;
        _lastDir = dir;
    }
}
