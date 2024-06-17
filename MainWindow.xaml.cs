using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace PlaytoztheGame
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int width = 50;
        private const int height = 50;
        private const int cellSize = 10;
        private int[,] gameGrid;
        private bool isRunning = false;
        private DispatcherTimer gameTimer;
        private Color _player1Color = Colors.Blue;
        private Color _player2Color = Colors.Red;
        private int currentPlayer = 1;

        public Color Player1Color
        {
            get { return _player1Color; }
            set
            {
                if (_player1Color != value)
                {
                    _player1Color = value;
                    OnPropertyChanged(nameof(Player1Color));
                    UpdateGameUI();
                }
            }
        }

        public Color Player2Color
        {
            get { return _player2Color; }
            set
            {
                if (_player2Color != value)
                {
                    _player2Color = value;
                    OnPropertyChanged(nameof(Player2Color));
                    UpdateGameUI();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();

            gameGrid = new int[width, height];

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(0.5);
            gameTimer.Tick += GameTimer_Tick;

            DataContext = this;

            UpdateGameUI();
        }

        private void UpdateGameUI()
        {
            gameCanvas.Children.Clear();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (gameGrid[i, j] == 1)
                    {
                        DrawCell(i, j, Player1Color);
                    }
                    else if (gameGrid[i, j] == 2)
                    {
                        DrawCell(i, j, Player2Color);
                    }
                    else
                    {
                        DrawCell(i, j, Colors.White);
                    }
                }
            }

            Player1ColorRect.Fill = new SolidColorBrush(Player1Color);
            Player2ColorRect.Fill = new SolidColorBrush(Player2Color);
        }

        private void DrawCell(int x, int y, Color color)
        {
            Rectangle rect = new Rectangle
            {
                Width = cellSize,
                Height = cellSize,
                Fill = new SolidColorBrush(color)
            };
            Canvas.SetLeft(rect, x * cellSize);
            Canvas.SetTop(rect, y * cellSize);
            gameCanvas.Children.Add(rect);
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isRunning)
            {
                Point clickPosition = e.GetPosition(gameCanvas);
                int xIndex = (int)(clickPosition.X / cellSize);
                int yIndex = (int)(clickPosition.Y / cellSize);

                if (xIndex >= 0 && xIndex < width && yIndex >= 0 && yIndex < height)
                {
                    gameGrid[xIndex, yIndex] = currentPlayer;
                    UpdateGameUI();
                    SwitchPlayer();
                }
            }
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = true;
            gameTimer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
            gameTimer.Stop();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
            gameTimer.Stop();
            gameGrid = new int[width, height];
            currentPlayer = 1;
            UpdateGameUI();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            UpdateGameState();
            UpdateGameUI();
        }

        private int CountEnemyNeighbors(int x, int y, int enemyPlayer)
        {
            int count = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < width && j >= 0 && j < height && !(i == x && j == y))
                    {
                        if (gameGrid[i, j] == enemyPlayer)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private void UpdateGameState()
        {
            int[,] newGrid = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int neighbors = CountNeighbors(i, j);

                    if (gameGrid[i, j] != 0)
                    {
                        if (neighbors == 2 || neighbors == 3)
                        {
                            newGrid[i, j] = gameGrid[i, j];
                        }
                        else
                        {
                            newGrid[i, j] = 0;
                        }
                    }
                    else
                    {
                        if (neighbors == 3)
                        {
                            newGrid[i, j] = currentPlayer;
                        }
                    }
                }
            }

            gameGrid = newGrid;
            UpdateGameUI();
        }

        private int CountNeighbors(int x, int y)
        {
            int count = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < width && j >= 0 && j < height && !(i == x && j == y))
                    {
                        if (gameGrid[i, j] != 0)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private void Player1ColorButton_Click(object sender, RoutedEventArgs e)
        {
            var colorPicker = new ColorPickerDialog(Player1Color);
            if (colorPicker.ShowDialog() == true)
            {
                Player1Color = colorPicker.SelectedColor;
                UpdateGameUI();
            }
        }

        private void Player2ColorButton_Click(object sender, RoutedEventArgs e)
        {
            var colorPicker = new ColorPickerDialog(Player2Color);
            if (colorPicker.ShowDialog() == true)
            {
                Player2Color = colorPicker.SelectedColor;
                UpdateGameUI();
            }
        }
        private void Button_ClickTG(object sender, RoutedEventArgs e)
        {
            string telegramLink = "https://t.me/ABOBAJojikan";

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = telegramLink,
                UseShellExecute = true
            });
        }
    }
}