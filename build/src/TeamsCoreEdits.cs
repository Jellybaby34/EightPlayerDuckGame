using System.Collections.Generic;

namespace DuckGame.IncreasedPlayerLimit
{
    public class TeamsCoreEdits
    {
        public static void Initialize()
        {
            Teams.core.teams = new List<Team>()
            {
            new Team("Player 1", "hats/noHat", true, false, new Vec2()),
            new Team("Player 2", "hats/noHat", true, false, new Vec2()),
            new Team("Player 3", "hats/noHat", true, false, new Vec2()),
            new Team("Player 4", "hats/noHat", true, false, new Vec2()),
            new Team("Player 5", "hats/noHat", true, false, new Vec2()),
            new Team("Player 6", "hats/noHat", true, false, new Vec2()),
            new Team("Player 7", "hats/noHat", true, false, new Vec2()),
            new Team("Player 8", "hats/noHat", true, false, new Vec2()),
            new Team("Sombreros", "hats/sombrero", true, false, new Vec2()),
            new Team("Dappers", "hats/dapper", true, false, new Vec2()),
            new Team("Dicks", "hats/dicks", true, false, new Vec2()),
            new Team("Frank", "hats/frank", false, true, new Vec2()),
            new Team("DUCKS", "hats/reallife", false, true, new Vec2()),
            new Team("Frogs?", "hats/frogs", true, false, new Vec2()),
            new Team("Drunks", "hats/drunks", false, false, new Vec2()),
            new Team("Joey", "hats/joey", false, true, new Vec2()),
            new Team("BALLZ", "hats/ballhead", false, false, new Vec2()),
            new Team("Agents", "hats/agents", false, false, new Vec2()),
            new Team("Sailors", "hats/sailors", false, false, new Vec2()),
            new Team("astropal", "hats/astrobud", false, true, new Vec2()),
            new Team("Cowboys", "hats/cowboys", false, true, new Vec2()),
            new Team("Pulpy", "hats/pulpy", false, true, new Vec2()),
            new Team("SKULLY", "hats/skelly", false, true, new Vec2()),
            new Team("Hearts", "hats/hearts", false, false, new Vec2()),
            new Team("LOCKED", "hats/locked", false, false, new Vec2()),
            new Team("Jazzducks", "hats/jazzducks", false, false, new Vec2(-2f, -7f)),
            new Team("Divers", "hats/divers", false, false, new Vec2()),
            new Team("Uglies", "hats/uglies", false, false, new Vec2()),
            new Team("Dinos", "hats/dinos", false, false, new Vec2()),
            new Team("Caps", "hats/caps", false, false, new Vec2()),
            new Team("Burgers", "hats/burgers", false, false, new Vec2()),
            new Team("Turing", "hats/turing", true, false, new Vec2()),
            new Team("Retro", "hats/retros", false, false, new Vec2()),
            new Team("Senpai", "hats/sensei", false, false, new Vec2()),
            new Team("BAWB", "hats/bawb", false, true, new Vec2(-1f, -10f)),
            new Team("SWACK", "hats/guac", true, true, new Vec2()),
            new Team("eggpal", "hats/eggy", false, true, new Vec2()),
            new Team("Valet", "hats/valet", false, false, new Vec2()),
            new Team("Pilots", "hats/pilots", false, false, new Vec2()),
            new Team("Cyborgs", "hats/cyborgs", false, false, new Vec2()),
            new Team("Fridges", "hats/fridge", false, false, new Vec2()),
            new Team("Witchtime", "hats/witchtime", false, false, new Vec2()),
            new Team("Wizards", "hats/wizbiz", false, false, new Vec2()),
            new Team("FUNNYMAN", "hats/FunnyMan", false, false, new Vec2()),
            new Team("Pumpkins", "hats/Dumplin", false, false, new Vec2()),
            new Team("CAPTAIN", "hats/devhat", false, true, new Vec2()),
            new Team("BRICK", "hats/brick", false, true, new Vec2()),
            new Team("Pompadour", "hats/pompadour", false, false, new Vec2()),
            new Team("Super", "hats/super", false, false, new Vec2()),
            new Team("Chancy", "hats/chancy", false, true, new Vec2()),
            new Team("Log", "hats/log", false, false, new Vec2()),
            new Team("Meeee", "hats/toomany", false, true, new Vec2()),
            new Team("BRODUCK", "hats/broduck", false, true, new Vec2()),
            new Team("brad", "hats/handy", false, true, new Vec2()),
            new Team("eyebob", "hats/gross", false, false, new Vec2())
            };
        }
    }
}