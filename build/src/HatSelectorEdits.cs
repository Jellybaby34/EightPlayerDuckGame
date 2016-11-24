
namespace DuckGame.IncreasedPlayerLimit
{
    public class HatSelectorEdits : HatSelector
    {
        public static ulong _localID = (ulong) Rando.Long(long.MinValue, long.MaxValue);
        public TeamSelect2 current = Level.current as TeamSelect2;

        public static ulong localID
        {
            get
            {
                if (Steam.user != null)
                    return Steam.user.id;
                return _localID;
            }
        }

        /*
        private int TeamIndexAdd(int index, int plus, bool alwaysThree = true)
        {
            if (alwaysThree && index < 8 && index >= 0)
            index = 7;
            int num = index + plus;
            if (num >= AllTeams().Count)
            return num - AllTeams().Count + 3;
            if (num < 3)
            return AllTeams().Count + (num - 3);
            return num;
        }
        */

        public int ControllerNumber()
        {
            if (Network.isActive)
                return Maths.Clamp(profileBoxNumber, 0, 7);
            if (inputProfile.name == "MPPlayer1")
            return 0;
            if (inputProfile.name == "MPPlayer2")
            return 1;
            if (inputProfile.name == "MPPlayer3")
            return 2;
            if (inputProfile.name == "MPPlayer4")
            return 3;
            if (inputProfile.name == "MPPlayer5")
            return 4;
            if (inputProfile.name == "MPPlayer6")
            return 5;
            if (inputProfile.name == "MPPlayer7")
            return 6;
            return inputProfile.name == "MPPlayer8" ? 7 : 0;
        }

        public Team FilterTeam(bool hardFilter = false)
        {
            Team t = null;
            Profile _profile = DuckNetwork.profiles[profileBoxNumber];
            TeamSelect2 current = Level.current as TeamSelect2;
            ProfileBox2 box = current.GetBox((byte)profileBoxNumber);
            HatSelector hatSelector = box._hatSelector;

            if (!Network.isActive)
                t = AllTeams()[(int)_desiredTeamSelection];
            int index = (int)_desiredTeamSelection;
            if (index >= AllTeams().Count)
                index = ControllerNumber();
            if (_profile != null && _profile.connection == DuckNetwork.localConnection && !hardFilter)
            {
                if (index >= Teams.core.teams.Count)
                {
                    Team allTeam = AllTeams()[index];
                    index = ControllerNumber();
                }
                t = AllTeams()[index];
            }
            Team allTeam1;
            if (_profile.connection == DuckNetwork.localConnection)
            {
                if (index >= Teams.core.teams.Count)
                {
                    Team allTeam2 = AllTeams()[index];
                    allTeam1 = AllTeams()[ControllerNumber()];
                    Team.MapFacade(localID, allTeam2);
                    Send.Message((NetMessage)new NMSpecialHat(allTeam2, localID));
                }
                else
                {
                    allTeam1 = AllTeams()[index];
                    Team.ClearFacade(_profile.steamID);
                    Send.Message((NetMessage)new NMSpecialHat((Team)null, localID));
                }
            }
            else
                allTeam1 = AllTeams()[index];
            t = allTeam1;

            if (hardFilter == true)
            {
                if (Network.isActive && box.duck != null)
                    Send.Message(new NMSetTeam(box.duck.profile.networkIndex, (byte)Teams.IndexOf(t)));
                if (t.hasHat)
                {
                    if (box.duck != null)
                    {
                        Hat equipment = box.duck.GetEquipment(typeof(Hat)) as Hat;
                        Hat hat = (Hat)new TeamHat(0.0f, 0.0f, t);
                        Level.Add((Thing)hat);
                        box.duck.Equip((Equipment)hat, false, false);
                        box.duck.Fondle((Thing)hat);
                        if (hatSelector.hat != null)
                            Level.Remove((Thing)hatSelector.hat);
                        hatSelector.hat = hat;
                        if (equipment != null)
                            Level.Remove((Thing)equipment);
                    }
                    else if (hatSelector.hat != null)
                        Level.Remove((Thing)hatSelector.hat);
                }
                else
                {
                    if (hatSelector.hat != null)
                        Level.Remove((Thing)hatSelector.hat);
                    hatSelector.hat = (Hat)null;
                    if (box.duck != null)
                    {
                        Hat equipment = box.duck.GetEquipment(typeof(Hat)) as Hat;
                        if (equipment != null)
                        {
                            box.duck.Unequip((Equipment)equipment, false);
                            Level.Remove((Thing)equipment);
                        }
                    }
                }
                if (_desiredTeamSelection <= 7)
                {
                    _desiredTeamSelection = 0;
                }
            }
            return t;
        }

    }
}
 