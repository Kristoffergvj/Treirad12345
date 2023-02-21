using System;
using System.Xml.Linq;

public class GameStarter
{
    public static void Main(String[] args)
    {
        TicTacToe game = new TicTacToe();
        game.play();

    }
}
public class TicTacToe : Confirmed
{
    string nameOfPlayerOne;
    string nameOfPlayerTwo;
    bool shouldGenerateRandomID;
    int boardSize;
    int rowPlacement; 
    int colPlacement; 
    int numberOfPlacements; 
    int numberOfWinsByPlayer1;
    int numberOfWinsByPlayer2;

    private Player player1;
    private Player player2;
    private GameBoard gameBoard;

    private int[,] rows;
    private int[,] columns;
    private int[,] diagonals;
    

    public TicTacToe()
    {

        nameOfPlayerOne = "PlayerOne";
        nameOfPlayerTwo = "PlayerTwo";
        shouldGenerateRandomID = false;
        boardSize = 3;
        rowPlacement = -1;
        colPlacement = -1;
        numberOfPlacements = 0;
        numberOfWinsByPlayer1 = 0;
        numberOfWinsByPlayer2 = 0;

        //try
        
            this.prepare();

        //catch(IndexOutOfRangeException e)
        //{
        //    Console.WriteLine("Inpaaat måste vara en av siffrorna i menyn");
        //}




        //this.getPlayerNames();
       this.playerNamefunction();
        
        
        

        this.player1 = new Player(nameOfPlayerOne);
        this.player2 = new Player(nameOfPlayerTwo);
        this.gameBoard = new GameBoard(new AlternatingColor(new Color(ConsoleColor.Red), new Color(ConsoleColor.Cyan)));

        this.rows = new int[3, boardSize];
        this.columns = new int[3, boardSize];
        this.diagonals = new int[3, 3];
        }
    string getInput()
    {
        string? input = Console.ReadLine();
        if (input != null)
            return input;
       
        return "";
    }
    public void playerNamefunction()
    {
        try
        {


            this.getPlayerNames();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Namnen får måste vara minst 1 tecken, högst 15 och får inte innehålla en siffra");
            playerNamefunction();
        }
    }
   
    public void play()
    {
        int playerID = 0, winnerID = 0;
        shouldGenerateRandomID = true;

        while (true)
        {
            // Startar endast vid nytt spel
            if (shouldGenerateRandomID)
            {
                playerID = this.idGenerator();
                shouldGenerateRandomID = false;
            }

            playerID = this.playerTurn(playerID); // Player tur
            ++numberOfPlacements;
            winnerID = Confirm(playerID);

            if (winnerID == 1)
            {
                NotificationCenter.resultMessage(player2.name);
                ++numberOfWinsByPlayer2;
            }
            else if (winnerID == 2)
            {
                NotificationCenter.resultMessage(player1.name);
                ++numberOfWinsByPlayer1;
            }
            else if ((numberOfPlacements == boardSize * boardSize) && (winnerID == -1))
            {
                NotificationCenter.resultMessage();
            }
            else
            {
                continue; // Om det inte finns en vinnare så skippa
            }

            this.gameBoard.PrintBoard(); // Update and print brädan
            playAgainOrExit(); // Välj att spela igen eller avsluta
        }
    }

    private void prepare()
    {
        MenuOptions[] m = { new StartGame(), new ExitGame() };
        NotificationCenter.welcome(m);
        

        

            int.TryParse(Console.ReadLine(), out int choice);
        if (choice < 1 || choice > m.Length)
        {
            Console.WriteLine("Input måste vara ett nummer i menyn");
            prepare();
        }
        else
            m[choice-1].menuMessage();
            m[choice - 1].select();
        
        


    }
    

    
    public void getPlayerNames()
    {
      
        
            NotificationCenter.namesAndSize(1);
            nameOfPlayerOne = getInput();
            NotificationCenter.namesAndSize(2);
            nameOfPlayerTwo = getInput();
        if (nameOfPlayerOne.Length > 15 || nameOfPlayerTwo.Length > 15)
        {
            throw new ArgumentException("Namnet får inte vara över 15 tecken");
        }
        else if (nameOfPlayerOne.Length < 1 || nameOfPlayerTwo.Length < 1)
        {
            throw new ArgumentException("Namnet måste vara över 0 tecken");
        }
        else if (nameOfPlayerOne.Any(char.IsDigit) || nameOfPlayerTwo.Any(char.IsDigit))
        {
            throw new ArgumentException("Namnet får inte innehålla en siffra");
        }

    }

