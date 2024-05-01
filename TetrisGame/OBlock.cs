namespace TetrisGame
{
    public class OBlock : Block
    {
        private readonly Position[][] sectors = new Position[][]
        {
            new Position[] {new(0,0), new(0,1), new(1,0), new(1,1)}
        };

        public override int Id => 4;
        protected override Position StartOffSet => new Position(0,4);
        protected override Position[][] Sectors => sectors;
    }
}
