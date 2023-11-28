using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Checker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _playerMove = true;

        private List<Button> ListOfButton;

        private Button _prevButton;

        private Button BlackChecker = new Button();
        private Button BlackKingChecker = new Button();
        private Button WhiteChecker = new Button();
        private Button WhiteKingChecker = new Button();

        private Button _nextButton;

        private int row = 0, column = 0, prevRow = 0, prevCol = 0;

        private int _countBlackChecker = 12, _countWhiteChecker = 12;

        private Button[,] _CheckerBoard;

        bool isMove = true;

        public MainWindow()
        {
            InitializeComponent();

            BlackChecker.Background = BACKGROUND("/View/Image/BlackChecker.png");
            BlackKingChecker.Background = BACKGROUND("/View/Image/BlackKingChecker.png");
            WhiteChecker.Background = BACKGROUND("/View/Image/WhiteChecker.png");
            WhiteKingChecker.Background = BACKGROUND("/View/Image/WhiteKingChecker.png");

            InitializationBoard();
            NewGame();
        }

        bool IsKillChecker()
        {

            return false;
        }

        void killMoveChecker(int row1, int col1, int row2, int col2)
        {
            Button button1 = GetButtonByCoordinates(row1, col1);
            Button button2 = GetButtonByCoordinates(row2, col2);

            Button buttonKill = GetButtonByCoordinates((row1 + row2) / 2, (col1 + col2) / 2);

            if (Math.Abs(row1 - row2) <= 1 && Math.Abs(col1 - col2) <= 1 &&
                (buttonKill.Background == WhiteChecker.Background || buttonKill.Background == WhiteKingChecker.Background) &&
                button1.Background == BlackChecker.Background && button2.Background == Brushes.Black)
            {
                buttonKill.Background = Brushes.Black;
            }
        }

        void killMoveKingChecker()
        {

        }

        void CheckerMove(object sender, RoutedEventArgs e)//ход простой шашки
        {
            if (isMove)
            {
                Button b = (Button)sender;
                prevRow = Grid.GetRow(b);
                prevCol = Grid.GetColumn(b);
                _prevButton = b;
                isMove = !isMove;
                _prevButton.BorderThickness = new Thickness(5);
            }
            else
            {
                _nextButton = (Button)sender;
                Button b = _nextButton;
                row = Grid.GetRow(_nextButton);
                column = Grid.GetColumn(_nextButton);
                SwapChecker(prevRow, prevCol, row, column);
                //killMoveChecker(prevRow, prevCol, row, column);
                //KingChecker();
                _prevButton.BorderThickness = new Thickness(0);

                isMove = !isMove;
            }
        }

        /*bool isBlackChecker(int row1, int col1)//является ли черной шашкой
        {
            Button button1 = GetButtonByCoordinates(row1, col1);
            if (button1.Background == BlackChecker.Background)
                return true;
            else
                return false;
        }

        void Swap(int row1, int col1, int row2, int col2)
        {
            Button button1 = GetButtonByCoordinates(row1, col1);
            Button button2 = GetButtonByCoordinates(row2, col2);
            
            // Проверить, что обе кнопки найдены
            if (Math.Abs(row1 - row2) <= 1 && Math.Abs(col1 - col2) <= 1 && button1.Background != Brushes.Black
                && button2.Background == Brushes.Black && isBlackChecker(row1, col1))
            {
                // Получить позиции кнопок в Grid
                int index1 = Grid.GetRow(button1);
                int index2 = Grid.GetRow(button2);
                int index12 = Grid.GetColumn(button1);
                int index22 = Grid.GetColumn(button2);

                // Поменять местами строки кнопок в Grid
                Grid.SetRow(button1, index2);
                Grid.SetRow(button2, index1);

                // Поменять местами строки кнопок в Grid
                Grid.SetColumn(button1, index22);
                Grid.SetColumn(button2, index12);
            }
        }*/

        private void SwapChecker(int row1, int col1, int row2, int col2)//поменять кнопки местами
        {
            ListOfButton = gameBoard.Children.Cast<Button>().ToList();
            // Найти кнопки по заданным координатам
            //Button button1 = GetButtonByCoordinates(row1, col1);
            //Button button2 = GetButtonByCoordinates(row2, col2);

            // Проверить, что обе кнопки найдены
            if (Math.Abs(row2 - row1) <= 1 && Math.Abs(col2 - col1) <= 1//неверно обращается к заднему фону
                && (_prevButton.Background == BlackChecker.Background/* || button1.Background == WhiteChecker.Background*/)
                && _nextButton.Background == Brushes.Black)
            {
                // Поменять местами строки кнопок в Grid
                Grid.SetRow(_prevButton, row);
                Grid.SetRow(_nextButton, prevRow);

                // Поменять местами строки кнопок в Grid
                Grid.SetColumn(_prevButton, column);
                Grid.SetColumn(_nextButton, prevCol);
            }
            else
            {
                MessageBox.Show("не работает");
            }


            /*else if (row2 - row1 == col2 - col1 &&
                button1.Background == BlackKingChecker.Background && button2.Background == Brushes.Black)
            {
                // Получить позиции кнопок в Grid
                int index1 = Grid.GetRow(button1);
                int index2 = Grid.GetRow(button2);
                int index12 = Grid.GetColumn(button1);
                int index22 = Grid.GetColumn(button2);

                // Поменять местами строки кнопок в Grid
                Grid.SetRow(button1, index2);
                Grid.SetRow(button2, index1);

                // Поменять местами строки кнопок в Grid
                Grid.SetColumn(button1, index22);
                Grid.SetColumn(button2, index12);
            }*/
        }

        private Button GetButtonByCoordinates(int row, int col)
        {
            // Найти кнопку в Grid по заданным координатам
            foreach (Button element in gameBoard.Children)
                if (Grid.GetRow(element) == row && Grid.GetColumn(element) == col)
                {
                    return element;
                }
            return null;
        }

        void ReloadBoard()//обновление доски после каждого хода
        {

        }

        private bool IsGameOver()//проверка на окончание игры
        {
            if (_countBlackChecker == 0 || _countWhiteChecker == 0)
                return true;
            else
                return false;
        }

        private bool IsKingChecker()
        {
            if (_nextButton.Background == BlackKingChecker.Background)
                return true;
            else
                return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)//кнопка новой игры
        {
            //NewGame();
        }

        void NewGame()//при нажатии на новую игру чтоб перезапускалось
        {
            CreateCheckers();
            ListOfButton = gameBoard.Children.Cast<Button>().ToList();

        }

        void KingChecker()
        {
            if (row == 0 && _prevButton.Background == BlackChecker.Background)
                _prevButton.Background = BlackKingChecker.Background;
            else if (row == 7 && _prevButton.Background == WhiteChecker.Background)
                _prevButton.Background = WhiteKingChecker.Background;
        }

        void InitializationBoard()//инициализация игрового поля
        {
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if ((r + c) % 2 == 0)
                    {
                        Button button = new Button();
                        button.Background = Brushes.Black;
                        button.Click += CheckerMove;

                        Grid.SetRow(button, r);
                        Grid.SetColumn(button, c);
                        gameBoard.Children.Add(button);
                    }
        }

        ImageBrush BACKGROUND(string path) // для установки фона шашки
        {
            Uri resourceUri = new Uri(path, UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            var whiteChecker = new ImageBrush();
            whiteChecker.ImageSource = temp;

            return whiteChecker;
        }

        public void CreateCheckers()//сощздание шашек
        {
            ListOfButton = gameBoard.Children.Cast<Button>().ToList();
            int a = 0;

            ListOfButton.ForEach(button =>
            {
                if (a < 12)
                {
                    button.Background = BACKGROUND("/View/Image/WhiteChecker.png");
                    a++;
                }
                else if (a >= 20)
                {
                    button.Background = BACKGROUND("/View/Image/BlackChecker.png");
                    a++;
                }
                else
                    a++;
            }
            );
        }
    }
}
