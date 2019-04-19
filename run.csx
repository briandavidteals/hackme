#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string passwordQuery = req.Query["password"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    var headers = req.Headers;
    string passwordHeader = null;
    
    if (headers != null && headers.Any(x => x.Key == "password"))
    {
        passwordHeader = headers["password"];
    }

    dynamic data = JsonConvert.DeserializeObject(requestBody);
    string passwordBody = data?.password;
    
    string password = "1234";

    if (string.IsNullOrEmpty(passwordQuery) && string.IsNullOrEmpty(passwordHeader) && string.IsNullOrEmpty(passwordBody))
    {
        return new BadRequestObjectResult("Bad Request: No password was given");
    }
    else
    {
        if(passwordHeader == password || passwordQuery == password || passwordBody == password)
        {
            return (ActionResult)new OkObjectResult($"Success: You got the password {password}! here's the secret 1328328423");
        }
        else
        {
            var result = new ObjectResult($"Forbidden: wrong password");
            result.StatusCode = 403;
            return (ActionResult)result;
        }
    }
}
