namespace FontStashSharp
{
    public struct Bounds
    {
        public float X, Y, X2, Y2;
        public float Width
        {
            get
            {
                return X2 - X;
            }
        }

        public float Height
        {
            get
            {
                return Y2 - Y;
            }
        }
    }
}

