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

            if(command == "scale")
            {

                var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(@"C:\config.txt");
                IKubernetes client = new Kubernetes(config);
                log.LogInformation("Starting Request!");

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

            return command != null
                ? (ActionResult)new OkObjectResult($"Scaling your AKS instance in 1 replica, command: {command}")
                : new BadRequestObjectResult("ERROR: I am only accept commands (scale, delete)");
        }
    }
}
