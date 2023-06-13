using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Web;

namespace FunctionApp4_net48
{
    public static class MyDurableFunction
    {
        [FunctionName("MyDurableFunction")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            // Orchestration logic goes here

            // Example:
            string input = context.GetInput<string>();
            log.LogInformation($"Processing input: {input}");

            // Call activities or other orchestrations
            string result = await context.CallActivityAsync<string>("MyActivityFunction", input);

            // Perform other actions, handle timers, wait for external events, etc.

            // Return the result
            await context.CallActivityAsync("MyResultFunction", result);
        }

        [FunctionName("MyActivityFunction")]
        public static string MyActivityFunction(
            [ActivityTrigger] string input,
            ILogger log)
        {
            // Activity logic goes here

            // Example:
            log.LogInformation($"Performing activity with input: {input}");

            // Return the activity result
            return $"Activity completed with input: {input}";
        }

        [FunctionName("MyResultFunction")]
        public static void MyResultFunction(
            [ActivityTrigger] string result,
            ILogger log)
        {
            // Result handling logic goes here

            // Example:
            log.LogInformation($"Handling result: {result}");
        }
    }
}




#if TTTTTTTTTTTTTTTT
    public static class MyDurableFunction
    {
        public class Input
        {
            public bool Sleep { get; set; }
        }

        [FunctionName("MyDurableFunction")]
        public static async Task<List<string>> RunOrchestrator(
                [OrchestrationTrigger] IDurableOrchestrationContext context,
                ILogger log)
        {
            var outputs = new List<string>();

            try
            {
                Input input = context.GetInput<Input>();
                var json = input.ToJson();

                Guid uuid = new Guid();

                outputs.Add("test");

                log.LogWarning("CreateBlob.");
                await context.CallActivityAsync<string>(nameof(CreateBlob), uuid.ToString());

                //// Replace "hello" with the name of your Durable Activity Function.
                log.LogWarning("SayHello Tokyo.");
                var s = await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo");
                outputs.Add(s);
                log.LogWarning("SayHello Tokyo2.");
                s = await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo");
                log.LogWarning("SayHello Seatle.");
                s = await context.CallActivityAsync<string>(nameof(SayHello), "Seattle");
                outputs.Add(s);
                log.LogWarning("SayHello London.");
                s = await context.CallActivityAsync<string>(nameof(SayHello), "London");
                outputs.Add(s);
                //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
                //outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));
            }
            catch (Exception ex)
            {
                outputs.Add(ex.Message);
            }
            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName(nameof(SayHello))]
        public static async Task<string> SayHello([ActivityTrigger] string name, ILogger log)
        {
            OutputDebugString($"SLEEPING FOR {name}");
            log.LogWarning($"SLEEPING FOR {name}");
            //Thread.Sleep(10000);
            await Task.Delay(3000);
            OutputDebugString($"done sleeping for {name}");
            log.LogWarning($"DONE SLEEPING FOR {name}");
            log.LogInformation("Saying hello to {name}.", name);
            return $"Hello {name}!";
        }


        const string connectionString = "UseDevelopmentStorage=true";
        [FunctionName(nameof(CreateBlob))]
        public static async Task CreateBlob([ActivityTrigger] string uuid, ILogger log)
        {
            //CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");
            CloudBlockBlob lockBlob = container.GetBlockBlobReference($"{uuid}.lock");

            ///*
            // Check if the lock blob already exists
            if (!await lockBlob.ExistsAsync())
            {
                // Create the lock blob
                await lockBlob.UploadTextAsync(string.Empty);
                log.LogWarning("Lock blob created.");
            }
            else
            {
                // Lock blob already exists
                log.LogWarning("Lock blob already exists.");
            }
            //*/
        }

        //private static CloudBlobClient blobClient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();

        //private static CloudBlobContainer _blobContainer = blobClient.GetContainerReference("mycontainer");

        [FunctionName("Function_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            int processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            log.LogError($"processid={processId}");
            // A VOIR
            //NameValueCollection nvc = HttpUtility.ParseQueryString(req.RequestUri.Query);
            Input input = new Input()
            {
                //Sleep = nvc["sleep"] is not null,
            };

            // Function input comes from the request content.
            // .NET core 6, not 4.8
            //string instanceId = await starter.StartNewAsync<Input>("Function", input);
            string result = await context.CallActivityAsync<string>("MyActivityFunction", input);

            // .NET core 6, not 4.8
            //log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // .NET core 6, not 4.8
            //return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [DllImport("kernel32", EntryPoint = "OutputDebugStringW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void OutputDebugString(string s);

        private static CloudBlobContainer CreateContainer()
        {
            CloudBlobClient blobClient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
            CloudBlobContainer _blobContainer = blobClient.GetContainerReference("mycontainer");
            return _blobContainer;
        }

        /// <summary>
        /// ne pas faire �a ici ?
        /// (pour l'instant utiliser CreateCloudBlobContainer)
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName(nameof(CreateLockBlob))]
        public static async Task<CloudBlockBlob> CreateLockBlob([ActivityTrigger] string uuid, ILogger log)
        {
            //var blob = await TestCreateBlob();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");
            CloudBlockBlob lockBlob = container.GetBlockBlobReference($"{uuid}.lock");

            // Check if the lock blob already exists
            if (!await lockBlob.ExistsAsync())
            {
                // Create the lock blob
                await lockBlob.UploadTextAsync(string.Empty);
                log.LogInformation("Lock blob created.");
            }
            else
            {
                // Lock blob already exists
                log.LogInformation("Lock blob already exists.");
            }
            return lockBlob;

        }

    }
}
#endif
