using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharePointSiteCreator.CRMCustomWorkflow
{
    public class CreateSharePointSite : CodeActivity
    {
        [RequiredArgument]
        [Input("Azure Function Url")]
        [Default("")]
        public InArgument<String> WebhookUrl { get; set; }

        [RequiredArgument]
        [Input("SharePoint Web Title")]
        [Default("")]
        public InArgument<String> SiteTitle { get; set; }

        [RequiredArgument]
        [Input("SharePoint Web Url Part")]
        [Default("")]
        public InArgument<String> SiteUrlPart { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            ITracingService tracer = executionContext.GetExtension<ITracingService>();
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                //getting the input parameters
                string url = this.WebhookUrl.Get(executionContext);
                string title = this.SiteTitle.Get(executionContext);
                string urlPart = this.SiteUrlPart.Get(executionContext);

                //creating the json post data
                var postdata = string.Format("{{ 'title' : '{0}', 'url' : '{1}' }}", title, urlPart);
                var data = Encoding.ASCII.GetBytes(postdata);

                //setting the web request properties
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                //writing the request stream
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                //reading the response
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
