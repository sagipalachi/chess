using ChessBE;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Windows.Forms;

public partial class ChessboardForm : Form
{
    private Panel[,] _chessBoardPanels;
    private Board board = new Board();

    public ChessboardForm()
    {
        InitializeWindow();
        InitializeChessboard();
        DrawPieces();
    }


    private void DrawPieces()
    {
        List<Piece> pieces = board.GetPieces();
        foreach (Piece piece in pieces)
        {
            DrawPiece(piece);
        }
    }

    private void DrawPiece(Piece piece)
    {
        Image? image = GetImageForPiece(piece);
        if (image != null)
        {
            Label label = new Label
            {
                Size = new Size(64,64),
                Location = new Point(0,0),
                Image = image,
                //ImageAlign = ContentAlignment.TopCenter
            };
            _chessBoardPanels[piece.Pos.Col, piece.Pos.Row].Controls.Add(label);
        }
    }

    private static Image? GetImageForPiece(Piece piece)
    {
        Image? res = null;
        string fname = "";
        if (piece.GetType() == typeof(King))
        { fname = (piece.Color == PieceColor.White) ? "KingW.png" : "KingB.png"; }
        if (piece.GetType() == typeof(Queen))
        { fname = (piece.Color == PieceColor.White) ? "QueenW.png" : "QueenB.png"; }
        if (piece.GetType() == typeof(Rook))
        { fname = (piece.Color == PieceColor.White) ? "RookW.png" : "RookB.png"; }
        if (piece.GetType() == typeof(Bishop))
        { fname = (piece.Color == PieceColor.White) ? "BishopW.png" : "BishopB.png"; }
        if (piece.GetType() == typeof(Knight))
        { fname = (piece.Color == PieceColor.White) ? "KnightW.png" : "KnightB.png"; }
        if (piece.GetType() == typeof(Pawn))
        { fname = (piece.Color == PieceColor.White) ? "PawnW.png" : "PawnB.png"; }

        if (fname.Length > 0) {
            string fullpath = "G:\\c#\\Chess1\\Chess1\\ChessAssets\\" + fname;
            Image img = Image.FromFile(fullpath);
            res = ResizeImage(img, new Size(50, 50));
        }
        return res;
    }

    public static Image ResizeImage(Image imgToResize, Size size)
    {
        // Get the image's original width and height
        int sourceWidth = imgToResize.Width;
        int sourceHeight = imgToResize.Height;

        // Calculate the new width and height
        float nPercentW = (float)size.Width / sourceWidth;
        float nPercentH = (float)size.Height / sourceHeight;
        float nPercent = Math.Min(nPercentW, nPercentH);

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        // Create a new bitmap with the new dimensions
        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage(b);

        // Set the interpolation mode for high quality resizing
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        // Draw the resized image
        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();

        return b;
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
                newPanel.BackColor = (row + col) % 2 == 0 ? Color.DarkGray : Color.DimGray;
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

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // ChessboardForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ChessboardForm";
            this.Load += new System.EventHandler(this.ChessboardForm_Load_1);
            this.ResumeLayout(false);

    }

    private void ChessboardForm_Load_1(object sender, EventArgs e)
    {

    }

    // You can add more functionality here, like placing chess pieces on the board.
}
