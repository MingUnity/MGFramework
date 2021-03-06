using MGFramework;
using MGFramework.UIModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIRegister
{
    public static void RegistAll()
    {
        Container.Regist<IView, SampleAView>(ViewId.SampleAView);
        Container.Regist<ISampleAPresenter, SampleAPresenter>();
        Container.Regist<ISampleAModel, SampleAModel>(1);

        Container.Regist<IView, SampleBView>(ViewId.SampleBView);
        Container.Regist<ISampleBPresenter, SampleBPresenter>();
        Container.RegistSingleton<ISampleBModel, SampleBModel>();
        Container.Regist<IBNode, BNode>();

        Container.Regist<IView, EmptyView>(ViewId.EmptyView);
        Container.Regist<IEmptyPresenter, EmptyPresenter>();
    }
}
