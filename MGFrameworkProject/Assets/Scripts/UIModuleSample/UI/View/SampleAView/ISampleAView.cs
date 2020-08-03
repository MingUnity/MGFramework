using MGFramework.UIModule;
using MGFramework;
using UnityEngine;

public interface ISampleAView : IView
{
    string Title { get; set; }

    string Count { get; set; }

    Texture2D Tex { get; set; }
}
