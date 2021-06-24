using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Nihilquest
{
    //reads any image pixel by pixel
    //needs an img path
    class imgReader
    {
        private Bitmap img;

        public imgReader(String path)
        {
            img = new Bitmap(path);
        }

        public Cell[,] readImg()
        {
            Cell[,] cellMap = new Cell[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (pixel != null)
                    {
                        if (pixel.R == 236 && pixel.G == 28 && pixel.B == 36)
                        {
                            //enemy

                            cellMap[i, j] = new Cell();
                            cellMap[i, j].Character = new Enemy("mob", i, j);
                        }
                        else if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                        {
                            //wall
                            cellMap[i, j] = new Cell();
                            cellMap[i, j].IsWall = true;
                        }
                    }
                }
            }
            return cellMap;
        }
    }
}
