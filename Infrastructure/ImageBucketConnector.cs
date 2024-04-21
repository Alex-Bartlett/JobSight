using Microsoft.Extensions.Logging;
using Shared.Models;
using SixLabors.ImageSharp;
using Supabase;
using Supabase.Storage.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ImageBucketConnector
    {
        private readonly Client _supabaseClient;
        private readonly ILogger _logger;
        private readonly string _bucket = "task-images";

        public ImageBucketConnector(Client supabaseClient, ILogger<ImageBucketConnector> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        /// <summary>
        /// Upload an image to the Supabase storage service.
        /// </summary>
        /// <param name="companyId">Id of company to store image under</param>
        /// <param name="imgStream">The incoming file stream</param>
        /// <param name="expirationInMinutes">Minutes until the generated url expires</param>
        /// <returns>A new job task image with ImageUrl and UrlExpiry populated</returns>
        /// <exception cref="ArgumentException">Thrown when the uploaded file isn't a supported image type.</exception>
        /// <exception cref="Exception">Thrown when an error occured uploading the image to Supabase</exception>
        public async Task<string> AddImage(int companyId, MemoryStream imgStream)
        {
            string path = $"{_bucket}/{companyId}"; // This may change in the future
            string fileName = Guid.NewGuid().ToString();
            fileName += ".jpeg"; // This may also change in the future

            try
            {
                await ConvertToJpeg(imgStream);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"File is not a supported image type: {e.Message}");
            }

            var imageFile = imgStream.ToArray();

            try
            {
                var response = await _supabaseClient.Storage
                    .From(path)
                    .Upload(imageFile, fileName);
                return fileName;
            }
            catch (SupabaseStorageException e)
            {
                _logger.LogError("Failed to upload image to bucket.");
                throw new Exception($"Failed to upload image: {e.Message}");
            }
        }

        /// <summary>
        /// Creates a signed url for a file that expires after a set amount of time.
        /// </summary>
        /// <param name="companyId">The id of the company, used to locate the relevant folder in the bucket</param>
        /// <param name="fileName">The name of the file, including the extension</param>
        /// <param name="expirationInMinutes">The time until the link expires</param>
        /// <returns>A signed url for the file, with the given expiry</returns>
        public async Task<string> CreateSignedUrl(int companyId, string fileName, int expirationInMinutes) 
        {
            string path = $"{companyId}/{fileName}";
            int expirationInSeconds = expirationInMinutes * 60;
            try
            {
                var url = await _supabaseClient.Storage
                    .From(_bucket)
                    .CreateSignedUrl(path, expirationInSeconds);
                return url;
            }
            catch (Exception) {
                _logger.LogError("Failed to create signed url for file.");
                throw;
            }
        }

        public async Task DeleteImage(int companyId, string fileName)
        {
            string path = $"{companyId}/{fileName}";
            try
            {
                await _supabaseClient.Storage
                    .From(_bucket)
                    .Remove(path);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to delete image from bucket.", [path]);
                throw;
            }
        }

        public async Task DeleteImages(int companyId, List<string> fileNames)
        {
            List<string> paths = fileNames.Select(fileName => $"{companyId}/{fileName}").ToList();
            try
            {
                await _supabaseClient.Storage
                    .From(_bucket)
                    .Remove(paths);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to delete multiple images from bucket.", [paths]);
                throw;
            }
        }

        private static async Task ConvertToJpeg(MemoryStream imgStream) 
        {
            // Reset the stream position to the beginning
            imgStream.Seek(0, SeekOrigin.Begin);
            using (var img = Image.Load(imgStream))
            {
                await img.SaveAsJpegAsync(imgStream);
            }
        }
    }
}
