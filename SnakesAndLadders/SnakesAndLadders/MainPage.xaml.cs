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
        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnRollDice_Clicked(object sender, EventArgs e)
        {
            int row = 0, column = 0;    // avoid a null assignment error - have a default value
            // retrieve values
            row = Convert.ToInt32(EntryRow.Text);
            column = Convert.ToInt32(EntryCol.Text);

            /*
             * set the purple box position.
             * The Grid.Row and Grid.Column properties are "shared" between the grid and the box
             * The grid owns the square, the box wants to use it.
             * it gets set using a method called SetValue for shared properties
             */
            BoxPurple.SetValue(Grid.RowProperty, row);
            BoxPurple.SetValue(Grid.ColumnProperty, column);

        }
    }
}
