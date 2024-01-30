using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife;

public class GameOfLife
{
    private bool[,] board;
    private int width;
    private int height;

    public int Width => width;
    public int Height => height;
    public int CurrentGeneration { get; private set; }

    public GameOfLife(int width, int height)
    {
        this.width = width;
        this.height = height;
        board = new bool[width, height];
    }

    // Indexer to access the cells
    public bool this[int x, int y]
    {
        get { return board[x, y]; }
    }

    // Randomly populate the board
    public void Randomize()
    {
        Random random = new Random();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //board[x, y] = random.Next(2) == 0; appears too blobby


                // Adjust the chance of being alive, e.g., 10% chance
#if WINDOWS
                board[x, y] = random.NextDouble() < 0.1; 
#elif ANDROID
                board[x, y] = random.NextDouble() < 0.07;
#endif
            }
        }
    }

    // Calculate the next generation
    public void NextGeneration()
    {
        bool[,] newBoard = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int aliveNeighbors = CountAliveNeighbors(x, y);
                bool currentCell = board[x, y];

                if (currentCell && (aliveNeighbors == 2 || aliveNeighbors == 3))
                {
                    newBoard[x, y] = true;
                }
                else if (!currentCell && aliveNeighbors == 3)
                {
                    newBoard[x, y] = true;
                }
                else
                {
                    newBoard[x, y] = false;
                }
            }
        }
        CurrentGeneration++;
        board = newBoard;
    }

    // Count alive neighbors for a cell
    private int CountAliveNeighbors(int x, int y)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int newX = (x + i + width) % width;
                int newY = (y + j + height) % height;

                count += board[newX, newY] ? 1 : 0;
            }
        }

        return count;
    }
}
