using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckGame.EightPlayerDuckGame
{
    public class TeamsCoreEdits
    {
        public static void AddExtraTeams() // Inserts our new teams into the team list
        {
            List<Team> newTeamList = Teams.core.teams;

            List<Team> extraTeams = new List<Team>
            {
                new Team("Player 5", "hats/noHat", true, false, new Vec2(), "", (Texture2D) null),
                new Team("Player 6", "hats/noHat", true, false, new Vec2(), "", (Texture2D) null),
                new Team("Player 7", "hats/noHat", true, false, new Vec2(), "", (Texture2D) null),
                new Team("Player 8", "hats/noHat", true, false, new Vec2(), "", (Texture2D) null)
            };

            newTeamList.InsertRange(4, extraTeams);

            Teams.core.teams = newTeamList;
        }
    }
}
