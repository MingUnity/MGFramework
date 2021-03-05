namespace MGFramework.ArgSystem
{
    /// <summary>
    /// 数据获取抽象接口
    /// </summary>
    public interface IArgPicker<T>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        T Get();
    }

    /// <summary>
    /// 数据获取抽象接口
    /// </summary>
    /// <typeparam name="Param">参数类型</typeparam>
    /// <typeparam name="Result">返回结果类型</typeparam>
    public interface IArgPicker<Param, Result>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        Result Get(Param param);
    }

    /// <summary>
    /// 数据获取抽象接口
    /// </summary>
    /// <typeparam name="ParamA">参数1类型</typeparam>
    /// <typeparam name="ParamB">参数2类型</typeparam>
    /// <typeparam name="Result">返回结果类型</typeparam>
    public interface IArgPicker<ParamA, ParamB, Result>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        Result Get(ParamA paramA, ParamB paramB);
    }
}