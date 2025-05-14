using System;
using System.Collections.Generic;

class Program
{
    static int Location_x = 0;
    static int Location_y = 0;
    static int Destination_x = 0;
    static int Destination_y = 0;
    static bool End = false;
    static bool Wrong_Input = false;
    static string Turn = White;
    static string[,] Board = new string[8, 8];
    const string White = "White";
    const string Black = "Black";
    const string Empty_Field = "            ";
    const string Distance = "      ";
    static List<string> Pieces = new List<string>()
    {
        "Pawn  ",
        "Rook  ",
        "Knight",
        "Bishop",
        "Queen ",
        "King  "
    };

    static void Main(string[] args)
    {
        Fill_Board(Board);
        Assemble_Pieces(Board);

        while (End == false)
        {
            Move(Board);
        }
        Console.WriteLine("The Game is over!");
        Console.ReadLine();
    }



    // Miscellaneous
    static void Transform_Input(string Input)
    {
        int[] Coordinates = new int[4];

        char[] characters = new char[4];

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = Input[i];
        }

        for (int i = 0; i < characters.Length; i++)
        {
            int Letter_Value = 0;
            int Number_Value = 0;

            if (i == 0 || i == 2)
            {
                Letter_Value = characters[i] - 'a';
                if (Letter_Value < 0 || Letter_Value >= 8)
                {
                    Wrong_Input = true;
                    Console.WriteLine("Wrong Input");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Wrong_Input = false;
                    Coordinates[i] = Letter_Value;
                }
            }
            else if (i == 1 || i == 3)
            {
                Number_Value = characters[i] - '8';
                Number_Value *= -1;
                if (Number_Value < 0 || Number_Value >= 8)
                {
                    Wrong_Input = true;
                    Console.WriteLine("Wrong input");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Wrong_Input = false;
                    Coordinates[i] = Number_Value;
                }
            }
        }

        Location_y = Coordinates[1];
        Location_x = Coordinates[0];

