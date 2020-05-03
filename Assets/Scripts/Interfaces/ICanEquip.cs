/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          ICanEquip interface
 * ------------------------------------------------
 */

public interface ICanEquip<T>
{
    void Equip(T item);
    void Unequip(T item);
}
