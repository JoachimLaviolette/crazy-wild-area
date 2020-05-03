/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          ICanModify interface
 * ------------------------------------------------
 */

public interface ICanModify
{
    void SetSpeedModifier(float speedModifier, float speedModifierPeriod);
    void SetArmorModifier(float armorModifier);
    void SetHealthModifier(float healthModifier);
}
