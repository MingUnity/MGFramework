using MGFramework;
using System;

public interface IBNode : IPoolObject
{
    string Name { get; set; }

    Action OnClick { set; }
}