using MGFramework.UIModule;
using UnityEngine.UI;
using MGFramework;

[ParentInfo(type = FindType.FindWithTag, param = "UILayer_02")]
public class EmptyView : ViewBase<IEmptyPresenter>, IEmptyView
{
    protected override void OnCreate()
    {
        _root.GetComponent<Button>().AddClickListener(_presenter.OnBack);
    }
}