using UnityEngine;

namespace MGFramework
{
    /// <summary>
    /// Transform 扩展
    /// </summary>
    public static class TransformExtends
    {
        /// <summary>
        /// 根据路径寻找
        /// </summary>
        public static T Find<T>(this Transform trans, string path) where T : Component
        {
            T t = null;

            Transform target = trans?.Find(path);

            t = target?.GetComponent<T>();

            return t;
        }

        /// <summary>
        /// 包含某元素
        /// 只要有一个角在容器内即返回true
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="trans">检测对象</param>
        public static bool Contains(this RectTransform container, RectTransform trans)
        {
            if (trans == null)
            {
                return false;
            }

            Vector3[] containerCorners = new Vector3[4];
            container.GetWorldCorners(containerCorners);
            float width = Mathf.Abs(containerCorners[2].x - containerCorners[0].x);
            float height = Mathf.Abs(containerCorners[2].y - containerCorners[0].y);
            Rect rect = new Rect(containerCorners[0].x, containerCorners[0].y, width, height);

            Vector3[] rtCorners = new Vector3[4];
            trans.GetWorldCorners(rtCorners);

            bool res = false;

            for (int i = 0; i < rtCorners.Length; i++)
            {
                Vector3 corner = rtCorners[i];

                if (rect.Contains(corner))
                {
                    res = true;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 全部包含某元素
        /// 被检测对象全部包含在容器中
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="trans">检测对象</param>
        public static bool TotalContains(this RectTransform container, RectTransform trans)
        {
            if (trans == null)
            {
                return false;
            }

            Vector3[] containerCorners = new Vector3[4];
            container.GetWorldCorners(containerCorners);
            float width = Mathf.Abs(containerCorners[2].x - containerCorners[0].x);
            float height = Mathf.Abs(containerCorners[2].y - containerCorners[0].y);
            Rect rect = new Rect(containerCorners[0].x, containerCorners[0].y, width, height);

            Vector3[] rtCorners = new Vector3[4];
            trans.GetWorldCorners(rtCorners);

            bool res = true;

            for (int i = 0; i < rtCorners.Length; i++)
            {
                Vector3 corner = rtCorners[i];

                if (!rect.Contains(corner))
                {
                    res = false;
                    break;
                }
            }

            return res;
        }

    }
}