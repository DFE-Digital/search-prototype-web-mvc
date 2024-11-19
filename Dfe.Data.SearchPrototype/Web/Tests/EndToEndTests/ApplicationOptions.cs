﻿namespace Dfe.Data.SearchPrototype.Web.Tests.EndToEndTests;

public sealed class ApplicationOptions
{
    public string Domain { get; set; } = string.Empty;
    public int Port { get; set; } = 443;
    public string Scheme { get; set; } = "https";
    public Uri BaseUrl
    {
        get
        {
            if (string.IsNullOrEmpty(Domain))
            {
                throw new ArgumentException("Domain is null or empty");
            }

            return new Uri(Port == 443 ?
                $"{Scheme}://{Domain}" :
                    $"{Scheme}://{Domain}:{Port}");
        }
    }
}
