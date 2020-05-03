using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          Bullet class
 * ------------------------------------------------
 */

public class Bullet : Focuser
{
    private Vector3 _direction;
    private float _speed;
    private float _damages;
    private Focuser _shooter;

    private const float _MAX_DISTANCE_FROM_ORIGIN = 170f;

    private void Start()
    {
        transform.localScale = transform.localScale;
    }

    protected override void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f))
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(_shooter, _damages);
                Destroy(gameObject);
            }
        }

        _direction = transform.forward;
        transform.position += _direction * _speed * Time.deltaTime;
        OnExitScreen();
    }

    /**
     * Setup the bullet
     */
    public void Setup(Vector3 targetPosition, float speed, float damages, Focuser shooter)
    {
        _speed = speed;
        _damages = damages;
        _shooter = shooter;
        _direction = (targetPosition - transform.position).normalized;
    }

    

    /**
     * Remove the bullet when out of the arena
     */
    private void OnExitScreen()
    {
        if (transform.position.x >= _MAX_DISTANCE_FROM_ORIGIN || transform.position.z >= _MAX_DISTANCE_FROM_ORIGIN) Destroy(gameObject);
    }
}
