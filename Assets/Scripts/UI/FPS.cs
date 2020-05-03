using UnityEngine;
using UnityEngine.UI;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          FPS class
 * ------------------------------------------------
 */

public class FPS : MonoBehaviour
{
    private Text _text;
    private int _lastFramerate;

    private const string _FPS_STRING = "{0} FPS";

    private void Awake()
    {
        _text = GetComponent<Text>();
        _lastFramerate = (int) (1f / Time.smoothDeltaTime);
    }
    private void Update()
    {
        int newFramerate = (int) (1f / Time.smoothDeltaTime);

        if (newFramerate > _lastFramerate + 3 || newFramerate < _lastFramerate - 3)
        {
            _lastFramerate = newFramerate;
            _text.text = string.Format(_FPS_STRING, newFramerate);
        }
    }
}
