using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using GameOfLife;
using System.IO;

namespace GameOfLife.Engine
{
    public class OriginalEngine : IGameEngine
    {
        private GridCell[,] _myGrid;        
        private int _numberOfCellOnX;
        private int _numberOfCellOnY;

        public GridCell[,] MyGrid { get => _myGrid; set => _myGrid = value; }
        public int NumberOfCellOnX { get => _numberOfCellOnX; set => _numberOfCellOnX = value; }
        public int NumberOfCellOnY { get => _numberOfCellOnY; set => _numberOfCellOnY = value; }

        public OriginalEngine(int numberOfCellsOnX, int numberOfCellsOnY)
        {
            NumberOfCellOnX = numberOfCellsOnX;
            NumberOfCellOnY = numberOfCellsOnY;
            MyGrid = new GridCell[numberOfCellsOnX, numberOfCellsOnY];
            for(int i = 0; i < numberOfCellsOnX; i++)
            {
                for(int y=0; y < numberOfCellsOnY; y++)
                {
                    MyGrid[i, y] = new GridCell(i, y, CellState.Dead);
                }
            }            
        }        

        public Boolean isCoordInGrid(int x, int y)
        {
            return x < NumberOfCellOnX && x >= 0 && y < NumberOfCellOnY && y >= 0;
        }        

        public int GetAliveNeighbors(int indexOnX, int indexOnY)
        {
            int compt = 0;
            //col gauche
            if(isCoordInGrid(indexOnX - 1, indexOnY -1) && MyGrid[indexOnX - 1, indexOnY - 1].State == CellState.Alive) compt++;            
            if(isCoordInGrid(indexOnX - 1, indexOnY) && MyGrid[indexOnX - 1, indexOnY].State == CellState.Alive) compt++;            
            if(isCoordInGrid(indexOnX - 1, indexOnY + 1) && MyGrid[indexOnX - 1, indexOnY + 1].State == CellState.Alive) compt++;            
            //col mid
            if(isCoordInGrid(indexOnX, indexOnY - 1) && MyGrid[indexOnX, indexOnY - 1].State == CellState.Alive) compt++;
            if(isCoordInGrid(indexOnX, indexOnY + 1) && MyGrid[indexOnX,indexOnY + 1].State == CellState.Alive) compt++;
            //col droite
            if (isCoordInGrid(indexOnX + 1, indexOnY - 1) && MyGrid[indexOnX + 1, indexOnY - 1].State == CellState.Alive) compt++;
            if (isCoordInGrid(indexOnX + 1, indexOnY) && MyGrid[indexOnX + 1, indexOnY].State == CellState.Alive) compt++;                        
            if(isCoordInGrid(indexOnX + 1, indexOnY + 1) && MyGrid[indexOnX + 1, indexOnY + 1].State == CellState.Alive) compt++;
            return compt;
        }

        public CellState GetNextCellState(GridCell gridCell, int aliveNeighbors)
        {
            if(gridCell.State == CellState.Alive)
            {
                if (aliveNeighbors == 2 || aliveNeighbors == 3)
                    return CellState.Alive;
                else
                    return CellState.Dead;
            }
            else
            {
                if (aliveNeighbors == 3)
                    return CellState.Alive;
                else
                    return CellState.Dead;
            }            
        }

        public void Iterate(int numberOfIterations)
        {
            List<GridCell> cellsToChange = new List<GridCell>();
            for(int i=0; i< numberOfIterations; i++)
            {
                foreach (GridCell cell in MyGrid)
                {
                    if (cell.State != GetNextCellState(cell, GetAliveNeighbors(cell.X, cell.Y))){
                        cellsToChange.Add(cell);
                    }
                }
                foreach (GridCell cell in cellsToChange)
                {
                    //MyGrid[cell.X, cell.Y].switchState();
                    SwitchCellState(cell.X, cell.Y);
                }                
            }            
        }

        public event EventHandler<CellStateChangedEventArgs> CellStateChanged;

        public void OnCellStateChanged(CellStateChangedEventArgs e)
        {
            CellStateChanged?.Invoke(this, e);          
        }        

        public void SwitchCellState(int indexOnX, int indexOnY)
        {
            if (isCoordInGrid(indexOnX, indexOnY))
            {
                MyGrid[indexOnX, indexOnY].ChangeState(MyGrid[indexOnX, indexOnY].State == CellState.Alive ? CellState.Dead : CellState.Alive);
                OnCellStateChanged(new CellStateChangedEventArgs(MyGrid[indexOnX, indexOnY]));
            }                
            else
            {
                throw new ArgumentException("Coordonnées en dehors de la grille.");
            }
        }

        public void save(String path)
        {
            testDir(path);
            path = path + @"\saves";
            path = testFile(path);
            //var json = new JavaScriptSerializer().Serialize(obj);
            //Console.WriteLine(json);
        }

        public void testDir(String path)
        {
            if (!System.IO.Directory.Exists(path))
                return;
            if (!System.IO.Directory.Exists(path + @"\saves"))
                Directory.CreateDirectory(path + @"\saves");
        }

        public String testFile(String path)
        {
            String file = @"\save";
            String ext = ".json";
            int compteur = 1;
            while (File.Exists(path + file + compteur + ext))
            {
                compteur++;
            }
            File.Create(path + file + compteur + ext);
            return path + file + compteur + ext;
        }
    }
}
