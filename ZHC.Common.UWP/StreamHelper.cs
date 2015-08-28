using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace ZHC.Common.UWP
{
    public class StreamHelper
    {

        public async static Task<IRandomAccessStream> Buffer2RandomAccessStream(IBuffer buffer)
        {
            IRandomAccessStream randomStream = new InMemoryRandomAccessStream();
            DataWriter dataWriter = new DataWriter(randomStream);
            dataWriter.WriteBuffer(buffer, 0, buffer.Length);
            await dataWriter.StoreAsync();
            return randomStream;
        }
        public static IBuffer RandomAccessStream2Buffer(IRandomAccessStream randomStream)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(randomStream.GetInputStreamAt(0));
            MemoryStream memoryStream = new MemoryStream();
            if (stream != null)
            {
                byte[] bytes = Stream2Bytes(stream);
                if (bytes != null)
                {
                    var binaryWriter = new BinaryWriter(memoryStream);
                    binaryWriter.Write(bytes);
                }
            }
            IBuffer buffer = WindowsRuntimeBufferExtensions.GetWindowsRuntimeBuffer(memoryStream, 0, (int)memoryStream.Length);
            return buffer;
        }


        public async static Task<IRandomAccessStream> Stream2RandomAccessStream(Stream stream)
        {
            byte[] bytes = Stream2Bytes(stream);
            IRandomAccessStream randomStream = new InMemoryRandomAccessStream();
            DataWriter dataWriter = new DataWriter(randomStream.GetOutputStreamAt(0));
            dataWriter.WriteBytes(bytes);
            await dataWriter.StoreAsync();

            return randomStream;
        }
        public static Stream RandomAccessStream2Stream(IRandomAccessStream randomStream)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(randomStream.GetInputStreamAt(0));
            return stream;
        }


        public static Stream Buffer2Stream(IBuffer buffer)
        {
            Stream stream = WindowsRuntimeBufferExtensions.AsStream(buffer);
            return stream;
        }
        public static IBuffer Stream2Buffer(Stream stream)
        {
            MemoryStream memoryStream = new MemoryStream();
            if (stream != null)
            {
                byte[] bytes = Stream2Bytes(stream);
                if (bytes != null)
                {
                    var binaryWriter = new BinaryWriter(memoryStream);
                    binaryWriter.Write(bytes);
                }
            }
            IBuffer buffer = WindowsRuntimeBufferExtensions.GetWindowsRuntimeBuffer(memoryStream, 0, (int)memoryStream.Length);
            return buffer;
        }


        public static FileInputStream RandomAccessStream2FileInputStream(IRandomAccessStream randomStream)
        {
            FileInputStream inputStream = randomStream.GetInputStreamAt(0) as FileInputStream;
            return inputStream;
        }
        public static FileOutputStream RandomAccessStream2FileOutputStream(IRandomAccessStream randomStream)
        {
            FileOutputStream outputStream = randomStream.GetOutputStreamAt(0) as FileOutputStream;
            return outputStream;
        }


        public static IBuffer String2Buffer(string str)
        {
            using (InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter dataWriter = new DataWriter(memoryStream))
                {
                    dataWriter.WriteString(str);
                    return dataWriter.DetachBuffer();
                }
            }
        }
        public static string Buffer2String(IBuffer buffer)
        {
            using (DataReader dataReader = DataReader.FromBuffer(buffer))
            {
                string fileContent = dataReader.ReadString(buffer.Length);
                return fileContent;
            }
        }

        public static string Buffer2Base64(IBuffer buffer)
        {
            using (var dataReader = DataReader.FromBuffer(buffer))
            {
                try
                {
                    var bytes = new byte[buffer.Length];
                    dataReader.ReadBytes(bytes);

                    return Convert.ToBase64String(bytes);
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
        }



        public static IBuffer Bytes2Buffer(byte[] bytes)
        {
            using (var dataWriter = new DataWriter())
            {
                dataWriter.WriteBytes(bytes);
                return dataWriter.DetachBuffer();
            }
        }
        public static byte[] Buffer2Bytes(IBuffer buffer)
        {
            using (var dataReader = DataReader.FromBuffer(buffer))
            {
                var bytes = new byte[buffer.Length];
                dataReader.ReadBytes(bytes);
                return bytes;
            }
        }


        public static byte[] Stream2Bytes(Stream stream)
        {
            if (stream.CanSeek) // stream.Length 已确定
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return bytes;
            }
            else // stream.Length 不确定
            {
                int initialLength = 32768; // 32k

                byte[] buffer = new byte[initialLength];
                int read = 0;

                int chunk;
                while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                {
                    read += chunk;

                    if (read == buffer.Length)
                    {
                        int nextByte = stream.ReadByte();

                        if (nextByte == -1)
                        {
                            return buffer;
                        }

                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[read] = (byte)nextByte;
                        buffer = newBuffer;
                        read++;
                    }
                }

                byte[] ret = new byte[read];
                Array.Copy(buffer, ret, read);
                return ret;
            }
        }
        public static Stream Bytes2Stream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }


        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 复制stream
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            input.Position = 0;
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    output.Position = 0;
                    return;
                }
                output.Write(buffer, 0, read);
            }

        }
    }
}
