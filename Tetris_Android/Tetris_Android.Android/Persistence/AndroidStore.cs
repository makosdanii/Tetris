using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Tetris_Android.Droid.Persistence.AndroidStore))]
namespace Tetris_Android.Droid.Persistence
{
    /// <summary>
    /// Játék tároló megvalósítása Android platformra.
    /// </summary>
    public class AndroidStore : Tetris_Android.IStore
    {
        /// <summary>
        /// Fájlok lekérdezése.
        /// </summary>
        /// <returns>A fájlok listája.</returns>
        public async Task<IEnumerable<String>> GetFiles()
        {
            var list = await Task.Run(() => Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).Select(file => Path.GetFileName(file)));
            return list;
        }

        /// <summary>
        /// Módosítás idejének lekrédezése.
        /// </summary>
        /// <param name="name">A fájl neve.</param>
        /// <returns>Az utolsó módosítás ideje.</returns>
        public async Task<DateTime> GetModifiedTime(String name)
        {
            FileInfo info = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}