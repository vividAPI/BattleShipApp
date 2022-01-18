using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models;

namespace ClassLibrary.Methods
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach(var letter in letters)
            {
                foreach(var number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            var output = false;
            foreach(var ship in player.ShipLocation)
            {
                if(ship.Status != GridSpotSatus.Sunk)
                {
                    output = true;
                }
            }
            return output;
        }

        static void AddGridSpot(PlayerInfoModel model, string? letter, int number)
        {
            SpotGridModel spot = new SpotGridModel
            {
                SpotNumber = number,
                SpotLetter = letter,
                Status = GridSpotSatus.Empty
            };

            model.ShotGrid.Add(spot);

        }

        public static int GetShotCounter(PlayerInfoModel player)
        {
            int shotCount = 0;
            foreach (var shot in player.ShotGrid)
            {
                if(shot.Status != GridSpotSatus.Empty)
                {
                    shotCount++;
                }
            }
            return shotCount;
        }

        public static bool PlaceShipLogic(PlayerInfoModel model, string? location)
        {
            
            bool output = false;
            (string row, int column) = SplitShotIntoRowandColumn(location);

            bool valLocation = ValidateGridLocation(model, row, column);
            bool isSpotOpen = ValidateShiptLocation(model, row, column);

            if (valLocation && isSpotOpen)
            {
                model.ShipLocation.Add(new SpotGridModel
                {
                    SpotLetter = row.ToUpper(),
                    SpotNumber = column,
                    Status = GridSpotSatus.Ship
                }); 
                output = true;
            }
            return output;
        }

        private static bool ValidateShiptLocation(PlayerInfoModel model, string row, int column)
        {
            //changed 6:04
            bool isValLocation = true;
            foreach (var ship in model.ShipLocation)
            {
                if(ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValLocation = false;
                }
            }
            return isValLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool valGridLocation = false;
            foreach(var ship in model.ShotGrid)
            {
                if(ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    valGridLocation = true;
                }
            }
            return valGridLocation;
        }

        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValShot = false;
            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    if(gridSpot.Status == GridSpotSatus.Empty)
                    {
                        isValShot = true;
                    }
                    
                }
            }
            return isValShot;
        }

        public static (string row, int column) SplitShotIntoRowandColumn(string shot)
        {
            string row = string.Empty;
            int column = 0;

            if(shot.Length != 2)
            {
                throw new ArgumentException("This was an invalid shot type", "shot");
            }
            char[] shotArray = shot.ToArray();
            
            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());
            
            return (row, column);
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            //refactor?
            bool isaHit = false;
            foreach (var ship in opponent.ShipLocation)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isaHit = true;
                    ship.Status = GridSpotSatus.Sunk;
                }
            }
            return isaHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
        {
            //refactor?
            bool isaHit = false;
            foreach (var gridShot in player.ShotGrid)
            {
                
                    if (gridShot.SpotLetter == row.ToUpper() && gridShot.SpotNumber == column)
                    {
                        if (isAHit)
                        {
                            gridShot.Status = GridSpotSatus.Hit;
                        }
                        else
                        {
                            gridShot.Status = GridSpotSatus.Miss;
                        } 
                    }
                
            }            
        }
    }
}
