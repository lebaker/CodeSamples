using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointSiteCreator.SharePoint
{
    public static class CreateSite
    {
        private static string _sharePointPID = "00000003-0000-0ff1-ce00-000000000000";
        public static void Run(string urlPart, string title, string webUrl, string clientId, string realm, string clientsecret)
        {
            var uri = new Uri(webUrl);
            var token = TokenHelper.GetAppOnlyAccessToken(_sharePointPID, uri.Authority, realm, clientId, clientsecret).AccessToken;
            using (var context = TokenHelper.GetClientContextWithAccessToken(uri.ToString(), token))
            {
                WebCreationInformation information = new WebCreationInformation();
                information.WebTemplate = "STS#0";
                information.Description = title; 
                information.Title = title;
                information.Url = urlPart;
                information.Language = 1033;

                Web newWeb = null;
                newWeb = context.Web.Webs.Add(information);
                context.ExecuteQuery();
            }

        }
    }
}
