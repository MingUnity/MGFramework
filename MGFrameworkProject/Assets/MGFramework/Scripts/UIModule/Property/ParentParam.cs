namespace MGFramework.UIModule
{
    /// <summary>
    /// 父节点参数
    /// </summary>
    public class ParentParam
    {
        private static ParentParam _default = new ParentParam(FindType.FindWithName, "Canvas");

        /// <summary>
        /// 搜索类型
        /// </summary>
        public FindType findType;

        /// <summary>
        /// 参数
        /// </summary>
        public string param;

        /// <summary>
        /// 默认值
        /// </summary>
        public static ParentParam Default => _default;

        public ParentParam(FindType findType,string param)
        {
            this.findType = findType;
            this.param = param;
        }
    }
}
