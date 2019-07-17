using System;
using System.Linq;
using System.Collections.Generic;
using GameOfLife.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameOfLife.Test
{
    [TestClass]
    public class EngineTest
    {
        //Une grille de 9 cellules
        private IGameEngine GetEngine()
        {
            return new OriginalEngine(3, 3); //Ne compilera qu'une fois que la classe OriginalEngine sera implémentée dans le projet GameOfLife.Engine
        }

        [TestMethod]
        public void A_Refuse_ChangementEtat_Si_Coord_Negatives()
        {
            Assert.ThrowsException<ArgumentException>(() => GetEngine().SwitchCellState(-1, 2));
        }

        [TestMethod]
        public void B_Refuse_ChangementEtat_Si_Coord_Hors_Grille()
        {
            Assert.ThrowsException<ArgumentException>(() => GetEngine().SwitchCellState(3, 2));
        }

        [TestMethod]
        public void C_Cree_Grille_Avec_3_Cellules_Horizontales_Vivantes()
        {
            // On active les 3 cellules horizontales du milieu, soit :
            // 0 0 0
            // 1 1 1
            // 0 0 0

            var engine = GetEngine();

            engine.SwitchCellState(0, 1);
            engine.SwitchCellState(1, 1);
            engine.SwitchCellState(2, 1);
            // Aucune exception ne doit être déclenchée, ces 3 coordonnées sont cohérentes par rapport à la taille de la grille
        }

        [TestMethod]
        public void D_Iteration_sur_3_Cellules_Horizontales_Vivantes()
        {
            // Lorsqu'on part de 3 cellules horizontales
            // 0 0 0
            // 1 1 1
            // 0 0 0

            // Et que l'on réalise une itération, on doit obligatoirement arriver au résultat suivant
            // 0 1 0
            // 0 1 0
            // 0 1 0

            var engine = GetEngine();
            engine.SwitchCellState(0, 1);
            engine.SwitchCellState(1, 1);
            engine.SwitchCellState(2, 1);

            engine.CellStateChanged += Engine_CellStateChanged;

            engine.Iterate(1);

            var cellulesVerticalesAttendues = new List<GridCell>()
            {
                new GridCell(1,0, CellState.Alive),
                new GridCell(1,2, CellState.Alive),
                new GridCell(0,1, CellState.Dead),
                new GridCell(2,1, CellState.Dead)
            };

            foreach (var celluleAttendue in cellulesVerticalesAttendues)
            {
                if (!cellulesVerticalesDetectees.Where(c => c.X == celluleAttendue.X && c.Y == celluleAttendue.Y && c.State == celluleAttendue.State).Any())
                    throw new Exception(string.Concat("La cellule ", celluleAttendue, " était attendue mais n'a pas été activée par le moteur. Vérifiez l'implémentation des règles d'évolution"));
            }
        }
        List<GridCell> cellulesVerticalesDetectees = new List<GridCell>();

        private void Engine_CellStateChanged(object sender, CellStateChangedEventArgs e)
        {
            cellulesVerticalesDetectees.Add(e.CellAfterChange);
        }
    }
}
