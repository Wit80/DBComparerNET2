using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSQLExecutor
{
    public class ConncetionString
    {
        private bool _noIntegratedSecurity { get; }
        private string _initialCatalog { get; } 
        private string _server { get; }
        private string _userName { get; }
        private string _password { get; }

        public ConncetionString(bool noIntegratedSecurity, string server, string userName = "", string password = "", string initialCatalog = "")
        {
            _noIntegratedSecurity = noIntegratedSecurity;
            _initialCatalog = initialCatalog;
            _server = server;
            _userName = userName;
            _password = password;
        }
        public string GetConnectionString() 
        {
            if (_noIntegratedSecurity)
                return $"Persist Security Info=True;User ID={_userName};Password={_password};Initial Catalog={_initialCatalog};Server={_server}";
            else
                return $"Server = {_server}; integrated security = true; database = {_initialCatalog}";

        }
        public string GetConnectionStringForDBList()
        {
            if (_noIntegratedSecurity)
                return $"Persist Security Info=True;User ID={_userName};Password={_password};Server={_server}";
            else
                return $"Server = {_server}; integrated security = true;";

        }
    }
}
