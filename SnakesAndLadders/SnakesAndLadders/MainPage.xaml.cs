using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SnakesAndLadders
{
    public partial class MainPage : ContentPage
    {
        //Constant
        const int FIRST_GAME_ROW = 10, FIRST_GAME_COLUMN = 1;

        //Create instance of random
        Random random;

        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnRollDice_Clicked(object sender, EventArgs e)
        {
            //Variables
            int diceRoll = 0;
            
            //Generate a random number between 1 and 6
            if(random == null)
            {
                random = new Random();
            }

            //Set label to dice number
            diceRoll = random.Next(1, 7);
            LblDiceRoll.Text = diceRoll.ToString();

            //Move piece, currently only purple
            MovePiece();
        }

        private void MovePiece()
        {
            //Variables
            //Must tell code what i want it to be so use (int) before it
            int currentRow = (int)PurplePiece.GetValue(Grid.RowProperty);

            //If piece is not on board, move to square 1 on board (10,1)
            if (currentRow == 11)
            {
                PurplePiece.SetValue(Grid.RowProperty, FIRST_GAME_ROW);
                PurplePiece.SetValue(Grid.ColumnProperty, FIRST_GAME_COLUMN;
            }
        }

    }
}
