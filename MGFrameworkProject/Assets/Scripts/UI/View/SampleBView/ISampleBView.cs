using MGFramework.UIModule;
using MGFramework;

public interface ISampleBView : IView
{
    BNodeData[] Datas { get; set; }

    string Title { get; set; }

    //void RefreshNode(int index, BNodeData data);
}