    private int idGenerator()
    {
        Random seed = new Random();
        int id;
        id = seed.Next(1, 3);

        return id;
    }

  
    private int playerTurn(int playerID)
    {
        if (playerID == 1)
        {
            Checker checker1 = new Checker('X');
            this.player1.move(this.gameBoard, checker1);
            rowPlacement = this.player1.rowIndex;
            colPlacement = this.player1.colIndex;
            playerID = 2; // Spelar tur
        }
        else
        {
            Checker checker2 = new Checker('O');
            this.player2.move(this.gameBoard, checker2);
            rowPlacement = this.player2.rowIndex;
            colPlacement = this.player2.colIndex;
            playerID = 1; 
        }

        return playerID;
    }


    public override int Confirm(int playerID)
    {
        BoardCell[,] board = gameBoard.getBoard();
        string diag = "";
        string diag2 = "";
        for (int i = 0; i < 3; i++)
        {
            string row = "";
            string col = "";
            for (int j = 0; j < 3; j++)
            {
                row += board[i, j].getChecker();
                col += board[j, i].getChecker();
                if (i == j)
                {
                    diag += board[i, j].getChecker();
                }
                //0,2 1,1 2,0
                if (j == 2 - i)
                {
                    diag2 += board[i, j].getChecker();
                }
  
            }

            if (row == "XXX" || row == "OOO" || col == "XXX" || col == "OOO")
            {
                return playerID;
                // Här kan du kolla om rad eller col == 1 -> player 1 vinner, om rad eller col == 8 -> player 2 vinner   
            }


            if (diag == "XXX" || diag == "OOO"||diag2=="XXX"||diag2=="OOO")
            {
                return playerID;
            }
           
        }
        return -1;
    }

    

private void playAgainOrExit()
{
        //Overloada prepare istället
        NotificationCenter.newGamePrompt();
        string input = getInput();
        string userDecision = "";

        if (input != null)
        
            userDecision = input;
        


    if (userDecision.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
    {
            // Väljer om man vi spel igen.
            Console.WriteLine("Startar nytt spel");

        // Återställer alla variabler
        shouldGenerateRandomID = true;
        numberOfPlacements = 0;

        this.gameBoard = new GameBoard(new Color(ConsoleColor.Red));
        this.rows = new int[3, boardSize];
        this.columns = new int[3, boardSize];
        this.diagonals = new int[3, 2];
    }
    else
    {
        // Om man bestämmer sig för att avsluta.
        NotificationCenter.printSummaryResults(numberOfWinsByPlayer1, player1.name,
                                               numberOfWinsByPlayer2, player2.name);
            
        Environment.Exit(0);
    }
}
}
/*
 Skapar en cell som används i bord klass.
 */
public class BoardCell
{
    char checker;
    public BoardCell(Checker checker)
    {
        this.checker = checker.CheckSign;
    }
    public void setChecker(Checker checker)
    {
        this.checker = checker.CheckSign;
    }
    public char getChecker()
    {
        return checker;
    }
}
//public interface IChecker
//{
//    char CheckSign { get; }
//}
public class Checker

{
    public Checker() { }
    public Checker(char checkSign)
    {
        CheckSign = checkSign;
    }

    public char CheckSign { get; private set; }

}
public class GameBoard : Confirmed
{
    public int boardSize { get; private set; } = 3;
    BoardCell[,] board;
    IColor color;
    public GameBoard(IColor color)
    {
        this.color = color;
        this.board = new BoardCell[3, 3];
        ;
        this.SetBoard(this.boardSize);
    }
   

