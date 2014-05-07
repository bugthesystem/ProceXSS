using System;
using System.Web;
using ProceXSS.Interface;

namespace ProceXSS.Infrastructure
{
    public class IpAdressHelper : IIpAdressHelper
    {
        public string GetIpInformation(HttpRequest request)
        {
            string ip = request.ServerVariables["HTTP_CLIENT_IP"];
            string alternateIp = request.ServerVariables["REMOTE_ADDR"];

            string result = (String.IsNullOrEmpty(ip))
                ? (String.IsNullOrEmpty(alternateIp) ? String.Empty : alternateIp)
                : ip;

            return result;
        }
    }
}