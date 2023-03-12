using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int cols;

        public Form1()
        {
            InitializeComponent();
            
            bPlay.Visible = false;
            bPause.Visible = false;
            bRandom.Visible = false;
            bStop.Visible = false;
            bClear.Visible = false;
        }
        private void NextGeneration()
        {
            graphics.Clear(Color.White);
            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var hasLife = field[x, y];

                    switch (cbRules.SelectedItem)
                    {
                        default:
                        case ("Game of Life"):
                            if (!hasLife && (neighboursCount == 3))
                                newField[x, y] = true;
                            else if (hasLife && (neighboursCount == 2 || neighboursCount == 3))
                                newField[x, y] = field[x, y];
                            else
                                newField[x, y] = false;
                            break;
                        case ("Life without death"):
                            if (!hasLife && (neighboursCount == 3))
                                newField[x, y] = true;
                            else if (hasLife)
                                newField[x, y] = field[x, y];
                            else
                                newField[x, y] = false;
                            break;
                        case ("Day & Night"):
                            if(!hasLife && (neighboursCount == 3 || neighboursCount == 6 || neighboursCount == 7 || neighboursCount == 8))
                                newField[x, y] = true;
                            else if (hasLife && (neighboursCount == 3 || neighboursCount == 4 || neighboursCount == 6 || neighboursCount == 7 || neighboursCount == 8))
                                newField[x, y] = field[x, y];
                            else
                                newField[x, y] = false;
                            break;
                        case ("Maze"):
                            if (!hasLife && (neighboursCount == 3))
                                newField[x, y] = true;
                            else if (hasLife && (neighboursCount == 1 || neighboursCount == 2 || neighboursCount == 3 || neighboursCount == 4 || neighboursCount == 5))
                                newField[x, y] = field[x, y];
                            else
                                newField[x, y] = false;
                            break;
                        case ("HighLife"):
                            if (!hasLife && (neighboursCount == 3|| neighboursCount == 6))
                                newField[x, y] = true;
                            else if (hasLife && (neighboursCount == 2 || neighboursCount == 3))
                                newField[x, y] = field[x, y];
                            else
                                newField[x, y] = false;
                            break;
                        case ("Corals"):
                            if (!hasLife && (neighboursCount == 3))
                                newField[x, y] = true;
                            else if (hasLife && (neighboursCount == 4 || neighboursCount == 5 || neighboursCount == 6 || neighboursCount == 7 || neighboursCount == 8))
                                newField[x, y] = field[x, y];
                            else
                                newField[x, y] = false;
                            break;
                    }

                    if (hasLife)
                        graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution, resolution);

                }
            }
            field = newField;
            picture.Refresh();
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;


                    var isSelf = col == x && row == y;
                    var hasLife = field[col, row];

                    if (hasLife && !isSelf)
                        count++;
                }
            }
            return count;
        }
        
        private void bRandom_Click(object sender, EventArgs e)
        {
            tbTimer.Enabled = false;
            timer1.Start();

            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;
                }
            }
        }
        
        private void StartGame()
        {
            picture.Image = new Bitmap(picture.Width, picture.Height);
            graphics = Graphics.FromImage(picture.Image);
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;

            bPlay.Visible = true;
            bPause.Visible = true;
            bRandom.Visible = true;
            bStart.Visible = false;
            bStop.Visible = true;
            bClear.Visible = true;


            resolution = (int)nudResolution.Value;
            rows = picture.Height / resolution;
            cols = picture.Width / resolution;
            field = new bool[cols, rows];
        }
        
        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }
        
        private void StopGame()
        {
            timer1.Enabled = false;

            nudDensity.Enabled = true;
            nudResolution.Enabled = true;

            bPlay.Visible = false;
            bPause.Visible = false;
            bRandom.Visible = false;
            bStart.Visible = true;
            bStop.Visible = false;
            bClear.Visible = false;


            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = false;
                }
            }
            graphics.Clear(Color.White);
            picture.Refresh();
        }
        
        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }
        
        private void PlayGame()
        {
            timer1.Enabled = true;
            timer1.Start();
            tbTimer.Enabled = false;

            switch (tbTimer.Value)
            {
                default:
                    timer1.Interval = 100;
                    break;
                case (1):
                    timer1.Interval = 400;
                    break;
                case (2):
                    timer1.Interval = 200;
                    break;
                case (3):
                    timer1.Interval = 100;
                    break;
                case (4):
                    timer1.Interval = 50;
                    break;
                case (5):
                    timer1.Interval = 25;
                    break;
            }            
        }
        
        private void bPlay_Click(object sender, EventArgs e)
        {
            PlayGame();
        }
        
        private void PauseGame()
        {
            timer1.Enabled = false;
            tbTimer.Enabled = true;
        }
        
        private void bPause_Click(object sender, EventArgs e)
        {
            PauseGame();
        }
        
        private void bClear_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = false;
                }
            }
            
            graphics.Clear(Color.White);
            picture.Refresh();
        }
        
        private void picture_MouseMove(object sender, MouseEventArgs e)
        {         
            if (e.Button == MouseButtons.Left)
            {
                if (timer1.Enabled)
                    return;
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = ValidateMoussePos(x, y);
                if (validationPassed)
                {
                    field[x, y] = true;
                    graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution, resolution);
                    picture.Refresh();
                }                
            }

            if (e.Button == MouseButtons.Right)
            {
                if (timer1.Enabled)
                    return;
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = ValidateMoussePos(x, y);
                if (validationPassed)
                {
                    field[x, y] = false;
                    graphics.FillRectangle(Brushes.White, x * resolution, y * resolution, resolution, resolution);
                    picture.Refresh();
                }
            }
        }
        
        private bool ValidateMoussePos(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        
    }
}
