using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace One_Key   // 电脑策略类
{
    class RL    // 电脑策略类
    {
        
        public static bool ChooseAction_Random()
        {
            int a = GetRandom.GetRandomInt(3);
            // MessageBox.Show(a.ToString());
            if ( a == 0 )
            {
                return true;
            }
            else
                return false;
        }
    }
}
