using System.IO;
using System.Collections.Generic;
using System.Linq;
using DnsClient;

namespace EvaluationTask
{
    public class DnsLookupClientWrapper
    {
        public DnsLookupClientWrapper(LookupClient client)
        {
            this._client = client;
        }
        public void SetDomains(List<string> domain_list)
        {
            this._domains = domain_list;
        }
        public List<string> GetDomains()
        {
            return this._domains;
        }
        public IDnsQueryResponse PerformQuery(string query, QueryType type)
        {
            return this._client.Query(query,type);
        }
        public void PrepareReport(string output_file, List<MXRecordParsed> parsed_records)
        {
            FileStream fs = new FileStream(output_file,FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fs,System.Text.Encoding.UTF8);
            sw.WriteLine("DNS server used for resolving records: " + _client.NameServers.FirstOrDefault().ToString());
            foreach (var record in parsed_records)
            {
                sw.WriteLine(record.ToString());
            }
            sw.Flush();
            fs.Flush();
        }
        public void PrepareReport(List<MXRecordParsed> parsed_records)
        {
            PrepareReport("report.txt", parsed_records);
        }

        LookupClient _client;
        List<string> _domains;
    }
}