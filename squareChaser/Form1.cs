using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace squareChaser
{
    public partial class Form1 : Form
    {
        //squares & circle
        Rectangle player1 = new Rectangle(10, 10, 25, 25);
        Rectangle player2 = new Rectangle(320, 320, 25, 25);
        Rectangle whiteSquare = new Rectangle();
        Rectangle speedBooster = new Rectangle();

        //brushes
        SolidBrush whitebrush = new SolidBrush(Color.White);
        SolidBrush hotPinkbrush = new SolidBrush(Color.HotPink); // player 1
        SolidBrush bluebrush = new SolidBrush(Color.Blue); // player 2
        SolidBrush yellowbrush = new SolidBrush(Color.Yellow);
        Pen yellowPen = new Pen(Color.Yellow);

        //score
        int player1Score = 0;
        int player2Score = 0;

        // player speed
        int player1Speed = 6;
        int player2Speed = 6;

        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        bool aDown = false;
        bool dDown = false;
        bool leftDown = false;
        bool rightDown = false;

        Random randGen = new Random();
        Stopwatch stopwatch = new Stopwatch();

        SoundPlayer speedBoost = new SoundPlayer(Properties.Resources.ufo);
        SoundPlayer point = new SoundPlayer(Properties.Resources.horn);
        SoundPlayer tada = new SoundPlayer(Properties.Resources.taDa);
        public Form1()
        {
            InitializeComponent();
            int p1XPosition = randGen.Next(25, 551);
            int p1YPosition = randGen.Next(25, 551);
            int p2XPosition = randGen.Next(25, 551);
            int p2YPosition = randGen.Next(25, 551);
            player1 = new Rectangle(p1XPosition, p1YPosition, 25, 25);
            player2 = new Rectangle(p2XPosition, p2YPosition, 25, 25);

            Draw_SpeedBooster();
            Draw_WhiteSquare();
        }

        private void Draw_SpeedBooster()
        {
            int boostXPostition = randGen.Next(25, 551);
            int boostYPostition = randGen.Next(25, 551);

            speedBooster = new Rectangle(boostXPostition, boostYPostition, 13, 13);
        }

        private void Draw_WhiteSquare()
        {
            int squareXPostition = randGen.Next(25, 551);
            int squareYPostition = randGen.Next(25, 551);

            whiteSquare = new Rectangle(squareXPostition, squareYPostition, 13, 13);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(hotPinkbrush, player1);
            e.Graphics.FillRectangle(bluebrush, player2);
            e.Graphics.FillRectangle(whitebrush, whiteSquare);
            e.Graphics.DrawEllipse(yellowPen, speedBooster);
            e.Graphics.FillEllipse(yellowbrush, speedBooster);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //move player 1 up and down
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= player1Speed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += player1Speed;
            }

            // move player 1 left and right
            if (aDown == true && player1.X > 0)
            {
                player1.X -= player1Speed;
            }

            if (dDown == true && player1.X < this.Width - player1.Width)
            {
                player1.X += player1Speed;
            }

            //move player 2 up and down
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= player2Speed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += player2Speed;
            }

            // move player 2 left and right
            if (leftDown == true && player2.X > 0)
            {
                player2.X -= player2Speed;
            }

            if (rightDown == true && player2.X < this.Width - player2.Width)
            {
                player2.X += player2Speed;
            }

            // check if either player intersects with white ball and if yes then add one to score
            if (player1.IntersectsWith(whiteSquare))
            {
                player1Score++;
                point.Play();
                p1ScoreLabel.Text = $"{player1Score}";
                Draw_WhiteSquare();
            }
            else if (player2.IntersectsWith(whiteSquare))
            {
                player2Score++;
                point.Play();
                p2ScoreLabel.Text = $"{player2Score}";
                Draw_WhiteSquare();

            }

            // check if either player intersects with speed booster and add to the speed
            if (player1.IntersectsWith(speedBooster))
            {
                stopwatch.Start();
                speedBoost.Play();
                player1Speed = 12;
                Draw_SpeedBooster();
            }
            else if (player2.IntersectsWith(speedBooster))
            {
                stopwatch.Start();
                speedBoost.Play();
                player2Speed = 12;
                Draw_SpeedBooster();
            }

            if (stopwatch.ElapsedMilliseconds >= 4000)
            {
                player1Speed = 6;
                player2Speed = 6;
                stopwatch.Stop();
                stopwatch.Reset();
            }

            //check score and stop game if either player is at 3
            if (player1Score == 5)
            {
                tada.Play();
                timer.Enabled = false;
                winLabel.Enabled = true;
                winLabel.Text = "Player 1 Wins!!!";
            }
            if (player2Score == 5)
            {
                tada.Play();
                timer.Enabled = false;
                winLabel.Enabled = true;
                winLabel.Text = "Player 2 Wins!!!";
            }

            Refresh();
        }


    }
}
