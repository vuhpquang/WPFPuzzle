using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project2_PuzzleGame
{
    class Board
    {

        private int columns;
        public int Columns { get => columns; set => columns = value; }
        private int rows;
        public int Rows { get => rows; set => rows = value; }

        public int[,] chess;
        public Button[,] showBtn;


        public void Init(int rs, int cls)
        {
            columns = cls;
            rows = rs;
            chess = new int[rs, cls];
            showBtn = new Button[rs, cls];

            // UI
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var button = new Button();
                    button.Tag = new Tuple<int, int>(i, j);

                    // Dua vao model quan li UI
                    chess[i, j] = 0;
                    showBtn[i, j] = button; // Luu tham chieu toi button

                }
            }
        }

        public static int LocalBtn(Button btn)
        {
            int result = 1;



            return result;
        }

        public bool checkWin()
        {
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    if((i == Rows -1) && (j == Columns -1) )
                    {
                        if (chess[i, j] != 0) return false;
                    }
                    else
                    {
                        if (chess[i, j] != i * Rows + j + 1) return false;
                    }
                }
            }

            return true;
        }

        public void Save(string fileName, int time)
        {
            string text = "";

            // save row, col
            text += Rows + " " + Columns + "\n";

            // save time
            text += time.ToString() + "\n";

            // save board
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    text += chess[i, j].ToString();
                    if (j != Columns - 1)
                    {
                        text += " ";
                    }
                }
                text += "\n";
            }

            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Save\\";
            File.WriteAllText(path + fileName + ".txt", text);
        }

        public void Load(string fileName)
        {
            string absolute_path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string path = absolute_path + "Save\\";

            string[] lines = File.ReadAllLines(path + fileName + ".txt");

            string[] mat = lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Rows = int.Parse(mat[0]);
            Columns = int.Parse(mat[1]);
            this.Init(Rows, Columns);

            MainWindow.time = int.Parse(lines[1]);

            int r = 2;
            for (int i = 0; i < Rows; i++)
            {
                string[] temp = lines[r++].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < Columns; j++)
                {
                    chess[i, j] = int.Parse(temp[j]);                 
                }
            }
        }
    }
}
