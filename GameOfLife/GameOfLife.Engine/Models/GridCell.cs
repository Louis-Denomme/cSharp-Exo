using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Engine
{

    public enum CellState { Dead, Alive };

    public class GridCell
    {        
        private int _x;
        private int _y;        
        private CellState _state;

        public GridCell(int x, int y, CellState state)
        {
            _x = x;
            _y = y;
            _state = state;
        }       

        public int X { get => _x;}
        public int Y { get => _y;}        
        public CellState State { get => _state; set => _state = value; }

        override
        public String ToString()
        {
            return "Cellule [ "+ X + " ; " + Y + " ] - "+ ( State == CellState.Alive ? "en vie" : "mort");            
        }

        public void ChangeState(CellState state)
        {
            if (State != state) State = state;            
        }

        public void switchState()
        {
            if (State == CellState.Alive)
                State = CellState.Dead;
            else
                State = CellState.Alive;
        }
    }
}
