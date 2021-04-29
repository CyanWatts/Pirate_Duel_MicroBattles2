using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; // for sleep()
using System.Runtime.InteropServices;   // for audio play



namespace One_Key   // Pirate duel
{
    public partial class Form1 : Form
    {
        // 防止一次被砍多次计分
        bool flagOfPirate1 = true;      // pirate1被砍给pirate2计分
        bool flagOfPirate2 = true;      // pirate2被砍给pirate1计分
        bool PreflagOfPirate1 = true;   // 解决ScoreTable 1.中同时杀死卡住的问题
        bool PreflagOfPirate2 = true;

        //bool scores = false;            // 有人得分 -> 重开游戏
        //int count = 0;                  // for testing
        bool notEnd = true;               // 防止MessageBox弹出多次？？？？？？？？？？？？原因未知
        bool Pirate1_ActionEnable = true;          // 死亡时不可操作
        bool Pirate2_ActionEnable = true;
        bool bSounding = false;   // 避免声音卡顿？？？？？？？？？？？？？？？？？？？？

        bool PvPMode = true;        // 模式bool量

        //Image imgKnife;               //************************************//

        [DllImport("winmm")]
        public static extern bool PlaySound(string szSound, int hMod, int i);   // for audio plays

        public Form1()
        {
            InitializeComponent();
        }

        //public static Bitmap LoadBmp(string bmpFileName)
        //{
        //    //return new Bitmap(Application.StartupPath + "\\GamePictures\\" + bmpFileName + ".bmp");
        //    return new Bitmap(Application.StartupPath + "\\GamePictures\\" + bmpFileName + ".png");
        //}

        //public void LoadPictures()
        //{
        //    bmpknife = LoadBmp("n1"); ;//位图
        //}

        //public void DrawKnife_Img()
        //{
        //    //Graphics.DrawImage(bmpknife, new PointF(100,100));
        //    Image img = Image.FromFile(Application.StartupPath + "\\GamePictures\\" + "n1" + ".png");
        //    Graphics.DrawImage(img, 0, 0);
            
        //}



        // 没有Refresh，方法体执行期间界面保持原样，方法体执行完后才绘制方法体中所做的更改。
        // 有Refresh，会强制立即绘制刚才所做的更改，无论方法体是否执行完。

        // 准星绘制
        public void DrawCollimator()
        {

            Graphics g = this.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Black);
            if (Game_Pirate_duel.pirate1.holdingKnife != null)
                g.FillRectangle(myBrush, new Rectangle(Game_Pirate_duel.pirate1.Collimator_p.X,
                    Game_Pirate_duel.pirate1.Collimator_p.Y,
                    Game_Pirate_duel.pirate1.ConllimatorSize,
                    Game_Pirate_duel.pirate1.ConllimatorSize));
            if (Game_Pirate_duel.pirate2.holdingKnife != null)
                g.FillRectangle(myBrush, new Rectangle(Game_Pirate_duel.pirate2.Collimator_p.X,
                    Game_Pirate_duel.pirate2.Collimator_p.Y,
                    Game_Pirate_duel.pirate2.ConllimatorSize,
                    Game_Pirate_duel.pirate2.ConllimatorSize));
        }

