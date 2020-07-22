using MGFramework.UIModule;
using UnityEngine;
using UnityEngine.UI;

public class ScrollNode : ISuperScrollNode
{
    private RawImage _img;

    public void Create(Transform root)
    {
        _img = root.GetComponent<RawImage>();
    }

    public string TexKey { set => ResourcesTextureLoader.Load(value, (tex) => _img.texture = tex); }

    public Transform Root => _img.transform;
}
