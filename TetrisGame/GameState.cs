namespace TetrisGame
{
    public class GameState
    {
        public Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.MoveBlock(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.MoveBlock(-1, 0);
                    }
                }
            }
        }

        public GameBoard GameBoard { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameBoard = new GameBoard(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.SectorPosition()) 
            {
                if (!GameBoard.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            } else
            {
                Block temp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = temp;
            }

            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateBlock();

            if (!BlockFits())
            {
                CurrentBlock.RotateBlockCounter();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateBlockCounter();

            if (!BlockFits())
            {
                CurrentBlock.RotateBlock();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.MoveBlock(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.MoveBlock(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.MoveBlock(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.MoveBlock(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameBoard.IsRowEmpty(0) && GameBoard.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.SectorPosition())
            {
                GameBoard[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameBoard.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            } else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.MoveBlock(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.MoveBlock(-1, 0);
                PlaceBlock();
            }
        }

        private int SectorDropDistance(Position pos)
        {
            int distance = 0;

            while (GameBoard.IsEmpty(pos.Row + distance + 1, pos.Column))
            {
                distance++;
            }

            return distance;
        }

        public int BlockDropDistance()
        {
            int distance = GameBoard.Rows;

            foreach (Position pos in CurrentBlock.SectorPosition())
            {
                distance = System.Math.Min(distance, SectorDropDistance(pos));
            }

            return distance;
        }

        public void DropBlock()
        {
            CurrentBlock.MoveBlock(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
