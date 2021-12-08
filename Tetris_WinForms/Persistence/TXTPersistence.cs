using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WPF
{
    class TXTPersistence : IPersistence
    {
        public String[] Read(string path)
        {
            String[] content;
            using (StreamReader reader = new StreamReader(path))
            {
                content = reader.ReadToEnd().Split('\n');
            }

            return content;
        }

        public void Write(string path, String[] content, int columns)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(columns);
                foreach (String line in content)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}
