using ChessBE;
using ChessBE.Pieces;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Forms;

/// <summary>
/// A Windows Form class implementing the front end of the Chess game
/// - Displayes the board
/// - Handles user's inputs (Mouse clicks on the pieces and tiles, Ctrl+R to refresh the board)
/// </summary>
public partial class ChessboardForm : Form
{
    private readonly Panel[,] panelsArray = new Panel[8, 8];
    private Board board = Board.GetInstance();
    private Dictionary<Label, Piece> labelToPiece = new Dictionary<Label, Piece>();
    private Piece? selectedPiece = null;
    private const int tileSize = 64; // Size of each tile
    private CheckBox manualOrAI = new CheckBox();
    private Label turnLabel = new Label();

    /// <summary>
    /// Constructor - initialize UI components and place the pieces in their start positions
    /// </summary>
    public ChessboardForm()
    {
        initializeWindow();
        initilaizeControls();
        drawPieces();
        this.KeyPreview = true;
        this.KeyDown += new KeyEventHandler(HandleKeyDown);

    }

    private void initilaizeControls()
    {
        initializeChessboard();
        initializeButtons();
    }

    private void initializeButtons()
    {
        const int leftAlightX = 550;

        manualOrAI.Text = "Single Player";
        manualOrAI.Checked = true;
        manualOrAI.Location = new System.Drawing.Point(leftAlightX, 50);
        manualOrAI.Click += new EventHandler(manualOrAIClick);

        this.Controls.Add(manualOrAI);

        turnLabel.Text = "Turn: " + Board.GetInstance().GetTurnColor();
        turnLabel.Location = new System.Drawing.Point(leftAlightX, 100);

        this.Controls.Add(turnLabel);


        
    }

    private void manualOrAIClick(Object sender, EventArgs e)
    {
        Board.GetInstance().SetAutoPlay(manualOrAI.Checked);
    }

    /// <summary>
    /// Register Ctrl+R to call beack a full refresh of the board
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleKeyDown(object sender, KeyEventArgs e)
    { 
      if (e.Control && e.KeyCode == Keys.R) {
            refreshAll();
      } 
    }

    /// <summary>
    /// Basic / default OnPaint callback
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaint(PaintEventArgs e)
    {

        // Call the base class implementation
        base.OnPaint(e);
    }

    /// <summary>
    /// Regresh all pieces on the board
    /// </summary>
    private void refreshAll()
    {
        this.Controls.Clear();
        initilaizeControls();
        drawPieces();
    }

    /// <summary>
    /// Draw all chess pieces 
    /// </summary>
    private void drawPieces()
    {
        foreach (Piece piece in board.whitePlayer.Pieces)
            drawPiece(piece);
        foreach (Piece piece in board.blackPlayer.Pieces)
            drawPiece(piece);
    }
    
    /// <summary>
    /// Draw a single piece (a label on top of a panel)
    /// </summary>
    /// <param name="piece"></param>
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

    /// <summary>
    /// Callback when the user clicks on a piece on the board
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
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

