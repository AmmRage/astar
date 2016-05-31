namespace astarCs
{
    public class AsPoint
    {

        public static int Width;
        public static int Height;

        public readonly int HashCode;
        public int X;
        public int Y;

        public AsPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.HashCode = X * Width + Y;
        }

        public AsPoint()
        {
            this.X = 0;
            this.Y = 0;
        }
        public override int GetHashCode()
        {
            return this.HashCode;
        }

        public override string ToString()
        {
            return X + "," + Y;
        }

        public override bool Equals(object obj)
        {
            return this.HashCode == ((AsPoint)obj).HashCode;
        }
    }
}