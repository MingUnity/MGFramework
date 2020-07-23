using MGFramework;
using System;
using System.Collections.Generic;

/// <summary>
/// int组
/// </summary>
public struct IntGroup : IEquatable<IntGroup>
{
    /// <summary>
    /// 临时数学计算使用列表
    /// </summary>
    private static readonly List<int> _tempMathList = new List<int>();

    /// <summary>
    /// 整形数组
    /// </summary>
    private int[] _ints;

    /// <summary>
    /// 获取某索引对应的值
    /// </summary>
    public int this[int index]
    {
        get
        {
            int res = -1;

            if (_ints == null)
            {
                throw new System.Exception("<Ming> ## Uni Exception ## Cls:IntGroup Func:this Info:Null array");
            }

            if (!_ints.TryGetValue(index, out res))
            {
                throw new System.InvalidOperationException("<Ming> ## Uni Exception ## Cls:IntGroup Func:this Info:Invalid index");
            }

            return res;
        }
    }

    private static IntGroup _empty = Get(new int[0]);

    /// <summary>
    /// 空组
    /// </summary>
    public static IntGroup Empty => _empty;

    /// <summary>
    /// 数量
    /// </summary>
    public int Count
    {
        get
        {
            int count = 0;

            if (_ints != null)
            {
                count = _ints.Length;
            }

            return count;
        }
    }

    private IntGroup(params int[] args)
    {
        if (args == null)
        {
            throw new System.ArgumentNullException("<Ming> ## Uni Exception ## Cls:IntGroup Func:Constructor Info:Args is null");
        }

        _ints = args;
    }

    /// <summary>
    /// 包含
    /// </summary>
    public bool Contains(int intVal)
    {
        bool result = false;

        if (_ints != null)
        {
            for (int i = 0; i < _ints.Length; i++)
            {
                if (_ints[i] == intVal)
                {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 获取
    /// </summary>
    public static IntGroup Get(params int[] args)
    {
        return new IntGroup(args);
    }

    /// <summary>
    /// 合并
    /// </summary>
    public static IntGroup Combine(params IntGroup[] args)
    {
        _tempMathList.Clear();

        IntGroup result = IntGroup.Empty;
        
        if (args != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                IntGroup group = args[i];

                for (int j = 0; j < group.Count; j++)
                {
                    _tempMathList.Add(group[j]);
                }
            }

            result = IntGroup.Get(_tempMathList.ToArray());
        }

        return result;
    }

    public static bool operator ==(IntGroup groupA, IntGroup groupB)
    {
        return groupA.Equals(groupB);
    }

    public static bool operator !=(IntGroup groupA, IntGroup groupB)
    {
        return !groupA.Equals(groupB);
    }

    public static IntGroup operator -(IntGroup groupA, IntGroup groupB)
    {
        _tempMathList.Clear();

        int[] intsA = groupA._ints;
        
        for (int i = 0; i < intsA.Length; i++)
        {
            int aInt = intsA[i];
            
            if (!groupB.Contains(aInt))
            {
                _tempMathList.Add(aInt);
            }
        }

        return IntGroup.Get(_tempMathList.ToArray());
    }

    public override bool Equals(object obj)
    {
        return this.Equals((IntGroup)obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public bool Equals(IntGroup other)
    {
        if (this._ints != null && other._ints != null && this._ints.Length == other._ints.Length)
        {
            int length = this._ints.Length;

            for (int i = 0; i < length; i++)
            {
                if (this._ints[i] != other._ints[i])
                {
                    return false;
                }
            }

            return true;
        }
        else if (this._ints == null && this._ints == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}