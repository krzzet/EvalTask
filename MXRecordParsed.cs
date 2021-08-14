using System;

namespace EvaluationTask
{
    public class MXRecordParsed
    {
        public MXRecordParsed(string domain, string ttl, string mx_class, string type, int priority, string host)
        {
            this._domain = domain;
            this._TTL = ttl;
            this._class = mx_class;
            this._type = type;
            this._priority = priority;
            this._host = host;
        }
        public override string ToString()
        {
            return $"Domain:{_domain} TTL:{_TTL} Class:{_class} Type:{_type} Priority:{_priority} Host:{_host} Host IP:{_host_ip}";
        }

        public string GetHost()
        {
            return this._host;
        }
        public void setHostIp(string ip)
        {
            this._host_ip = ip;
        }
        public static MXRecordParsed make(string input_string)
        {
            string[] args = input_string.Split(" ");
            return new MXRecordParsed(args[0], args[1], args[2], args[3], Convert.ToInt32(args[4]),args[5]);
        }
        string _domain;
        string _TTL;
        string _class;
        string _type;
        int _priority;
        string _host;
        string _host_ip;
    }
}