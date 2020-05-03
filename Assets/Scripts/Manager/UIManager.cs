using UnityEngine;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          UIManager class
 * ------------------------------------------------
 */

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    private static UIManager _instance;

    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private InventoryUI _inventoryUI;
    [SerializeField]
    private ScoringUI _scoringUI;

    private void Awake()
    {
        _instance = this;
    }

    private UIManager() { }

    /**
     * Return the inventory UI
     */
    public static InventoryUI GetInventoryUI()
    {
        return _instance._inventoryUI;
    }

    /**
     * Return the scoring UI
     */
    public static ScoringUI GetScoringUI()
    {
        return _instance._scoringUI;
    }
}
