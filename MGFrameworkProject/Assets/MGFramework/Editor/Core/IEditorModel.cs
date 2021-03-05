namespace MGFrameworkEditor.Core
{
    /// <summary>
    /// 编辑器视图模型
    /// </summary>
    public interface IEditorModel
    {
        /// <summary>
        /// 视图
        /// </summary>
        IEditorView View { get; set; }

        /// <summary>
        /// 装载数据
        /// </summary>
        void Setup();

        /// <summary>
        /// 卸载数据
        /// </summary>
        void UnSetup();
    }
}
