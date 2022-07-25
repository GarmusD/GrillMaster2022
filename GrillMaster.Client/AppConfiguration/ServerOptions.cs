using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillMaster.Client.AppConfiguration
{
    public class ServerOptions
    {
        public string Host { get => _host; set => SetHost(value); }
        public int Port { get => _port; set => SetPort(value); }
        internal Uri Uri => GetUri();

        private string _host = ConfigDefaults.ServerOptions.Server;
        private int _port = ConfigDefaults.ServerOptions.Port;

        private Uri GetUri()
        {
            Uri startup = new (Host);
            return new UriBuilder() 
            {
                Scheme = startup.Scheme,
                Host = startup.Host,
                Port = Port
            }.Uri;
        }

        private void SetHost(string value)
        {
            if(value == ConfigDefaults.Placeholders.Host) _host = ConfigDefaults.ServerOptions.Server;
            else _host = value;
        }

        private void SetPort(int value)
        {
            if (value == 0) value = ConfigDefaults.ServerOptions.Port;
            _port = value;
        }
    }
}
