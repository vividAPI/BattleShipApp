using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class PlayerInfoModel
    {
        public string? PlayerName { get; set; }
        public List<SpotGridModel>? ShipLocation { get; set; } = new List<SpotGridModel>();
        public List<SpotGridModel>? ShotGrid { get; set; } = new List<SpotGridModel>();

    }
}
