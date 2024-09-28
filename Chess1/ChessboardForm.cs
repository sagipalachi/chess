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
    private const int tileSize = 64; // Size of each tile

    public ChessboardForm()
    {
        initializeWindow();
        initializeChessboard();
        drawPieces();
    }

    protected override void OnPaint(PaintEventArgs e)
    {

        // Call the base class implementation
        base.OnPaint(e);
    }

    private void drawPieces()
    {
        foreach (Piece piece in board.whitePlayer.Pieces)
            drawPiece(piece);
        foreach (Piece piece in board.blackPlayer.Pieces)
            drawPiece(piece);
    }

    private void drawPiece(Piece piece)
    {
        Image? image = getImageForPiece(piece);
        if (image != null)
        {
            Label label = new Label { Size = new Size(64, 64), Location = new Point(0, 0), Image = image,};
            panelsArray[piece.Pos.Col, piece.Pos.Row].Controls.Add(label);
            labelToPiece.Add(label, piece);
            label.MouseClick += handleLabelPieceClick;
            label.Parent.BackColor = (piece.Pos.Row + piece.Pos.Col)%2 == 0 ? Color.DarkGray : Color.DimGray;
        }
    }

    private void handleLabelPieceClick(Object? sender, EventArgs ea)
    {
       if (sender is Label label)
       {
            if (labelToPiece.TryGetValue(label, out Piece? piece))
            {
                if (Board.GetInstance().CheckTurn(piece))
                {
                    selectedPiece = null;
                    resetPanels();
                    selectedPiece = piece;
                    label.Parent.BackColor = Color.Blue;
                    List<Position> positions = piece.GetPotentialPositions();
                    foreach (Position pos in positions)
                    {
                        Color c = (pos.Row + pos.Col) % 2 == 0 ? Color.Green : Color.DarkGreen;
                        panelsArray[pos.Col, pos.Row].BackColor = c;
                        panelsArray[pos.Col, pos.Row].BorderStyle = BorderStyle.FixedSingle;
                    }
                }
                else if (selectedPiece != null)
                {

                    Position oldPos = selectedPiece.Pos;
                    Position targetPos = new Position(label.Parent.Location.X / tileSize, label.Parent.Location.Y / tileSize);
                    if (selectedPiece.Move(targetPos))
                    {
                        panelsArray[oldPos.Col, oldPos.Row].Controls.Clear();
                        panelsArray[selectedPiece.Pos.Col, selectedPiece.Pos.Row].Controls.Clear();
                        selectedPiece = null;
                        drawPieces();
                        Board.GetInstance().passTurn();
                        resetPanels();
                    }
                }
            }
            
        }
    }

    private void handlePanelClick(Object? sender, EventArgs ea)
    {

        if (sender is Panel panel && selectedPiece != null)
        {
            Position oldPos = selectedPiece.Pos;
            Position targetPos = new Position(panel.Location.X / tileSize, panel.Location.Y / tileSize);
            if (selectedPiece.Move(targetPos))
            {
                selectedPiece = null;
                drawPieces();
                Board.GetInstance().passTurn();
                panelsArray[oldPos.Col, oldPos.Row].Controls.Clear();
                resetPanels();

            }
        }
    }

    private void resetPanels()
    {
        for (var col = 0; col < panelsArray.GetLength(0); col++)
        {
            for (var row = 0; row < panelsArray.GetLength(1); row++)
            {
                panelsArray[col,row].BackColor = (row + col) % 2 == 0 ? Color.DarkGray : Color.DimGray;
            }
        }

    }

    private static Image? getImageForPiece(Piece piece)
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
            res = resizeImage(img, new Size(50, 50));
        }
        return res;
    }
    
    // simplified resize operation without taking aspect ratio into account
    // assuming dimenstions ratio are the same
    private static Image resizeImage(Image originalImage, Size size)
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

    private void initializeChessboard()
    {
        for (var col = 0; col < panelsArray.GetLength(0); col++)
        {
            for (var row = 0; row < panelsArray.GetLength(1); row++)
            {
                var newPanel = new Panel
                {
                    Size = new Size(tileSize, tileSize),
                    Location = new Point(tileSize * col, tileSize * row)
                };

                Controls.Add(newPanel); // Add to the form's Controls
                newPanel.MouseClick += handlePanelClick;
                panelsArray[col, row] = newPanel; // Store in our 2D array

                // Alternate background colors
                newPanel.BackColor = (row + col) % 2 == 0 ? Color.DarkGray : Color.DimGray;
            }
        }
    }

    private void initializeWindow()
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
