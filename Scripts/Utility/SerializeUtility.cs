using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MGFramework
{
    /// <summary>
    /// 序列化工具
    /// </summary>
    public static class SerializeUtility
    {
        /// <summary>
        /// 序列化
        /// </summary>
        public static byte[] Serialize(object data)
        {
            byte[] result = null;

            if (data != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, data);

                    stream.Seek(0, SeekOrigin.Begin);

                    result = stream.GetBuffer();
                }
            }

            return result;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public static T DeSerialize<T>(byte[] buffer)
        {
            T result = default(T);

            if (buffer != null)
            {
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    result = (T)formatter.Deserialize(stream);
                }
            }

            return result;
        }
    }
}