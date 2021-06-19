using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nihilquest
{
    class Cell
    {
        private Rectangle rectangle;
        private bool isLegal;
        private bool isWall;
        private bool isDoor;
        private bool isExit;
        private int g;
        private int h;
        private int f;
        private Character character;
        private Item item;
        public Cell(Rectangle rectangle, bool isLegal)
        {
            this.rectangle = rectangle;
            this.isLegal = isLegal;
        }

        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }
        public bool IsLegal { get => isLegal; set => isLegal = value; }
        public Character Character { get => character; set => character = value; }
        public Item Item { get => item; set => item = value; }
        public bool IsWall { get => isWall; set => isWall = value; }
        public bool IsDoor { get => isDoor; set => isDoor = value; }
        public bool IsExit { get => isExit; set => isExit = value; }
        public int G { get => G; set => G = value; }
        public int H { get => h; set => h = value; }
        public int F { get => f; set => f = value; }

        public bool hasCharacter()
        {
            return Character != null;
        }
        public bool hasItem()
        {
            return Item != null;
        }
    }
}
