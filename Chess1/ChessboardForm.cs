using ChessBE;
using ChessBE.Pieces;
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
    private Panel[,] panelsArray = new Panel[8, 8];
    private Board board = Board.GetInstance();
    private Dictionary<Label, Piece> labelToPiece = new Dictionary<Label, Piece>();
    private Piece? selectedPiece = null;

    public ChessboardForm()
    {
        InitializeWindow();
        InitializeChessboard();
        DrawPieces();
    }

    protected override void OnPaint(PaintEventArgs e)
    {

        // Call the base class implementation
        base.OnPaint(e);
    }

    private void DrawPieces()
    {
        foreach(Piece piece in board.blackPlayer.Pieces.Values)
            DrawPiece(piece);
        foreach (Piece piece in board.whitePlayer.Pieces.Values)
            DrawPiece(piece);
    }

    private void DrawPiece(Piece piece)
    {
        Image? image = GetImageForPiece(piece);
        if (image != null)
        {
            Label label = new Label { Size = new Size(64, 64), Location = new Point(0, 0), Image = image,};
            panelsArray[piece.Pos.Col, piece.Pos.Row].Controls.Add(label);
            labelToPiece.Add(label, piece);
            label.MouseClick += HandleLabelPieceClick;
            if (piece == selectedPiece)
            {
                label.Parent.BackColor = Color.Blue;
            } 
            else
            {
                label.Parent.BackColor = (piece.Pos.Row + piece.Pos.Col)%2 == 0 ? Color.DarkGray : Color.DimGray;
            }
        }
    }

    void HandleLabelPieceClick(Object? sender, EventArgs ea)
    {
       if (sender is Label label)
       {
            if (labelToPiece.TryGetValue(label, out Piece? piece))
            {
                selectedPiece = piece;
            }
            DrawPieces();
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
            string fullpath = Application.StartupPath + "ChessAssets\\" + fname;
            Image img = Image.FromFile(fullpath);
            res = ResizeImage(img, new Size(50, 50));
        }
        return res;
    }
    
    // simplified resize operation without taking aspect ratio into account
    // assuming dimenstions ratio are the same
    private static Image ResizeImage(Image originalImage, Size size)
    {
        // Create a new bitmap with the new dimensions
        Bitmap b = new Bitmap(size.Width, size.Height);
        Graphics g = Graphics.FromImage(b);

        // Set the interpolation mode for high quality resizing
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        // Draw the resized image
        g.DrawImage(originalImage, 0, 0, size.Width, size.Height);

        g.Dispose();

        return b;
    }

    private void InitializeChessboard()
    {
        const int tileSize = 64; // Size of each tile

        for (var row = 0; row < panelsArray.GetLength(0); row++)
        {
            for (var col = 0; col < panelsArray.GetLength(1); col++)
            {
                var newPanel = new Panel
                {
                    Size = new Size(tileSize, tileSize),
                    Location = new Point(tileSize * col, tileSize * row)
                };

                Controls.Add(newPanel); // Add to the form's Controls
                panelsArray[row, col] = newPanel; // Store in our 2D array

                // Alternate background colors
                newPanel.BackColor = (row + col) % 2 == 0 ? Color.DarkGray : Color.DimGray;
            }
        }
    }

    private void InitializeWindow()
    {
        const int WIDTH = 524;
        const int HEIGHT = 550;
        this.Size = new Size(WIDTH, HEIGHT);
        this.MinimumSize = new Size(WIDTH, HEIGHT); 
        this.MaximumSize = new Size(WIDTH, HEIGHT); 

        // Set the title of the window
        this.Text = "Chess Game (Sagi)";
    }
}
