using IA_Library_ECS;

namespace IA_Library
{
    public class GridManager
    {
        private int[,] cells;

        public int Width { get; }
        public int Height { get; }
        
        public GridManager(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new int[width, height];
        }
        
        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}