using System.Diagnostics;

namespace GameOfLife;

public partial class MainPage : ContentPage
{
    private GameOfLife game;
    private int maxGenerations = 10000;
    public int currentGenerations = 0;
    private bool isRunning;
    public MainPage()
    {
        InitializeComponent();

        game = new GameOfLife(350, 350);
        game.Randomize();
        
        var gameDrawable = new DrawGameDrawable(game);
        gameDrawable.OnGenerationProcessed += UpdateProgressBar;
        gameView.Drawable = gameDrawable;


        StartGame();
    }

    private void UpdateProgressBar(int obj)
    {
        double progress = (double)game.CurrentGeneration / maxGenerations;
        progressBar.Progress = progress;
        prog.Text = $"Total Number of Generations = {game.CurrentGeneration}";

        if (game.CurrentGeneration >= maxGenerations)
        {
            StopGame();
        }
    }

    private void StartGame()
    {
        
        isRunning = true;
        this.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(200), () =>
        {
            
            // Invalidate the GraphicsView to trigger a redraw
            gameView.Invalidate();

            return isRunning; 
        });
        
    }

    private void StopGame()
    {
        isRunning = false;
    }
    private void OnStartClicked(object sender, EventArgs e)
    {
        StartGame();
    }

    //private void DrawGame(ICanvas canvas, RectF dirtyRect)
    //{
    //    canvas.FillColor = Colors.Black;
    //    canvas.FillRectangle(dirtyRect);

        //    float cellWidth = dirtyRect.Width / game.Width;
        //    float cellHeight = dirtyRect.Height / game.Height;

        //    for (int x = 0; x < game.Width; x++)
        //    {
        //        for (int y = 0; y < game.Height; y++)
        //        {
        //            if (game[x, y]) // Check if the cell is alive
        //            {
        //                canvas.FillColor = Colors.White;
        //                canvas.FillRectangle(x * cellWidth, y * cellHeight, cellWidth, cellHeight);
        //            }
        //        }
        //    }

        //    game.NextGeneration();
        //}


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        isRunning = false;
    }

  

    private void OnStopClicked(object sender, EventArgs e)
    {
        StopGame();
    }

}

public class DrawGameDrawable : IDrawable
{
    public event Action<int> OnGenerationProcessed;
    private readonly GameOfLife _game;

    public DrawGameDrawable(GameOfLife game)
    {
        _game = game;
    }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.White; // Background color
        canvas.FillRectangle(dirtyRect);

        // Cell size calculation
        float cellWidth = dirtyRect.Width / _game.Width;
        float cellHeight = dirtyRect.Height / _game.Height;
        float margin = 1; // Adjust the margin size as needed

        // Iterate over each cell to draw it
        for (int x = 0; x < _game.Width; x++)
        {
            for (int y = 0; y < _game.Height; y++)
            {
                if (_game[x, y]) // Check if the cell is alive
                {
                    // Set the color for alive cells
                    canvas.FillColor = Colors.Black;
                    canvas.FillRectangle(
                    x * cellWidth + margin / 2,
                    y * cellHeight + margin / 2,
                    cellWidth - margin,
                    cellHeight - margin
                        );
                }
                else
                {
                    // Set the color for dead cells (or skip drawing them)
                    continue;
                }

                // Draw the cell as a rectangle
                canvas.FillRectangle(x * cellWidth, y * cellHeight, cellWidth, cellHeight);
            }
        }

        // Update the game to the next generation
        _game.NextGeneration();

        // Trigger the event with the current generation count
        OnGenerationProcessed?.Invoke(_game.CurrentGeneration);
    }
}
