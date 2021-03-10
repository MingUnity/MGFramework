using MGFramework.UIModule;
using UnityEngine.UI;
using MGFramework;
using UnityEngine;

[ParentInfo(type = FindType.FindWithTag,param ="UILayer_01")]
public class SampleAView : ViewBase<ISampleAPresenter>, ISampleAView
{
    private Text _txtTitle;
    private Button _btnConfirm;
    private InputField _inputCount;
    private RawImage _imgTest;
    
    public string Title { get => _txtTitle.text; set => _txtTitle.text = value; }
    public string Count { get => _inputCount.text; set => _inputCount.text = value; }
    public Texture2D Tex { get => _imgTest.texture as Texture2D; set => _imgTest.texture = value; }

    protected override void OnCreate()
    {
        _txtTitle = _root.Find<Text>("TxtTitle");
        _inputCount = _root.Find<InputField>("InputCount");
        _btnConfirm = _root.Find<Button>("BtnConfirm").AddClickListener(_presenter.OnConfirm);
        _imgTest = _root.Find<RawImage>("ImgTest");
    }
}