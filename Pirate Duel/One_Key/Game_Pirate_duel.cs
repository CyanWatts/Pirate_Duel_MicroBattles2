using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Runtime.InteropServices;   // for audio play
//using System.Windows.Forms;

namespace One_Key   // 游戏类
{
    class Game_Pirate_duel  // 游戏类
    {

        public static Knife knife1;
        public static Knife knife2;
        public static Pirate pirate1;
        public static Pirate pirate2;
        public static int init_v_x = 3;     // init x方向速度绝对值
        public static Knife[] knives = new Knife[2];   // 刀子集合
        
        public static int SpeedIntoFloor = 15;  // 能插入地面的Y速度

        public static Block[] getmap1()
        {
            return Maps.getmap1();
        }



        public static void Gamestart()
        {
            
            knife1 = new Knife(90, 150);
            knife2 = new Knife(770, 150);

            knives[0] = knife1;
            knives[1] = knife2;         // 刀子集合

            pirate1 = new Pirate(110, 150);
            pirate2 = new Pirate(720, 150);

            // 刀子所有权
            pirate1.holdingKnife = knife1;
            pirate2.holdingKnife = knife2;

            knife1.pirate = pirate1;
            knife2.pirate = pirate2;

            // Prepirate
            knife1.Prepirate = pirate1;
            knife2.Prepirate = pirate2;

            // 所在砖块 
            // pirate1.floor = Maps.edge[1];
            pirate1.floor = new Rectangle(0, 200, 280, Maps.judgeheight);
            // pirate2.floor = Maps.edge[2];
            pirate2.floor = new Rectangle(600, 200, 280, Maps.judgeheight);

            // 速度初始化
            knife1.v = new Velocity(init_v_x, 0);
            knife2.v = new Velocity(-init_v_x, 0);

            pirate1.v = new Velocity(init_v_x, 0);
            pirate2.v = new Velocity(-init_v_x, 0);

            // 分数初始化为0  -> 转为在ScoreTable中的Init，由Form1调用
            // ScoreTable.ScoreOfPlayer1 = 0;
            // ScoreTable.ScoreOfPlayer2 = 0;

        }

        public static bool HitWall(Pirate p)
        {
            if (p.v.X > 0)     // 向右走
            {
                if (p.position_x + Pirate.width >= 880)
                {
                    p.v.X = -p.v.X;

                    if(p.holdingKnife!=null)
                    {
                        p.holdingKnife.position_x += Pirate.width;    // 50 Pirate 宽 //+ Knife 宽
                        p.position_x -= Knife.width;                 // 20 Knife 宽
                    }
                    return true;    

                }
                    
            }
            else               // 向左走
            {
                if (p.position_x <= 0)
                {
                    p.v.X = -p.v.X;
                    if (p.holdingKnife != null)
                    {
                        p.holdingKnife.position_x -= Pirate.width;    // Pirate 宽
                        p.position_x += Knife.width;                 // Knife 宽
                    }
                    return true;
                }
                    
            }
            return false;
        }
        
        public static bool HitWall(Knife k)    // 空中反弹判断
        {
            // 不在空中
            if (k.isOnFloor || k.isInHand)     
                return false;

            // 在空中
            else if (k.v.X > 0)     // 向右走
            {
                if (k.position_x + Knife.width >= 880)
                {
                    k.v.X = -k.v.X;
                    return true;
                }

            }
            else               // 向左走
            {
                if (k.position_x <= 0)
                {
                    k.v.X = -k.v.X;
                    return true;
                }

            }
            return false;
        }

