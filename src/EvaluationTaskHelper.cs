using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DnsClient;

namespace EvaluationTask
{
    public class EvaluationTaskHelper
    {
        public static List<string> PrepareDomainNames(string input_type, string input_source)
        {
            List<string> domain_names = new List<string>();
            switch (input_type)
            {
                case "-f":
                    StreamReader file = new StreamReader(@input_source);
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        domain_names.Add(line);
                    }
                    break;
                case "-s":
                    domain_names = input_source.Split(',').ToList();
                    break;
                default:
                    Console.WriteLine("Invalid parameter");
                    break;
            }
            return domain_names;
        }

        public static LookupClient PrepareCustomLookupClient(string ip, string port)
        {
            return new LookupClient(IPAddress.Parse(ip),Convert.ToInt32(port));
        }
    }
}