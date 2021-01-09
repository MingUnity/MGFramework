using MGFramework.UIModule;
using UnityEngine.UI;
using MGFramework;
using System.Collections.Generic;
using UnityEngine;
using MGFramework.ResourceModule;

[ParentInfo(type = FindType.FindWithTag, param = "UILayer_02")]
[ResInfo(abPath = "AssetBundle/b.ab", assetName = "SampleB")]
public class SampleBView : ViewBase<ISampleBPresenter>, ISampleBView
{
    private Text _txtTitle;
    private IocObjectPool<IBNode> _pool;
    private List<IBNode> _nodes = new List<IBNode>();
    private BNodeData[] _cache;

    public string Title { get => _txtTitle.text; set => _txtTitle.text = value; }
    public BNodeData[] Datas
    {
        get
        {
            return _cache;
        }
        set
        {
            _nodes.ForEach(item => _pool.Remove(item));
            _nodes.Clear();

            _cache = value;

            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    int index = i;

                    IBNode node = CreateNode(value[i]);
                    node.OnClick = () => _presenter.OnItem(index);

                    _nodes.Add(node);
                }
            }
        }
    }

    public void RefreshNode(int index, BNodeData data)
    {
        ParseNodeData(_nodes.GetValueAnyway(index), data);
    }

    protected override void OnCreate()
    {
        _txtTitle = _root.Find<Text>("TxtTitle");

        Transform template = _root.Find("PnlItem/Template");
        _pool = new IocObjectPool<IBNode>(template);

        _root.Find<Button>("BtnUpdate").AddClickListener(_presenter.OnUpdate);
        _root.Find<Button>("BtnClear").AddClickListener(_presenter.OnClear);
        _root.Find<Button>("BtnBack").AddClickListener(_presenter.OnBack);
    }

    private IBNode CreateNode(BNodeData data)
    {
        IBNode node = _pool.Get();

        ParseNodeData(node, data);

        return node;
    }

    private void ParseNodeData(IBNode node, BNodeData data)
    {
        if (data == null || node == null)
        {
            return;
        }

        node.Name = data.name;
    }
}