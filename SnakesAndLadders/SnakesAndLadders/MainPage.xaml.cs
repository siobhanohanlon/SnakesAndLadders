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
        //Constants
        const int GAME_START_ROW = 11; //Start Row
        const int RHS_COL = 10, LHS_COL = 1, TOP_ROW = 1, BOTTOM_ROW = 10; //Row and Column dementions
        const int NUMBER_OF_SNAKES = 3, NUMBER_OF_LADDERS = 3; //# of snakes and ladders
        const int MAX_PLAYERS = 5; //# of players
        //Strings
        const string ROLL_DICE = "Roll Dice";
        const string NEW_GAME = "New Game";
        const string PLAYER = "Player";

        //Current player
        int _currentPlayer = 1;
        bool _IsLR;
        int _LR;
        int _currentBoundary;

        //Create a random class
        Random random;

        //Array for snakes
        int[][] _snakes;

        public MainPage()
        {
            InitializeComponent();
            // set up the snakes and ladders at the start of app
            SetUpSnakes();
        }

        //Reset the Board for New Game. Repositions the pieces on the board, set the direction of movement.
        private void SetUpNewGame()
        {
            //_IsLR = true;   // always starts on Row 10 - moving Left to right
            _LR = 1;
            _currentBoundary = RHS_COL;
            BtnRollDice.Text = ROLL_DICE;
            _currentPlayer = 1;
            LblPlayer.Text = PLAYER + " " + _currentBoundary.ToString();
        }

        private void SetUpSnakes()
        {
            // initialise the global arra with all the snakes.
            // already decided on the representation
            //int[] OneSnake = new int[4] { 4, 9, 7, 10 };
            _snakes = new int[3][]
            {
                new int[4] {4, 9, 7, 10},
                new int[4] {7, 5, 7, 7},
                new int[4] {2, 4, 2, 9}
            };  // end of snakes array
        }



        #region All the movement methods for horizontal, vertical, snakes, ladders
        /// <summary>
        ///     all the logic for making a move for any piece on theboard.
        /// </summary>
        /// <param name="diceRoll">The number of spaces to move.</param>
        private async void MovePiece(int diceRoll)
        {
            BtnRollDice.IsEnabled = false;
            BtnRollDice.Text = "Moving";

            BoxView currentPiece;
            string currentPlayerName = PLAYER + _currentPlayer.ToString();
            currentPiece = (BoxView)GrdGameLayout.FindByName(currentPlayerName);
            int currentRow = (int)currentPiece.GetValue(Grid.RowProperty);
            int currentCol, diff;

            if (currentRow == GAME_START_ROW)  // first move only
            {
                // put the piece on square 1 on the board
                currentPiece.SetValue(Grid.RowProperty, BOTTOM_ROW);
                currentPiece.SetValue(Grid.ColumnProperty, LHS_COL);
                diceRoll--;
            }

            ChangeDirection(currentPiece);
            // piece is on square 1 on the board
            currentCol = (int)currentPiece.GetValue(Grid.ColumnProperty);

            // 0, 5, 7 -----   (0 - 5) = 5 >= 7
            //if (Math.Abs(_currentBoundary - diceRoll) >= currentCol)
            // 0, 7, 5                    0, 3, 5
            if (Math.Abs(_currentBoundary - currentCol) >= diceRoll)
            {
                //MoveRight(PurplePiece, diceRoll);
                await MoveHorizontal(currentPiece, diceRoll);
            }
            else
            {
                // diceroll < BOARD_RHS - currentCol
                //MoveRight(PurplePiece, RHS_COL - currentCol); 
                diff = Math.Abs(_currentBoundary - currentCol);
                await MoveHorizontal(currentPiece, diff);

                diceRoll -= diff;
                if (diceRoll > 0)
                {
                    await MoveUpRow(currentPiece);
                    diceRoll--;
                    //MoveLeft(PurplePiece, diceRoll);
                    await MoveHorizontal(currentPiece, diceRoll);
                }
            } // move finished
            CheckForSnakes(currentPiece);
            BtnRollDice.Text = ROLL_DICE;
            BtnRollDice.IsEnabled = true;
        }   // end of MovePiece()

        /// <summary>
        /// Move a piece by spaces in either the left or right direction.
        /// </summary>
        /// <param name="Piece"></param>
        /// <param name="Spaces"></param>
        private async Task MoveHorizontal(BoxView Piece, int Spaces)
        {
            int currCol = (int)Piece.GetValue(Grid.ColumnProperty);
            //int newCol = currCol + Spaces;
            //if(!_IsLR)
            //{
            //    newCol = currCol - Spaces;
            //}
            //Piece.SetValue(Grid.ColumnProperty, newCol);
            // have a value for _LR (1 or -1)
            //Piece.SetValue(Grid.ColumnProperty, currCol + (Spaces * _LR));

            //Piece.TranslateTo
            int hDistance = ((int)GrdGameLayout.Width / 12) * Spaces * _LR;
            uint timeValue = (uint)(Spaces * 150);  // 150ms per space
            // need the system to wait for the animation to finish
            // do this using a construct called "await" and "async"
            await Piece.TranslateTo(hDistance, 0, timeValue);
            Piece.TranslationX = 0;
            // also need to change Column Property - translation won't
            Piece.SetValue(Grid.ColumnProperty, currCol + (Spaces * _LR));
        }

        private async Task MoveUpRow(BoxView Piece)
        {
            int currentRow = (int)Piece.GetValue(Grid.RowProperty);
            int vDistance = ((int)GrdGameLayout.Height / 12) * -1;
            // need the system to wait for the animation to finish
            // do this using a construct called "await" and "async"
            await Piece.TranslateTo(0, vDistance, 250);
            Piece.TranslationY = 0;
            // move up, row value goes down
            Piece.SetValue(Grid.RowProperty, currentRow - 1);
            ChangeDirection(Piece);
        }

        private void MoveLeft(BoxView Piece, int Spaces)
        {
            int currCol = (int)Piece.GetValue(Grid.ColumnProperty);
            Piece.SetValue(Grid.ColumnProperty, currCol - Spaces);
        }

        /// <summary>
        /// Change direction of movement for the current piece, reset the boundary for the maths
        /// </summary>
        private void ChangeDirection(BoxView Piece)
        {
            //_LR *= -1;  // goes from 1 to -1, or -1 to 1

            int currentRow = (int)Piece.GetValue(Grid.RowProperty);
            if (currentRow % 2 == 0)    // going L to R
            {
                //_IsLR = true;
                _LR = 1;
                _currentBoundary = RHS_COL;
            }
            else
            {
                //_IsLR = false;
                _LR = -1;
                _currentBoundary = LHS_COL;
            }
        }

        private async void MoveAlongSnakeLadderAsync(BoxView Piece,
                                  int FinishX, int FinishY)
        {
            // 4,9 - 7, 10
            // need the start point from the piece
            int startX = (int)Piece.GetValue(Grid.ColumnProperty);
            int startY = (int)Piece.GetValue(Grid.RowProperty);
            int hSpaces = FinishX - startX; // 7 - 4 = 3, 5 - 7 = -2
            int vSpaces = FinishY - startY; // 10 - 9
            uint timeValue = (uint)(Math.Max(hSpaces, vSpaces) * 150);  // 150ms per space

            //Piece.TranslateTo
            int hDistance = ((int)GrdGameLayout.Width / 12) * hSpaces;
            int vDistance = ((int)GrdGameLayout.Width / 12) * vSpaces;
            // need the system to wait for the animation to finish
            // do this using a construct called "await" and "async"
            await Piece.TranslateTo(hDistance, vDistance, timeValue);
            Piece.TranslationX = 0;
            Piece.TranslationY = 0;
            // also need to change Column Property - translation won't
            Piece.SetValue(Grid.ColumnProperty, FinishX);
            Piece.SetValue(Grid.RowProperty, FinishY);
            ChangeDirection(Piece); // verifies the direction of movement
        }


        #endregion

        #region Event Handlers and other misc methods

        private void CheckForSnakes(BoxView Piece)
        {
            // scroll through the array for snakes and check if the current
            // piece is on the head of a snake. use a loop, include a break
            // to exit the loop if I find a snake
            // OneSnake = new int[4] { HeadC, HeadR, TailC, TailR };
            int iCounter = 0;
            int HeadC = 0, HeadR = 1, TailC = 2, TailR = 3;
            int playerCol = (int)Piece.GetValue(Grid.ColumnProperty);
            int playerRow = (int)Piece.GetValue(Grid.RowProperty);

            while (iCounter < NUMBER_OF_SNAKES)
            {
                if (playerCol == _snakes[iCounter][HeadC] &&
                    playerRow == _snakes[iCounter][HeadR])  // on a snake
                {
                    // could put up message saying "You landed on a snake"
                    MoveAlongSnakeLadderAsync(Piece,
                                              _snakes[iCounter][TailC],
                                              _snakes[iCounter][TailR]);
                    iCounter = NUMBER_OF_SNAKES + 1;
                }
                iCounter++;
            }   // end while

        }


        private void BtnRollDice_Clicked(object sender, EventArgs e)
        {
            int diceRoll = 0;
            string ButtonText = ((Button)sender).Text;

            //switch (BtnRollDice.Text)
            switch (ButtonText)
            {
                case ROLL_DICE:
                    {
                        // generate a random number between 1 and 6
                        if (random == null)
                        {
                            random = new Random();
                        }
                        diceRoll = random.Next(1, 7);
                        LblDiceRoll.Text = diceRoll.ToString(); // update UI
                        MovePiece(diceRoll);    // currently only move the purple piece
                        _currentPlayer++;
                        if (_currentPlayer > MAX_PLAYERS) _currentPlayer = 1;
                        break;
                    }
                case NEW_GAME:
                    {
                        SetUpNewGame();
                        break;
                    }
            }
        }

        #endregion
    } // end public partial class MainPage : ContentPage
}
