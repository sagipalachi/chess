using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

public partial class ChessboardForm : Form
{
    private Panel[,] _chessBoardPanels;

    public ChessboardForm()
    {
        InitializeWindow();
        InitializeChessboard();
    }

    private void InitializeChessboard()
    {
        const int tileSize = 64; // Size of each tile
        const int gridSize = 8; // Number of rows and columns

        _chessBoardPanels = new Panel[gridSize, gridSize];

        for (var row = 0; row < gridSize; row++)
        {
            for (var col = 0; col < gridSize; col++)
            {
                var newPanel = new Panel
                {
                    Size = new Size(tileSize, tileSize),
                    Location = new Point(tileSize * col, tileSize * row)
                };

                Controls.Add(newPanel); // Add to the form's Controls
                _chessBoardPanels[row, col] = newPanel; // Store in our 2D array

                // Alternate background colors
                newPanel.BackColor = (row + col) % 2 == 0 ? Color.DarkBlue : Color.LightGray;
            }
        }
    }

    private void InitializeWindow()
    {
        this.Size = new Size(524, 550); // Width: 800, Height: 600

        // Optionally, set the minimum and maximum size
        this.MinimumSize = new Size(524, 550); // Minimum size
        this.MaximumSize = new Size(524, 550); // Maximum size

        // Set the title of the window
        this.Text = "Chess Game (Sagi)";
    }

    private void ChessboardForm_Load(object sender, EventArgs e)
    {

    }

    // You can add more functionality here, like placing chess pieces on the board.
}
