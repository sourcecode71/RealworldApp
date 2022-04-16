using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Web.Services
{
    public class ConverterService<T>
    {
        public T FromByteArray(byte[] array)
        {
            if (array == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(array))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        public byte[] ToByteArray(T data)
        {
            if (data == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, data);
                return ms.ToArray();
            }
        }
    }
}
