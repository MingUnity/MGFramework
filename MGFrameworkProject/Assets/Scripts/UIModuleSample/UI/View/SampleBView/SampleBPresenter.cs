using MGFramework.UIModule;
using MGFramework;
using System;

public class SampleBPresenter : PresenterBase<ISampleBView>, ISampleBPresenter
{
    [AutoBuild]
    private ISampleBModel _model;

    public void OnBack()
    {
        UIManager.Instance.Pop();
        //UIManager.Instance.Quit(ViewId.SampleBView,true);
        //UIManager.Instance.Enter(ViewId.SampleAView);
    }

    public void OnClear()
    {
        _view.Datas = null;
    }

    public void OnUpdate()
    {
        _model.Request(OnModelResponse);
    }

    public void OnItem(int index)
    {
        Item item = _model[index];

        BNodeData data = _view.Datas?.GetValueAnyway(index);

        if (item != null && data != null)
        {
            data.name = item.nickName;

            _view.RefreshNode(index, data);
        }
    }

    public override void OnShowStart()
    {
        _model.Request(OnModelResponse);
    }

    public override void OnHideCompleted()
    {
        _view.Datas = null;
    }

    private void OnModelResponse(Item[] items)
    {
        if (items == null)
        {
            return;
        }

        _view.Title = $"数量：{items.Length}";

        BNodeData[] datas = new BNodeData[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            datas[i] = new BNodeData()
            {
                name = items[i].index.ToString()
            };
        }

        _view.Datas = datas;
    }
}
