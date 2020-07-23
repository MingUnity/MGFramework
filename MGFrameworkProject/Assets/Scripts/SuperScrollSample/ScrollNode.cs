using MGFramework.UIModule;
using UnityEngine;
using UnityEngine.UI;

public class ScrollNode : SuperScrollNodeBase
{
    private RawImage _img;

    public void Create(Transform root)
    {
        _img = root.GetComponent<RawImage>();
    }

    public override void ResetAsyncData()
    {
        TexKey = null;
    }

    public string TexKey
    {
        set
        {
            if(string.IsNullOrEmpty(value))
            {
                _img.texture = null;
                return;
            }
            
            ResourcesTextureLoader.Load(value, (tex) => _img.texture = tex);
        }
    }
    public bool Active { get => _img.gameObject.activeSelf; set => _img.gameObject.SetActive(value); }

    protected override Transform Root => _img.transform;
}