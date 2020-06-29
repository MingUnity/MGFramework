using System;
using MGFramework;
using MGFramework.UIModule;
using UnityEngine.UI;

public class BNode : BasicPoolObject, IBNode
{
    private Text _txtName;
    private Button _btnRoot;
    private Action _onClick;

    public string Name { get => _txtName.text; set => _txtName.text = value; }
    public Action OnClick { set => _onClick = value; }

    public override void Reset()
    {
        _txtName.text = string.Empty;
        _onClick = null;
    }

    protected override void OnCreate()
    {
        _txtName = _root.Find<Text>("Text");
        _btnRoot = _root.GetComponent<Button>().AddClickListener(() => _onClick.Invoke());
    }
}