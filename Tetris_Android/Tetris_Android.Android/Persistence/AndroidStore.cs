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
    /// J�t�k t�rol� megval�s�t�sa Android platformra.
    /// </summary>
    public class AndroidStore : Tetris_Android.IStore
    {
        /// <summary>
        /// F�jlok lek�rdez�se.
        /// </summary>
        /// <returns>A f�jlok list�ja.</returns>
        public async Task<IEnumerable<String>> GetFiles()
        {
            var list = await Task.Run(() => Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).Select(file => Path.GetFileName(file)));
            return list;
        }

        /// <summary>
        /// M�dos�t�s idej�nek lekr�dez�se.
        /// </summary>
        /// <param name="name">A f�jl neve.</param>
        /// <returns>Az utols� m�dos�t�s ideje.</returns>
        public async Task<DateTime> GetModifiedTime(String name)
        {
            FileInfo info = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}