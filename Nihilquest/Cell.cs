using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nihilquest
{
    class Cell
    {
        private Rectangle rectangle;
        private bool isLegal;
        private Character character;
        private Item item;
        public Cell(Rectangle rectangle, bool isLegal)
        {
            this.rectangle = rectangle;
            this.isLegal = isLegal;
        }

        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }
        public bool IsLegal { get => isLegal; set => isLegal = value; }
        internal Character Character { get => character; set => character = value; }
        internal Item Item { get => item; set => item = value; }

        public bool hasCharacter()
        {
            return Character != null;
        }
        public bool hasItem(Item item)
        {
            return Item != null;
        }
    }
}
