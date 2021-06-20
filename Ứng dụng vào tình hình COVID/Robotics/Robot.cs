using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Robotics
{
    public partial class Robot : Form
    {
        private int M = 12, N = 18;
        private int[,] A, Deg, F;
        private int[] Hx = { -1, 0, 1, 0 };
        private int[] Hy = { 0, 1, 0, -1 };
        private int deltaX, deltaY;
        private int INDEX = -1;
        private int IndexLoangDatam = 0;
        private Graphics graphics;
        private ImageCreate imageCreate;
        private List<Point> Ngocut = new List<Point>();
        private List<Point> Queue = new List<Point>();
        public Robot()
        {
            InitializeComponent();
            graphics = pictureBoxContent.CreateGraphics();
            A = new int[M, N];
            Deg = new int[M, N];
            F = new int[M, N];

            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                {
                    A[i, j] = 1;
                    Deg[i, j] = 0;
                    F[i, j] = -1;
                }
            deltaX = 600 / M;
            deltaY = 900 / N;
            PaintMatrix(graphics);
        }

        public void PaintMatrix(Graphics g)
        {
            ImageCreate image = new ImageCreate();
            for (int i = 0; i <= M; i++)
                g = image.Draw_Line_Graphics(5, 5 + i * deltaX, 905, 5 + i * deltaX, 2, Color.Blue, g);
            for (int i = 0; i <= N; i++)
                g = image.Draw_Line_Graphics(5 + i * deltaY, 5, 5 + i * deltaY, 605, 2, Color.Blue, g);
        }

        private void btncreateMap_Click(object sender, EventArgs e)
        {
            PaintMatrix(graphics);
            INDEX = 0;
        }

        private void pictureBoxContent_MouseClick(object sender, MouseEventArgs e)
        {
            if (INDEX == 0)
            {
                ImageCreate image = new ImageCreate();
                int y = (e.X - 5) / deltaY;
                int x = (e.Y - 5) / deltaX;
                if (A[x, y] == 1)
                {
                    A[x, y] = 0;
                    graphics = image.Draw_Rangce_Graphics(5 + y * deltaY + 1, 5 + x * deltaX + 1, deltaY - 2, deltaX - 2, Color.Black, graphics);
                }
                else
                {
                    A[x, y] = 1;
                    graphics = image.Draw_Rangce_Graphics(5 + y * deltaY + 1, 5 + x * deltaX + 1, deltaY - 2, deltaX - 2, Color.White, graphics);
                }
            }
        }

        public void CreateMap()
        {
            imageCreate = new ImageCreate(M, N, A);
            imageCreate.Create("Image/maping.png");
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            INDEX = 1;
            CreateMap();
            Image myimage = new Bitmap(@"Image//maping.png");
            pictureBoxContent.BackgroundImage = myimage;
        }

        private bool check(int x, int y)
        {
            if (x < 0 || y < 0 || x >= M || y >= N)
                return false;
            else
                return true;
        }

        public void DegNode()
        {
            ImageCreate image = new ImageCreate();
            for (int i = 0; i < M; i++)
                    for (int j = 0; j < N; j++)
                        if (A[i, j] == 0)
                        {
                            Ngocut.Add(new Point(i, j));
                        }
            Thread.Sleep(500);
        }
        private void LoangDatam()
        {
            ImageCreate image = new ImageCreate();

            Image myimage = new Bitmap("Image/maping.png");
            graphics.Clear(Color.White);
            graphics = image.Draw_Bitmap_Graphics(0, 0, "Image/maping.png", graphics);
            for (int i = 0; i < Ngocut.Count; i++)
            {
                int x = Ngocut[i].X;
                int y = Ngocut[i].Y;
            }
            Queue.AddRange(Ngocut);
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    F[i, j] = -1;
            for (int i = 0; i < Ngocut.Count; i++)
            {
                F[Ngocut[i].X, Ngocut[i].Y] = 0;
            }
            //Bắt đầu loang đa tâm

            int dem = 0;
            while (dem < Queue.Count)
            {
                Point p = Queue[dem];
                int x = p.X;
                int y = p.Y;
                for (int i = 0; i < 4; i++)
                {
                    int u = x + Hx[i];
                    int v = y + Hy[i];
                    if (check(u, v) && A[u, v] == 1 && F[u, v] == -1)
                    {
                        F[u, v] = F[x, y] + 1;
                        Point point = new Point(u, v);
                        Queue.Add(point);
                    }
                }
                dem++;
            }
        }


        private void radButton1_Click(object sender, EventArgs e)
        {
            DegNode();
            LoangDatam();
            timerLoangDaTam.Start();
            IndexLoangDatam = 0;
        }

        private void timerLoangDaTam_Tick(object sender, EventArgs e)
        {
            ImageCreate image = new ImageCreate();
            if (IndexLoangDatam < Queue.Count)
            {
                int u = Queue[IndexLoangDatam].X;
                int v = Queue[IndexLoangDatam].Y;
                if(F[u, v] < 10)
                    graphics = image.Draw_String_Graphics(5 + v * deltaY + 12, 5 + u * deltaX + 15, 14,"F" + F[u, v], graphics);
                else
                    if (F[u, v] < 30)
                        graphics = image.Draw_String_Graphics(5 + v * deltaY + 7, 5 + u * deltaX + 15, 14,"F" + F[u, v], graphics);
                IndexLoangDatam++;
            }
            else
            {
                timerLoangDaTam.Stop();
            }
        }
    }
}
