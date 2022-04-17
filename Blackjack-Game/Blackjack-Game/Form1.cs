namespace Blackjack_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPlaySingle_Click(object sender, EventArgs e)
        {
            GameBoard gb = new GameBoard(0);
            this.Hide();
            gb.ShowDialog();
            this.Show();
        }
    }
}