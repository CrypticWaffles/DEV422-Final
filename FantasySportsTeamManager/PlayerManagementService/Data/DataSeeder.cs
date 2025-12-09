using PlayerManagementService.Model;
using PlayerManagementService.Data;
using System.Collections.Generic;
using System.Linq;

namespace PlayerManagementService.Data
{
    public static class DataSeeder
    {
        public static void SeedPlayers(FantasySportsContext context)
        {
            // Check if players already exist. If so, do nothing.
            if (context.Players.Any())
            {
                return;
            }

            var players = new List<Player>
            {
                new Player("Caleb Williams", "QB"),
                new Player("Marvin Harrison Jr.", "WR"),
                new Player("Drake Maye", "QB"),
                new Player("Joe Alt", "OT"),
                new Player("Malik Nabers", "WR"),
                new Player("Brock Bowers", "TE"),
                new Player("JC Latham", "OT"),
                new Player("Michael Penix Jr.", "QB"),
                new Player("Rome Odunze", "WR"),
                new Player("J.J. McCarthy", "QB"),
                new Player("Olu Fashanu", "OT"),
                new Player("Bo Nix", "QB"),
                new Player("Taliese Fuaga", "OT"),
                new Player("Travis Kelce", "TE"),
                new Player("Christian McCaffrey", "RB")
            };

            // Add the list to the database context
            context.Players.AddRange(players);

            // Save changes to insert rows into the SQL table
            context.SaveChanges();
        }
    }
}