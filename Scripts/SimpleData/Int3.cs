using System;

namespace MGFramework.Data
{
    [Serializable]
    public struct Int3
    {
        private static Int3 _zero = new Int3(0, 0, 0);

        public static Int3 Zero => _zero;

        public int x;

        public int y;

        public int z;

        public Int3(int x, int y, int z)
        {
            this.x = x;

            this.y = y;

            this.z = z;
        }

        public override bool Equals(object val)
        {
            bool res = false;

            if (val is Int3)
            {
                Int3 target = (Int3)val;

                res = x == target.x && y == target.y && z == target.z;
            }

            return res;
        }

        public static bool operator ==(Int3 a, Int3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Int3 a, Int3 b)
        {
            return !a.Equals(b);
        }

        public static Int3 operator +(Int3 a, Int3 b)
        {
            return new Int3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Int3 operator -(Int3 a, Int3 b)
        {
            return new Int3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[Int3 x:{x} y:{y} z:{z}]";
        }
    }
}
