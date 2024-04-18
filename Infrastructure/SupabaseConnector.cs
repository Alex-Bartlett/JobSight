using SixLabors.ImageSharp;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SupabaseConnector
    {
        private readonly Client _supabaseClient;

        public SupabaseConnector(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task AddImage(int companyId, MemoryStream imgStream)
        {
            string path = $"task-images/{companyId}"; // This may change in the future
            string fileName = Guid.NewGuid().ToString();
            fileName += ".jpeg";

            try
            {
                await ConvertToJpeg(imgStream);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to convert image to jpeg: {e.Message}");
            }

            var imageFile = imgStream.ToArray();

            try
            {
                var response = await _supabaseClient.Storage
                    .From(path)
                    .Upload(imageFile, fileName);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to upload image: {e.Message}");
            }

         /*   if (response != null) // Testing
            {
                throw new Exception($"Failed to upload image: {response}");
            }*/
        }

        private async Task ConvertToJpeg(MemoryStream imgStream) 
        {
            imgStream.Seek(0, SeekOrigin.Begin);
            using (var img = Image.Load(imgStream))
            {
                await img.SaveAsJpegAsync(imgStream);
            }
        }
    }
}
