using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary
{
    public class SQLScriptsList
    {
        public SQLScriptsList(List<string> script1, List<string> script2, List<int> difs, string statusTxt)
        {
            this.script1 = script1;
            this.script2 = script2;
            this.difs = difs;
            statusText = statusTxt;

        }

        public List<string> script1 { get; }
        public List<string> script2 { get; }
        public List<int> difs { get; }
        public string statusText { get; }
    }
}
