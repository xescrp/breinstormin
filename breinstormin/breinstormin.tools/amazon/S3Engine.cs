using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.S3;
using Amazon;
using Amazon.S3.Model;

namespace breinstormin.tools.amazon
{
    public class S3Engine
    {
        public static string BUCKET_NAME = System.Configuration.ConfigurationManager.AppSettings["S3Bucket"];
        public static string S3_KEY = System.Configuration.ConfigurationManager.AppSettings["S3Key"];



        public static AmazonS3 GetS3Client()
        {
            if (string.IsNullOrEmpty(BUCKET_NAME)) { BUCKET_NAME = "elasticbeanstalk-eu-west-1-993712732203"; }
            System.Collections.Specialized.NameValueCollection appConfig = 
                System.Configuration.ConfigurationManager.AppSettings;

            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(
                    appConfig["AWSAccessKey"],
                    appConfig["AWSSecretKey"]
                    );
            return s3Client;
        }


        private static void CreateBucket(AmazonS3 client, string bucketname)
        {
            Console.Out.WriteLine("Checking S3 bucket with name " + bucketname);

            ListBucketsResponse response = client.ListBuckets();

            bool found = false;
            foreach (S3Bucket bucket in response.Buckets)
            {
                if (bucket.BucketName == bucketname)
                {
                    Console.Out.WriteLine("   Bucket found will not create it.");
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                Console.Out.WriteLine("   Bucket not found will create it.");

                client.PutBucket(new PutBucketRequest().WithBucketName(bucketname));

                Console.Out.WriteLine("Created S3 bucket with name " + bucketname);
            }
        }

        public static string CreateNewFile(AmazonS3 client, string filepath)
        {
            String S3_KEY = System.IO.Path.GetFileName(filepath);
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(BUCKET_NAME);
            request.WithKey(S3_KEY);
            request.WithFilePath(filepath);
            //request.WithContentBody("This is body of S3 object.");
            client.PutObject(request);
            return S3_KEY;
        }

        public static string CreateNewFolder(AmazonS3 client, string foldername)
        {
            String S3_KEY = foldername;
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(BUCKET_NAME);
            request.WithKey(S3_KEY);
            request.WithContentBody("");
            client.PutObject(request);
            return S3_KEY;
        }

        public static string CreateNewFileInFolder(AmazonS3 client, string foldername, string filepath)
        {
            String S3_KEY = foldername + "/" + System.IO.Path.GetFileName(filepath);
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(BUCKET_NAME);
            request.WithKey(S3_KEY);
            request.WithFilePath(filepath);
            //request.WithContentBody("This is body of S3 object.");
            client.PutObject(request);
            return S3_KEY;
        }

        public static string UploadFile(AmazonS3 client, string filepath)
        {
            //S3_KEY is name of file we want upload
            S3_KEY = System.IO.Path.GetFileName(filepath);
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(BUCKET_NAME);
            request.WithKey(S3_KEY);
            //request.WithInputStream(MemoryStream);
            request.WithFilePath(filepath);
            client.PutObject(request);
            return S3_KEY;
        }

        public static System.IO.MemoryStream GetFile(AmazonS3 s3Client, string filekey)
        {
            using (s3Client)
            {
                S3_KEY = filekey;
                System.IO.MemoryStream file = new System.IO.MemoryStream();
                try
                {
                    GetObjectResponse r = s3Client.GetObject(new GetObjectRequest()
                    {
                        BucketName = BUCKET_NAME,
                        Key = S3_KEY
                    });
                    try
                    {
                        long transferred = 0L;
                        System.IO.BufferedStream stream2 = new System.IO.BufferedStream(r.ResponseStream);
                        byte[] buffer = new byte[0x2000];
                        int count = 0;
                        while ((count = stream2.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            file.Write(buffer, 0, count);
                        }
                    }
                    finally
                    {
                    }
                    return file;
                }
                catch (AmazonS3Exception)
                {
                    //Show exception
                }
            }
            return null;
        }

        public static void DeleteFile(AmazonS3 Client, string filekey)
        {
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = BUCKET_NAME,
                Key = filekey
            };
            S3Response response = Client.DeleteObject(request);
        }

        public static void CopyFile(AmazonS3 s3Client, string sourcekey, string targetkey)
        {
            String destinationPath = targetkey;
            CopyObjectRequest request = new CopyObjectRequest()
            {
                SourceBucket = BUCKET_NAME,
                SourceKey = sourcekey,
                DestinationBucket = BUCKET_NAME,
                DestinationKey = targetkey
            };
            CopyObjectResponse response = s3Client.CopyObject(request);
        }

        public static void ShareFile(AmazonS3 s3Client, string filekey)
        {
            S3Response response1 = s3Client.SetACL(new SetACLRequest()
            {
                CannedACL = S3CannedACL.PublicRead,
                BucketName = BUCKET_NAME,
                Key = filekey
            });
        }

        public static String MakeUrl(AmazonS3 s3Client, string filekey)
        {
            string preSignedURL = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = BUCKET_NAME,
                Key = filekey,
                Expires = System.DateTime.Now.AddYears(10)

            });

            return preSignedURL;
        }

    }
}