                    Position targetPos = new Position(label.Parent.Location.X / tileSize, label.Parent.Location.Y / tileSize);
                    List<Position> oldPositions;
                    if (selectedPiece.Move(targetPos, true,  out oldPositions))
                    {
                        foreach (Position oldPos in oldPositions)
                        {
                            panelsArray[oldPos.Col, oldPos.Row].Controls.Clear();
                        }
                        panelsArray[selectedPiece.Pos.Col, selectedPiece.Pos.Row].Controls.Clear();
                        selectedPiece = null;
                        drawPieces();
                        if (checkEndGame()) {
                            return;
                        }
                        resetPanels();
                        notifyCheck();
                        if (passTurn(out oldPositions))
                        {
                            foreach (Position oldPos in oldPositions)
                            {
                                panelsArray[oldPos.Col, oldPos.Row].Controls.Clear();
                            }
                            drawPieces();
                            if (checkEndGame()) {
                                return;
                            }
                            resetPanels();
                            notifyCheck();

                        }
                    }
                }
            }
        }
    }

    
    private bool passTurn(out List<Position> oldPositions)
    {
        bool res = Board.GetInstance().passTurn(out oldPositions);
        turnLabel.Text = "Turn: " + Board.GetInstance().GetTurnColor();
        Console.Beep();
        return res;
    }

    /// <summary>
    /// Callback when the user clicks on a tile that has no piece on it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ea"></param>
    private void handlePanelClick(Object? sender, EventArgs ea)
    {

        if (sender is Panel panel && selectedPiece != null)
        {
            Position targetPos = new Position(panel.Location.X / tileSize, panel.Location.Y / tileSize);
            List<Position> oldPositions = new List<Position>();
            if (selectedPiece.Move(targetPos, true, out oldPositions))
            {
                foreach (Position oldPos in oldPositions)
                {
                    panelsArray[oldPos.Col, oldPos.Row].Controls.Clear();
                }
                selectedPiece = null;
                drawPieces();
                if (checkEndGame())
                {
                    return;
                }
                resetPanels();
                notifyCheck();
                if (passTurn(out oldPositions))
                {

                    foreach (Position oldPos in oldPositions)
                    {
                        panelsArray[oldPos.Col, oldPos.Row].Controls.Clear();
                    }
                    drawPieces();
                    if (checkEndGame())
                    {
                        return;
                    }
                    resetPanels();
                    notifyCheck();
                }
            }
        }
    }

    /// <summary>
    /// Check for checkmate and display a winning message
    /// </summary>
    private bool checkEndGame()
    {
        if (Board.GetInstance().BoardCheckmateStatus == CheckmateStatus.None)
            return false;

        if (Board.GetInstance().BoardCheckmateStatus == CheckmateStatus.White)
        {
            MessageBox.Show("Black won!");
        }
        else if (Board.GetInstance().BoardCheckmateStatus == CheckmateStatus.Black)
        {
            MessageBox.Show("White won!");
        }
        Board.ResetBoard();
        this.board = Board.GetInstance();
        this.Controls.Clear();
        this.initilaizeControls();
        this.drawPieces();
        return true;
    }

    /// <summary>
    /// Redraw the chessboard tiles as panels
    /// </summary>
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

    /// <summary>
    /// Indicate a chess status on the board by changing the panel color of the relevant king to "crimson" (red)
    /// </summary>
    private void notifyCheck()
    {
        Position? k = null;
        if (board.BoardCheckStatus == CheckStatus.White)
        {
            k = board.GetKingPos(PieceColor.White);
            
        }
        else if (board.BoardCheckStatus == CheckStatus.Black)
        {
            k = board.GetKingPos(PieceColor.Black);
        }
        if (k != null)
        {
            panelsArray[k.Col, k.Row].BackColor = Color.Crimson;   
        }
    }

    /// <summary>
    /// Load the icons / images of the various chess pieces
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// simplified resize operation without taking aspect ratio into account
    /// </summary>
    /// <param name="originalImage"></param>
    /// <param name="size"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Initialize the Form Window - fixed size
    /// </summary>
    private void initializeWindow()
    {
        const int WIDTH = 700;
        const int HEIGHT = 550;
        this.Size = new Size(WIDTH, HEIGHT);
        this.MinimumSize = new Size(WIDTH, HEIGHT); 
        this.MaximumSize = new Size(WIDTH, HEIGHT); 

        // Set the title of the window
        this.Text = "Chess Game (Sagi)";
    }

    /// <summary>
    /// Initialize the Form Component
    /// </summary>
    private void InitializeComponent()
    {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ChessboardForm";
            this.Load += new System.EventHandler(this.ChessboardForm_Load);
            this.ResumeLayout(false);

    }

    /// <summary>
    /// Load callback of the form - empty for now
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChessboardForm_Load(object sender, EventArgs e)
    {

    }
}
