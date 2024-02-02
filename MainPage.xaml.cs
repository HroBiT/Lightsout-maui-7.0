using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Aplikacja
{
    public partial class MainPage : ContentPage
    {
        private const int GridSize = 5;
        private Button[,] buttons = new Button[GridSize, GridSize];
        private bool[,] grid = new bool[GridSize, GridSize];
        private int moves = 0;

        public MainPage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            for (int row = 0; row < GridSize; row++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int col = 0; col < GridSize; col++)
            {
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Button button = new Button
                    {
                        BackgroundColor = Colors.LightGray,
                        BorderColor = Colors.Black,
                        BorderWidth = 1
                    };
                    button.Clicked += OnButtonClicked;
                    buttons[row, col] = button;
                    gameGrid.Add(button, col, row);
                }
            }

            ToggleButton(2, 2);
        }

        private void StartNewGame()
        {
            Random rand = new Random();

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    grid[row, col] = false;
                    buttons[row, col].BackgroundColor = Colors.LightGray;
                }
            }

            ToggleButtonAndNeighbors(3, 3);

            for (int i = 0; i < GridSize * GridSize; i++)
            {
                int row = rand.Next(GridSize);
                int col = rand.Next(GridSize);
                ToggleButtonAndNeighbors(row, col);
            }

            moves = 0;
            moveCount.Text = moves.ToString();
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            var position = FindButtonPosition(clickedButton);
            if (position.HasValue)
            {
                ToggleButtonAndNeighbors(position.Value.Item1, position.Value.Item2);
                moves++;
                moveCount.Text = moves.ToString();
                if (IsGameWon())
                {
                    DisplayAlert("Gratulacje!", "Wygrałeś w " + moves.ToString() + " ruchach!", "Nowa Gra").ContinueWith(t => StartNewGame());
                }
            }
        }

        private void ToggleButtonAndNeighbors(int row, int col)
        {
            ToggleButton(row, col);
            ToggleButton(row - 1, col);
            ToggleButton(row + 1, col);
            ToggleButton(row, col - 1);
            ToggleButton(row, col + 1);

            
            Console.WriteLine("cos robie");
            PrintGridState();
        }

        private void ToggleButton(int row, int col)
        {
            if (row >= 0 && row < GridSize && col >= 0 && col < GridSize)
            {
                grid[row, col] = !grid[row, col];
                buttons[row, col].BackgroundColor = grid[row, col] ? Colors.DarkGray : Colors.LightGray;
            }
        }

        private void PrintGridState()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Console.Write(grid[i, j] ? "1 " : "0 ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private (int, int)? FindButtonPosition(Button button)
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    if (buttons[row, col] == button)
                    {
                        return (row, col);
                    }
                }
            }
            return null;
        }

        private bool IsGameWon()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    if (grid[row, col])
                    {
                        return false;
                    }
                }
            }

            Console.WriteLine("robie cos ?");
            PrintGridState();

            return true;
        }

        private void OnNewGameClicked(object sender, EventArgs e)
        {
            StartNewGame();
        }
    }
}
