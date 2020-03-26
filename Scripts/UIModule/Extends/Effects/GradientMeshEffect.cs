using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MGFramework.UIModule
{
    [AddComponentMenu("UI/Effects/Gradient")]
    [RequireComponent(typeof(Graphic))]
    public class GradientMeshEffect : BaseMeshEffect
    {
        /// <summary>
        /// 方向
        /// </summary>
        public Direction direction = Direction.BottomToTop;

        /// <summary>
        /// 顶部颜色
        /// </summary>
        public Color32 topColor = Color.white;

        /// <summary>
        /// 底部颜色
        /// </summary>
        public Color32 bottomColor = Color.black;

        /// <summary>
        /// 顶点缓存列表
        /// </summary>
        private List<UIVertex> _cacheVertexList = new List<UIVertex>();

        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            if (!IsActive())
            {
                return;
            }

            int count = vertexHelper.currentVertCount;

            if (count == 0)
            {
                return;
            }

            _cacheVertexList.Clear();

            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = new UIVertex();

                vertexHelper.PopulateUIVertex(ref vertex, i);

                _cacheVertexList.Add(vertex);
            }

            float topY = _cacheVertexList[0].position.y;

            float bottomY = _cacheVertexList[0].position.y;

            float leftX = _cacheVertexList[0].position.x;

            float rightX = _cacheVertexList[0].position.x;

            for (int i = 1; i < count; i++)
            {
                float y = _cacheVertexList[i].position.y;

                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }

                float x = _cacheVertexList[i].position.x;

                if (x > rightX)
                {
                    rightX = x;
                }
                else if (x < leftX)
                {
                    leftX = x;
                }
            }

            float width = rightX - leftX;

            float height = topY - bottomY;

            switch (direction)
            {
                case Direction.BottomToTop:
                    {
                        for (int i = 0; i < count; i++)
                        {
                            UIVertex vertex = _cacheVertexList[i];

                            Color32 color = Color32.Lerp(bottomColor, topColor, (vertex.position.y - bottomY) / height);

                            vertex.color = color;

                            vertexHelper.SetUIVertex(vertex, i);
                        }
                    }

                    break;

                case Direction.LeftToRight:
                    {
                        for (int i = 0; i < count; i++)
                        {
                            UIVertex vertex = _cacheVertexList[i];

                            Color32 color = Color32.Lerp(bottomColor, topColor, (vertex.position.x - leftX) / width);

                            vertex.color = color;

                            vertexHelper.SetUIVertex(vertex, i);
                        }
                    }


                    break;

                case Direction.RightBottomToLeftTop:
                    {
                        float total = Mathf.Sqrt(width * width + height * height);

                        for (int i = 0; i < count; i++)
                        {
                            UIVertex vertex = _cacheVertexList[i];

                            float dis = Distance(vertex.position.x, vertex.position.y, rightX, bottomY);

                            Color32 color = Color32.Lerp(bottomColor, topColor, dis / total);

                            vertex.color = color;

                            vertexHelper.SetUIVertex(vertex, i);
                        }
                    }


                    break;

                case Direction.LeftBottomToRightTop:
                    {
                        float total = Mathf.Sqrt(width * width + height * height);

                        for (int i = 0; i < count; i++)
                        {
                            UIVertex vertex = _cacheVertexList[i];

                            float dis = Distance(vertex.position.x, vertex.position.y, leftX, bottomY);

                            Color32 color = Color32.Lerp(bottomColor, topColor, dis / total);

                            vertex.color = color;

                            vertexHelper.SetUIVertex(vertex, i);
                        }
                    }


                    break;
            }
        }

        /// <summary>
        /// 应用
        /// </summary>
        public void Apply()
        {
            OnDidApplyAnimationProperties();
        }

        /// <summary>
        /// 求距离
        /// </summary>
        private float Distance(float x1, float y1, float x2, float y2)
        {
            return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        /// <summary>
        /// 方向
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// 底->顶
            /// </summary>
            BottomToTop = 0,

            /// <summary>
            /// 左->右
            /// </summary>
            LeftToRight,

            /// <summary>
            /// 右下->左上
            /// </summary>
            RightBottomToLeftTop,

            /// <summary>
            /// 左下->右上
            /// </summary>
            LeftBottomToRightTop
        }
    }
}