using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DBComparer
{
    public class Helper
    {
        public static SortedDictionary<int, string> GetFullPath(TreeViewEventArgs e)
        {
            SortedDictionary<int, string> route = new SortedDictionary<int, string>();
            int iLevel = e.Node.Level;
            TreeNode tn = e.Node;
            do
            {
                route.Add(iLevel, tn.Text);
                if (iLevel > 0)
                {
                    iLevel--;
                    tn = tn.Parent;
                }
            } while (iLevel > 0);
            if(!route.ContainsKey(iLevel))
                route.Add(iLevel, tn.Text);
            return route;
        }
    }
}
