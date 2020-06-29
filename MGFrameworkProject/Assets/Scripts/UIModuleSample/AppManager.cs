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
        UIManager.Instance.Enter(ViewId.SampleAView, true);
    }
}