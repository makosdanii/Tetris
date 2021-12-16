using System;
using System.Threading.Tasks;

namespace Tetris_Android
{
    /// <summary>
    /// Sudoku fájl kezelő felülete.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<String[]> LoadAsync(String path);

		/// <summary>
		/// Fájl mentése.
		/// </summary>
		/// <param name="path">Elérési útvonal.</param>
		/// <param name="table">A fájlba kiírandó játéktábla.</param>
		Task SaveAsync(String path, String[] content, int columns);
	}
}