        Destination_y = Coordinates[3];
        Destination_x = Coordinates[2];

    }
    static bool Check_for_King(string[,] board)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j].Contains(Pieces[5]))
                {
                    return true;
                }
            }
        }
        return false;
    }



    // Movement Related
    static void Move(string[,] board)
    {
        Console.Clear();
        // makes it so that the top doesn't get duplicated.
        Console.WriteLine("\x1b[3J");

        Display_Board(board);
        Console.WriteLine("Type like this to move your Pieces: \"a1a2\".");
        Console.WriteLine("a1 is the current position of your Piece and a2 is the place you want your Piece to be.");
        Console.WriteLine("It's " + Turn + "'s turn now!");

        do
        {
            string input = Console.ReadLine();
            Transform_Input(input);
            if (Wrong_Input == true)
            {
                Console.WriteLine("Incorrect Notation!!!");
                Console.ReadLine();
                return;
            }

            if (Check_Move(board) == false)
            {
                Wrong_Input = true;
                Console.WriteLine("Illegal Move!!!");
                Console.ReadLine();
                return;
            }
        }
        while (Wrong_Input == true);

        if (Check_for_King(board) == false)
        {
            End = true;
            Console.WriteLine(Turn + " has won the game by slaying the King of the opponent!");
            Console.ReadLine();
            return;
        }




        if (Turn == White)
        {
            Turn = Black;
        }
        else
        {
            Turn = White;
        }
    }
    static bool Check_Move(string[,] board)
    {
        string Piece = board[Location_y, Location_x];
        string Destination = board[Destination_y, Destination_x];

        if (Turn == White)
        {
            if (Destination.Contains(White))
            {
                Console.WriteLine("You can't capture a white piece when White is the turn player.");
                Console.ReadLine();
                return false;
            }
            else if (Piece.Contains(Black))
            {
                Console.WriteLine("You can't move a black piece on White's turn!");
                Console.ReadLine();
                return false;
            }
            else
            {
                if (Destination_x == Location_x && Destination_y == Location_y)
                {
                    Console.WriteLine("You have to move your Piece!");
                    Console.ReadLine();
                    return false;
                }
            }
        }
        else
        {
            if (Destination.Contains(Black))
            {
                Console.WriteLine("You can't capture a black piece when Black is the turn player.");
                Console.ReadLine();
                return false;
            }
            else if (Piece.Contains(White))
            {
                Console.WriteLine("You can't move a White piece on Black's turn!");
                Console.ReadLine();
                return false;
            }
            else
            {
                if (Destination_x == Location_x && Destination_y == Location_y)
                {
                    Console.WriteLine("You have to move your Piece!");
                    Console.ReadLine();
                    return false;
                }
            }
        }

        if (Legal_Move(board, Piece) == false)
        {
            return false;
        }
        else
        {
            board[Location_y, Location_x] = Empty_Field;
            return true;
        }


    }
    static bool Legal_Move(string[,] board, string Piece)
    {
        if (Piece.Contains(Pieces[0]))
        {
            return Pawn_Movement(board);
        }
        else if (Piece.Contains(Pieces[1])) 
        {
            return Rook_Movement(board);
        }
        else if (Piece.Contains(Pieces[2])) 
        {
            return Knight_Movement(board);
        }
        else if (Piece.Contains(Pieces[3])) 
        {
            return Bishop_Movement(board);
        }
        else if (Piece.Contains(Pieces[4])) 
        {
            return Queen_Movement(board);
        }
        else if (Piece.Contains(Pieces[5])) 
        {
            return King_Movement(board);
        }
        else
        {
            return false;
        }
    }
    static bool King_Movement(string[,] board)
    {
        if (Destination_x - Location_x > 2 || Destination_x - Location_x < -2)
        {
            return false;
        }
        if (Destination_y - Location_y > 2 && Destination_y - Location_y < -2)
        {
            return false;
        }

        board[Destination_y, Destination_x] = board[Location_y, Location_x];
        return true;
    }
    static bool Queen_Movement(string[,] board)
    {
        if (Location_x == Destination_x || Location_y == Destination_y)
        {
            return Rook_Movement(board);
        }
        else
        {
            return Bishop_Movement(board);
        }
    }
    static bool Bishop_Movement(string[,] board)
    {
        int One = Location_x;
        int Two = Location_y;

        // Diagonal right down
        if (One < Destination_x && Two < Destination_y)
        {
            while (One != 8 || Two != 8)
            {
                One++;
                Two++;

                if (Two == Destination_y && One == Destination_x)
                {
                    board[Destination_y, Destination_x] = board[Location_y, Location_x];
                    return true;
                }
                else if (board[Two, One] != "            ")
                {
                    return false;
                }
            }
        }

        // Diagonal right up
        else if (One < Destination_x && Two > Destination_y)
        {
            while (One != 8 || Two > 0)
            {
                One++;
                Two--;

                if (Two == Destination_y && One == Destination_x)
                {
                    board[Destination_y, Destination_x] = board[Location_y, Location_x];
                    return true;
                }
                else if (board[Two, One] != "            ")
                {
                    return false;
                }
            }
        }

        // Diagonal left down
        else if (One > Destination_x && Two < Destination_y)
        {
            while (One > 0 || Two != 8)
            {
                One--;
                Two++;

                if (Two == Destination_y && One == Destination_x)
                {
                    board[Destination_y, Destination_x] = board[Location_y, Location_x];
                    return true;
                }
                else if (board[Two, One] != "            ")
                {
                    return false;
                }
            }
        }

        // Diagonal Left Up
        else if (One > Destination_x && Two > Destination_y)
        {
            while (One > 0 || Two > 0)
            {
                One--;
                Two--;

                if (Two == Destination_y && One == Destination_x)
                {
                    board[Destination_y, Destination_x] = board[Location_y, Location_x];
                    return true;
                }
                else if (board[Two, One] != "            ")
                {
                    return false;
                }
            }
        }

        // if nothing applies
        Console.WriteLine("No Diagonal");
        return false;
    }
    static bool Knight_Movement(string[,] board)
    {
        if ((Destination_x == Location_x + 2 || Destination_x == Location_x - 2) && (Destination_y == Location_y + 1 || Destination_y == Location_y - 1))
        {
            board[Destination_y, Destination_x] = board[Location_y, Location_x];
            return true;
        }
        else if ((Destination_y == Location_y + 2 || Destination_y == Location_y - 2) && (Destination_x == Location_x + 1 || Destination_x == Location_x - 1))
        {
            board[Destination_y, Destination_x] = board[Location_y, Location_x];
            return true;
        }
        else
        {
            return false;
        }
    }
    static bool Rook_Movement(string[,] board)
    {
        if (Destination_x == Location_x)
        {
            if (Check_for_Pieces_Rook(board) == false)
            {
                return false;
            }
            board[Destination_y, Destination_x] = board[Location_y, Location_x];
            return true;
        }
        else if (Destination_y == Location_y)
        {
            if (Check_for_Pieces_Rook(board) == false)
            {
                return false;
            }
            board[Destination_y, Destination_x] = board[Location_y, Location_x];
            return true;
        }
        else
        {
            return false;
        }
    }
    static bool Pawn_Movement(string[,] board)
    {
        if (Turn == White)
        {
            if (Location_y == 1 && Destination_y == 3 && Location_x == Destination_x)
            {
                if (board[2, Location_x] != Empty_Field)
                {
                    return false;
                }
                board[Destination_y, Destination_x] = board[Location_y, Location_x];
                return true;
            }
            else if (Location_x == Destination_x && Location_y == Destination_y - 1)
            {
                board[Destination_y, Destination_x] = board[Location_y, Location_x];
                // Transform_Pawn(board);
                return true;
            }
            else if (Location_y == Destination_y - 1 && (Destination_x == Location_x - 1 || Destination_x == Location_x + 1))
            {
                board[Destination_y, Destination_x] = board[Location_y, Location_x];
                // Transform_Pawn(board);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (Location_y == 6 && Destination_y == 4 && Location_x == Destination_x)
            {
                if (board[5, Location_x + 1] == "      ")
                {
                    return false;
                }
                board[Destination_y, Destination_x] = board[Location_y, Location_x];
                return true;
            }
            else if (Location_x == Destination_x && Location_y == Destination_y + 1)
            {
                board[Destination_y, Destination_x] = board[Location_y, Location_x];
                return true;
            }
            else if (Location_y == Destination_y + 1 && (Destination_x == Location_x - 1 || Destination_x == Location_x + 1))
            {
                board[Destination_y, Destination_x] = board[Location_y, Location_x];
                return true;
            }
            else
            {
                return false;
            }
        }
    }



    /* Transforming the Pawn is in Development
    static void Transform_Pawn(string[,] board)
    {
        bool Correct_Answer = false;
        if (Destination_x == 7)
        {
            do
            {
                Console.WriteLine("What should your Pawn turn into?");
                Console.WriteLine
                    (
                    "[1]\tQueen\n" +
                    "[2]\tBishop\n" +
                    "[3]\tKnight\n" +
                    "[4]\tRook"
                    );
                int Input = Convert.ToInt32(Console.ReadLine());

                switch (Input)
                {
                    case 1:
                        board[Destination_y, Destination_x] = Figures[4];
                        Correct_Answer = true;
                        break;
                    case 2:
                        board[Destination_y, Destination_x] = Figures[3];
                        Correct_Answer = true;
                        break;
                    case 3:
                        board[Destination_y, Destination_x] = Figures[2];
                        Correct_Answer = true;
                        break;
                    case 4:
                        board[Destination_y, Destination_x] = Figures[1];
                        Correct_Answer = true;
                        break;
                }

            } while (Correct_Answer == false);
        }
    }*/



    // Check if the Rook would Skip other Pieces
    static bool Check_for_Pieces_Rook(string[,] board)
    {
        if (Location_x == Destination_x)
        {
            return Check_Rook_Horizontal(board);
        }
        else if (Location_y == Destination_y)
        {
            return Check_Rook_Vertical(board);
        }
        else
        {
            return false;
        }
    }
    static bool Check_Rook_Vertical(string[,] board)
    {
        int next_to_up = 0;
        int next_to_down = 0;


        for (int i = Location_y; i < 8; i++)
        {
            if (board[i, Location_x] != "            ")
            {
                next_to_down = i;
                break;
            }
        }
        for (int i = Location_y; i >= 0; i--)
        {
            if (board[i, Location_x] != "            ")
            {
                next_to_up = i;
                break;
            }
        }

        if (Destination_y > next_to_down || Destination_y < next_to_up)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    static bool Check_Rook_Horizontal(string[,] board)
    {
        // This Method checks if the Rook

        int next_to_left = 0;
        int next_to_right = 0;

        for (int i = Location_x; i < 8; i++)
        {
            if (board[Location_y, i] != "            ")
            {
                next_to_right = i;
                break;
            }
        }
        for (int i = Location_x; i >= 0; i--)
        {
            if (board[Location_y, i] != "            ")
            {
                next_to_left = i;
                break;
            }
        }

        if (Destination_x > next_to_right || Destination_x < next_to_left)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    // Methods for helping/doing the Board displaying
    static void Display_Board(string[,] board)
    {
        Display_Letters();
        Seperating_Lines();
        for (int i = 0; i < 8; i++)
        {
            Empty_Lines(3);
            Console.Write((i - 8) * -1);
            for (int j = 0; j < 8; j++)
            {
                Console.Write("|" + Distance + board[i, j] + Distance);
            }
            Console.Write("|");
            Console.WriteLine();
            Empty_Lines(3);
            Seperating_Lines();
        }
    }
    static void Fill_Board(string[,] board)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = Empty_Field;
            }
        }
    }
    static void Empty_Lines(int Amount)
    {
        for (int z = 0; z < Amount; z++) 
        { 
            Console.Write(" ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write("|");
                Console.Write(Distance + Empty_Field + Distance);
            }
            Console.Write("|");
            Console.WriteLine();
        }
    }
    static void Seperating_Lines()
    {
        Console.Write("  ");
        for(int i = 0; i < 199; i++)
        {
            Console.Write("-");
        }
        Console.WriteLine();
    }
    static void Display_Letters()
    {
        Console.Write(" ");
        for (int i = 1; i < 9; i++)
        { 
            Console.Write(Empty_Field + Letter(i) + Empty_Field);
        }
        Console.WriteLine();
    }
    static char Letter(int value)
    {
        Dictionary<int, char> Letters = new Dictionary<int, char>()
        {
            { 1, 'A' },
            { 2, 'B' },
            { 3, 'C' },
            { 4, 'D' },
            { 5, 'E' },
            { 6, 'F' },
            { 7, 'G' },
            { 8, 'H' }
        };

        return Letters[value];
    }



    // To put the pieces on the chess board
    static void Assemble_Pieces(string[,] board)
    {
        Place_Pawns(board);
        Place_Rooks(board);
        Place_Knights(board);
        Place_Bishops(board);
        Place_Queens(board);
        Place_Kings(board);
    }
    static void Place_Pawns(string[,] board)
    {
        for (int i = 0; i < 8; i++)
        {
            board[1, i] = White + "_" + Pieces[0];
            board[6, i] = Black + "_" + Pieces[0];
        }
    }
    static void Place_Rooks(string[,] board)
    {
        board[0, 0] = White + "_" + Pieces[1];
        board[7, 0] = Black + "_" + Pieces[1];

        board[0, 7] = White + "_" + Pieces[1];
        board[7, 7] = Black + "_" + Pieces[1];
    }
    static void Place_Knights(string[,] board)
    {
        board[0, 1] = White + "_" + Pieces[2];
        board[7, 1] = Black + "_" + Pieces[2];

        board[0, 6] = White + "_" + Pieces[2];
        board[7, 6] = Black + "_" + Pieces[2];
    }
    static void Place_Bishops(string[,] board)
    {
        board[0, 2] = White + "_" + Pieces[3];
        board[7, 2] = Black + "_" + Pieces[3];

        board[0, 5] = White + "_" + Pieces[3];
        board[7, 5] = Black + "_" + Pieces[3];
    }
    static void Place_Queens(string[,] board)
    {
        board[0, 3] = White + "_" + Pieces[4];
        board[7, 3] = Black + "_" + Pieces[4];
    }
    static void Place_Kings(string[,] board)
    {
        board[0, 4] = White + "_" + Pieces[5];
        board[7, 4] = Black + "_" + Pieces[5];
    }
}