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
    
    // Don't share this password with anyone!!!! 
    // Oh noes! I got hacked!  now my password is much more secure, take that hackers!
    char c1 = (char)49;
    char c2 = (char)51;
    char c3 = (char)49;
    char c4 = (char)51;
    string password = $"{c1}{c2}{c3}{c4}";

    if (string.IsNullOrEmpty(passwordQuery) && string.IsNullOrEmpty(passwordHeader) && string.IsNullOrEmpty(passwordBody))
    {
        return new BadRequestObjectResult("Welcome to HackMe.  Bad Request: No password was given. Please specify a 4-digit password pin in Query String, like https://hackthehacker.azurewebsites.net/api/HackMe418201990200?password=1234");
    }
    else
    {
        if(passwordHeader == password || passwordQuery == password || passwordBody == password)
        {
            return (ActionResult)new OkObjectResult($"Welcome to HackMe.  Success: You got the password {password}! You can restore the $1000000 in stolen bitcoin!");
        }
        else
        {
            var result = new ObjectResult($"Welcome to HackMe.  Forbidden: wrong password.  Btw, valid passwords are 4 digit pins, and cannot start with the digit 0");
            result.StatusCode = 403;
            return (ActionResult)result;
        }
    }
}
