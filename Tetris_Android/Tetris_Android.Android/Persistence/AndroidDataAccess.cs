using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Tetris_Android;

[assembly: Dependency(typeof(Tetris_Android.Droid.Persistence.AndroidDataAccess))]
namespace Tetris_Android.Droid.Persistence
{
    /// <summary>
    /// Tic-Tac-Toe adatel�r�s megval�s�t�sa Android platformra.
    /// </summary>
    public class AndroidDataAccess : IDataAccess
    {
        /// <summary>
        /// F�jl bet�lt�se.
        /// </summary>
        /// <param name="path">El�r�si �tvonal.</param>
        /// <returns>A beolvasott mez��rt�kek.</returns>
        public async Task<String[]> LoadAsync(String path)
        {
            // a bet�lt�s a szem�lyen k�nyvt�rb�l t�rt�nik
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // a f�jlm�veletet taszk seg�ts�g�vel v�gezz�k (aszinkron m�don)
            return await Task<String[]>.Run(() => 
            {
                //if (System.IO.Path.GetExtension(filePath) != ".txt") throw new ArgumentException();
                
                return File.ReadAllText(filePath).Split('\n');
            });
        }

        /// <summary>
        /// F�jl ment�se.
        /// </summary>
        /// <param name="path">El�r�si �tvonal.</param>
        /// <param name="table">A f�jlba ki�rand� j�t�kt�bla.</param>
        public async Task SaveAsync(String path, String[] content, int columns)
        {
            // f�jl l�trehoz�sa
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // ki�r�s (aszinkron m�don)
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