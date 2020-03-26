using MGFrameworkEditor.Core;
using System;

namespace MGFrameworkEditor.UIModule
{
    /// <summary>
    /// 创建MVP模型
    /// </summary>
    public class CreateMVPModel : IEditorModel
    {
        private IEditorView _view;

        private string _keyword;

        public IEditorView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;

                if (_view is CreateMVPView)
                {
                    CreateMVPView view = _view as CreateMVPView;

                    view.OnCreateEvent += OnCreate;
                }
            }
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword
        {
            get
            {
                CreateMVPView view = View as CreateMVPView;

                if (view != null)
                {
                    _keyword = view.Keyword;
                }

                return _keyword;
            }
            set
            {
                _keyword = value;

                CreateMVPView view = View as CreateMVPView;

                if(view!=null)
                {
                    view.Keyword = value;
                }
            }
        }

        /// <summary>
        /// 创建事件
        /// </summary>
        public event Action<string> OnCreateEvent;

        public void Setup()
        {
            Keyword = _keyword;
        }

        public void UnSetup()
        {
            _keyword = Keyword;
        }

        private void OnCreate()
        {
            OnCreateEvent?.Invoke(Keyword);
        }
    }
}