using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;   // for Rectangle
//using System.Runtime.InteropServices;   // for audio play
//using System.Windows.Forms;

namespace One_Key   // 海盗类
{
    class Pirate    // 海盗类
    {


        public bool isOnFloor;  // 在地上 / 在空中
        public bool isRising;   // 0：在下降 / 1:在上升， 下降时脚碰到地面会落地
        public bool isDying;    // 本轮死亡：用于死亡效果的判断

        public Velocity v;      // 速度

        public Knife holdingKnife = null;   // 指向自己有的刀子
        public Rectangle floor;          // 指向所在的地面  -> 减少相交判断复杂度！！！！！！！！
        public int jumpStrength = 11;
        //public Knife temKnife = null;       // 扔刀子时候的临时变量

        public double Strength = 0.43;   // 力量参数：调整速度快慢的系数 推荐Value: 0.43

        public Rectangle pirate;    // 显示矩形 是否有用存疑！！！！！！！！！ // 目前像素块实现就需要了：判断与刀子相交
        public Rectangle foot;      // 显示矩形 是否有用存疑！！！！！！！！！ // 目前像素块实现就需要了：判断落地相交而不是用于显示

        public int position_x;          // 2.简易版像素块显示所需海盗位置坐标
        public int position_y;
        public static int width = 50, height = 50;  // 像素块大小 

        

        // 准星
        public Point Collimator_center;     // 中心基准点
        public Point Collimator_basep;      // 移动基准点
        public Point Collimator_p;          // 实际位置
        // 遍历以实现投掷角度切换
        //public static double[] CollimatorAngles = { 0, 15, 30, 45, 60, 75, 90, 75, 60, 45, 30, 15, 0, 330, 300, 330};
        //public static int N_CollimatorAngles = 16;
        // 改为构造函数中的自动生成
        public double[] CollimatorAngles;
        public int goLeft;

        public static double AngleStep = 7.5;// 角度数组CollimatorAngles粒度
        public static int N_CollimatorAngles;
        public int CollimatorCounter;           // 更新用Counter
        public int RoateCounter;
        public int ConllimatorSize;         // 像素块大小



        public Pirate(int x, int y)       // 构造函数，初始化
        {
            isOnFloor = true;
            isRising = false;             //////// true
                                          //v = floor_v;
            isDying = false;

            Collimator_center = new Point(0, 0);
            Collimator_basep = new Point(0, 0);
            Collimator_p = new Point(0, 0);
            CollimatorCounter = 0;
            RoateCounter = 0;
            //Strength = 0.5;

            // CollimatorAngles 数组初始化
            {
                ConllimatorSize = 10;   // 5
                CollimatorAngles = new double[100];
                N_CollimatorAngles = 0;
                int i;
                for (i = 0; i <= 90 / AngleStep; i++)
                {
                    CollimatorAngles[N_CollimatorAngles] = AngleStep * i;
                    N_CollimatorAngles++;
                }
                for (i -= 2; i >= -60 / AngleStep; i--)
                {
                    CollimatorAngles[N_CollimatorAngles] = AngleStep * i;
                    N_CollimatorAngles++;
                }
                for (i += 2; i < 0; i++)
                {
                    CollimatorAngles[N_CollimatorAngles] = AngleStep * i;
                    N_CollimatorAngles++;
                }
            }
            // 初始化完毕

            position_x = x;
            position_y = y;

        }

        //public int max(int a, int b)
        //{
        //    if (a > b)
        //        return a;
        //    else
        //        return b;
        //}

        //public int min(int a, int b)
        //{
        //    if (a > b)
        //        return b;
        //    else
        //        return a;
        //}

        public int Action()            // int:用于操作情况判断
        {
            if (holdingKnife == null)   // 没刀子：跳
                return Jump();
            else                        // 有刀子：扔刀子
                return ThrowKnife();    // ******************0:无声音  1:跳跃  2:扔刀子************************
        }

        public int Jump()       // int:用于操作情况判断
        {
            if(isOnFloor)       // 在地上，允许跳跃
            {
                isOnFloor = false;
                isRising = true;
                 
                v.Y = - jumpStrength * Velocity.init_v_y;       // !!!!!!!!!!!!! 直接赋值 -> 10倍重力
                return 1;
            }
            else                // 在天上，不做任何修改
            {
                return 0;
            }
        }

        public void Dies()      // 死亡
        {
            isDying = true;
            v.X = 0;
        }

        public int  ThrowKnife()
        {
            if(holdingKnife==null)  // 手上没刀
            {
                // nop 空操作
                return 0;
            }
            else                    // 手上有刀
            {
                holdingKnife.pirate = null;
                holdingKnife.isInHand = false;
                holdingKnife.isRising = true;

                holdingKnife.calculate_Speed(Collimator_center, Collimator_p, Strength); 
                
                Knife temKnife = holdingKnife;
                temKnife.pirate = null;
                holdingKnife = null;
                return 2;
            }
        }

        public double AngleToRadian(double angle)       // 角度转弧度，因为三角函数要用弧度
        {
            return angle * Math.PI / 180.0;
        }

        public Point PointRotate(Point center, Point p1, double angle)
        {
            angle = AngleToRadian(angle);
            double x1 = (p1.X - center.X) * Math.Cos(angle) + (p1.Y - center.Y) * Math.Sin(angle) + center.X;
            double y1 = -(p1.X - center.X) * Math.Sin(angle) + (p1.Y - center.Y) * Math.Cos(angle) + center.Y;
            p1.X = (int)x1;
            p1.Y = (int)y1;
            return p1;
        }

        public void UpdateCollimator()            // 更新准星
        {
            //if((RoateCounter++)%3 == 0)         // 控制准星切换速度
            //{
                
            if (v.X > 0)       // 向右走
            {
                goLeft = -1;
                Collimator_center.X = position_x;
            }
            else if(v.X < 0)              // 向左走
            {
                goLeft = 1;
                Collimator_center.X = position_x + width;
            }

            Collimator_center.Y = position_y + height / 2;    // 中心基准点

            Collimator_basep.X = Collimator_center.X;
            Collimator_basep.Y = position_y - width;        // 移动原点

            Collimator_p = PointRotate(Collimator_center, Collimator_basep,
                goLeft * CollimatorAngles[(CollimatorCounter++) % N_CollimatorAngles]);
            //}    
        }

        public void UpdateVelocity()    // 多种情况的速度更新
        {
            // 更新速度
            if (isOnFloor)               // 在地上
            {
                // 匀速
            }
            else                        // 在天上
            {
                v = Velocity.Gravity(v);
                if (v.Y > 0)
                    isRising = false;
            }
        }

        public void PirateMove()        // 海盗（像素块）一步移动
        {
            position_x += v.X;
            position_y += v.Y;
        }


    }
}
