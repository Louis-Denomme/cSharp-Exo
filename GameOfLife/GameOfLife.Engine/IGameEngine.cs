using System;

namespace GameOfLife.Engine
{
    /// <summary>
    /// Représente les fonctionnalités que peut manipuler la couche de présentation
    /// </summary>
    /// <remarks>
    /// Cette classe devra implémenter un constructeur demandant le nombre de cellule en X et le nombre de cellule en Y.
    /// </remarks>
    public interface IGameEngine
    {
        /// <summary>
        /// Permet à l'utilisateur de changer "manuellement" l'état d'une cellule
        /// </summary>
        /// <remarks>Si les coordonnées sont hors grille, le moteur doit déclencher une ArgumentException</remarks>
        void SwitchCellState(int indexOnX, int indexOnY);

        /// <summary>
        /// Déclenche une passe sur toutes les cellules de la grille
        /// pour faire évoluer leur état, en applique les règles du jeu de la vie
        /// </summary>
        void Iterate(int numberOfIterations);

        /// <summary>
        /// Permet à la couche de présentation d'être notifiée lorsqu'une cellule a changé d'état
        /// </summary>
        event EventHandler<CellStateChangedEventArgs> CellStateChanged;
        void OnCellStateChanged(CellStateChangedEventArgs e);


        /// <summary>
        /// Donne le nombre de cellules voisines vivantes par rapport à la position indiquée en paramètre
        /// </summary>
        int GetAliveNeighbors(int indexOnX, int indexOnY);

        /// <summary>
        /// Donne le futur état de la cellule passée en paramètre, lorsque l'on applique la règle du jeu de la vie
        /// </summary>
        CellState GetNextCellState(GridCell gridCell, int aliveNeighbors);

        /// <summary>
        /// Sauvegarde l'actuelle disposition des colonnes
        /// </summary>
        void save(String path);
    }

    /// <summary>
    /// Représente les données d'une cellule après que celle-ci a changé d'état
    /// </summary>
    public class CellStateChangedEventArgs : EventArgs
    {
        public CellStateChangedEventArgs(GridCell cellAfterChange)
        {
            CellAfterChange = cellAfterChange;
        }

        public GridCell CellAfterChange { get; set; }
    }
}