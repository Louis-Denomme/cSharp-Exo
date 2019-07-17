using GameOfLife.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GameOfLife.UI
{
    public partial class Form1 : Form
    {
        const int lineWidh = 1;        
        Color BorderColor = Color.Gray;
        Color AliveColor = Color.Black;
        Color DeadColor = Color.White;

        Pen pen = new Pen(Color.Gray);

        public bool mustDrawTheGrid;

        const int sizeGrid = 50;

        private List<GridCellUi> Cells = new List<GridCellUi>(sizeGrid * sizeGrid);
        private OriginalEngine myGame = new OriginalEngine(sizeGrid, sizeGrid);

        IGameEngine engine;

        public Form1()
        {
            InitializeComponent();            
            B_Reset_Click(this, null);
        }       

        private int CalculateCellSize(Panel panel)
        {
            int panelWidth = panel.Width;
            int panelHeight = panel.Height;


            return panelWidth / sizeGrid;
        }       

        private void B_Reset_Click(object sender, EventArgs e)
        {
            engine = new OriginalEngine(sizeGrid, sizeGrid);
            engine.CellStateChanged += Engine_CellStateChanged; ;

            mustDrawTheGrid = true;
            panel1.Invalidate();
        }

        private void Engine_CellStateChanged(object sender, CellStateChangedEventArgs e)
        {            
            GridCellUi test = Cells.Where(cell => cell.X == e.CellAfterChange.X && cell.Y == e.CellAfterChange.Y).First();
            SwitchColorState(test);
        }

        private void Panel1_Paint_1(object sender, PaintEventArgs e)
        {
            if (!mustDrawTheGrid)
                return;

            panel1.BackColor = DeadColor;
            var cellSize = CalculateCellSize(panel1);
            for (int i = 0; i <= sizeGrid; i++)
            {
                //Vertical
                e.Graphics.DrawLine(pen, i * cellSize, 0, i * cellSize, sizeGrid * cellSize);

                //Horizontal
                e.Graphics.DrawLine(pen, 0, i * cellSize, sizeGrid * cellSize, i * cellSize);
            }
            for (int i = 0; i < sizeGrid; i++)
            {
                for (int y = 0; y < sizeGrid; y++)
                {
                    var location = new Point((int)Math.Round((float)i * cellSize), (int)Math.Round((float)y * cellSize));
                    Cells.Add(new GridCellUi(i, y, location));
                }
            }
            
        }

        private void Launch_button(object sender, EventArgs e)
        {
            for (int i = 0; i<= 10; i++)
            {
                engine.Iterate(1);
                Thread.Sleep(200);
            }            
        }

        private void Save_Button(object sender, EventArgs e)
        {
            engine.save(@"C:\Users\Louis\Documents\CESI\C#");
        }

        private void SwitchColorState(GridCellUi cell)
        {
            var cellSize = CalculateCellSize(panel1);
            if (cell.State == CellState.Alive)
                pen.Color = DeadColor;
            else
                pen.Color = AliveColor;

            Rectangle rect = new Rectangle(cell.Location.X, cell.Location.Y, cellSize , cellSize);
            Graphics myGraph = panel1.CreateGraphics();
            myGraph.FillRectangle(new SolidBrush(pen.Color), rect);
            pen.Color = BorderColor;
            myGraph.DrawRectangle(pen, rect);
            cell.switchState();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            GridCellUi test = Cells.Where(cell => cell.Location.X < e.X && cell.Location.Y < e.Y).Last();
            engine.SwitchCellState(test.X, test.Y);
            SwitchColorState(test);
        }
    }
}
