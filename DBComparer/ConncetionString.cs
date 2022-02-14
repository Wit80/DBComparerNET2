using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparer
{
    public static class ConncetionString
    {
        public static string GetConnectionString(bool noIntegratedSecurity, string server, string userName = "", string password = "", string initialCatalog = "") 
        {
            if (noIntegratedSecurity)
                return $"Persist Security Info=True;User ID={userName};Password={password};Initial Catalog={initialCatalog};Server={server}";
            else
                return $"Server={server}; integrated security=true; database={initialCatalog}";

        }
        public static string GetConnectionStringForDBList(bool noIntegratedSecurity, string server, string userName = "", string password = "")
        {
            if (noIntegratedSecurity)
                return $"Persist Security Info=True;User ID={userName};Password={password};Server={server}";
            else
                return $"Server={server}; integrated security=true;";

        }
    }
}