    public void SetBoard(int boardSize)
    {
        
        BoardCell[,] row = new BoardCell[boardSize, boardSize];
        for (int i = 0; i < row.GetLength(0); i++)
        {
            for (int j = 0; j < row.GetLength(1); j++)
            {
                this.board[i, j] = new BoardCell(new Checker(' '));

            }
        }
        

    }
    public BoardCell[,] getBoard()
    {
        return this.board;
    }
    public void PrintBoard()
    {
        color.setColor();
        Console.WriteLine($"+___+___+___+");
        Console.WriteLine($"| {board[0, 0].getChecker()} | {board[0, 1].getChecker()} | {board[0, 2].getChecker()} |");
        Console.WriteLine($"+___+___+___+");
        Console.WriteLine($"| {board[1, 0].getChecker()} | {board[1, 1].getChecker()} | {board[1, 2].getChecker()} |");
        Console.WriteLine($"+___+___+___+");
        Console.WriteLine($"| {board[2, 0].getChecker()} | {board[2, 1].getChecker()} | {board[2, 2].getChecker()} |");
        Console.WriteLine($"+___+___+___+");

    }
    public override int Confirm(int position)
    {
        if (position < 1 || position > (this.boardSize * this.boardSize)) { return -1; }

        int count = 1, i = 0, j = 0;
        bool flag = true;

        for (i = 0; i < 3; ++i)
        {
            for (j = 0; j < 3; ++j)
            {
                if (count == position)
                {
                    flag = false; break;
                }
                ++count;
            }

            if (!flag) { break; }
        }

        if (board[i, j].getChecker() != ' ') { return -1; }

        return 1;

    }
}
public class Player
{
    public string name { get; private set; }
    public int rowIndex { get; private set; } 
    public int colIndex { get; private set; } 
    public string input = " ";

    public Player(string name)
    {
        this.name = name;
        rowIndex = -1;
        colIndex = -1;

    }
    
    
    public void move(GameBoard board, Checker checker)
    {
        int movePos;
        while (true)
        {
            NotificationCenter.boardPlacement(1, this.name, board.boardSize);
            board.PrintBoard();

            if (int.TryParse(Console.ReadLine(), out int input))
            {

                movePos = input;

                if (board.Confirm(movePos)==-1)
                {
                    // Om input är en siffra fast inte inom range
                    NotificationCenter.boardPlacement(3, this.name, board.boardSize);
                    continue;
                }
                else
                {

                    // Om det är en ledig position
                    break;
                }
            }
            else
            {
                // Input är ingen siffra

                NotificationCenter.boardPlacement(2, this.name, board.boardSize);
            }
        }

        // Plaserar ditt drag
        placeTheMove(board, checker, movePos);
    }

    public void placeTheMove(GameBoard gameBoard, Checker checker, int movePosition)
    {
        int count = 1, i = 0, j = 0;
        bool flag = true;

        for (i = 0; i < gameBoard.getBoard().Length; ++i)
        {
            for (j = 0; j < 3; ++j)
            {
                if (count == movePosition)
                {
                    gameBoard.getBoard()[i, j].setChecker(checker);
                    flag = false;
                    break;
                }
                ++count;
            }

            if (!flag) { break; }
        }

        this.rowIndex = i;
        this.colIndex = j;
    }
}
public class NotificationCenter
{
    public NotificationCenter() { }
    public static void welcome(MenuOptions[] m)
    {
        Console.WriteLine("Välkommen till TicTacToe! Se nedan för instruktioner för spelet innan ni börjar:");

        Console.WriteLine();
        for (int i = 0; i < m.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {m[i].message()}");
            
        }
        Console.WriteLine();

        Console.WriteLine("Välj vilket menyalternativ du vill använda genom att skriva siffran på alternativet");
        

       
    }

