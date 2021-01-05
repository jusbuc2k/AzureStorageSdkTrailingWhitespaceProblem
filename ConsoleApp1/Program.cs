using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "UseDevelopmentStorage=true";
            var blobService = new BlobServiceClient(connectionString);
            var container = blobService.GetBlobContainerClient("test");
            var blobClient = container.GetBlobClient("test.txt");

            Task.Run(async () =>
            {
                var buffer = System.Text.UTF8Encoding.UTF8.GetBytes("Hello World");
                var data = new System.IO.MemoryStream(buffer);

                Console.Write("Uploading test blob content...");
                await blobClient.UploadAsync(data, overwrite: true);
                Console.WriteLine("OK");

                Console.Write("SetMetadataAsync (should work)...");
                await blobClient.SetMetadataAsync(new Dictionary<string, string>()
                {
                    // note no space after bar
                    { "foo", "bar" }
                });
                Console.WriteLine("OK");

                try
                {
                    Console.Write("SetMetadataAsync (should fail)...");
                    await blobClient.SetMetadataAsync(new Dictionary<string, string>()
                    {
                        // note bar has a space after it
                        { "foo", "bar " }
                    });
                    // shouldn't get here
                    Console.Write("OK");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("FAIL");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);                    
                    Console.WriteLine(ex.StackTrace);
                }

            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
