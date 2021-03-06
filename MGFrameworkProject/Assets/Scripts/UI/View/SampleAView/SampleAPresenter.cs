﻿using MGFramework.UIModule;
using MGFramework;
using System;

public class SampleAPresenter : PresenterBase<ISampleAView>, ISampleAPresenter
{
    [AutoBuild("1")]
    private ISampleAModel _aModel;

    [AutoBuild]
    private ISampleBModel _bModel;

    public void OnConfirm()
    {
        int count = 0;

        int.TryParse(_view.Count, out count);

        _bModel = Container.Resolve<ISampleBModel>();
        _bModel.Count = count;

        UIManager.Instance.Quit(ViewId.SampleAView);
        UIManager.Instance.Enter(ViewId.SampleBView, EnterOptions.PushStack | EnterOptions.CombineStackTop);
    }

    public override void OnShowStart()
    {
        _aModel.Request(OnAModelResponse);
    }

    public override void OnHideCompleted()
    {
        _view.Count = string.Empty;
    }

    private void OnAModelResponse(string title)
    {
        _view.Title = title;
    }
}

