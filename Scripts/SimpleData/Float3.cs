using System;

namespace MGFramework.Data
{
    [Serializable]
    public struct Float3
    {
        private static Float3 _zero = new Float3(0, 0, 0);

        public static Float3 Zero => _zero;

        public float x;

        public float y;

        public float z;

        public Float3(float x, float y, float z)
        {
            this.x = x;

            this.y = y;

            this.z = z;
        }

        public override bool Equals(object val)
        {
            bool res = false;

            if (val is Float3)
            {
                Float3 target = (Float3)val;

                res = x == target.x && y == target.y && z == target.z;
            }

            return res;
        }

        public static bool operator ==(Float3 a, Float3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Float3 a, Float3 b)
        {
            return !a.Equals(b);
        }

        public static Float3 operator +(Float3 a, Float3 b)
        {
            return new Float3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Float3 operator -(Float3 a, Float3 b)
        {
            return new Float3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Float3 x:{x} y:{y} z:{z}]";
        }
    }
}
