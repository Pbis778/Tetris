using System.Collections;
using System.Collections.Generic;

namespace TetrisGame
{
    public abstract class Block
    {
        protected abstract Position[][] Sectors { get; }
        protected abstract Position StartOffSet { get; }
        public abstract int Id { get; }

        private int rotationState;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffSet.Row, StartOffSet.Column);
        }

        public IEnumerable<Position> SectorPosition()
        {
            foreach (Position pos in Sectors[rotationState])
            {
                yield return new Position(pos.Row + offset.Row, pos.Column + offset.Column);
            }
        }

        public void RotateBlock()
        {
            rotationState = (rotationState + 1) % Sectors.Length;
        }

        public void RotateBlockCounter()
        {
            if (rotationState == 0)
            {
                rotationState = Sectors.Length - 1;
            } else
            {
                rotationState--;
            }
        }

        public void MoveBlock(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffSet.Row;
            offset.Column = StartOffSet.Column;
        }
    }
}
