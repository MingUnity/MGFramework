using MGFramework.UIModule;
using MGFramework;

public class EmptyPresenter : PresenterBase<IEmptyView>, IEmptyPresenter
{
    public void OnBack()
    {
        UIManager.Instance.Pop();
    }
}

