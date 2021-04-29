using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;   // for Rectangle

namespace One_Key   // 刀子类
{
    class Knife // 刀子类
    {
        public bool isInHand;   // 是否在手上：0:不在手上（在空中或者地上） / 1：在手上
        public bool isOnFloor;  // 在地上（可拾取）
        public bool isRising;   // 0：在下降 / 1:在上升， 主要用于只在刀子下降时碰触地面才会插到地上OnFloor，上升时不会

        public Velocity v;
        public Rectangle knife;     // 1.图形显示矩形 是否有用存疑！！！！！！！！！ -> 用于判断碰撞！！！！！
        public Pirate pirate = null;       // 在谁手上
        public Pirate Prepirate = null;    // 用于自己的刀子不杀自己的判断


        public int position_x;          // 2.简易版像素块显示
        public int position_y;
        public static int width = 20, height = 40;  // 像素块大小 

        public Knife(int x, int y)      // 构造函数，初始化
        {
            isInHand = true;
            isOnFloor = false;
            isRising = true;


            position_x = x;
            position_y = y;

        }

        public void UpdateVelocity()    // 多种情况的速度更新
        {
            if (pirate != null)            // 在手上
            {
                // v = pirate.v             // 
                v.X = pirate.v.X;
                v.Y = pirate.v.Y;          // 问题13：不能直接赋值v，是引用，共用内存，之后修改Knife的v也会修改Pirate的v
                
            }
            else
            {
                if (isOnFloor)           // 在地上
                {
                    // 不操作
                }
                else                    // 在天上
                {
                    v = Velocity.Gravity(v);// 重力影响
                    if (v.Y > 0)
                        isRising = false;
                }
            }

        }

        public void calculate_Speed(Point center, Point p, double Strength) // 根据角度（方向向量）计算刀子速度
        {
            // 恒定速度
            //v.X = 8*v.X;                    //  21   // test speed：
            //v.Y = - 10*Velocity.init_v_y;   // -20

            int x_difference = p.X - center.X;
            int y_difference = p.Y - center.Y;

            v.X = (int)(Strength * x_difference);
            v.Y = (int)(Strength * y_difference);

        }

        public void KnifeMove()         // 刀子（像素块）一步移动
        {
            position_x += v.X;
            position_y += v.Y;
        }



    }
}
