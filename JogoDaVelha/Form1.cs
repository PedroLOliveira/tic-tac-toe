namespace JogoDaVelha
{
    public partial class Form1 : Form
    {
        #region variables
        public enum Cell { Empty = 0, X = 1, O = 2 };

        public Cell[] Table = new Cell[9];

        bool button_disabled = false;
        int humanWins = 0, aiWins = 0, draws = 0;

        static List<List<int>> winnerIndexes = new()
        {
            new List<int>() { 0, 1, 2 },
            new List<int>() { 3, 4, 5 },
            new List<int>() { 6, 7, 8 },
            new List<int>() { 0, 3, 6 }, 
            new List<int>() { 1, 4, 7 }, 
            new List<int>() { 2, 5, 8 },
            new List<int>() { 0, 4, 8 }, 
            new List<int>() { 2, 4, 6 }
        };

        public Cell CurrentTurn = Cell.X;

        int ChoosenMove = -1;

        public Cell Human = Cell.X;
        public Cell AI = Cell.O;
        #endregion

        public Form1()
        {
            InitializeComponent();
            CurrentTurn = Cell.X;
            Human = Cell.X;
            AI = Cell.O;
            ChoosenMove = -1;
        }

        #region update game
        public void Clear()
        {
            CurrentTurn = Cell.X;
            Table = new Cell[9];
            button1.Text = "";
            button2.Text = "";
            button3.Text = "";
            button4.Text = "";
            button5.Text = "";
            button6.Text = "";
            button7.Text = "";
            button8.Text = "";
            button9.Text = "";
            button_disabled = false;
            ChoosenMove = -1;
        }

        public void Play()
        {
            if (CurrentTurn == Human)
            {
                Button button = GetButtonByIndex(ChoosenMove);
                button.Text = "X";
                Table = MoveInTable(Table, CurrentTurn, ChoosenMove);
                CurrentTurn = GetRival(CurrentTurn);
                CheckForWinner();
            }
            else if (CurrentTurn == AI)
            {
                Minimax(DuplicateTable(Table), CurrentTurn);
                Button button = GetButtonByIndex(ChoosenMove);
                button.Text = "O";
                Table = MoveInTable(Table, CurrentTurn, ChoosenMove);
                CurrentTurn = GetRival(CurrentTurn);
                CheckForWinner();
            }
        }

        static Cell[] MoveInTable(Cell[] Table, Cell Move, int Position)
        {
            Cell[] newTable = DuplicateTable(Table);
            newTable[Position] = Move;
            return newTable;
        }
        #endregion

        #region minimax
        int Minimax(Cell[] InputTable, Cell Player)
        {
            Cell[] Table = DuplicateTable(InputTable);

            if (GetScore(Table, AI) != 0)
                return GetScore(Table, AI);
            else if (CheckDrawGame(Table)) 
                return 0;

            List<int> scores = new();
            List<int> moves = new();

            for (int i = 0; i < 9; i++)
            {
                if (Table[i] == Cell.Empty)
                {
                    scores.Add(Minimax(MoveInTable(Table, Player, i), GetRival(Player)));
                    moves.Add(i);
                }
            }

            if (Player == AI)
            {
                int MaxScoreIndex = scores.IndexOf(scores.Max());
                ChoosenMove = moves[MaxScoreIndex];
                return scores.Max();
            }
            else
            {
                int MinScoreIndex = scores.IndexOf(scores.Min());
                ChoosenMove = moves[MinScoreIndex];
                return scores.Min();
            }
        }
        #endregion

        #region side methods
        static Cell GetRival(Cell Cell)
        {
            if (Cell == Cell.X)
                return Cell.O;
            else
                return Cell.X;
        }

        static int GetScore(Cell[] Table, Cell Player)
        {
            if (Winner(Table, Player)) 
                return 1;
            else if (Winner(Table, GetRival(Player))) 
                return -1;
            else 
                return 0;
        }

        static Cell[] DuplicateTable(Cell[] Table)
        {
            Cell[] Clone = new Cell[9];
            for (int i = 0; i < 9; i++) 
                Clone[i] = Table[i];

            return Clone;
        }

        private Button GetButtonByIndex(int index)
        {
            return index switch
            {
                0 => button1,
                1 => button2,
                2 => button3,
                3 => button4,
                4 => button5,
                5 => button6,
                6 => button7,
                7 => button8,
                8 => button9,
                _ => throw new NotImplementedException()
            };
        }
        #endregion

        #region winner checks
        private void CheckForWinner()
        {
            if (Winner(Table, Human))
            {
                humanWins++;
                label4.Text = Convert.ToString(humanWins);
                MessageBox.Show("Humano venceu!");
                button_disabled = true;
            }
            else if (Winner(Table, AI))
            {
                aiWins++;
                label5.Text = Convert.ToString(aiWins);
                MessageBox.Show("Maquina venceu!");
                button_disabled = true;
            }
            else if (CheckDrawGame(Table))
            {
                draws++;
                label6.Text = Convert.ToString(draws);
                MessageBox.Show("Empate");
                button_disabled = true;
            }
            if (CurrentTurn == AI && !button_disabled)
            {
                Play();
            }
        }

        static bool CheckDrawGame(Cell[] Table)
        {
            foreach (Cell p in Table)
                if (p == Cell.Empty)
                    return false;

            return true;
        }

        private static bool Winner(Cell[] table, Cell player)
        {
            if (WinnerIndexes(table, player, winnerIndexes[0]) ||
                WinnerIndexes(table, player, winnerIndexes[1]) ||
                WinnerIndexes(table, player, winnerIndexes[2]) ||
                WinnerIndexes(table, player, winnerIndexes[3]) ||
                WinnerIndexes(table, player, winnerIndexes[4]) ||
                WinnerIndexes(table, player, winnerIndexes[5]) ||
                WinnerIndexes(table, player, winnerIndexes[6]) ||
                WinnerIndexes(table, player, winnerIndexes[7]))
                return true;

            return false;
        }

        private static bool WinnerIndexes(Cell[] table, Cell player, List<int> indexes)
        {
            if (table[indexes[0]] == player && table[indexes[1]] == player && table[indexes[2]] == player)
                return true;
            return false;
        }
        #endregion

        #region Button interactions
        private void button1_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button1.Text == "")
            {
                ChoosenMove = 0;
                Play();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button2.Text == "")
            {
                ChoosenMove = 1;
                Play();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button3.Text == "")
            {
                ChoosenMove = 2;
                Play();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button4.Text == "")
            {
                ChoosenMove = 3;
                Play();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button5.Text == "")
            {
                ChoosenMove = 4;
                Play();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button6.Text == "")
            {
                ChoosenMove = 5;
                Play();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button7.Text == "")
            {
                ChoosenMove = 6;
                Play();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button8.Text == "")
            {
                ChoosenMove = 7;
                Play();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (CurrentTurn == Human && !button_disabled && button9.Text == "")
            {
                ChoosenMove = 8;
                Play();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion
    }
}