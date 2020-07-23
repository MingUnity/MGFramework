using MGFramework.UIModule;
using UnityEngine;
using UnityEngine.UI;

public class ScrollNode : ISuperScrollNode
{
    private RawImage _img;
    
    public string TexKey
    {
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _img.texture = null;
                return;
            }

            ResourcesTextureLoader.Load(value, (tex) => _img.texture = tex);
        }
    }

    public bool Active { get => _img.gameObject.activeSelf; set => _img.gameObject.SetActive(value); }

    public void Create(Transform root)
    {
        _img = root.GetComponent<RawImage>();
    }

    public void TurnLast()
    {
        _img.transform.SetAsLastSibling();
    }

    public void TurnFirst()
    {
        _img.transform.SetAsFirstSibling();
    }

    public void ResetAsyncData()
    {
        TexKey = null;
    }
}