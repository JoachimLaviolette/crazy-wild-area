using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Bird class
 * ------------------------------------------------
 */

public class Bird : EnvironmentComponent
{
    private const float _MIN_MOVE_SPEED = 5f;
    private const float _MAX_MOVE_SPEED = 40f;
    private const float _MIN_HEIGHT = 20f;
    private const float _MAX_HEIGHT = 25f;

    private void Start()
    {
        _moveSpeed = Random.Range(_MIN_MOVE_SPEED, _MAX_MOVE_SPEED);
        _maxDistanceFromOrigin = 170f;

        Vector3 position = transform.position;
        position.y = Random.Range(_MIN_HEIGHT, _MAX_HEIGHT);
        transform.position = position;
    }
}
