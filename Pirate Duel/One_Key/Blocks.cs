using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_Key   // 砖块类
{
    class Block     // 砖块类
    {
        public int position_x;
        public int position_y;
        public int width;
        public int height;

        public Block()
        {

        }

        public Block(int px, int py, int wi, int he)
        {
            position_x = px;
            position_y = py;
            width = wi;
            height = he;
        }
    }
}
