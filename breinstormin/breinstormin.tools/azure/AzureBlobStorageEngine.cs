using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;

namespace breinstormin.tools.azure
{
    public class AzureBlobStorageEngine
    {

        string _accountname;
        string _accountkey;
        string _endpointprotocol;
        private static CloudStorageAccount _storageAccount = null;
        private static string _partitionKey = string.Empty;
        private static string _id = string.Empty;
        CloudBlobClient _blobClient = null;



        public AzureBlobStorageEngine(string accountname, string accountkey, string endpointprotocol) 
        {
            _accountname = accountname;
            _accountkey = accountkey;
            _endpointprotocol = endpointprotocol;

            bool https = endpointprotocol.ToLower() == "https";

            _storageAccount = new CloudStorageAccount(new StorageCredentials(accountname, accountkey), https);
            //_storageAccount = CloudStorageAccount.Parse(
            //    string.Format("DefaultEndpointsProtocol={0};AccountName={1}; AccountKey={2}", endpointprotocol, accountname, accountkey));

            _blobClient = _storageAccount.CreateCloudBlobClient();
        }

        public void UploadData(System.IO.Stream stream, string containername, string blobname, string contenttype) 
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containername);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobname);

            blockBlob.Properties.ContentType = contenttype;
            blockBlob.UploadFromStream(stream);  

        }

        public System.IO.Stream DownloadData(string containername, string blobname, string targetfilename) 
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containername);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobname);

            System.IO.FileStream str = new System.IO.FileStream(targetfilename, System.IO.FileMode.OpenOrCreate);
            blockBlob.DownloadToStream(str);
            return str;
        }

        public IListBlobItem[] GetDataList(string containername) 
        {
            List<IListBlobItem> _cloudblobs = new List<IListBlobItem>();
            CloudBlobContainer container = _blobClient.GetContainerReference(containername);
            foreach (IListBlobItem item in container.ListBlobs(null, false)) 
            {
                _cloudblobs.Add(item);
            }

            return _cloudblobs.ToArray();
        }

        public void DeleteData(string containername, string blobname) 
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(containername);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobname);

            blockBlob.Delete();
        }
    }
}
