using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Akschaosmonkey.Repositories
{
    class BlobStorageRepository
    {

        public async Task<string> GetKubeConfigFile()
        {
            //var storageConnectionString = "hi";
            var storageConnectionString = "hi";
            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient serviceClient = account.CreateCloudBlobClient();

            var container = serviceClient.GetContainerReference("kubeconfig");

            var blobName = container.GetPageBlobReference("config.txt");

            var blobcontent = await blobName.OpenReadAsync();

            string result = blobcontent.ToString();
          
            return result;
        }

    }
}
