﻿using System.Net.Http.Headers;

namespace SealabAPI.DataAccess.Extensions
{
    public static class IFormFileExtension
    {
        public static string GetFileName(this IFormFile file)
        {
            return ContentDispositionHeaderValue.Parse(
                            file.ContentDisposition).FileName.ToString().Trim('"');
        }

        public static string SetFilePath(this IFormFile file, string path, string fileName)
        {
            var ext = Path.GetExtension(file.GetFileName());
            return Path.Combine(path, fileName + ext);
        }

        public static async Task<MemoryStream> GetFileStream(this IFormFile file)
        {
            MemoryStream filestream = new();
            await file.CopyToAsync(filestream);
            return filestream;
        }

        public static async Task<byte[]> GetFileArray(this IFormFile file)
        {
            MemoryStream filestream = new();
            await file.CopyToAsync(filestream);
            return filestream.ToArray();
        }
    }
}
