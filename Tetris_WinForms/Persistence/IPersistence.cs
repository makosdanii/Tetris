using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WPF
{
    public interface IPersistence
    {
        public abstract String[] Read(String path);
        public abstract void Write(String path, String[] content, int columns);

    }
}
