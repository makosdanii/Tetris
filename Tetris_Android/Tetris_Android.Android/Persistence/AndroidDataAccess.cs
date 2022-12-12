using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Tetris_Android;

[assembly: Dependency(typeof(Tetris_Android.Droid.Persistence.AndroidDataAccess))]
namespace Tetris_Android.Droid.Persistence
{
    /// <summary>
    /// Tic-Tac-Toe adatelérés megvalósítása Android platformra.
    /// </summary>
    public class AndroidDataAccess : IDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A beolvasott mezõértékek.</returns>
        public async Task<String[]> LoadAsync(String path)
        {
            // a betöltés a személyen könyvtárból történik
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // a fájlmûveletet taszk segítségével végezzük (aszinkron módon)
            return await Task<String[]>.Run(() => 
            {
                //if (System.IO.Path.GetExtension(filePath) != ".txt") throw new ArgumentException();
                
                return File.ReadAllText(filePath).Split('\n');
            });
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, String[] content, int columns)
        {
            // fájl létrehozása
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // kiírás (aszinkron módon)
            await Task.Run(() =>
            {
                //if (System.IO.Path.GetExtension(path) != ".txt") throw new ArgumentException();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(columns);
                    foreach (String line in content)
                    {
                        writer.WriteLine(line);
                    }
                }
            });
        }
    }
}