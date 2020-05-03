using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          EnvironmentComponent class
 * ------------------------------------------------
 */

public abstract class EnvironmentComponent : MonoBehaviour
{
    protected float _moveSpeed;
    protected float _maxDistanceFromOrigin;

    protected void Update()
    {
        Translate();
        OnExitScreen();
    }

    /**
     * Translate the component position over time
     */
    protected void Translate()
    {
        transform.position += Vector3.right * Time.deltaTime * _moveSpeed;
        OnExitScreen();
    }

    /**
     * Remove the component when too far from the arena
     */
    protected void OnExitScreen()
    {
        if (transform.position.x >= _maxDistanceFromOrigin)
        {
            EnvironmentManager.RemoveComponent(this);
            Destroy(gameObject);
        }
    }
}
