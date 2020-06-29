using MGFramework.UIModule;
using UnityEngine;

public class DefaultInputKeyHandler : MonoBehaviour, IInputKeyHandler
{
    public bool TriggerDown => Input.GetMouseButtonDown(0);

    public bool Trigger => Input.GetMouseButton(0);

    public bool TriggerUp => Input.GetMouseButtonUp(0);
    
    private void OnEnable()
    {
        InputManager.Add(this);
    }

    private void OnDisable()
    {
        InputManager.Remove(this);
    }

    private void OnDestroy()
    {
        InputManager.Remove(this);
    }
}