    public static void namesAndSize(int index)
        {

            switch (index)
            {
                case 1:
                    Console.Write("Skriv in spelare1's namn: ");
                    break;
                case 2:
                    Console.Write("Skriv in spelare2's namn: ");
                    break;
                case 3:
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine("Felaktig input! Måste vara en siffra!");
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Index kan bara vara 1/2/3!");
                    break;
            }
        }
        public static void boardPlacement(int index, String playerName, int boardSize)
        {
            switch (index)
            {
                case 1:
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    Console.Write("Spelare " + playerName + ", ");
                    Console.WriteLine("Välj ditt drag. (skriv in en siffra från 1 - " + boardSize * boardSize + ")");
                    Console.WriteLine("Example: 1 (betyder: cell[1, 1]); 3 (betyder: cell[1, 3])");
                    Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    Console.WriteLine();
                    break;
                case 2:
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine("Felaktig input! Måste vara en siffra!");
                    Console.WriteLine("----------------------------------");
                    break;
                case 3:

                    Console.WriteLine();
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("Platsen är tagen eller existerar inte! Försök igen.");
                    Console.WriteLine("-------------------------------------------------------");
                    break;
                default:
                    Console.WriteLine("Index för boardPlacement() måste vara 1/2/3!");
                    break;
            }
        }

        public static void resultMessage(String winnerName)
        {

            Console.WriteLine();
            Console.WriteLine("***********************************************");
            Console.WriteLine("Grattis " + winnerName + "! Du har vunnit!");
            Console.WriteLine("***********************************************");
            Console.WriteLine();
        }

        public static void resultMessage()
        {
            Console.WriteLine("*********************************");
            Console.WriteLine("Ops! Det blev lika~");
            Console.WriteLine("*********************************");
        }

        public static void newGamePrompt()
        {
            Console.WriteLine("************************************************************");
            Console.WriteLine("Skulle ni vilja köra engång till?");
            Console.WriteLine("Tryck på \"y/Y\" för att spela igen! Eller vilken knapp som helst för att avsluta.");
            Console.WriteLine("************************************************************");
            Console.WriteLine();
        }

        public static void printSummaryResults(int wins1, String name1, int wins2, String name2)
        {
            String finalChampion;

            Console.WriteLine("√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√");
            Console.WriteLine("Bra spelat allihopa!");
            Console.WriteLine("Player " + name1 + " has won: " + wins1 + " time(s).");
            Console.WriteLine("Player " + name2 + " has won: " + wins2 + " time(s).");

            if (wins1 == wins2)
            {
                Console.WriteLine("Den slutgiltiga vinnaren är: Båda två, grattis!!!");
            }
            else
            {
                finalChampion = wins1 > wins2 ? name1 : name2;
                Console.WriteLine("Den slutgiltiga vinnaren är: " + finalChampion + "!!!");
            }

            Console.WriteLine("√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√√");
            Console.WriteLine();
        }
    }
    public abstract class Confirmed
    {
        public abstract int Confirm(int confirm);
    }
    public interface IColor
    {
        void setColor();
    }
    public class Color : IColor
    {

        ConsoleColor color;
        public Color(ConsoleColor color)
        {
            this.color = color;
        }
        public void setColor()
        {
            Console.ForegroundColor = color;
        }
    }
    public class AlternatingColor : IColor
    {
        IColor color1;
        IColor color2;
        int i;
        public AlternatingColor(IColor color1) : this(color1, color1) { }
        public AlternatingColor(IColor color1, IColor color2)
        {
            this.color1 = color1;
            this.color2 = color2;
        }

        public void setColor()
        {
            if (i % 2 == 0)
            {
                color1.setColor();
            }
            else color2.setColor();
            i++;

        }
    }
    public abstract class MenuOptions
    {
        
        public abstract void select();
        public abstract string message();
       public void menuMessage()
    {
        Console.WriteLine("**************************************************");
        Console.Write("Du har valt att ");
    }
     
    }
public class StartGame : MenuOptions
{
    public override void select()
    {
        
        Console.WriteLine("starta spelet!");
        Console.WriteLine("Lycka till!! :)");
        Console.WriteLine("**************************************************");
        Console.WriteLine();
    }

    public override string message()
    {
        
        return "Starta spelet";
    }
}
    public class ExitGame : MenuOptions
    {
        public override void select()
        {
        
        Console.WriteLine("avsluta spelet :(");
        Console.WriteLine("Kom gärna tillbaka för mer spel!");
        Console.WriteLine("**********************************************");
        Environment.Exit(0);
        }
    
        public override string message()
        {

            return "Avsluta spelet";
        }
    }

