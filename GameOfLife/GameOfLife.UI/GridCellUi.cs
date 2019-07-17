using System.Drawing;
using GameOfLife.Engine;

namespace GameOfLife.UI
{
    public class GridCellUi : GridCell
    {
        public readonly Point Location;

        public GridCellUi(int x, int y, Point location, CellState state = CellState.Dead) : base(x, y, state)
        {
            Location = location;
        }
    }
}