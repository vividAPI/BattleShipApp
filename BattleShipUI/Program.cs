using ClassLibrary.Models;
using ClassLibrary.Methods;

WelcomeMessage();

PlayerInfoModel activePlayer = CreatePlayer("Player 1");
PlayerInfoModel opponent = CreatePlayer("Player 2");
PlayerInfoModel winner = null;
do
{
    //Display grid from activePlayer one on where they fired (shotgrid)

    DisplayShotGrid(activePlayer);

    //Ask activePlayer one for a shot
    //Determine if the shot was valid, loop until valid
    //Determine shot results

    RecordPlayerShot(activePlayer, opponent);

    //Determine if the game continue

    bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

    //If over set activePlayer one as winner
    //else, swap positions (activePlayer to opponent)
    if (doesGameContinue)
    {
        // Swap using a temp variable
        //PlayerInfoModel temp = opponet;
        //opponet = activePlayer;
        //activePlayer = temp;

        //Swap with Tuple
        (activePlayer,opponent) = (opponent,activePlayer);


    }
    else
    {
        winner = activePlayer;
    }
    
} while (winner == null);

IdentifyWinnder(winner);
Console.ReadLine();

static void IdentifyWinnder(PlayerInfoModel winner)
{
    Console.WriteLine($"Congratulations to {winner.PlayerName} for winning!");
    Console.WriteLine($"{winner.PlayerName} took {GameLogic.GetShotCounter(winner)} shots.");
}

static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
{
    bool isValShot = false;
    string row = string.Empty;
    int column = 0;

    //Ask for a shot  (we ask for "B2")
    //Determine what row and column that is - split
    //Determine if that is a valid shot
    //Go back to the beginning if not a valid shot
    do
    {
        string shot = AskForShot(activePlayer);
        try
        {
            (row, column) = GameLogic.SplitShotIntoRowandColumn(shot);
            isValShot = GameLogic.ValidateShot(activePlayer, row, column);
        }
        catch (Exception ex)
        {

           //Console.WriteLine(ex.Message);
            isValShot = false;
        }

        if(isValShot == false)
        {
            Console.WriteLine("Invalid Shot Location. Please try again.");
        }

    } while (isValShot == false);
     

    //Determine results
    bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

    if (isAHit)
    {
        Console.WriteLine($"Good job {activePlayer.PlayerName}, {row.ToUpper()}{column} was a hit!");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine (); 
    }
    else if (!isAHit)
    {
        Console.WriteLine($"Sorry {activePlayer.PlayerName}, {row.ToUpper()}{column}, was a miss :-(.");
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }

    //Record results
    GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
    Console.Clear();

}

static string AskForShot(PlayerInfoModel activePlayer)
{
    Console.WriteLine();
    Console.Write($"{activePlayer.PlayerName} please enter your shot selection: ");
    return Console.ReadLine();
    
}

static void DisplayShotGrid(PlayerInfoModel activePlayer)
    //update to make grid one day
{
    
    string currentRow = activePlayer.ShotGrid[0].SpotLetter;

    foreach(var gridSpot in activePlayer.ShotGrid)
    {
        if(gridSpot.SpotLetter != currentRow)
        {
            Console.WriteLine();
            
            currentRow = gridSpot.SpotLetter;
        }
        if (gridSpot.Status == GridSpotSatus.Empty)
        {
           Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} "); 
        }
        else if (gridSpot.Status == GridSpotSatus.Hit)
        {
            Console.Write(" X ");

        }
        else if(gridSpot.Status == GridSpotSatus.Miss)
        {
            Console.Write(" O ");
        }
        else
        {
            Console.Write(" ? ");
        }
    }
    Console.WriteLine();
    Console.WriteLine();
}

static void WelcomeMessage()
{
    Console.WriteLine("Welcome to BattleShip");
    Console.WriteLine("Created By: Alex Calder");
    Console.WriteLine("---------------");
    Console.WriteLine();
}

static PlayerInfoModel CreatePlayer(string playerTitle)

{
    Console.WriteLine($"Player Info For {playerTitle}:");
    PlayerInfoModel output = new PlayerInfoModel();
    //Ask User For their name
    output.PlayerName = AskForPlayerName(playerTitle);

    //Load up shot grid
    GameLogic.InitializeGrid(output);
    
    //Ask user for their 5 ship placements

    PlaceShipsUI(output);

    //Clear Screen
    Console.Clear();

    return output;
}

static string AskForPlayerName(string playerTitle)
{
    Console.Write($"Hello {playerTitle}, what is your name: ");

    return Console.ReadLine();

}

static void PlaceShipsUI(PlayerInfoModel model)
{
    do
    {
        Console.Write($"{model.PlayerName} where do you want to place your ship number {model.ShipLocation.Count + 1 }: ");
        string? location = Console.ReadLine();

        bool isValLocation = false;

        try
        {
            isValLocation = GameLogic.PlaceShipLogic(model, location);
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error" + ex.Message);
        }
        
        if(isValLocation == false)
        {
            Console.WriteLine("That was not a valid location. Please try again.");
        }

    } while (model.ShipLocation.Count < 5);

} 

 