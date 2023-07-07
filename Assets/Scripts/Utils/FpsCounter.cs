using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    public Text _fpsText;
    public float _hudRefreshRate = 1f;

    public int fps;

    private float _timer;

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            fps = (int)(1f / Time.unscaledDeltaTime);
            if(_fpsText != null)
                _fpsText.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}