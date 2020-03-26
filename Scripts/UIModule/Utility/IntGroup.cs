using MGFramework;
using System;

/// <summary>
/// int组
/// </summary>
public struct IntGroup : IEquatable<IntGroup>
{
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

    public IntGroup(params int[] args)
    {
        if (args == null)
        {
            throw new System.ArgumentNullException("<Ming> ## Uni Exception ## Cls:IntGroup Func:Constructor Info:Args is null");
        }

        _ints = args;
    }

    /// <summary>
    /// 获取
    /// </summary>
    public static IntGroup Get(params int[] args)
    {
        return new IntGroup(args);
    }

    public static bool operator ==(IntGroup groupA, IntGroup groupB)
    {
        return groupA.Equals(groupB);
    }

    public static bool operator !=(IntGroup groupA, IntGroup groupB)
    {
        return !groupA.Equals(groupB);
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