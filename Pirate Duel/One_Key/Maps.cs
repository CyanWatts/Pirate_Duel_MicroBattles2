using System;
using System.Collections.Generic;
using System.Drawing;   //////// For Rectangle
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_Key   // 地图类
{   //////// Class Maps 存地形所在的矩形
    class Maps  // 地图类
    {
        public static int judgeheight = 10;  // 仅上边沿检测碰撞
        public static Rectangle[] edge;
        

        public static Block[] getmap1()  //////// 获得地图所有Block( int index )所有地图能否整合成一个？
        {
            // init Blocks
            int num_blocks = 4;
            Block[] blocks = new Block[num_blocks];
            Block ground = new Block(0, 360, 880, 40);
            Block b1 = new Block(0, 200, 280, 40);
            Block b2 = new Block(600, 200, 280, 40);
            Block b3 = new Block(350, 260, 180, 40);    // Version 2.0 Revised // Block b3 = new Block(340, 260, 200, 40);

            blocks[0] = ground;
            blocks[1] = b1;
            blocks[2] = b2;
            blocks[3] = b3;

            int i;
            edge = new Rectangle[num_blocks];
            for (i = 0; i < num_blocks; i++)
            {
                edge[i] = new Rectangle(blocks[i].position_x,
                   blocks[i].position_y,
                   blocks[i].width,
                   judgeheight);
            }

            // 关于相交
            // Rectangle    Rectangle.Intersect      得交集
            // Bool         r1.IntersectsWith(r2)    判断相交

            return blocks;
        }
    }
}
