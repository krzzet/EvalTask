using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using DnsClient.Protocol;

namespace EvaluationTask
{
    class EvaluationTask
    {
        static void Main(string[] args)
        {
            DnsLookupClientWrapper wrapper;
            if (args.Length == 4) {
                wrapper = new DnsLookupClientWrapper(EvaluationTaskHelper.PrepareCustomLookupClient(args[2], args[3]));
            } else {
                wrapper = new DnsLookupClientWrapper(new LookupClient());
            }
            wrapper.SetDomains(EvaluationTaskHelper.PrepareDomainNames(args[0],args[1]));

            ConcurrentBag<IReadOnlyList<DnsResourceRecord>> mx_records_bag = new ConcurrentBag<IReadOnlyList<DnsResourceRecord>>();
            Parallel.ForEach(wrapper.GetDomains(), domain =>{
                IDnsQueryResponse result = wrapper.PerformQuery(domain,QueryType.MX);
                mx_records_bag.Add(result.Answers);
            });
            List<MXRecordParsed> parsed_records = new List<MXRecordParsed>();
            while (!mx_records_bag.IsEmpty)
            {
                IReadOnlyList<DnsResourceRecord> record_list;
                mx_records_bag.TryTake(out record_list);
                foreach(var record in record_list)
                {
                    parsed_records.Add(MXRecordParsed.make(record.ToString()));
                }
            }
            
            Parallel.ForEach(parsed_records, record =>{
                IDnsQueryResponse result = wrapper.PerformQuery(record.GetHost(),QueryType.A);
                var a_record = result.Answers.ARecords().FirstOrDefault();
                record.setHostIp(a_record.Address.ToString());
            });
            string input_msg = "Create report at default location? (y/n) (Press any other key to skip) \n";
            string location_msg = "Provide path for file: \n";
            Console.WriteLine(input_msg);
            string input = Convert.ToString(Console.ReadLine());
            if (input.Equals("y"))
            {
                wrapper.PrepareReport(parsed_records);
            } 
            else if (input.Equals("n"))
            {
                Console.WriteLine(location_msg);
                input = Convert.ToString(Console.ReadLine());
                wrapper.PrepareReport(input,parsed_records);
            } 
        }
    }
}
