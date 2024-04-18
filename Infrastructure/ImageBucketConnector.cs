using SixLabors.ImageSharp;
using Supabase;
using Supabase.Storage.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ImageBucketConnector
    {
        private readonly Client _supabaseClient;

        public ImageBucketConnector(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        /// <summary>
        /// Upload an image to the Supabase storage service.
        /// </summary>
        /// <param name="companyId">Id of company to store image under</param>
        /// <param name="imgStream">The incoming file stream</param>
        /// <returns>The new file path</returns>
        /// <exception cref="ArgumentException">Thrown when the uploaded file isn't a supported image type.</exception>
        /// <exception cref="Exception">Thrown when an error occured uploading the image to Supabase</exception>
        public async Task<string> AddImage(int companyId, MemoryStream imgStream)
        {
            string path = $"task-images/{companyId}"; // This may change in the future
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
                return response;
            }
            catch (SupabaseStorageException e)
            {
                throw new Exception($"Failed to upload image: {e.Message}");
            }
        }

        private async Task ConvertToJpeg(MemoryStream imgStream) 
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
