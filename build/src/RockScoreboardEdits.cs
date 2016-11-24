using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DuckGame.IncreasedPlayerLimit
{
    public class RockScoreboardEdits
    {
        private static Type inputtype = typeof(RockScoreboard);
        public static Assembly ass = Assembly.GetAssembly(inputtype);
        private static Type type = ass.GetType("DuckGame.Global");

        public static void setGlobalStat(string statbinding, float value)
        {
            dynamic globaldata = type.GetField("_data", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(null);

            Type globalDataType = ass.GetType("DuckGame.GlobalData");
            dynamic statBinding = globalDataType.GetField(statbinding, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(globaldata);

            Type statBindingType = ass.GetType("DuckGame.StatBinding");
            PropertyInfo propertyInfo = statBindingType.GetProperty("value", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo propertyIsFloat = statBindingType.GetProperty("isFloat", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (propertyIsFloat.GetValue(statBinding, null) == false)
                value = (int)value;

            propertyInfo.SetValue(statBinding, value );
        }

        public static dynamic getGlobalStat(string statbinding)
        {
            dynamic globaldata = type.GetField("_data", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(null);

            Type globalDataType = ass.GetType("DuckGame.GlobalData");
            dynamic statBinding = globalDataType.GetField(statbinding, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(globaldata);

            Type statBindingType = ass.GetType("DuckGame.StatBinding");
            PropertyInfo propertyInfo = statBindingType.GetProperty("value", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            object myclass = propertyInfo.GetValue(statBinding, null);

            return myclass;
        }

        public static void Initialize()
        {

            RockScoreboard rockScore = (RockScoreboard)Level.current;

            /*
            FieldInfo matchesPlayed = globaldatatype.GetField("matchesPlayed", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo longestMatchPlayed = globaldatatype.GetField("longestMatchPlayed", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo onlineWins = globaldatatype.GetField("onlineWins", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo winsAsSwack = globaldatatype.GetField("winsAsSwack", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            */

            MethodInfo globalWinMatch = type.GetMethod("WinMatch", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            Type pedestal = ass.GetType("DuckGame.Pedestal");

            Type ginormoboard = ass.GetType("DuckGame.GinormoBoard");

            Type boardmode = ass.GetType("DuckGame.BoardMode");
            var boardmode1 = boardmode.GetField("Wins", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
            var boardmode2 = boardmode.GetField("Points", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(null);

            Type music = ass.GetType("DuckGame.Music");
            PropertyInfo musicvolume = music.GetProperty("volume", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            MethodInfo musicvolumeset = musicvolume.GetSetMethod();
            MethodInfo musicplay = music.GetMethod("Play", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(bool), typeof(float) }, null);

            Type options = ass.GetType("DuckGame.Options");
            var optionsdata = options.GetField("_data", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
            Type optionsdatavolume = ass.GetType("DuckGame.OptionsData");
            dynamic ayylmao = optionsdatavolume.GetProperty("musicVolume", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(optionsdata);


            FieldInfo didSkip = ass.GetType("DuckGame.HighlightLevel").GetField("didSkip", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            Type distancemarker = ass.GetType("DuckGame.DistanceMarker");

            FieldInfo _inputsField = inputtype.GetField("_inputs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _inputs = _inputsField.GetValue(Level.current as RockScoreboard);

            FieldInfo _afterHighlightsField = inputtype.GetField("_afterHighlights", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _afterHighlights = _afterHighlightsField.GetValue(Level.current as RockScoreboard);

            FieldInfo _skipFadeField = inputtype.GetField("_skipFade", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _skipFade = _skipFadeField.GetValue(Level.current as RockScoreboard);

            FieldInfo _weatherField = inputtype.GetField("_weather", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _weather = _weatherField.GetValue(Level.current as RockScoreboard);

            FieldInfo _sunshineField = inputtype.GetField("_sunshineTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _sunshineTarget = _sunshineField.GetValue(Level.current as RockScoreboard);

            FieldInfo _screenField = inputtype.GetField("_screenTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _screenTarget = _screenField.GetValue(Level.current as RockScoreboard);

            FieldInfo _pixelField = inputtype.GetField("_pixelTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _pixelTarget = _pixelField.GetValue(Level.current as RockScoreboard);

            FieldInfo _sunLayerField = inputtype.GetField("_sunLayer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _sunLayer = _sunLayerField.GetValue(Level.current as RockScoreboard);

            FieldInfo sunThingField = inputtype.GetField("sunThing", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic sunThing = sunThingField.GetValue(Level.current as RockScoreboard);

            FieldInfo rainbowThingField = inputtype.GetField("rainbowThing", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic rainbowThing = rainbowThingField.GetValue(Level.current as RockScoreboard);

            FieldInfo rainbowThing2Field = inputtype.GetField("rainbowThing2", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic rainbowThing2 = rainbowThing2Field.GetValue(Level.current as RockScoreboard);

            FieldInfo _crowdField = inputtype.GetField("_crowd", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _crowd = _crowdField.GetValue(Level.current as RockScoreboard);

            FieldInfo _fieldField = inputtype.GetField("_field", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _field = _fieldField.GetValue(Level.current as RockScoreboard);

            FieldInfo _bleacherSeatsField = inputtype.GetField("_bleacherSeats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _bleacherSeats = _bleacherSeatsField.GetValue(Level.current as RockScoreboard);

            FieldInfo _bleachersField = inputtype.GetField("_bleachers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _bleachers = _bleachersField.GetValue(Level.current as RockScoreboard);

            FieldInfo _intermissionTextField = inputtype.GetField("_intermissionText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _intermissionText = _intermissionTextField.GetValue(Level.current as RockScoreboard);

            FieldInfo _winnerPostField = inputtype.GetField("_winnerPost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _winnerPost = _winnerPostField.GetValue(Level.current as RockScoreboard);

            FieldInfo _winnerBannerField = inputtype.GetField("_winnerBanner", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _winnerBanner = _winnerBannerField.GetValue(Level.current as RockScoreboard);

            FieldInfo _fontField = inputtype.GetField("_font", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _font = _fontField.GetValue(Level.current as RockScoreboard);

            FieldInfo _modeField = inputtype.GetField("_mode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _mode = _modeField.GetValue(Level.current as RockScoreboard);

            FieldInfo _intermissionSlideField = inputtype.GetField("_intermissionSlide", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _intermissionSlide = _intermissionSlideField.GetValue(Level.current as RockScoreboard);

            FieldInfo _tieField = inputtype.GetField("_tie", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _tie = _tieField.GetValue(Level.current as RockScoreboard);

            FieldInfo _highestSlotField = inputtype.GetField("_highestSlot", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _highestSlot = _highestSlotField.GetValue(Level.current as RockScoreboard);

            FieldInfo _fieldWidthField = inputtype.GetField("_fieldWidth", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _fieldWidth = _fieldWidthField.GetValue(Level.current as RockScoreboard);

            FieldInfo _matchOverField = inputtype.GetField("_matchOver", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _matchOver = _matchOverField.GetValue(Level.current as RockScoreboard);

            FieldInfo _winningTeamField = inputtype.GetField("_winningTeam", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _winningTeam = _winningTeamField.GetValue(Level.current as RockScoreboard);

            FieldInfo _stateField = inputtype.GetField("_state", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _state = _stateField.GetValue(Level.current as RockScoreboard);

            FieldInfo _scoreBoardField = inputtype.GetField("_scoreBoard", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _scoreBoard = _scoreBoardField.GetValue(Level.current as RockScoreboard);

            FieldInfo _wallField = inputtype.GetField("_wall", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _wall = _wallField.GetValue(Level.current as RockScoreboard);

            FieldInfo _fieldForegroundField = inputtype.GetField("_fieldForeground", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _fieldForeground = _fieldForegroundField.GetValue(Level.current as RockScoreboard);

            FieldInfo _fieldForeground2Field = inputtype.GetField("_fieldForeground2", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _fieldForeground2 = _fieldForeground2Field.GetValue(Level.current as RockScoreboard);

            FieldInfo _bottomRightField = typeof(Level).GetField("_bottomRight", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _bottomRight = _bottomRightField.GetValue(Level.current as RockScoreboard);


            if (Network.isActive && Network.isServer)
            {
                int num = 0;
                foreach (Profile profile in DuckNetwork.profiles)
                {
                    if (profile.connection != null)
                    {
                        InputObject inputObject = new InputObject();
                        inputObject.profileNumber = (sbyte)num;
                        Level.Add((Thing)inputObject);
                        _inputs.Add(inputObject);
                        ++num;
                    }
                }
            }
            didSkip.SetValue(Level.current as RockScoreboard, false);
            if (_afterHighlights)
                _skipFade = true;
            _weather = new RockWeather(Level.current as RockScoreboard);
            _weather.Start();
            Level.Add((Thing)_weather);
            for (int index = 0; index < 350; ++index)
                _weather.Update();
            if (RockScoreboard._sunEnabled)
            {
                float num = 9f / 16f;
                _sunshineTarget = new RenderTarget2D(DuckGame.Graphics.width / 12, (int)((double)DuckGame.Graphics.width * (double)num) / 12, false);
                _screenTarget = new RenderTarget2D(DuckGame.Graphics.width, (int)((double)DuckGame.Graphics.width * (double)num), false);
                _pixelTarget = new RenderTarget2D(160, (int)(320.0 * (double)num / 2.0), false);
                _sunLayer = new Layer("SUN LAYER", 99999, (Camera)null, false, new Vec2());
                Layer.Add(_sunLayer);
                Thing thing = (Thing)new SpriteThing(150f, 120f, new Sprite("sun", 0.0f, 0.0f));
                thing.z = -9999f;
                thing.depth = -0.99f;
                thing.layer = _sunLayer;
                thing.xscale = 1f;
                thing.yscale = 1f;
                thing.collisionSize = new Vec2(1f, 1f);
                thing.collisionOffset = new Vec2(0.0f, 0.0f);
                Level.Add(thing);
                sunThing = thing;
                SpriteThing spriteThing1 = new SpriteThing(150f, 80f, new Sprite("rainbow", 0.0f, 0.0f));
                spriteThing1.alpha = 0.15f;
                spriteThing1.z = -9999f;
                spriteThing1.depth = -0.99f;
                spriteThing1.layer = _sunLayer;
                spriteThing1.xscale = 1f;
                spriteThing1.yscale = 1f;
                spriteThing1.color = new Color(100, 100, 100);
                spriteThing1.collisionSize = new Vec2(1f, 1f);
                spriteThing1.collisionOffset = new Vec2(0.0f, 0.0f);
                Level.Add((Thing)spriteThing1);
                rainbowThing = (Thing)spriteThing1;
                rainbowThing.visible = false;
                SpriteThing spriteThing2 = new SpriteThing(150f, 80f, new Sprite("rainbow", 0.0f, 0.0f));
                spriteThing2.z = -9999f;
                spriteThing2.depth = -0.99f;
                spriteThing2.layer = _sunLayer;
                spriteThing2.xscale = 1f;
                spriteThing2.yscale = 1f;
                spriteThing2.color = new Color((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue, 90);
                spriteThing2.collisionSize = new Vec2(1f, 1f);
                spriteThing2.collisionOffset = new Vec2(0.0f, 0.0f);
                Level.Add((Thing)spriteThing2);
                rainbowThing2 = (Thing)spriteThing2;
                rainbowThing2.visible = false;
            }
            List<Team> allRandomized = Teams.allRandomized;
            if (RockScoreboard.returnLevel == null && !Network.isActive)
            {
                allRandomized[0].Join(Profiles.DefaultPlayer1, true);
                allRandomized[1].Join(Profiles.DefaultPlayer2, true);
                allRandomized[0].score = 10;
                allRandomized[1].score = 2;
                Teams.Player3.score = 3;
                Teams.Player4.score = 4;
            }
            _crowd = new Crowd();
            Level.Add((Thing)_crowd);
            Crowd.mood = Mood.Calm;
            _field = new FieldBackground("FIELD", 9999);
            Layer.Add((Layer)_field);
            _bleacherSeats = new Sprite("bleacherSeats", 0.0f, 0.0f);
            _bleachers = RockWeather.weather != Weather.Snowing ? new Sprite("bleacherBack", 0.0f, 0.0f) : new Sprite("bleacherBackSnow", 0.0f, 0.0f);
            _bleachers.center = new Vec2((float)(_bleachers.w / 2), (float)(_bleachers.height - 3));
            _intermissionText = new Sprite("rockThrow/intermission", 0.0f, 0.0f);
            _winnerPost = new Sprite("rockThrow/winnerPost", 0.0f, 0.0f);
            _winnerBanner = new Sprite("rockThrow/winnerBanner", 0.0f, 0.0f);
            _font = new BitmapFont("biosFont", 8, -1);
            List<Team> teamList1 = new List<Team>();
            foreach (Team team in Teams.all)
            {
                if (team.activeProfiles.Count > 0)
                    teamList1.Add(team);
            }
            foreach (Team team in teamList1)
            {
                team.rockScore = team.score;
                if (RockScoreboard.wallMode)
                    team.score = Math.Min(team.score, GameMode.winsPerSet);
            }
            if (_mode == ScoreBoardMode.ShowScores)
            {
                _intermissionSlide = 1f;
                DuckGame.Graphics.fade = 1f;
                Layer.Game.fade = 0.0f;
                Layer.Background.fade = 0.0f;
                Crowd.UpdateFans();
                int num1 = 0;
                Stack<Depth> depthStack = new Stack<Depth>();
                for (int index = 0; index < 8; ++index)
                    depthStack.Push(new Depth((float)index * 0.02f));
                int num2 = 0;
                foreach (Team team in teamList1)
                {
                    Depth depth = depthStack.Pop();
                    float num3 = 223f;
                    float ypos = 0.0f;
                    float num4 = 26f;
                    if (num1 == 1)
                        num4 = 24f;
                    else if (num1 == 2)
                        num4 = 27f;
                    else if (num1 == 3)
                        num4 = 32f;
                    float num5 = (float)(158.0 - num1 * num4);
                    int prevScoreboardScore = team.prevScoreboardScore;
                    int num6 = GameMode.winsPerSet * 2;
                    int num7 = team.score;
                    if (RockScoreboard.wallMode && num7 > GameMode.winsPerSet)
                        num7 = GameMode.winsPerSet;
                    rockScore._slots.Add(new Slot3D());
                    if (num7 >= GameMode.winsPerSet && num7 == num2)
                        _tie = true;
                    else if (num7 >= GameMode.winsPerSet && num7 > num2)
                    {
                        _tie = false;
                        num2 = num7;
                        _highestSlot = rockScore._slots[rockScore._slots.Count - 1];
                    }
                    List<Profile> profileList = new List<Profile>();
                    Profile profile1 = (Profile)null;
                    bool flag = false;
                    foreach (Profile activeProfile in team.activeProfiles)
                    {
                        if (flag)
                        {
                            profile1 = activeProfile;
                            flag = false;
                        }
                        if (activeProfile.wasRockThrower)
                        {
                            activeProfile.wasRockThrower = false;
                            flag = true;
                        }
                        profileList.Add(activeProfile);
                    }
                    if (profile1 == null)
                        profile1 = team.activeProfiles[0];
                    profileList.Remove(profile1);
                    profileList.Insert(0, profile1);
                    profile1.wasRockThrower = true;
                    byte num8 = (byte)(rockScore._slots.Count - 1);
                    int num9 = 0;
                    foreach (Profile profile2 in profileList)
                    {
                        if (profile2 == profile1)
                        {
                            rockScore._slots[(int)num8].duck = (Duck)new RockThrowDuck(num3 - (float)(num9 * 10), ypos - 16f, profile2);
                            rockScore._slots[(int)num8].duck.planeOfExistence = num8;
                            rockScore._slots[(int)num8].duck.ignoreGhosting = true;
                            rockScore._slots[(int)num8].duck.forceMindControl = true;
                            Level.Add((Thing)rockScore._slots[(int)num8].duck);
                            rockScore._slots[(int)num8].duck.connection = DuckNetwork.localConnection;
                            TeamHat equipment = rockScore._slots[rockScore._slots.Count - 1].duck.GetEquipment(typeof(TeamHat)) as TeamHat;
                            if (equipment != null)
                                equipment.ignoreGhosting = true;
                            rockScore._slots[rockScore._slots.Count - 1].duck.z = num5;
                            rockScore._slots[rockScore._slots.Count - 1].duck.depth = depth;
                            rockScore._slots[rockScore._slots.Count - 1].ai = new DuckAI(profile2.inputProfile);
                            if (Network.isActive && profile2.connection != DuckNetwork.localConnection)
                                rockScore._slots[rockScore._slots.Count - 1].ai._manualQuack = rockScore.GetNetInput((sbyte)profile2.networkIndex);
                            rockScore._slots[rockScore._slots.Count - 1].duck.derpMindControl = false;
                            rockScore._slots[rockScore._slots.Count - 1].duck.mindControl = (InputProfile)rockScore._slots[rockScore._slots.Count - 1].ai;
                            rockScore._slots[rockScore._slots.Count - 1].rock = new ScoreRock((float)((double)num3 + 18.0 + (double)prevScoreboardScore / (double)num6 * (double)_fieldWidth), ypos, profile2);
                            rockScore._slots[rockScore._slots.Count - 1].rock.planeOfExistence = num8;
                            rockScore._slots[rockScore._slots.Count - 1].rock.ignoreGhosting = true;
                            Level.Add((Thing)rockScore._slots[rockScore._slots.Count - 1].rock);
                            rockScore._slots[rockScore._slots.Count - 1].rock.z = num5;
                            rockScore._slots[rockScore._slots.Count - 1].rock.depth = rockScore._slots[rockScore._slots.Count - 1].duck.depth + 1;
                            rockScore._slots[rockScore._slots.Count - 1].rock.grounded = true;
                            rockScore._slots[rockScore._slots.Count - 1].duck.isRockThrowDuck = true;
                        }
                        else
                        {
                            Duck duck = (Duck)new RockThrowDuck(num3 - (float)(num9 * 12), ypos - 16f, profile2);
                            duck.forceMindControl = true;
                            duck.planeOfExistence = num8;
                            duck.ignoreGhosting = true;
                            Level.Add((Thing)duck);
                            duck.depth = depth;
                            duck.z = num5;
                            duck.derpMindControl = false;
                            DuckAI duckAi = new DuckAI(profile2.inputProfile);
                            if (Network.isActive && profile2.connection != DuckNetwork.localConnection)
                                duckAi._manualQuack = rockScore.GetNetInput((sbyte)profile2.networkIndex);
                            duck.mindControl = (InputProfile)duckAi;
                            duck.isRockThrowDuck = true;
                            duck.connection = DuckNetwork.localConnection;
                            rockScore._slots[rockScore._slots.Count - 1].subDucks.Add(duck);
                            rockScore._slots[rockScore._slots.Count - 1].subAIs.Add(duckAi);
                        }
                        ++num9;
                    }
                    rockScore._slots[rockScore._slots.Count - 1].slotIndex = num1;
                    rockScore._slots[rockScore._slots.Count - 1].startX = num3;
                    ++num1;
                }
                for (int index = 0; index < 4; ++index)
                {
                    Block block = new Block(-50f, 0.0f, 1200f, 32f, PhysicsMaterial.Default);
                    block.planeOfExistence = (byte)index;
                    Level.Add((Thing)block);
                }
                if (!_tie && num2 > 0)
                    _matchOver = true;
                if (_tie)
                    GameMode.showdown = true;
            }
            else
            {
                if (Teams.active.Count > 1 && !_afterHighlights)
                {
                    setGlobalStat("matchesPlayed", getGlobalStat("matchesPlayed") + 1);
                    //matchesPlayed.SetValue(globaldata, matchesPlayed.GetValue(globaldata) + 1);
                    globalWinMatch.Invoke(null, new Team[] { Teams.winning[0] });
                    if (Network.isActive)
                    {
                        foreach (Profile activeProfile in Teams.winning[0].activeProfiles)
                        {
                            if (activeProfile.connection == DuckNetwork.localConnection)
                            {
                                DuckNetwork.GiveXP("Won Match", 0, 10, 4, 9999999, 9999999, 9999999);
                                break;
                            }
                        }
                    }
                    if (GameMode.winsPerSet > (int)getGlobalStat("longestMatchPlayed"))
                        setGlobalStat("longestMatchPlayed", getGlobalStat("longestMatchPlayed"));
                        //longestMatchPlayed.SetValue(globaldata, GameMode.winsPerSet);
                }
                _intermissionSlide = 0.0f;
                teamList1.Sort((Comparison<Team>)((a, b) =>
                {
                    if (a.score == b.score)
                        return 0;
                    return a.score >= b.score ? -1 : 1;
                }));
                float num1 = (float)(160.0 - (double)(teamList1.Count * 42 / 2) + 21.0);
                foreach (Team team in Teams.all)
                    team.prevScoreboardScore = 0;
                List<List<Team>> source = new List<List<Team>>();
                foreach (Team team in teamList1)
                {
                    int score = team.score;
                    bool flag = false;
                    for (int index = 0; index < source.Count; ++index)
                    {
                        if (source[index][0].score < score)
                        {
                            source.Insert(index, new List<Team>());
                            source[index].Add(team);
                            flag = true;
                            break;
                        }
                        if (source[index][0].score == score)
                        {
                            source[index].Add(team);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        source.Add(new List<Team>());
                        source.Last<List<Team>>().Add(team);
                    }
                }
                _winningTeam = teamList1[0];
                rockScore.controlMessage = 1;
                _state = ScoreBoardState.None;
                Crowd.mood = Mood.Dead;
                bool flag1 = false;
                if (!_afterHighlights)
                {
                    int place = 0;
                    int num2 = 0;
                    foreach (List<Team> teamList2 in source)
                    {
                        foreach (Team team in teamList2)
                        {
                            Level.Add((Thing)Activator.CreateInstance(pedestal, new object[] { num1 + (num2 * 42), 150f, team, place }));
                            ++num2;
                        }
                        ++place;
                    }
                    if (_winningTeam.activeProfiles.Count > 1)
                        ++_winningTeam.wins;
                    else
                        ++_winningTeam.activeProfiles[0].wins;
                    foreach (Profile activeProfile in _winningTeam.activeProfiles)
                    {
                        ++activeProfile.stats.trophiesWon;
                        activeProfile.stats.trophiesSinceLastWin = activeProfile.stats.trophiesSinceLastWinCounter;
                        activeProfile.stats.trophiesSinceLastWinCounter = 0;
                        if (Network.isActive && activeProfile.connection == DuckNetwork.localConnection && !flag1)
                        {
                            flag1 = true;
                            setGlobalStat("onlineWins", getGlobalStat("onlineWins") + 1);
                            //onlineWins.SetValue(globaldata, onlineWins.GetValue(globaldata)+1);
                            if (activeProfile.team.name == "SWACK")
                                setGlobalStat("winsAsSwack", getGlobalStat("winsAsSwack") + 1);
                                //winsAsSwack.SetValue(globaldata, winsAsSwack.GetValue(globaldata) + 1);
                        }
                        if (!Network.isActive && activeProfile.team.name == "SWACK")
                            setGlobalStat("winsAsSwack", getGlobalStat("winsAsSwack") + 1);
                            //winsAsSwack.SetValue(globaldata, winsAsSwack.GetValue(globaldata) + 1);
                    }
                    MonoMain.LogPlay();
                    foreach (Team team in teamList1)
                    {
                        foreach (Profile activeProfile in team.activeProfiles)
                        {
                            ++activeProfile.stats.trophiesSinceLastWinCounter;
                            ++activeProfile.stats.gamesPlayed;
                        }
                    }
                    Main.lastLevel = "";
                }
            }
            _bottomRight = new Vec2(1000f, 1000f);
            rockScore.lowestPoint = 1000f;
            _scoreBoard = Activator.CreateInstance(ginormoboard, new object[] { 300f, -320f, _mode == ScoreBoardMode.ShowScores ? boardmode2 : boardmode1 });
            //            _scoreBoard = new GinormoBoard(300f, -320f, _mode == ScoreBoardMode.ShowScores ? boardmode.Points : BoardMode.Wins);
            _scoreBoard.z = -130f;
            Level.Add((Thing)_scoreBoard);
            rockScore.backgroundColor = new Color(0, 0, 0);
            musicvolumeset.Invoke(null, new object[] { ayylmao });
            if (_mode != ScoreBoardMode.ShowWinner && !_afterHighlights)
                musicplay.Invoke(null, new object[] { "SportsTime", true, 0.0f });

            Level.current.camera.y = 0.0f;
            _field.ypos = 0.0f;
            Sprite s1 = RockWeather.weather != Weather.Snowing ? (RockWeather.weather != Weather.Raining ? new Sprite("fieldNoise", 0.0f, 0.0f) : new Sprite("fieldNoiseRain", 0.0f, 0.0f)) : new Sprite("fieldNoiseSnow", 0.0f, 0.0f);
            s1.scale = new Vec2(4f, 4f);
            s1.depth = (Depth)0.5f;
            s1.y -= 16f;
            _field.AddSprite(s1);
            Sprite s2 = new Sprite("fieldWall", 0.0f, 0.0f);
            s2.scale = new Vec2(4f, 4f);
            s2.depth = (Depth)0.5f;
            s2.y -= 16f;
            _wall = new WallLayer("FIELDWALL", 80);
            if (RockScoreboard.wallMode)
                _wall.AddWallSprite(s2);
            Layer.Add((Layer)_wall);
            _fieldForeground = new FieldBackground("FIELDFOREGROUND", 80);
            _fieldForeground.fieldHeight = -13f;
            Layer.Add((Layer)_fieldForeground);
            _fieldForeground2 = new FieldBackground("FIELDFOREGROUND2", 70);
            _fieldForeground2.fieldHeight = -15f;
            Layer.Add((Layer)_fieldForeground2);
            if (_mode != ScoreBoardMode.ShowWinner)
            {
                Sprite s3 = new Sprite("rockThrow/chairSeat", 0.0f, 0.0f);
                s3.CenterOrigin();
                s3.x = 300f;
                s3.y = 20f;
                s3.scale = new Vec2(1.2f, 1.2f);
                _fieldForeground.AddSprite(s3);
                Sprite s4 = new Sprite("rockThrow/tableTop", 0.0f, 0.0f);
                s4.CenterOrigin();
                s4.x = 450f;
                s4.y = 14f;
                s4.scale = new Vec2(1.2f, 1.4f);
                _fieldForeground2.AddSprite(s4);
                int num = -95;
                Sprite spr1 = new Sprite("rockThrow/chairBottomBack", 0.0f, 0.0f);
                Thing thing1 = (Thing)new SpriteThing(300f, -10f, spr1);
                thing1.center = new Vec2((float)(spr1.w / 2), (float)(spr1.h / 2));
                thing1.z = (float)(106 + num);
                thing1.depth = (Depth)0.5f;
                thing1.layer = Layer.Background;
                Level.Add(thing1);
                Sprite spr2 = new Sprite("rockThrow/chairBottom", 0.0f, 0.0f);
                Thing thing2 = (Thing)new SpriteThing(300f, -6f, spr2);
                thing2.center = new Vec2((float)(spr2.w / 2), (float)(spr2.h / 2));
                thing2.z = (float)(120 + num);
                thing2.depth = (Depth)0.8f;
                thing2.layer = Layer.Background;
                Level.Add(thing2);
                Sprite spr3 = new Sprite("rockThrow/chairFront", 0.0f, 0.0f);
                Thing thing3 = (Thing)new SpriteThing(300f, -9f, spr3);
                thing3.center = new Vec2((float)(spr3.w / 2), (float)(spr3.h / 2));
                thing3.z = (float)(122 + num);
                thing3.depth = (Depth)0.9f;
                thing3.layer = Layer.Background;
                Level.Add(thing3);
                Sprite spr4 = new Sprite("rockThrow/tableBottomBack", 0.0f, 0.0f);
                Thing thing4 = (Thing)new SpriteThing(450f, -7f, spr4);
                thing4.center = new Vec2((float)(spr4.w / 2), (float)(spr4.h / 2));
                thing4.z = (float)(106 + num);
                thing4.depth = (Depth)0.5f;
                thing4.layer = Layer.Background;
                Level.Add(thing4);
                Sprite spr5 = new Sprite("rockThrow/tableBottom", 0.0f, 0.0f);
                Thing thing5 = (Thing)new SpriteThing(450f, -7f, spr5);
                thing5.center = new Vec2((float)(spr5.w / 2), (float)(spr5.h / 2));
                thing5.z = (float)(120 + num);
                thing5.depth = (Depth)0.8f;
                thing5.layer = Layer.Background;
                Level.Add(thing5);
                Sprite spr6 = new Sprite("rockThrow/keg", 0.0f, 0.0f);
                Thing thing6 = (Thing)new SpriteThing(460f, -24f, spr6);
                thing6.center = new Vec2((float)(spr6.w / 2), (float)(spr6.h / 2));
                thing6.z = (float)(120 + num - 4);
                thing6.depth = -0.4f;
                thing6.layer = Layer.Game;
                Level.Add(thing6);
                Sprite spr7 = new Sprite("rockThrow/cup", 0.0f, 0.0f);
                Thing thing7 = (Thing)new SpriteThing(445f, -21f, spr7);
                thing7.center = new Vec2((float)(spr7.w / 2), (float)(spr7.h / 2));
                thing7.z = (float)(120 + num - 6);
                thing7.depth = -0.5f;
                thing7.layer = Layer.Game;
                Level.Add(thing7);
                Sprite spr8 = new Sprite("rockThrow/cup", 0.0f, 0.0f);
                Thing thing8 = (Thing)new SpriteThing(437f, -20f, spr8);
                thing8.center = new Vec2((float)(spr8.w / 2), (float)(spr8.h / 2));
                thing8.z = (float)(120 + num);
                thing8.depth = -0.3f;
                thing8.layer = Layer.Game;
                Level.Add(thing8);
                Sprite spr9 = new Sprite("rockThrow/cup", 0.0f, 0.0f);
                Thing thing9 = (Thing)new SpriteThing(472f, -20f, spr9);
                thing9.center = new Vec2((float)(spr9.w / 2), (float)(spr9.h / 2));
                thing9.z = (float)(120 + num - 7);
                thing9.depth = -0.5f;
                thing9.layer = Layer.Game;
                thing9.angleDegrees = 80f;
                Level.Add(thing9);
            }
            for (int index = 0; index < 3; ++index)
            {
                dynamic distanceMarker = Activator.CreateInstance(distancemarker, new object[] { (float)(230 + index * 175), -25f, (int)Math.Round(index * GameMode.winsPerSet / 2.0) });
                distanceMarker.z = 0.0f;
                distanceMarker.depth = (Depth)0.34f;
                distanceMarker.layer = Layer.Background;
                Level.Add((Thing)distanceMarker);
            }
            Sprite spr = RockWeather.weather != Weather.Snowing ? new Sprite("bleacherBack", 0.0f, 0.0f) : new Sprite("bleacherBackSnow", 0.0f, 0.0f);
            for (int index = 0; index < 24; ++index)
            {
                SpriteThing spriteThing = new SpriteThing((float)(100 + index * (spr.w + 13)), (float)(spr.h + 15), spr);
                spriteThing.center = new Vec2((float)(spr.w / 2), (float)(spr.h - 1));
                spriteThing.collisionOffset = new Vec2(spriteThing.collisionOffset.x, (float)-spr.h);
                spriteThing.z = 0.0f;
                spriteThing.depth = (Depth)0.33f;
                spriteThing.layer = Layer.Background;
                Level.Add((Thing)spriteThing);
            }
            SpriteThing spriteThing3 = new SpriteThing(600f, 0.0f, new Sprite("blackSquare", 0.0f, 0.0f));
            spriteThing3.z = -90f;
            spriteThing3.centery = 7f;
            spriteThing3.depth = (Depth)0.1f;
            spriteThing3.layer = Layer.Background;
            spriteThing3.xscale = 100f;
            spriteThing3.yscale = 7f;
            Level.Add((Thing)spriteThing3);
            _weather.Update();

            _inputsField.SetValue(Level.current as RockScoreboard, _inputs);
            _afterHighlightsField.SetValue(Level.current as RockScoreboard, _afterHighlights);
            _skipFadeField.SetValue(Level.current as RockScoreboard, _skipFade);
            _weatherField.SetValue(Level.current as RockScoreboard, _weather);
            _sunshineField.SetValue(Level.current as RockScoreboard, _sunshineTarget);
            _screenField.SetValue(Level.current as RockScoreboard, _screenTarget);
            _pixelField.SetValue(Level.current as RockScoreboard, _pixelTarget);
            _sunLayerField.SetValue(Level.current as RockScoreboard, _sunLayer);
            sunThingField.SetValue(Level.current as RockScoreboard, sunThing);
            rainbowThingField.SetValue(Level.current as RockScoreboard, rainbowThing);
            rainbowThing2Field.SetValue(Level.current as RockScoreboard, rainbowThing2);
            _crowdField.SetValue(Level.current as RockScoreboard, _crowd);
            _fieldField.SetValue(Level.current as RockScoreboard, _field);
            _bleacherSeatsField.SetValue(Level.current as RockScoreboard, _bleacherSeats);
            _bleachersField.SetValue(Level.current as RockScoreboard, _bleachers);
            _intermissionTextField.SetValue(Level.current as RockScoreboard, _intermissionText);
            _winnerPostField.SetValue(Level.current as RockScoreboard, _winnerPost);
            _winnerBannerField.SetValue(Level.current as RockScoreboard, _winnerBanner);
            _fontField.SetValue(Level.current as RockScoreboard, _font);
            _modeField.SetValue(Level.current as RockScoreboard, _mode);
            _intermissionSlideField.SetValue(Level.current as RockScoreboard, _intermissionSlide);
            _tieField.SetValue(Level.current as RockScoreboard, _tie);
            _highestSlotField.SetValue(Level.current as RockScoreboard, _highestSlot);
            _fieldWidthField.SetValue(Level.current as RockScoreboard, _fieldWidth);
            _matchOverField.SetValue(Level.current as RockScoreboard, _matchOver);
            _winningTeamField.SetValue(Level.current as RockScoreboard, _winningTeam);
            _stateField.SetValue(Level.current as RockScoreboard, _state);
            _scoreBoardField.SetValue(Level.current as RockScoreboard, _scoreBoard);
            _wallField.SetValue(Level.current as RockScoreboard, _wall);
            _fieldForegroundField.SetValue(Level.current as RockScoreboard, _fieldForeground);
            _fieldForeground2Field.SetValue(Level.current as RockScoreboard, _fieldForeground2);
            _bottomRightField.SetValue(Level.current as RockScoreboard, _bottomRight);

        }
    }
}