using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Akschaosmonkey
{
    public static class Function
    {
        [FunctionName("akschaosmonkey")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string command = req.Query["command"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            command = command ?? data?.command;

            return command != null
                ? (ActionResult)new OkObjectResult($"Scaling your AKS instance in 1 replica, command: {command}")
                : new BadRequestObjectResult("ERROR: I am only accept commands (scale, delete)");
        }
    }
}
