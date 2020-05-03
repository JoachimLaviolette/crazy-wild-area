using System.Collections;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          IDamageable interface
 * ------------------------------------------------
 */

public interface IDamageable
{
    /**
     * Damage the damageable
     */
    void Damage(Focuser source, float damages);    

    /**
     * Play damage animation
     */
    IEnumerator PlayDamageAnimation();
}
