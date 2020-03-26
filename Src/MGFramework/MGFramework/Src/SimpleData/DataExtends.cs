using UnityEngine;

namespace MGFramework.Data
{
    public static class DataExtends
    {
        public static Vector2 ToVector2(this Float2 val)
        {
            return new Vector2(val.x, val.y);
        }

        public static Vector3 ToVector3(this Float3 val)
        {
            return new Vector3(val.x, val.y, val.z);
        }

        public static Vector2 ToVector2(this Int2 val)
        {
            return new Vector2(val.x, val.y);
        }

        public static Vector3 ToVector3(this Int3 val)
        {
            return new Vector3(val.x, val.y, val.z);
        }
    }
}
