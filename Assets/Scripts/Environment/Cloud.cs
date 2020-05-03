using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Cloud class
 * ------------------------------------------------
 */

public class Cloud : EnvironmentComponent
{
    private const float _MIN_SPEED = 3f;
    private const float _MAX_SPEED = 5f;
    private const float _MIN_SCALE = .1f;
    private const float _MAX_SCALE = 2.5f;
    private const float _MIN_HEIGHT = 20f;

    private void Start()
    {
        _moveSpeed = Random.Range(_MIN_SPEED, _MAX_SPEED);
        _maxDistanceFromOrigin = 170f;

        float scale = Random.Range(_MIN_SCALE, _MAX_SCALE);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = new Vector3(transform.position.x, scale * 3 + _MIN_HEIGHT, transform.position.z);
    }
}
