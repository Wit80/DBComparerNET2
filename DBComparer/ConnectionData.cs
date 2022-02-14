using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparer
{
    public class ConnectionData
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public bool IsUserPasswordAutentification { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
