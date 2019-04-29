using System;

namespace BlazorClientPrerender.Shared.Extensions
{
    public class ApiBaseUrl
    {
        public string Url { get; set; }

        public Uri Uri 
        { 
            get 
            {
                return new Uri(Url);
            } 
        }

        public ApiBaseUrl(string url)
        {
            Url = url;
        }
    }
}