        // 死亡闪烁
        public void DrawKill()              
        {
            Graphics g = this.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Red);
            g.FillRectangle(myBrush, new Rectangle(0, 0, 880, 450));
        }   

        //////// DrawMap
        public void DrawMap()
        {
            Block[] blocks = Game_Pirate_duel.getmap1();    //////// Maps.getmap1();
            int len = blocks.Length;
            Rectangle[] mrectangles = new Rectangle[len];   // ShowBlock也是每次绘制把不动的砖块Draw了
            int i;
            // 画每块Block
            for (i = 0; i < len; i++)
            {
                mrectangles[i] = new Rectangle(blocks[i].position_x,
                    blocks[i].position_y,
                    blocks[i].width,
                    blocks[i].height);
            }//////// 绘制所用Rectangle
            Graphics g = this.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Goldenrod);
            g.FillRectangles(myBrush, mrectangles);
            // 画边界
            //this.Refresh();
            Pen mypen = new Pen(Color.DimGray);
            g.DrawLine(mypen, new Point(0, 0), new Point(880, 0));
            g.DrawLine(mypen, new Point(0, 0), new Point(0, 450));
            g.DrawLine(mypen, new Point(880, 450), new Point(0, 450));
            g.DrawLine(mypen, new Point(880, 450), new Point(880, 0));


        }

        public void DrawKnife() //////// 画蛇用了Refresh，画食物没用
        {
            Rectangle[] krectangles = new Rectangle[2];
            krectangles[0] = new Rectangle(Game_Pirate_duel.knife1.position_x,
                Game_Pirate_duel.knife1.position_y,
                Knife.width, Knife.height);
            krectangles[1] = new Rectangle(Game_Pirate_duel.knife2.position_x,
                Game_Pirate_duel.knife2.position_y,
                Knife.width, Knife.height);

            //////// 引用指向自己矩形
            Game_Pirate_duel.knife1.knife = krectangles[0];
            Game_Pirate_duel.knife2.knife = krectangles[1];
            ////////

            ///
            //this.Refresh();     //////// 画蛇用了Refresh，画食物没用
            //this.Invalidate();
            Graphics g = this.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Gray);
            g.FillRectangles(myBrush, krectangles);
        }

        public void DrawPirate()
        {
            Rectangle[] prectangles = new Rectangle[2];
            prectangles[0] = new Rectangle(Game_Pirate_duel.pirate1.position_x,
                Game_Pirate_duel.pirate1.position_y,
                Pirate.width, Pirate.height);
            prectangles[1] = new Rectangle(Game_Pirate_duel.pirate2.position_x,
                Game_Pirate_duel.pirate2.position_y,
                Pirate.width, Pirate.height);

            this.Refresh(); //////// 2个Refresh都注释掉的话会拉长，都使用的话后一个调用的会消失
            //this.Invalidate();
            Graphics g = this.CreateGraphics();
            SolidBrush myBrush1, myBrush2;
            if (!Game_Pirate_duel.pirate1.isDying)
                myBrush1 = new SolidBrush(Color.LightCoral);
            else
                myBrush1 = new SolidBrush(Color.DarkRed);
            g.FillRectangle(myBrush1, prectangles[0]);

            if (!Game_Pirate_duel.pirate2.isDying)
                myBrush2 = new SolidBrush(Color.SteelBlue);
            else
                myBrush2 = new SolidBrush(Color.DarkBlue);
            g.FillRectangle(myBrush2, prectangles[1]);
        }

        private void TimerRefreshScreen_Tick(object sender, EventArgs e)
        {
            DrawPirate();
            DrawKnife();
            DrawMap();
            DrawCollimator();
        }

        private void TimerUpdate_Tick(object sender, EventArgs e)   // 更新时钟
        {
            // 更新速度并更改位置

            // 更新与部分碰撞检测
            {
                Game_Pirate_duel.pirate1.UpdateVelocity();
                Game_Pirate_duel.pirate1.PirateMove();
                Game_Pirate_duel.pirate2.UpdateVelocity();
                Game_Pirate_duel.pirate2.PirateMove();

                Game_Pirate_duel.pirate1.UpdateCollimator();
                Game_Pirate_duel.pirate2.UpdateCollimator();

                Game_Pirate_duel.knife1.UpdateVelocity();
                Game_Pirate_duel.knife1.KnifeMove();
                Game_Pirate_duel.knife2.UpdateVelocity();
                Game_Pirate_duel.knife2.KnifeMove();

                // 更新用于碰撞检测的Rectangle
                // 1.Pirate.pirate
                Game_Pirate_duel.pirate1.pirate = new Rectangle(Game_Pirate_duel.pirate1.position_x,
                    Game_Pirate_duel.pirate1.position_y,
                    Pirate.width, Pirate.height);
                Game_Pirate_duel.pirate2.pirate = new Rectangle(Game_Pirate_duel.pirate2.position_x,
                    Game_Pirate_duel.pirate2.position_y,
                    Pirate.width, Pirate.height);

                // 2.Pirate.foot
                Game_Pirate_duel.pirate1.foot = new Rectangle(Game_Pirate_duel.pirate1.position_x,
                    Game_Pirate_duel.pirate1.position_y + 40,
                    Pirate.width, 20);           //  10个单位高的脚
                Game_Pirate_duel.pirate2.foot = new Rectangle(Game_Pirate_duel.pirate2.position_x,
                    Game_Pirate_duel.pirate2.position_y + 40,
                    Pirate.width, 20);           //  
                                                 // 3.Knife.knife
                Game_Pirate_duel.knife1.knife = new Rectangle(Game_Pirate_duel.knife1.position_x,
                    Game_Pirate_duel.knife1.position_y,
                    Knife.width, Knife.height);           //  
                Game_Pirate_duel.knife2.knife = new Rectangle(Game_Pirate_duel.knife2.position_x,
                    Game_Pirate_duel.knife2.position_y,
                    Knife.width, Knife.height);


                Game_Pirate_duel.HitFloor(Game_Pirate_duel.pirate1);
                Game_Pirate_duel.HitFloor(Game_Pirate_duel.pirate2);

                Game_Pirate_duel.HitWall(Game_Pirate_duel.pirate1);
                Game_Pirate_duel.HitWall(Game_Pirate_duel.pirate2);
                
                // 刀子碰墙壁声音地面声音
                if (Game_Pirate_duel.HitFloor(Game_Pirate_duel.knife1) && !bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "knifedown" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }
                if (Game_Pirate_duel.HitFloor(Game_Pirate_duel.knife2) && !bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "knifedown" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }

                // 刀子碰墙壁声音
                if (Game_Pirate_duel.HitWall(Game_Pirate_duel.knife1) && !bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "knifehitwall" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }
                if (Game_Pirate_duel.HitWall(Game_Pirate_duel.knife2) && !bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "knifehitwall" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }

                // 捡起刀子声音
                if (Game_Pirate_duel.pickUpKnife(Game_Pirate_duel.pirate1) && !bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "pickupknife" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }
                if (Game_Pirate_duel.pickUpKnife(Game_Pirate_duel.pirate2) && !bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "pickupknife" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }

                //// 走路声音
                //if ((Game_Pirate_duel.pirate1.isOnFloor || Game_Pirate_duel.pirate2.isOnFloor) && !bSounding)
                //{
                //    PlaySound(Application.StartupPath + "\\GameSounds\\" + "run" + ".mp3", 0, 1);
                //    bSounding = true;
                //    TimerAfterSound.Start();
                //}
            }


            // 死亡碰撞检测   

            // (已完成)目前在对第二个，即Player1得分情况进行修改，完善之后对称修改到第一个中

            // Pirate1被杀死
            if (Game_Pirate_duel.IsKill(Game_Pirate_duel.pirate1) && flagOfPirate1)
            {
                
                if (!bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "hit" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }
                Pirate1_ActionEnable = false;
                flagOfPirate1 = false;
                ScoreTable.Score(2);
                DrawKill();
                Game_Pirate_duel.pirate1.Dies();
                label2.Text = "Player2:" + ScoreTable.ScoreOfPlayer2.ToString();

            }
            // Pirate2被杀死
            if (Game_Pirate_duel.IsKill(Game_Pirate_duel.pirate2) && flagOfPirate2)
            {
                
                if (!bSounding)
                {
                    PlaySound(Application.StartupPath + "\\GameSounds\\" + "hit" + ".wav", 0, 1);
                    bSounding = true;
                    TimerAfterSound.Start();
                }
                Pirate2_ActionEnable = false;
                flagOfPirate2 = false;
                ScoreTable.Score(1);
                DrawKill();
                Game_Pirate_duel.pirate2.Dies();
                label1.Text = "Player1:" + ScoreTable.ScoreOfPlayer1.ToString();

            }

            // 延迟部分，要在判断之前
            // 目标是解决ScoreTable 问题1.中BUG

            if ((flagOfPirate1 == true && flagOfPirate2 == false)
                || (flagOfPirate2 == true && flagOfPirate1 == false)// 前两个：有且仅有一个干掉了另一方
                || (flagOfPirate1 == false && flagOfPirate1 == false && PreflagOfPirate1 == true && PreflagOfPirate2 == true))
            {                                                       // 后一个：同时干掉了对方
                //Thread.Sleep(1000);                 // 砍完暂停一秒（取消）
                TimerRefreshScreen.Interval = 50;   // 暂停后慢动作
                // 继续绘制2秒,被干掉的显示出被干掉的样子取消被杀死判断，
                // 没被干掉的继续绘制并且继续被杀死判断

                // KeyEnable = false;              // 停止操作  -> 后来发现：好像不用
                TimerEndRefresh.Enabled = true;

            }

            // 更新Preflag
            {
                PreflagOfPirate1 = flagOfPirate1;
                PreflagOfPirate2 = flagOfPirate2;
            }



        }

        private void button1_Click(object sender, EventArgs e)  // Game Start 玩家对战PVP 
        {
            //this.Size = new Size(200, 200);                   // 可改变窗体大小

            //Form1 F1 = new Form1();
            //Size s = new Size(200, 200);
            //F1.Size = s;
            //Application.Run(F1);

            PvPMode = true;

            ScoreTable.Init();
            Game_Pirate_duel.Gamestart();
            label1.Text = "Player1:" + ScoreTable.ScoreOfPlayer1.ToString();
            label2.Text = "Player2:" + ScoreTable.ScoreOfPlayer2.ToString();

            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;

            notEnd = true;
            Pirate1_ActionEnable = true;
            Pirate2_ActionEnable = true;

            flagOfPirate1 = true;      // pirate1被砍给pirate2计分
            flagOfPirate2 = true;      // pirate2被砍给pirate1计分
            PreflagOfPirate1 = true;
            PreflagOfPirate2 = true;


            //DrawMap();    // 只Draw一次会消失
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            //button2.Visible = false;
            TimerRefreshScreen.Enabled = true;
            TimerUpdate.Enabled = true;
            ////////##########
            TimerEndRefresh.Enabled = false;
            TimerComAct.Enabled = false;
            ////////##########
        }


        //public double AngleToRadian(double angle)       // 角度转弧度，因为三角函数要用弧度
        //{
        //    return angle * Math.PI / 180.0;
        //}

        //public Point PointRotate(Point center, Point p1, double angle)
        //{
        //    angle = AngleToRadian(angle);
        //    double x1 = (p1.X - center.X) * Math.Cos(angle) + (p1.Y - center.Y) * Math.Sin(angle) + center.X;
        //    double y1 = -(p1.X - center.X) * Math.Sin(angle) + (p1.Y - center.Y) * Math.Cos(angle) + center.Y;
        //    p1.X = (int)x1;
        //    p1.Y = (int)y1;
        //    return p1;
        //}


        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        public void PirateAction(int Action_Num)
        {
            if (Action_Num == 0)        // 没有实际tion
                return;
            // 跳跃声音
            else if(Action_Num == 1)    // Jump
            {
                PlaySound(Application.StartupPath + "\\GameSounds\\" + "jump" + ".mp3", 0, 1);
                bSounding = true;
                TimerAfterSound.Start();
            }
            // 扔刀子声音
            else if(Action_Num == 2)    // ThorwKnife
            {
                PlaySound(Application.StartupPath + "\\GameSounds\\" + "knifewave" + ".wav", 0, 1);
                bSounding = true;
                TimerAfterSound.Start();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)   // 操作
        {
            // Action
            if (e.KeyCode == Keys.A && Pirate1_ActionEnable && Game_Pirate_duel.pirate1!=null)     // 操作键按下 且 没死掉
            {   // 按下A              // Pirate1可以Action(还活着)     // 游戏已经开始
                PirateAction(Game_Pirate_duel.pirate1.Action());
            }

            else if (e.KeyCode == Keys.L && Pirate2_ActionEnable && Game_Pirate_duel.pirate2 != null && PvPMode)// 操作键按下 且 没死掉
            {        // 按下A              // Pirate1可以Action(还活着)     // 游戏已经开始             // 且为玩家对战模式
                PirateAction(Game_Pirate_duel.pirate2.Action());
            }
        }

        private void TimerEndRefresh_Tick(object sender, EventArgs e)
        {
            //EndingDraw();

            switch (ScoreTable.IsEnd())         // 结束判断，在重开游戏之前
            {
                case 0: break;
                case 1:     // Pirate1 Wins
                    if(notEnd)                  // notEnd：防止多次MessageBox弹窗  待确定！！！！！！！！！！1
                    {
                        notEnd = false;
                        //PlaySound(Application.StartupPath + "\\GameSounds\\" + "victory1" + ".wav", 0, 1);
                        PlaySound(Application.StartupPath + "\\GameSounds\\" + "victory2" + ".wav", 0, 1);
                        MessageBox.Show("Player1 Wins!");

                        label3.Text = "Player1 Wins! Play Again:";
                        label3.Visible = true;
                        TimerUpdate.Enabled = false;
                        TimerEndRefresh.Enabled = true;///#########
                        button1.Visible = true;
                        button2.Visible = true;
                        button3.Visible = true;
                    }
                    break;
                case 2:     // Pirate2 Wins
                    if (notEnd)                  // notEnd：防止多次MessageBox弹窗  待确定！！！！！！！！！！1
                    {
                        notEnd = false;
                        //PlaySound(Application.StartupPath + "\\GameSounds\\" + "victory" + ".mp3", 0, 1);
                        PlaySound(Application.StartupPath + "\\GameSounds\\" + "victory2" + ".wav", 0, 1);
                        MessageBox.Show("Player2 Wins!");
                        
                        label3.Text = "Player2 Wins! Play Again:";
                        label3.Visible = true;
                        TimerUpdate.Enabled = false;
                        TimerEndRefresh.Enabled = true;///#########
                        button1.Visible = true;
                        button2.Visible = true;
                        button3.Visible = true;
                    }
                    break;
                case 3:     // Tie Game
                    if (notEnd)                  // notEnd：防止多次MessageBox弹窗  待确定！！！！！！！！！！1
                    {
                        notEnd = false;
                        PlaySound(Application.StartupPath + "\\GameSounds\\" + "tiegame" + ".wav", 0, 1);
                        MessageBox.Show("It's a Tie! Good Game!");

                        label3.Text = "It's a Tie! GG! Play Again:";
                        label3.Visible = true;
                        TimerUpdate.Enabled = false;
                        TimerEndRefresh.Enabled = true;///#########
                        button1.Visible = true;
                        button2.Visible = true;
                        button3.Visible = true;
                    }
                    break;
                default:
                    MessageBox.Show("Error occurs at 'TimerUpdate_Tick.switch'");
                    break;
            }           // 结束判断，在重开游戏之前

            //Pirate1_ActionEnable = false;
            //Pirate2_ActionEnable = false;

            //Thread.Sleep(2000);
            Game_Pirate_duel.Gamestart();
            flagOfPirate1 = true;
            flagOfPirate2 = true;
            TimerEndRefresh.Enabled = false;
            TimerRefreshScreen.Interval = 25;
            Pirate1_ActionEnable = true;
            Pirate2_ActionEnable = true;

        }

        private void TimerAfterSound_Tick(object sender, EventArgs e)
        {
            bSounding = false;
            TimerAfterSound.Stop();
        }
        private void button2_Click(object sender, EventArgs e)  // 人机对战PVE开始
        {
            PvPMode = false;
            ScoreTable.Init();
            Game_Pirate_duel.Gamestart();
            label1.Text = "Player1:" + ScoreTable.ScoreOfPlayer1.ToString();
            label2.Text = "Player2:" + ScoreTable.ScoreOfPlayer2.ToString();
            
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;

            notEnd = true;
            Pirate1_ActionEnable = true;
            Pirate2_ActionEnable = true;

            flagOfPirate1 = true;      // pirate1被砍给pirate2计分
            flagOfPirate2 = true;      // pirate2被砍给pirate1计分
            PreflagOfPirate1 = true;
            PreflagOfPirate2 = true;

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            TimerRefreshScreen.Enabled = true;
            TimerUpdate.Enabled = true;
            TimerEndRefresh.Enabled = false;
            TimerComAct.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)  // 退出
        {
            this.Close();
            //DrawKnife_Img();
        }


        private void TimerComAct_Tick_1(object sender, EventArgs e)      // 触发timer：for locking of flagOfPirate1
        {
            if(!PvPMode && Pirate2_ActionEnable && Game_Pirate_duel.pirate2 != null)    // PVE模式
            {   // PVE模式    // Computer还活着      // 游戏已经开始
                if(RL.ChooseAction_Random())
                    Game_Pirate_duel.pirate2.Action();
                // else nop
            }
        }

        /* ******************* 垃圾事件 ******************** */
        private void timer2_Tick(object sender, EventArgs e)    // 触发timer：for locking of flagOfPirate2
        {
            flagOfPirate2 = true;
            timer2.Enabled = false;
        }

        

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        /* ******************* 垃圾事件 ******************** */

    }
}




