using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;

namespace ZHC.Common.UWP
{
    public class StorageHelper
    {
        #region exsit
        public static async Task<bool> IsFileExsit(string path)
        {
            bool result = false;
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(path);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static async Task<bool> IsFileExsit(string path, StorageFolder folder)
        {
            bool result = false;
            try
            {
                var file = await folder.GetFileAsync(path);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static async Task<StorageFile> IsFileExsitAndReturnStorageFile(string path)
        {
            StorageFile result = null;
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.GetFileAsync(path);
                return file;
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static async Task<StorageFile> IsFileExsitAndReturnStorageFile(string path, StorageFolder folder)
        {
            StorageFile result = null;
            try
            {
                StorageFile file = await folder.GetFileAsync(path);
                return file;
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion

        #region folder
        public static async Task<StorageFolder> CreateFolder(string folderName)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            return await storageFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
        }

        public static async Task<StorageFolder> CreateFolder(string folderName, StorageFolder folder)
        {
            return await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
        }
        #endregion


        #region txt
        public static async Task<string> ReadTextFileAsync(string path)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(path);
                return await FileIO.ReadTextAsync(file);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task<T> ReadTextFileAsync<T>(string path) where T : class,new()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(path);
                var contents = await FileIO.ReadTextAsync(file);

                return JsonHelper.JsonToObj<T>(contents);
            }
            catch
            {
                return default(T);
            }
        }

        public static async Task<string> ReadTextFileAsync(string path, StorageFolder folder)
        {
            try
            {
                var file = await folder.GetFileAsync(path);
                return await FileIO.ReadTextAsync(file);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task<bool> WriteToTextFileAsync<T>(string path, T t) where T : class,new()
        {
            bool result = false;
            try
            {
                var contents = JsonHelper.JsonSerializer(t);

                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, contents);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static async Task<bool> WriteToTextFileAsync(string path, string contents)
        {
            bool result = false;
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, contents);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        public static async Task<bool> WriteToTextFileAsync(string path, string contents, StorageFolder folder)
        {
            bool result = false;
            try
            {
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, contents);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        #endregion


        #region byte

        public static async Task<byte[]> ReadByteFileAsync(string path)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(path);
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                byte[] result = StreamHelper.Buffer2Bytes(buffer);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<byte[]> ReadByteFileAsync(string path, StorageFolder folder)
        {
            try
            {
                var file = await folder.GetFileAsync(path);
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                byte[] result = StreamHelper.Buffer2Bytes(buffer);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<bool> WriteToByteFileAsync(string path, byte[] data)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                IBuffer buffer = StreamHelper.Bytes2Buffer(data);
                await FileIO.WriteBufferAsync(file, buffer);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> WriteToByteFileAsync(string path, byte[] data, StorageFolder folder)
        {
            try
            {
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                IBuffer buffer = StreamHelper.Bytes2Buffer(data);
                await FileIO.WriteBufferAsync(file, buffer);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion


        #region IBuffer
        public static async Task<IBuffer> ReadIBufferFileAsync(string path)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(path);
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                return buffer;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<IBuffer> ReadIBufferFileAsync(string path, StorageFolder folder)
        {
            try
            {
                var file = await folder.GetFileAsync(path);
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                return buffer;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<bool> WriteScreenshotToCusFolderAsync(string saveName, StorageFolder folder, IBuffer data, int PixelWidth, int PixelHeight)
        {
            try
            {
                var file = await folder.CreateFileAsync(saveName, CreationCollisionOption.ReplaceExisting);

                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);

                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)PixelWidth, (uint)PixelHeight, DisplayInformation.GetForCurrentView().LogicalDpi, DisplayInformation.GetForCurrentView().LogicalDpi, data.ToArray());

                    await encoder.FlushAsync();
                }
                return true;
            }
            catch
            {
            }
            return false;
        }
        #endregion



        #region stream



        public static async Task<Stream> ReadSreamFileAsync(string path)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(path);
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                Stream result = StreamHelper.Buffer2Stream(buffer);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<Stream> ReadSreamFileAsync(string path, StorageFolder folder)
        {
            try
            {
                var file = await folder.GetFileAsync(path);
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                Stream result = StreamHelper.Buffer2Stream(buffer);
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<bool> WriteToSreamFileAsync(string path, Stream data)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                IBuffer buffer = StreamHelper.Stream2Buffer(data);
                await FileIO.WriteBufferAsync(file, buffer);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                data.Dispose();
            }
        }
        public static async Task<bool> WriteToSreamFileAsync(string path, Stream data, StorageFolder folder)
        {
            try
            {
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                IBuffer buffer = StreamHelper.Stream2Buffer(data);
                await FileIO.WriteBufferAsync(file, buffer);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                data.Dispose();
            }
        }

        #endregion


        #region Lib
        public static async Task<bool> WriteSreamFileToCusFolderAsync(string path, Stream data, StorageFolder folder)
        {
            try
            {
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                IBuffer buffer = StreamHelper.Stream2Buffer(data);
                await FileIO.WriteBufferAsync(file, buffer);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                data.Dispose();
            }
        }

        public static async Task<bool> WriteByteFileToCusFolderAsync(string path, byte[] data, StorageFolder folder)
        {
            try
            {
                var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                IBuffer buffer = StreamHelper.Bytes2Buffer(data);
                await FileIO.WriteBufferAsync(file, buffer);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion




    }
}
