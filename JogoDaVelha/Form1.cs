namespace JogoDaVelha
{
    public partial class Form1 : Form
    {
        #region Variables
        bool aiTurn = false, button_disabled = false;
        int humanWins = 0, aiWins = 0, draws = 0;
        readonly string human = "X", ai = "O";
        List<List<int>> winnerIndexes = new()
        {
            new List<int> { 0, 1, 2 },
            new List<int> { 3, 4, 5 },
            new List<int> { 6, 7, 8 },
            new List<int> { 0, 3, 6 },
            new List<int> { 1, 4, 7 },
            new List<int> { 2, 5, 8 },
            new List<int> { 0, 4, 8 },
            new List<int> { 2, 4, 6 }
        };
        internal class Move
        {
            public int Points { get; set; }
            public int Index { get; set; }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region Clear the game screen
        private void Clear()
        {
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
            aiTurn = false;
        }
        #endregion

        #region Get Table, Button and EmptyCells
        private List<string> GetTable()
        {
            return new() { button1.Text, button2.Text, button3.Text, button4.Text, button5.Text, button6.Text, button7.Text, button8.Text, button9.Text };
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

        private List<int> GetEmptyCells()
        {
            return GetTable().Select((value, index) => (value, index))
                .Where(x => string.IsNullOrEmpty(x.value))
                .Select(x => x.index).ToList();
        }
        #endregion

        #region Play
        private void Play(Button button, string player)
        {
            button.Text = player;
            if (aiTurn)
            {
                aiTurn = false;
                button_disabled = false;
            }
            else
            {
                aiTurn = true;
                button_disabled = false;
            }
            CheckForWinner();
        }
        #endregion

        #region Project and calculate where to play
        private void ProjectPlay(string player)
        {
            var betterMove = CalculateBetterMove(player);
            Play(GetButtonByIndex(betterMove.Index), player);
        }

        private Move CalculateBetterMove(string player)
        {
            var table = GetTable();
            var emptyCells = GetEmptyCells();

            var move = new Move() { Index = -1, Points = -1000 };
            for (var i = 0; i < emptyCells.Count; i++)
            {
                int score = MovementScore(emptyCells[i], player);

                if (score > move.Points)
                {
                    move.Points = score;
                    move.Index = emptyCells[i];
                }
            }
            return move;
        }

        private int MovementScore(int index, string player)
        {
            // 02 pontos se for a posi��o do meio
            // 01 ponto se for alguma das posi��es das bordas
            // Menos 02 pontos, se j� tiver uma pe�a do advers�rio na mesma linha, coluna ou diagonal da posi��o selecionada
            // 10 pontos se a posi��o impedir a vit�ria do advers�rio
            // 10 pontos se a jogada resultar em vit�ria
            var table = GetTable();
            int score = 0, center = 4;
            int[] corners = { 0, 2, 6, 8 };

            if (corners.Contains(index))
                score += 1;
            if (center == index)
                score += 2;
            foreach (var indexes in winnerIndexes.Where(i => i.Contains(index)))
            {
                if (CountPlayerInIndexes(table, player == ai ? human : ai, indexes) == 1)
                    score -= 2;
                if (WinnerIndexes(table, player, indexes))
                    score += 10;
                if (WinnerIndexes(table, player == ai ? human : ai, indexes))
                    score += 10;
            }
            return score;
        }
        #endregion

        #region Winning checks
        private void CheckForWinner()
        {
            var table = GetTable();
            if (Winner(table, human))
            {
                humanWins++;
                label4.Text = Convert.ToString(humanWins);
                MessageBox.Show("Humano venceu!");
                button_disabled = true;
            }
            else if (Winner(table, ai))
            {
                aiWins++;
                label5.Text = Convert.ToString(aiWins);
                MessageBox.Show("Maquina venceu!");
                button_disabled = true;
            }
            else if (!GetEmptyCells().Any())
            {
                draws++;
                label6.Text = Convert.ToString(draws);
                MessageBox.Show("Empate");
                button_disabled = true;
            }
            if (aiTurn && !button_disabled)
            {
                ProjectPlay(ai);
            }
        }

        private bool Winner(List<string> table, string player)
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

        private static bool WinnerIndexes(List<string> table, string player, List<int> indexes)
        {
            if (table[indexes[0]] == player && table[indexes[1]] == player && table[indexes[2]] == player)
                return true;
            return false;
        }

        private static int CountPlayerInIndexes(List<string> table, string player, List<int> indexes)
        {
            int count = 0;
            if (table[indexes[0]] == player)
                count++;
            if (table[indexes[1]] == player)
                count++;
            if (table[indexes[2]] == player)
                count++;
            return count;
        }
        #endregion

        #region Button interactions
        private void button1_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button1.Text == "")
                Play(button1, human);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button2.Text == "")
                Play(button2, human);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button3.Text == "")
                Play(button3, human);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button4.Text == "")
                Play(button4, human);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button5.Text == "")
                Play(button5, human);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button6.Text == "")
                Play(button6, human);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button7.Text == "")
                Play(button7, human);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button8.Text == "")
                Play(button8, human);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!aiTurn && !button_disabled && button9.Text == "")
                Play(button9, human);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion
    }
}