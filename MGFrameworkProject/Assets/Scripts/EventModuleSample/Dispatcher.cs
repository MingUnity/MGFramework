using MGFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    private float index = 0;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 1)
        {
            _timer = 0;

            EventManager.Instance.Dispatch<Vector3>(EventId.POSITION, new Vector3(index, index, index));
            index += 0.2f;
        }
    }
}

public static class EventId
{
    public const int POSITION = 0;
}
