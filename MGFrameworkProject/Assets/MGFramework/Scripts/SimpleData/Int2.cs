using System;

namespace MGFramework.Data
{
    [Serializable]
    public struct Int2
    {
        private static Int2 _zero = new Int2(0, 0);

        public static Int2 Zero => _zero;

        public int x;

        public int y;

        public Int2(int x, int y)
        {
            this.x = x;

            this.y = y;
        }

        public override bool Equals(object val)
        {
            bool res = false;

            if (val is Int2)
            {
                Int2 target = (Int2)val;

                res = x == target.x && y == target.y;
            }

            return res;
        }

        public static bool operator ==(Int2 a, Int2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Int2 a, Int2 b)
        {
            return !a.Equals(b);
        }

        public static Int2 operator +(Int2 a, Int2 b)
        {
            return new Int2(a.x + b.x, a.y + b.y);
        }

        public static Int2 operator -(Int2 a, Int2 b)
        {
            return new Int2(a.x - b.x, a.y - b.y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Int2 x:{x} y:{y}]";
        }
    }
}
