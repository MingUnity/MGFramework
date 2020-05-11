using MGFramework.UIModule;
using MGFramework;
using System;

public class SampleAPresenter : PresenterBase<ISampleAView>, ISampleAPresenter
{
    private ISampleAModel _aModel;

    public SampleAPresenter()
    {
        _aModel = Container.Resolve<ISampleAModel>();
    }

    public void OnConfirm()
    {
        int count = 0;

        int.TryParse(_view.Count, out count);

        ISampleBModel bModel = Container.Resolve<ISampleBModel>();
        bModel.Count = count;

        UIManager.Instance.Quit(ViewId.SampleAView);
        UIManager.Instance.Enter(ViewId.SampleBView,true);
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

