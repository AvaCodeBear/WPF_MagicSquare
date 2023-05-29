using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicSquare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Row> MatrixRows { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.MatrixRows = new List<Row>(5);
            for(int i = 0; i < 5; i++)
                this.MatrixRows.Add(new Row());
 
            TheMatrix.ItemsSource = this.MatrixRows;
        }
        // generates a random 5 row magic square using linear equation y = mx + b 
        // with random values for x, b and start x
        private void GenSquare_Click(object sender, RoutedEventArgs e)
        {
            // y = mx+b
            int m = new Random().Next(1, 11); // slope is a random num between 1 and 10
            int b = new Random().Next(0, 100); // constant is a random num between 0 and 100
            int x = new Random().Next(-1, 9);    // start x is a random num between 0 and 9
            int scratch;
            int magicNum = 0;

            List<List<string>> gridData = new List<List<string>>(5);
            for (int i = 0; i < 5; i++)
                gridData.Add(new List<string>(5) { "","","","",""});

            int r = 4;
            int c = 2;
            for(int cell = 0; cell < 25; cell++)
            {
                int svr = r;
                int svc = c; // save col, row
                scratch = m * x++ + b;
                magicNum += scratch;
                gridData[r][c] = scratch.ToString();
                // decrement r by 2, inc col by 1 wrap around if new value off grid
                if (r == 1)
                    r = 4;
                else if (r == 0)
                    r = 3;
                else
                    r -= 2;

                if (c == 4)
                    c = 0;
                else
                    c += 1;
                if (gridData[r][c] != "") // new cell is already populated
                {
                    r = svr-1;
                    c = svc;
                    if (r < 0)
                        r = 4;
                }
            }
            WriteGridVals(gridData);
            magicNum /= 5;
            MessageLabel.Content = "The Sum of Each Row, Column and Diaganol is " + magicNum.ToString();
        }

        private void Rotate_Right_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> gridVals = ReadGridVals();
            List<List<string>> newgridVals = new List<List<string>>(5);

            for(int i = 0; i < 5; i++)
                newgridVals.Add(new List<String>(5) { gridVals[4][i], gridVals[3][i], gridVals[2][i], gridVals[1][i], gridVals[0][i] });

            WriteGridVals(newgridVals);

        }
        private void Rotate_Left_Click(object sender, RoutedEventArgs e)
        {
            // Rotate Right 3 Times is same as Rotate Left
            Rotate_Right_Click(sender, e);
            Rotate_Right_Click(sender, e);
            Rotate_Right_Click(sender, e);
        }

        private void FoldH_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> gridVals = ReadGridVals();
            List<string> tmp = gridVals[0];
            gridVals[0] = gridVals[4];
            gridVals[4] = tmp;
            tmp = gridVals[1];
            gridVals[1] = gridVals[3];
            gridVals[3] = tmp;
            WriteGridVals(gridVals);
        }

        private void FoldV_Click(object sender, RoutedEventArgs e)
        {
            // Rotate Right twice, then fold horizontally is same as fold vertically
            Rotate_Right_Click(sender, e);
            Rotate_Right_Click(sender, e);
            FoldH_Click(sender, e);
        }

        private List<List<string>> ReadGridVals()
        {
            List<List<string>> rVal = new List<List<string>>(5);
            for(int i = 0; i < 5; i++)
            {
                rVal.Add(new List<string>(5));
                rVal[i].Add(this.MatrixRows[i].Cval0);
                rVal[i].Add(this.MatrixRows[i].Cval1);
                rVal[i].Add(this.MatrixRows[i].Cval2);
                rVal[i].Add(this.MatrixRows[i].Cval3);
                rVal[i].Add(this.MatrixRows[i].Cval4);
            }
            return rVal;
        }

        private void WriteGridVals(List<List<string>> gridVals)
        {
            for(int i = 0; i < 5; i++)
            {
                MatrixRows[i].Cval0 = gridVals[i][0];
                MatrixRows[i].Cval1 = gridVals[i][1];
                MatrixRows[i].Cval2 = gridVals[i][2];
                MatrixRows[i].Cval3 = gridVals[i][3];
                MatrixRows[i].Cval4 = gridVals[i][4];
            }
            TheMatrix.Items.Refresh();
        }

    }

    public class Row
    {
        public Row() { Cval0 = Cval1 = Cval2 = Cval3 = Cval4 = ""; }
        public string Cval0 { get; set; }
        public string Cval1 { get; set; }
        public string Cval2 { get; set; }
        public string Cval3 { get; set; }
        public string Cval4 { get; set; }
    }
}
