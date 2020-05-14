using System.Collections.Generic;

namespace MGFramework.UIModule
{
    /// <summary>
    /// 特殊堆栈
    /// 可删除
    /// </summary>
    public class SpecialStack<T>
    {
        /// <summary>
        /// 列表
        /// </summary>
        private List<T> _list = new List<T>();

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            _list.Remove(item);
            _list.Add(item);
        }

        /// <summary>
        /// 出栈
        /// </summary>
        public bool Pop(out T item) 
        {
            bool result = false;

            if (_list.Count > 0)
            {
                item = _list[_list.Count - 1];

                _list.RemoveAt(_list.Count - 1);

                result = true;
            }
            else
            {
                item = default(T);
            }

            return result;
        }

        /// <summary>
        /// 取栈顶
        /// </summary>
        public bool Peek(out T item)
        {
            bool res = false;

            if (_list.Count > 0)
            {
                item = _list[_list.Count - 1];

                res = true;
            }
            else
            {
                item = default(T);
            }

            return res;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete(T item)
        {
            _list.Remove(item);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }
    }
}