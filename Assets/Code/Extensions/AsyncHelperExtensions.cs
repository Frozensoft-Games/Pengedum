using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Extensions
{
    public class AsyncHelperExtensions : MonoBehaviour
    {
        // Helper method to write text to a file async
        public static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            FileStream sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        }

        // Helper method to write bytes to a file async
        public static async Task WriteBytesAsync(string filePath, byte[] data)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
                fileStream.Close();
            }
        }

        // Helper method to read text from a file async
        public static async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }

        // Helper method to read bytes from a file async
        public static async Task<byte[]> ReadBytesAsync(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, true))
            {
                var encodedData = new byte[fileStream.Length];
                await fileStream.ReadAsync(encodedData, 0, (int)fileStream.Length);
                fileStream.Close();

                return encodedData;
            }
        }

        // Helper method to load images async
        public static async Task<Texture2D> LoadTexture2DAsync(string filePath, bool generateMipMaps = false)
        {
            byte[] bytes = await ReadBytesAsync(filePath);
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, generateMipMaps);
            texture.LoadImage(bytes);

            return texture;
        }

        public static async Task<Sprite> LoadImageAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;
            if (!File.Exists(filePath)) return null;

            Texture2D texture = await LoadTexture2DAsync(filePath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}