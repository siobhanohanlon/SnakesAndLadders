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
        const int GAME_START_ROW = 11;
        const int BOARD_RHS = 10, BOARD_LHS = 1;
        const int BOARD_TOP = 1, BOARD_BOTTOM = 10;

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
            int currentRow = (int)PurplePiece.GetValue(Grid.RowProperty);
            int currentCol;

            if(currentRow == GAME_START_ROW)
            {
                //Put piece on board
                PurplePiece.SetValue(Grid.RowProperty, BOARD_BOTTOM);
                PurplePiece.SetValue(Grid.ColumnProperty, BOARD_LHS);

                //Adjust diceroll as first move is adding piece to board
                diceRoll--;
            }

            //Piece is on square 1
            currentCol = (int)PurplePiece.GetValue(Grid.ColumnProperty);
            
            //Wont go off board
            if((BOARD_RHS - diceRoll) >= currentCol)
            {
                //Move right Method
                MoveRight(PurplePiece, diceRoll);
                currentCol += diceRoll;
                PurplePiece.SetValue(Grid.ColumnProperty, currentCol);
            }

            //Dice roll over left on board
            //Diceroll > BOARD_RHS - currentcol
            else
            {
                //Move right
                int move = (BOARD_RHS - currentCol);
                MoveRight(PurplePiece, move);
                diceRoll -= move;

                //if greater than 0
                if(diceRoll>0)
                {
                    //Move up a row
                    MoveUpRow(PurplePiece);
                    diceRoll--;

                    //Move Left
                    MoveLeft(PurplePiece, diceRoll);
                }
            }
        }

        //Move Right Function
        private void MoveRight(BoxView Piece, int Spaces)
        {
            //Get current row
            int currentCol = (int)PurplePiece.GetValue(Grid.ColumnProperty);

            //Change collumn movements by dice roll
            int move= (currentCol + Spaces);
            Piece.SetValue(Grid.ColumnProperty, move);
        }

        //Move Up Row
        private void MoveUpRow(BoxView Piece)
        {
            //Find current row
            int currentR = (int)PurplePiece.GetValue(Grid.RowProperty);

            //Increase row by 1, must minus as starting at bottom
            Piece.SetValue(Grid.RowProperty, currentR);
        }

        //Move Left
        private void MoveLeft(BoxView Piece, int Spaces)
        {
            //find current Columnn
            int currentCol = (int)PurplePiece.GetValue(Grid.ColumnProperty);

            //Change collumn movements by diceroll
            int move = (currentCol - Spaces);
            Piece.SetValue(Grid.ColumnProperty, move);
        }
    }
}
