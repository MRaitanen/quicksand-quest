using UnityEngine;

public class TickSystem : MonoBehaviour
{
    public static float tickTime = 0.2f;

    private float _tickerTime;

    public delegate void TickAction();
    public static event TickAction OnTickAction;

    private void Update()
    {
        _tickerTime += Time.deltaTime;
        if (_tickerTime >= tickTime)
        {
            _tickerTime = 0;
            TickEvent();
        }
    }

    private void TickEvent()
    {
        OnTickAction?.Invoke();
    }
}
