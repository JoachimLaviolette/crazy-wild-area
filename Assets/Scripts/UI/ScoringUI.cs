using UnityEngine;
using UnityEngine.UI;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          ScoringUI class
 * ------------------------------------------------
 */

public class ScoringUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _scoringGO;
    [SerializeField]
    private Text _killText;
    [SerializeField]
    private Text _playerCountText;
    private Entity _focusedEntity;

    private const string _KILLS_STRING = "Kills: {0}";
    private const string _PLAYERS_STRING = "Players: {0}";

    private void Awake()
    {
        EntityManager._onPlayerCountChanged += UpdateUI;
    }

    private void Update()
    {
        HandleInput();
    }

    /**
     * Handle user input to update UI state
     */
    private void HandleInput()
    {
        _scoringGO.SetActive(EntityManager.GetFocusedEntity() != null);
    }

    /**
     * Delegate called to update scoring UI
     */
    private void UpdateUI(object sender, Entity.OnScoringChangedEventArgs args)
    {
        _killText.text = string.Format(_KILLS_STRING, args.killCount);
    }

    /**
     * Delegate called to update scoring UI
     */
    private void UpdateUI(object sender, EntityManager.OnPlayerCountChangedEventArgs args)
    {
        _playerCountText.text = string.Format(_PLAYERS_STRING, args.playerCount);
    }

    /**
     * Add the scoring UI as subscriber to the given entity
     */
    public void Subscribe(Entity entity)
    {
        entity._onScoringChanged += UpdateUI;
        _focusedEntity = entity;
    }

    /**
     * Remove the scoring UI from subscribers of the given entity
     */
    public void Unsubscribe(Entity entity)
    {
        entity._onScoringChanged -= UpdateUI;
        if (_focusedEntity == entity) _focusedEntity = null;
    }
}
