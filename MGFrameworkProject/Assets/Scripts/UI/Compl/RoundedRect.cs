using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 圆角矩形
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Graphic))]
public class RoundedRect : MonoBehaviour
{
    /// <summary>
    /// 当前gameobject图形组件
    /// </summary>
    private Graphic _graphic;

    /// <summary>
    /// 材质
    /// </summary>
    [SerializeField]
    private Material _material;

    /// <summary>
    /// 圆角像素值
    /// </summary>
    [SerializeField]
    private float _roundedPixel = 8;

    /// <summary>
    /// 帧末尾等待
    /// </summary>
    private static WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private void Awake()
    {
        _graphic = this.GetComponent<Graphic>();
        _graphic.RegisterDirtyLayoutCallback(SetRounded);

        SetMaterial();
        SetRounded();
    }

    private IEnumerator Start()
    {
        yield return _waitForEndOfFrame;

        SetRounded();
    }

    private void OnValidate()
    {
        SetMaterial();
        SetRounded();
    }

    private void OnDestroy()
    {
        if (_graphic != null)
        {
            _graphic.UnregisterDirtyLayoutCallback(SetRounded);
        }
    }

    /// <summary>
    /// 设置材质
    /// </summary>
    private void SetMaterial()
    {
        if (_graphic == null)
        {
            return;
        }

        if (_material == null)
        {
            _material = Resources.Load<Material>("Materials/UIRoundRect");
            _material = Material.Instantiate<Material>(_material);
            _material.name = string.Format("UIRoundRect(Clone)", name);
        }

        if (_material != null)
        {
            if (_material.shader != null)
            {
                _material.renderQueue = _material.shader.renderQueue;
            }

            _graphic.material = _material;
        }
        else
        {
            Debug.LogError("<Ming> ## Uni Error ## Cls:RoundedRect Func:SetMaterial Info:Resources/Materials/UIRoundRect not exists");
        }
    }

    /// <summary>
    /// 设置圆角矩形shader数据
    /// </summary>
    private void SetRounded()
    {
        if (_material == null || _graphic == null)
        {
            return;
        }

        float width = _graphic.rectTransform.rect.width;
        float height = _graphic.rectTransform.rect.height;

        if (width != 0 && height != 0)
        {
            _material.SetFloat("_Width", width);
            _material.SetFloat("_Height", height);
        }

        _material.SetFloat("_RoundedRadius", _roundedPixel);
    }
}