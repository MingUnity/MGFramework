using MGFramework.UIModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager
{
    [RuntimeInitializeOnLoadMethod]
    public static void Entry()
    {
        UIRegister.RegistAll();

        UIManager.Instance.Enter(ViewId.SampleAView, true);
    }
}
