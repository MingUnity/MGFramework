using MGFramework.UIModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private void Awake()
    {
        UIRegister.RegistAll();
    }

    private void Start()
    {
        UIManager.Instance.OnViewEnterStartEvent += Instance_OnViewEnterStartEvent;
        UIManager.Instance.OnViewEnterCompletedEvent += Instance_OnViewEnterCompletedEvent;
        UIManager.Instance.OnViewQuitStartEvent += Instance_OnViewQuitStartEvent;
        UIManager.Instance.OnViewQuitCompletedEvent += Instance_OnViewQuitCompletedEvent;

        UIManager.Instance.Enter(ViewId.SampleAView);
    }

    private void Instance_OnViewEnterStartEvent(int viewId)
    {
        Debug.Log("Enter Start " + viewId);
    }

    private void Instance_OnViewQuitCompletedEvent(int viewId)
    {
        Debug.Log("Quit Completed " + viewId);
    }

    private void Instance_OnViewQuitStartEvent(int viewId)
    {
        Debug.Log("Quit Start " + viewId);
    }

    private void Instance_OnViewEnterCompletedEvent(int viewId)
    {
        Debug.Log("Enter Completed " + viewId);
    }
}