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
        //Constant to check if player on board
        const int FIRST_GAME_ROW = 10, FIRST_GAME_COLUMN = 1;
        const int GAME_START_ROW = 11;

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
            MovePiece(diceRoll);
        }

        private void MovePiece(int diceRoll)
        {
            //Variables
            //Must tell code what i want it to be so use (int) before it
            int currentRow = (int)PurplePiece.GetValue(Grid.RowProperty);
            int currentColumn;

            //First Move
            if (currentRow == GAME_START_ROW)
            {
                //Put piece on board
                PurplePiece.SetValue(Grid.RowProperty, FIRST_GAME_ROW);
                PurplePiece.SetValue(Grid.ColumnProperty, FIRST_GAME_COLUMN);

                //Adjust diceroll as first move is adding piece to board
                diceRoll--;
            }

            //Move piece by diceroll amount
            currentColumn = (int)PurplePiece.GetValue(Grid.ColumnProperty);
            currentColumn += diceRoll;
            PurplePiece.SetValue(Grid.ColumnProperty, currentColumn);

            //When roll goes over space on board
            int over = (diceRoll + currentColumn);
            if (over > 10)
            {
                over -= 10;

                int row = ((int)PurplePiece.GetValue(Grid.RowProperty)+1);
                int column = ((int)PurplePiece.GetValue(Grid.ColumnProperty)-10);
                

                PurplePiece.SetValue(Grid.RowProperty, row);
                PurplePiece.SetValue(Grid.ColumnProperty, column);
            }
        }
    }
}
