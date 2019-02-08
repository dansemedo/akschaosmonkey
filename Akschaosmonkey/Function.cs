using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using k8s;
using Akschaosmonkey.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using k8s.Models;
using System.Collections.Generic;
using System.Linq;

namespace Akschaosmonkey
{
    public static class Function
    {
        [FunctionName("akschaosmonkey")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function processed a request.");

            string command = req.Query["command"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            command = command ?? data?.command;

            // var blobstorage = new BlobStorageRepository();

            // string kubeconfigFile = await blobstorage.GetKubeConfigFile();

            // var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeconfigFile);

            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(@"C:\config.txt");
            IKubernetes client = new Kubernetes(config);

            log.LogInformation("Kubernetes Client Configured successfully...");

            if (command == "listpods")
            {

                var list = client.ListNamespacedPod("dev");
                foreach (var item in list.Items)
                {
                    log.LogInformation(item.Metadata.Name);
                }
                if (list.Items.Count == 0)
                {
                    log.LogInformation("Empty!");
                }

            }
            else if (command == "scale")
            {
                var replicaset = client.ListNamespacedReplicaSet("dev").Items.First();

                var podname = replicaset.Metadata.Name;
                var podlabel = replicaset.Metadata.Labels;

                log.LogInformation(podlabel.ToString());

               // client.PatchNamespacedReplicaSetScale(new V1Patch(podlabel),podname, "dev");
                client.PatchNamespacedPod(new V1Patch(podlabel), podname, "dev");

                log.LogInformation("ReplicateSet of ---" + podname + " --- scaled successfully....");
            }

            return command != null
                ? (ActionResult)new OkObjectResult($"Azure Function executed successfully, command executed: {command}")
                : new BadRequestObjectResult("ERROR: I am only accept the commands (scale, delete, listpods)");
        }
    }
}
