using System;

namespace MGFramework
{
    [Serializable]
    public struct Float2
    {
        private static Float2 _zero = new Float2(0, 0);

        public static Float2 Zero => _zero;

        public float x;

        public float y;

        public Float2(float x, float y)
        {
            this.x = x;

            this.y = y;
        }

        public override bool Equals(object val)
        {
            bool res = false;

            if (val is Float2)
            {
                Float2 target = (Float2)val;

                res = x == target.x && y == target.y;
            }

            return res;
        }

        public static bool operator ==(Float2 a, Float2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Float2 a, Float2 b)
        {
            return !a.Equals(b);
        }

        public static Float2 operator +(Float2 a, Float2 b)
        {
            return new Float2(a.x + b.x, a.y + b.y);
        }

        public static Float2 operator -(Float2 a, Float2 b)
        {
            return new Float2(a.x - b.x, a.y - b.y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Float2 x:{x} y:{y}]";
        }
    }
}
