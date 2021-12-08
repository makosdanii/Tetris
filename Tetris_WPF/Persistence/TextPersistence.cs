using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WPF
{
    class TextPersistence : IPersistence
    {
        public Task<String[]> ReadAsync(string path)
        {
            return Task.Run<String[]>(() =>
            {
                if (System.IO.Path.GetExtension(path) != ".txt") throw new ArgumentException();

                String[] content;
                using (StreamReader reader = new StreamReader(path))
                {
                    content = reader.ReadToEnd().Split('\n');
                }
                return content;

            });

        }

        public void Write(string path, String[] content, int columns)
        {
            if (System.IO.Path.GetExtension(path) != ".txt") throw new ArgumentException();

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
