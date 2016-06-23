#r "Newtonsoft.Json"
#r "SharePointSiteCreator.SharePoint.dll"

using System;
using System.Net;
using Newtonsoft.Json;
using Microsoft.SharePoint.Client;
using SharePointSiteCreator.SharePoint;
public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");
    
    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.url == null || data.title == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Please pass title/url properties in the input object"
        });
    }
    
    string url = System.Environment.GetEnvironmentVariable("SharePointWebUrl", EnvironmentVariableTarget.Process);
    string clientId = System.Environment.GetEnvironmentVariable("ClientId", EnvironmentVariableTarget.Process);
    string clientSecret = System.Environment.GetEnvironmentVariable("ClientSecret", EnvironmentVariableTarget.Process);
    string realm = System.Environment.GetEnvironmentVariable("Realm", EnvironmentVariableTarget.Process);
    
    
    CreateSite.Run($"{data.title}", $"{data.url}", url, clientId, realm, clientSecret);

    return req.CreateResponse(HttpStatusCode.OK, new {
        result = $"Site created"
    });
}