        public static bool HitFloor(Pirate p)  // 处理与地面碰撞
        {
            if (p.isOnFloor)  // 在地上
            {
                if (p.foot.IntersectsWith(p.floor))   // 还在block上
                    return false;
                else                                    // 走下block
                {
                    p.isOnFloor = false;                // 速度处理此处未作 !!!!!!!!!!!!!!!!
                    
                    //p.v = p.v + new Velocity(0, 10);  // 测试用!!!!!!!!!!
                    return false;
                }

            }

            else                // 在天上（p.floor为空
            {
                if (p.isRising) // 上升不判断
                    return false;
                else            // 在天上 且 下降 
                {
                    if (Maps.edge == null)  // 地板边界空时直接跳出（刚开始
                        return false;
                    int i;
                    //bool flag = false;
                    int n = Maps.edge.GetLength(0);
                    for (i = 0; i < n; i++)
                    {
                        //if (p.pirate.IntersectsWith(Maps.edge[i]))
                        //{
                        //    p.isOnFloor = true; // isRising在跳跃动作时修改 
                        //    p.floor = Maps.edge[i];
                        //    p.v.Y = 0;           // 竖直速度归零

                        //}
                        if (p.foot.IntersectsWith(Maps.edge[i]))
                        {
                            p.isOnFloor = true; // isRising在跳跃动作时修改 
                            p.floor = Maps.edge[i];
                            p.v.Y = 0;           // 竖直速度归零
                            p.position_y = Maps.edge[i].Y - Pirate.height;
                            if(p.holdingKnife!=null)
                            {
                                p.holdingKnife.position_y = p.position_y;
                            }
                            return true;
                        }
                    }
                    return false;
                }

            }
        }

        public static bool HitFloor(Knife k)
        {
            // 在手上不判断
            if (k.pirate != null)
                return false;

            // 在地上不判断
            else if (k.isOnFloor)
                return false;

            // 在天上
            else if (k.isRising) // 上升不判断
                return false;
            else            // 在天上 且 下降 
            {
                if (Maps.edge == null)  // 地板边界空时直接跳出（刚开始
                    return false;
                int i;
                int n = Maps.edge.GetLength(0);
                for (i = 0; i < n; i++)
                {
                    if (k.knife.IntersectsWith(Maps.edge[i]))
                    {
                        k.isOnFloor = true; // isRising在跳跃动作时修改

                        if(Math.Abs(k.v.Y)< SpeedIntoFloor)  // 解决ScoreTable 25.
                            k.position_y = Maps.edge[i].Y - Knife.height;   
                        else
                            k.position_y = Maps.edge[i].Y - Knife.height/2;

                        k.v.Y = 0;           
                        k.v.X = 0;           // 速度归零
                        return true;
                    }
                }
                
            }
            return false;
        }

        public static bool pickUpKnife(Pirate p)   // Pirate拾取刀子 碰撞检测
        {
            if(p.holdingKnife!=null)        // 手中有刀子
            {
                // nop 空操作
                return false;
            }
            else                            // 手中没刀子，碰撞检测
            {
                int i;
                for(i=0;i<2;i++)
                {
                    if (!knives[i].isOnFloor)   // 刀子没在地上
                        continue;
                    else                        // 在地上，开始对knives[i]检测
                    {
                        if(knives[i].knife.IntersectsWith(p.pirate))    // 拾取
                        {
                            // for knife
                            knives[i].isInHand = true;
                            knives[i].isOnFloor = false;
                            //knives[i].isRising = true;    // 存疑？？？？？？？？似乎没用
                            knives[i].pirate = p;
                            knives[i].Prepirate = p;
                            knives[i].UpdateVelocity();  // 存疑？？？？？？？？？
                            
                            if(p.v.X>0)                 // 向右走
                            {
                                knives[i].position_x = p.position_x - Knife.width;
                                knives[i].position_y = p.position_y;
                            }
                            else                        // 向左走        
                            {
                                knives[i].position_x = p.position_x + Pirate.width;
                                knives[i].position_y = p.position_y;
                            }
                            

                            // for pirate
                            p.holdingKnife = knives[i];
                            return true;                  // 代替break
                            //break;                      // 防止一次拿两把刀，导致有一把卡在身上

                        }
                    }
                }
            }
            return false;
        }

        public static bool IsKill(Pirate p)
        {

            if ((knives[0].isInHand || knives[0].isOnFloor)
             && (knives[1].isInHand || knives[1].isOnFloor))   // 不可能砍人
                return false;
            else                                               // 至少有一个在空中 -> 有可能砍人 -> 自己的刀不砍自己: Knife类加一个PrePirate引用
            {
                int i;
                for(i=0;i<2;i++)
                {
                    if (knives[i].isInHand || knives[i].isOnFloor)
                        continue;
                    else                    // 在空中 
                    {
                        if(knives[i].Prepirate!=p)   // 不是自己扔的
                            if(knives[i].knife.IntersectsWith(p.pirate))    // 别人砍到了p
                            {
                                return true;
                            }
                    }
                }
            }
            return false;
        }

    }
}
