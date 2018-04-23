using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DuckGame.IncreasedPlayerLimit
{
    public class ProfileBox2Edits : Thing
    {
        private static Type inputtype = typeof(ProfileBox2);
        public static Assembly ass = Assembly.GetAssembly(inputtype);

        private SinWave _pulse = new SinWave(0.05f, 0.0f);
        private Vec2 _consolePos = Vec2.Zero;
        private Vec2 _gunSpawnPoint = Vec2.Zero;
        private BitmapFont _font;
        private BitmapFont _fontSmall;
        private bool _playerActive;
        private int _teamSelection;
        private Sprite _plaque;
        private bool _ready;
        private InputProfile _inputProfile;
        private Profile _playerProfile;
        private Sprite _doorLeft;
        private Sprite _doorRight;
        private Sprite _doorLeftBlank;
        private Sprite _doorRightBlank;
        private SpriteMap _doorSpinner;
        private SpriteMap _doorIcon;
        private Sprite _roomLeftBackground;
        private Sprite _roomLeftForeground;
        private SpriteMap _tutorialMessages;
        private Sprite _tutorialTV;
        private SpriteMap _selectConsole;
        private Sprite _consoleHighlight;
        private Sprite _aButton;
        private Sprite _readySign;
        private float _doorX;
        private int _currentMessage;
        private float _screenFade;
        private float _consoleFade;
        private dynamic _projector;
        private TeamSelect2 _teamSelect;
        private Profile _defaultProfile;
        private Sprite _hostCrown;
        private Sprite _consoleFlash;
        private SpriteMap _lightBar;
        private SpriteMap _roomSwitch;
        private int _controllerIndex;
        private Duck _duck;
        private VirtualShotgun _gun;
        public dynamic _window;
        public HatSelector _hatSelector;
        private DuckNetStatus _prevStatus;
        private float _prevDoorX;

        public int rightRoom()
        {
            int roomPosition = 0;

            if (_controllerIndex == 0 || _controllerIndex == 3 || _controllerIndex == 5)
                roomPosition = 0;
            else if (_controllerIndex == 1 || _controllerIndex == 6)
                roomPosition = 1;
            else if (_controllerIndex == 2 || _controllerIndex == 4 || _controllerIndex == 7)
                roomPosition = 2;

            return roomPosition;
        }

        public static void ProfileBox2Correction()
        {
            // I would like to thank the program Gray Storm and its relevant paper for the idea applied here
            // We inject a jump into the method assembly so that when its called, it calls our method :D
            Type[] constructorParams = new Type[] { typeof(float), typeof(float), typeof(InputProfile), typeof(Profile), typeof(TeamSelect2), typeof(int) };

            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2).GetConstructor(constructorParams).MethodHandle);
            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2Edits).GetConstructor(constructorParams).MethodHandle);

            IntPtr ptr = typeof(ProfileBox2).GetConstructor(constructorParams).MethodHandle.GetFunctionPointer(); ; // Where we want to change

            int target = (int)typeof(ProfileBox2Edits).GetConstructor(constructorParams).MethodHandle.GetFunctionPointer(); // Where we want the program to go

            long dstAddress = target;//new address to call
            long srcAddress = (long)ptr + 5; //memory location of caller + size of call sequence (5 bytes, 1 for the opcode, 4 for the offset)
            long newCallPtr = dstAddress - srcAddress;

            byte[] redirector = new byte[] {
                        0xE9,   // CALL INSTRUCTION + TARGET ADDRESS IN LITTLE ENDIAN
                        (byte)(newCallPtr & 0xff),
                        (byte)((newCallPtr >> 8) & 0xff),
                        (byte)((newCallPtr >> 16) & 0xff),
                        (byte)((newCallPtr >> 24) & 0xff),
                        0xC3 // Return OpCode so we don't run the rest of the function
                    };
            Marshal.Copy(redirector, 0, ptr, redirector.Length);

            // Part 2 - Detouring Draw()

            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2).GetMethod("Draw", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle);
            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2Edits).GetMethod("Draw", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle);

            ptr = typeof(ProfileBox2).GetMethod("Draw", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle.GetFunctionPointer(); ; // Where we want to change
            target = (int)typeof(ProfileBox2Edits).GetMethod("Draw", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle.GetFunctionPointer(); // Where we want the program to go

            dstAddress = target;//new address to call
            srcAddress = (long)ptr + 5; //memory location of caller + size of call sequence (5 bytes, 1 for the opcode, 4 for the offset)
            newCallPtr = dstAddress - srcAddress;

            redirector = new byte[] {
                        0xE9,   // CALL INSTRUCTION + TARGET ADDRESS IN LITTLE ENDIAN
                        (byte)(newCallPtr & 0xff),
                        (byte)((newCallPtr >> 8) & 0xff),
                        (byte)((newCallPtr >> 16) & 0xff),
                        (byte)((newCallPtr >> 24) & 0xff),
                        0xC3 // Return OpCode so we don't run the rest of the function despite never getting to this opcode
                    };
            Marshal.Copy(redirector, 0, ptr, redirector.Length);

            // Part 3 - Detour Spawn()

            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2).GetMethod("Spawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle);
            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2Edits).GetMethod("Spawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle);

            ptr = typeof(ProfileBox2).GetMethod("Spawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle.GetFunctionPointer(); ; // Where we want to change
            target = (int)typeof(ProfileBox2Edits).GetMethod("Spawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle.GetFunctionPointer(); // Where we want the program to go

            dstAddress = target;//new address to call
            srcAddress = (long)ptr + 5; //memory location of caller + size of call sequence (5 bytes, 1 for the opcode, 4 for the offset)
            newCallPtr = dstAddress - srcAddress;

            redirector = new byte[] {
                        0xE9,   // CALL INSTRUCTION + TARGET ADDRESS IN LITTLE ENDIAN
                        (byte)(newCallPtr & 0xff),
                        (byte)((newCallPtr >> 8) & 0xff),
                        (byte)((newCallPtr >> 16) & 0xff),
                        (byte)((newCallPtr >> 24) & 0xff),
                        0xC3 // Return OpCode so we don't run the rest of the function despite never getting to this opcode
                    };
            Marshal.Copy(redirector, 0, ptr, redirector.Length);

            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle);
            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2Edits).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle);

            ptr = typeof(ProfileBox2).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle.GetFunctionPointer(); ; // Where we want to change
            target = (int)typeof(ProfileBox2Edits).GetMethod("Despawn", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { }, null).MethodHandle.GetFunctionPointer(); // Where we want the program to go

            dstAddress = target;//new address to call
            srcAddress = (long)ptr + 5; //memory location of caller + size of call sequence (5 bytes, 1 for the opcode, 4 for the offset)
            newCallPtr = dstAddress - srcAddress;

            redirector = new byte[] {
                        0xE9,   // CALL INSTRUCTION + TARGET ADDRESS IN LITTLE ENDIAN
                        (byte)(newCallPtr & 0xff),
                        (byte)((newCallPtr >> 8) & 0xff),
                        (byte)((newCallPtr >> 16) & 0xff),
                        (byte)((newCallPtr >> 24) & 0xff),
                        0xC3 // Return OpCode so we don't run the rest of the function despite never getting to this opcode
                    };
            Marshal.Copy(redirector, 0, ptr, redirector.Length);
        }

        public void Spawn()
        {
            if (_duck != null)
            {
                _teamSelection = _controllerIndex;
                typeof(ProfileBox2).GetMethod("SelectTeam", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(this, null);
                typeof(ProfileBox2).GetMethod("ReturnControl", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(this, null);

            }
            else
            {
                int rightRoomValue = rightRoom();
                _window = new List<Window>();

                if (rightRoomValue == 2)
                {
                    _duck = new Duck(x + 46f, y + 30f, _playerProfile);
                    Window windowSpawn = new Window(x + 2f, y + 34f);
                    windowSpawn.noframe = true;
                    Level.Add(windowSpawn);
                    _window.Add(windowSpawn);
                }
                else if (rightRoomValue == 1)
                {
                    _duck = new Duck(x + 46f, y + 30f, _playerProfile);
                    Window windowSpawn = new Window(x + 2f, y + 34f);
                    windowSpawn.noframe = true;
                    Level.Add(windowSpawn);
                    _window.Add(windowSpawn);
                    windowSpawn = new Window(x + 78f, y + 34f);
                    windowSpawn.noframe = true;
                    Level.Add(windowSpawn);
                    _window.Add(windowSpawn);
                }
                else if (rightRoomValue == 0)
                {
                    _duck = new Duck(x + 46f, y + 30f, _playerProfile);
                    Window windowSpawn = new Window(x + 78f, y + 34f);
                    windowSpawn.noframe = true;
                    Level.Add(windowSpawn);
                    _window.Add(windowSpawn);
                }
                Level.Add(_duck);
                if (_duck == null || !_duck.HasEquipment(typeof(TeamHat)))
                    return;
                _hatSelector.hat = _duck.GetEquipment(typeof(TeamHat)) as TeamHat;
            }
        }

        public void Despawn()
        {
            if (Network.isServer)
            {
                if (_duck != null)
                {
                    Thing.Fondle(_duck, DuckNetwork.localConnection);
                    Level.Remove(_duck);
                    if (!Network.isActive && _duck.ragdoll != null)
                        Level.Remove(_duck.ragdoll);
                }
                if (_gun != null)
                {
                    Thing.Fondle(_gun, DuckNetwork.localConnection);
                    Level.Remove(_gun);
                }
                if (_window != null)
                {
                    foreach (Window window in _window)
                    {
                        if (window != null)
                        {
                            window.lobbyRemoving = true;
                            Thing.Fondle(window, DuckNetwork.localConnection);
                            Level.Remove(window);
                        }
                    }
                }

                /*if (_window2 != null)
                {
                    _window2.lobbyRemoving = true;
                    Thing.Fondle(_window2, DuckNetwork.localConnection);
                    Level.Remove(_window2);
                }*/
            }
            _duck = null;
            _gun = null;
        }

        public ProfileBox2Edits(float xpos, float ypos, InputProfile profile, Profile defaultProfile, TeamSelect2 teamSelect, int index) : base(xpos, ypos, null)
        {
            // Since this gets called by an inserted jump at the beginning of the ProfileBox2 constructor, rather than 'this'
            // returning an instance of ProfileBox2Edits, it returns the ProfileBox2 being instanciated.

            Type profileBox2 = typeof(ProfileBox2);
            Type teamProjector = ass.GetType("DuckGame.TeamProjector");
            dynamic thisVar = this;

            _hostCrown = new Sprite("hostCrown", 0.0f, 0.0f);
            _hostCrown.CenterOrigin();
            _lightBar = new SpriteMap("lightBar", 2, 1, false);
            _lightBar.frame = 0;
            _roomSwitch = new SpriteMap("roomSwitch", 7, 5, false);
            _roomSwitch.frame = 0;
            _controllerIndex = index;
            _font = new BitmapFont("biosFont", 8, -1);
            _fontSmall = new BitmapFont("smallBiosFont", 7, 6);
            layer = Layer.Game;
            _collisionSize = new Vec2(10f, 10f);
            _plaque = new Sprite("plaque", 0.0f, 0.0f);
            _plaque.center = new Vec2(16f, 16f);
            _inputProfile = profile;
            _playerProfile = defaultProfile;
            _teamSelection = _controllerIndex;
            _doorLeft = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("selectDoorLeft"), 0.0f, 0.0f);
            _doorLeft.depth = 0.95f;
            _doorRight = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("selectDoorRight"), 0.0f, 0.0f);
            _doorRight.depth = 0.9f;
            _doorLeftBlank = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("selectDoorLeftBlank"), 0.0f, 0.0f);
            _doorLeftBlank.depth = 0.95f;
            _doorRightBlank = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("selectDoorRightBlank"), 0.0f, 0.0f);
            _doorRightBlank.depth = 0.9f;
            _doorSpinner = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("doorSpinner"), 17, 16, false);
            _doorSpinner.AddAnimation("spin", 0.2f, 1 != 0, 0, 1, 2, 3, 4, 5, 6, 7);
            _doorSpinner.SetAnimation("spin");
            _doorIcon = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("doorSpinner"), 17, 16, false);
            _teamSelect = teamSelect;
            _defaultProfile = defaultProfile;

            int rightRoomValue = rightRoom();

            if (rightRoomValue == 2)
            {
                _roomLeftBackground = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("rightRoomBackground"), 0.0f, 0.0f);
                _roomLeftForeground = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("rightRoomForeground"), 0.0f, 0.0f);

                /*
                Block test = new Block(x + 2f, y + 47f, 77f, 8f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);

                test = new Block( x, y - 3f, 77f, 4f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);

                test = new Block( x, y + 42f, 14f, 15f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);

                test = new Block( x + 77f, y - 3f, 4f, 60f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);
                */

                Level.Add(new InvisibleBlock(x + 2f, y + 47f, 77f, 8f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x, y - 3f, 77f, 4f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x, y + 42f, 14f, 15f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x + 77f, y - 3f, 4f, 60f, PhysicsMaterial.Metal));
                
                ScaffoldingTileset scaffoldingTileset = new ScaffoldingTileset(x + 68f, y + 45f);
                Level.Add(scaffoldingTileset);
                scaffoldingTileset.depth = -0.5f;
                scaffoldingTileset.PlaceBlock();

                _readySign = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("readyLeft"), 0.0f, 0.0f);
            }
            else if (rightRoomValue == 1)
            {
                _roomLeftBackground = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("middleRoomBackground"), 0.0f, 0.0f);
                _roomLeftForeground = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("middleRoomForeground"), 0.0f, 0.0f);

                /*
                Block test = new Block(x + 2f, y + 47f, 64f, 10f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);

                test = new Block( x, y - 3f, 80f, 4f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);

                test = new Block(x + 66f, y + 42f, 15f, 15f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);

                test = new Block( x - 1f, y + 42f, 15f, 15f, PhysicsMaterial.Metal);
                test.graphic = new SpriteMap(Mod.GetPath<IncreasedPlayerLimit>("ayylmao"), (int)test.width, (int)test.height, false);
                Level.Add(test);
                */

                
                Level.Add(new InvisibleBlock(x + 2f, y + 47f, 64f, 10f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x, y - 3f, 80f, 4f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x + 66f, y + 42f, 15f, 15f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x - 1f, y + 42f, 15f, 15f, PhysicsMaterial.Metal));
                

                ScaffoldingTileset scaffoldingTileset = new ScaffoldingTileset(x + 56f, y + 45f);
                Level.Add(scaffoldingTileset);
                scaffoldingTileset.depth = -0.5f;
                scaffoldingTileset.PlaceBlock();

                _readySign = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("readyLeft"), 0.0f, 0.0f);
            }
            else if (rightRoomValue == 0)
            {
                _roomLeftBackground = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("leftRoomBackground"), 0.0f, 0.0f);
                _roomLeftForeground = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("leftRoomForeground"), 0.0f, 0.0f);               
                
                Level.Add(new InvisibleBlock(x + 2f, y + 47f, 64f, 10f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x, y - 3f, 80f, 4f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x + 66f, y + 42f, 14f, 15f, PhysicsMaterial.Metal));
                Level.Add(new InvisibleBlock(x, y - 3f, 4f, 60f, PhysicsMaterial.Metal));
                
                ScaffoldingTileset scaffoldingTileset = new ScaffoldingTileset(x + 11f, y + 45f);
                Level.Add(scaffoldingTileset);
                scaffoldingTileset.depth = -0.5f;
                scaffoldingTileset.PlaceBlock();

                _readySign = new Sprite(Mod.GetPath<IncreasedPlayerLimit>("readyRight"), 0.0f, 0.0f);
            }

            _readySign.depth = 0.2f;
            _roomLeftBackground.depth = -0.6f;
            _roomLeftForeground.depth = 0.1f;
            _tutorialMessages = new SpriteMap("tutorialScreensPC", 53, 30, false);
            _aButton = new Sprite("aButton", 0.0f, 0.0f);
            _tutorialTV = new Sprite("tutorialTV", 0.0f, 0.0f);
            _consoleHighlight = new Sprite("consoleHighlight", 0.0f, 0.0f);
            _consoleFlash = new Sprite("consoleFlash", 0.0f, 0.0f);
            _consoleFlash.CenterOrigin();
            _selectConsole = new SpriteMap("selectConsole", 20, 19, false);
            _selectConsole.AddAnimation("idle", 1f, 1 != 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            _selectConsole.SetAnimation("idle");

            if (Network.isServer)
            {
                _hatSelector = new HatSelector(x, y, _playerProfile, thisVar);
                _hatSelector.profileBoxNumber = (sbyte)index;
                Level.Add(_hatSelector);
            }
            if (rightRoomValue == 2)
            {
                _projector = Activator.CreateInstance(teamProjector, new object[] { x + 38f, y + 46f, _playerProfile });
            }
            else if (rightRoomValue == 1)
            {
                _projector = Activator.CreateInstance(teamProjector, new object[] { x + 40f, y + 46f, _playerProfile });
            }
            else if (rightRoomValue == 0)
            {
                _projector = Activator.CreateInstance(teamProjector, new object[] { x + 42f, y + 46f, _playerProfile });
            }
            Level.Add(_projector);
        }

        public override void Draw()
        {
            if (_hatSelector != null && _hatSelector.fadeVal > 0.899999976158142)
            {
                _projector.visible = false;
                if (_duck == null)
                    return;
                _duck.mindControl = new InputProfile("");
            }
            else
            {
                if (_duck != null)
                    _duck.mindControl = null;
                _projector.visible = true;

                int rightRoomValue = rightRoom();

                Color BlueGray = new Color(47, 73, 79);

                if (_doorX < 82.0f)
                {
                    Sprite sprite1 = _doorLeft;
                    Sprite sprite2 = _doorRight;
                    bool flag1 = _playerProfile.slotType == SlotType.Closed;
                    bool flag2 = _playerProfile.slotType == SlotType.Friend;
                    bool flag3 = _playerProfile.slotType == SlotType.Invite;
                    bool flag4 = _playerProfile.slotType == SlotType.Reserved;
                    bool flag5 = _playerProfile.slotType == SlotType.Local;
                    bool flag6 = _playerProfile.networkStatus != DuckNetStatus.Disconnected;
                    if (Network.isActive)
                    {
                        sprite1 = _doorLeftBlank;
                        sprite2 = _doorRightBlank;
                    }
                    else
                    {
                        flag1 = false;
                        flag2 = false;
                        flag3 = false;
                        flag4 = false;
                        flag6 = false;
                    }
                    Sprite doorLeftBlank = _doorLeftBlank;
                    Sprite doorRightBlank = _doorRightBlank;
                    if (rightRoomValue == 2)
                    {
                        Rectangle sourceRectangle1 = new Rectangle((int)_doorX, 0.0f, _doorLeft.width, _doorLeft.height);
                        Graphics.Draw(doorLeftBlank, this.x - 1f, y, sourceRectangle1);
                        Rectangle sourceRectangle2 = new Rectangle((int)-_doorX, 0.0f, _doorRight.width, _doorRight.height);
                        Graphics.Draw(doorRightBlank, this.x + 38f, y, sourceRectangle2);
                        if (_doorX == 0.0)
                        {
                            _fontSmall.depth = doorLeftBlank.depth + 10;
                            _fontSmall.scale = new Vec2(0.75f, 0.75f);
                            if (!Network.isActive || flag5 && Network.isServer)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 10;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("PRESS", new Vec2(x + 4f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("START", new Vec2(x + 50f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag1)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 8;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                            }
                            else if (flag6)
                            {
                                _doorSpinner.depth = doorLeftBlank.depth + 10;
                                Graphics.Draw(_doorSpinner, x + 31f, y + 20f);
                            }
                            else if (flag2)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 11;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("PALS", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("ONLY", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag3)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 12;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("VIPS", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("ONLY", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag4 && _playerProfile.reservedUser != null)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 12;
                                Graphics.Draw(_doorIcon, this.x + 31f, y + 20f);
                                float num = 120f;
                                float x = this.x + 10f;
                                Graphics.DrawRect(new Vec2(x, y + 35f), new Vec2(x + num, y + 52f), Color.Black, doorLeftBlank.depth + 20, true, 1f);
                                string text1 = "WAITING FOR";
                                _fontSmall.Draw(text1, new Vec2((float)(x + num / 2.0 - _fontSmall.GetWidth(text1, false, (InputProfile)null) / 2.0), y + 36f), Color.White, doorLeftBlank.depth + 30, null, false);
                                string text2 = _playerProfile.name;
                                if (text2.Length > 16)
                                    text2 = text2.Substring(0, 16);
                                _fontSmall.Draw(text2, new Vec2((float)(x + num / 2.0 - _fontSmall.GetWidth(text2, false, (InputProfile)null) / 2.0), y + 44f), Color.White, doorLeftBlank.depth + 30, null, false);
                            }
                            else if (flag5)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 13;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("HOST", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("SLOT", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 9;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("OPEN", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("SLOT", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                        }
                    }
                    else if (rightRoomValue == 1)
                    {
                        Rectangle sourceRectangle1 = new Rectangle((int)_doorX, 0.0f, _doorLeft.width, _doorLeft.height);
                        Graphics.Draw(doorLeftBlank, this.x - 1f, y, sourceRectangle1);
                        Rectangle sourceRectangle2 = new Rectangle((int)-_doorX, 0.0f, _doorRight.width, _doorRight.height);
                        Graphics.Draw(doorRightBlank, this.x + 38f, y, sourceRectangle2);
                        if (_doorX == 0.0)
                        {
                            _fontSmall.depth = doorLeftBlank.depth + 10;
                            _fontSmall.scale = new Vec2(0.75f, 0.75f);
                            if (!Network.isActive || flag5 && Network.isServer)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 10;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("PRESS", new Vec2(x + 4f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("START", new Vec2(x + 50f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag1)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 8;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                            }
                            else if (flag6)
                            {
                                _doorSpinner.depth = doorLeftBlank.depth + 10;
                                Graphics.Draw(_doorSpinner, x + 31f, y + 20f);
                            }
                            else if (flag2)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 11;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("PALS", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("ONLY", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag3)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 12;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("VIPS", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("ONLY", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag4 && _playerProfile.reservedUser != null)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 12;
                                Graphics.Draw(_doorIcon, this.x + 31f, y + 20f);
                                float num = 120f;
                                float x = this.x + 10f;
                                Graphics.DrawRect(new Vec2(x, y + 35f), new Vec2(x + num, y + 52f), Color.Black, doorLeftBlank.depth + 20, true, 1f);
                                string text1 = "WAITING FOR";
                                _fontSmall.Draw(text1, new Vec2((float)(x + num / 2.0 - _fontSmall.GetWidth(text1, false, (InputProfile)null) / 2.0), y + 36f), Color.White, doorLeftBlank.depth + 30, null, false);
                                string text2 = _playerProfile.name;
                                if (text2.Length > 16)
                                    text2 = text2.Substring(0, 16);
                                _fontSmall.Draw(text2, new Vec2((float)(x + num / 2.0 - _fontSmall.GetWidth(text2, false, (InputProfile)null) / 2.0), y + 44f), Color.White, doorLeftBlank.depth + 30, null, false);
                            }
                            else if (flag5)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 13;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("HOST", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("SLOT", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 9;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("OPEN", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("SLOT", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                        }
                    }
                    else
                    {
                        Rectangle sourceRectangle1 = new Rectangle((int)_doorX, 0.0f, _doorLeft.width, _doorLeft.height);
                        Graphics.Draw(doorLeftBlank, this.x - 1f, y, sourceRectangle1);
                        Rectangle sourceRectangle2 = new Rectangle((int)-_doorX, 0.0f, _doorRight.width, _doorRight.height);
                        Graphics.Draw(doorRightBlank, this.x + 38f, y, sourceRectangle2);
                        if (_doorX == 0.0)
                        {
                            _fontSmall.depth = doorLeftBlank.depth + 10;
                            _fontSmall.scale = new Vec2(0.75f, 0.75f);
                            if (!Network.isActive || flag5 && Network.isServer)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 10;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("PRESS", new Vec2(x + 4f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("START", new Vec2(x + 50f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag1)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 8;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                            }
                            else if (flag6)
                            {
                                _doorSpinner.depth = doorLeftBlank.depth + 10;
                                Graphics.Draw(_doorSpinner, x + 31f, y + 20f);
                            }
                            else if (flag2)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 11;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("PALS", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("ONLY", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag3)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 12;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("VIPS", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("ONLY", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag4 && _playerProfile.reservedUser != null)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 12;
                                Graphics.Draw(_doorIcon, this.x + 31f, y + 20f);
                                float num = 120f;
                                float x = this.x + 10f;
                                Graphics.DrawRect(new Vec2(x, y + 35f), new Vec2(x + num, y + 52f), Color.Black, doorLeftBlank.depth + 20, true, 1f);
                                string text1 = "WAITING FOR";
                                _fontSmall.Draw(text1, new Vec2((float)(x + num / 2.0 - _fontSmall.GetWidth(text1, false, (InputProfile)null) / 2.0), y + 36f), Color.White, doorLeftBlank.depth + 30, null, false);
                                string text2 = _playerProfile.name;
                                if (text2.Length > 16)
                                    text2 = text2.Substring(0, 16);
                                _fontSmall.Draw(text2, new Vec2((float)(x + num / 2.0 - _fontSmall.GetWidth(text2, false, (InputProfile)null) / 2.0), y + 44f), Color.White, doorLeftBlank.depth + 30, null, false);
                            }
                            else if (flag5)
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 13;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("HOST", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("SLOT", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                            else
                            {
                                _doorIcon.depth = doorLeftBlank.depth + 10;
                                _doorIcon.frame = 9;
                                Graphics.Draw(_doorIcon, x + 31f, y + 20f);
                                _fontSmall.DrawOutline("OPEN", new Vec2(x + 6f, y + 26f), Color.White, BlueGray, doorLeftBlank.depth + 10);
                                _fontSmall.DrawOutline("SLOT", new Vec2(x + 52f, y + 26f), Color.White, BlueGray, doorRightBlank.depth + 10);
                            }
                        }
                    }
                }
                if (_playerProfile.team == null || _doorX <= 0.0)
                    return;
                if (rightRoomValue == 2) // Right
                {
                    Graphics.Draw(_roomLeftBackground, x - 1f, y + 1f);

                    Graphics.Draw(_roomLeftForeground, x - 1f, y + 1f, new Rectangle(0.0f, 0.0f, 82f, 56f));

                    if (Network.isActive && (Network.isServer && _playerProfile.connection == DuckNetwork.localConnection || _playerProfile.connection == Network.host))
                    {
                        _hostCrown.depth = -0.5f;
                        Graphics.Draw(_hostCrown, x + 68f, y + 3f);
                    }
                }
                else if (rightRoomValue == 1) // Middle
                {
                    Graphics.Draw(_roomLeftBackground, x - 1f, y + 1f);

                    // Draws the whole sprite at once. Easy peasy
                    Graphics.Draw(_roomLeftForeground, x - 1f, y + 1f, new Rectangle(0f, 0f, 82f, 56f));

                    // Draws each individual side and is a nightmare to line up
                    /*
                    Graphics.Draw(_roomLeftForeground, x , y + 2f, new Rectangle(0f, 1f, 4f, 55f)); // Left side
                    Graphics.Draw(_roomLeftForeground, x + 4f, y + 47f, new Rectangle(4f, 46f, 62f, 10f)); // Bottom bar
                    Graphics.Draw(_roomLeftForeground, x + 52f, y + 2f, new Rectangle(51f, 0f, 31f, 9f)); // Ready box
                    Graphics.Draw(_roomLeftForeground, x + 66f, y + 42f, new Rectangle(66f, 41f, 16f, 15f)); // Box near window
                    */

                    if (Network.isActive && (Network.isServer && _playerProfile.connection == DuckNetwork.localConnection || _playerProfile.connection == Network.host))
                    {
                        _hostCrown.depth = -0.5f;
                        Graphics.Draw(_hostCrown, x + 13f, y + 3f);
                    }
                }
                else if (rightRoomValue == 0) // Left
                {
                    Graphics.Draw(_roomLeftBackground, x + 1f, y + 1f);

                    // Draws the whole sprite at once. Easy peasy
                    Graphics.Draw(_roomLeftForeground, x - 1f, y + 1f, new Rectangle(0f, 0f, 82f, 56f));

                    // Draws each individual side and is a nightmare to line up
                    /*
                    Graphics.Draw(_roomLeftForeground, x , y + 2f, new Rectangle(0f, 1f, 4f, 55f)); // Left side
                    Graphics.Draw(_roomLeftForeground, x + 4f, y + 47f, new Rectangle(4f, 46f, 62f, 10f)); // Bottom bar
                    Graphics.Draw(_roomLeftForeground, x + 52f, y + 2f, new Rectangle(51f, 0f, 31f, 9f)); // Ready box
                    Graphics.Draw(_roomLeftForeground, x + 66f, y + 42f, new Rectangle(66f, 41f, 16f, 15f)); // Box near window
                    */

                    if (Network.isActive && (Network.isServer && _playerProfile.connection == DuckNetwork.localConnection || _playerProfile.connection == Network.host))
                    {
                        _hostCrown.depth = -0.5f;
                        Graphics.Draw(_hostCrown, x + 11f, y + 3f);
                    }
                }
                _font.alpha = 1f;
                _font.depth = 0.6f;
                string currentDisplayName = _playerProfile.team.currentDisplayName;
                _selectConsole.depth = -0.5f;
                _consoleHighlight.depth = -0.49f;

                if (rightRoomValue == 2)
                {
                    _consolePos = new Vec2(x + 58f, y + 10f);
                    _consoleFlash.scale = new Vec2(0.75f, 0.75f);
                    if (_selectConsole.imageIndex == 0)
                        _consoleFlash.alpha = 0.3f;
                    else if (_selectConsole.imageIndex == 1)
                        _consoleFlash.alpha = 0.1f;
                    else if (_selectConsole.imageIndex == 2)
                        _consoleFlash.alpha = 0.0f;
                    Graphics.Draw(_consoleFlash, _consolePos.x + 9f, _consolePos.y + 7f);
                    Graphics.Draw(_selectConsole, _consolePos.x, _consolePos.y);
                    if (_consoleFade > 0.00999999977648258)
                    {
                        _consoleHighlight.alpha = _consoleFade;
                        Graphics.Draw(_consoleHighlight, _consolePos.x, _consolePos.y);
                    }
                    Graphics.Draw(_readySign, x + 1f, y + 2f);

                    float num2 = 0.0f;
                    float num3 = 0.0f;
                    _font.scale = new Vec2(0.45f, 0.45f);
                    if (currentDisplayName.Length > 9)
                    {
                        _font.scale = new Vec2(0.4f, 0.4f);
                        num2 = 0f;
                        num3 = 1f;
                    }
                    if (currentDisplayName.Length > 12)
                    {
                        _font.scale = new Vec2(0.35f, 0.35f);
                        num2 = 0.5f;
                        num3 = 4f;
                    }
                    _font.Draw(currentDisplayName, (float)(x + 50.0 - _font.GetWidth(currentDisplayName, false, null) / 2.0) - num3, y + 52f + num2, Color.White, 0.7f, null, false);
                }
                else if (rightRoomValue == 1)
                {
                    _consolePos = new Vec2(x + 46f, y + 10f);
                    _consoleFlash.scale = new Vec2(0.75f, 0.75f);
                    if (_selectConsole.imageIndex == 0)
                        _consoleFlash.alpha = 0.3f;
                    else if (_selectConsole.imageIndex == 1)
                        _consoleFlash.alpha = 0.1f;
                    else if (_selectConsole.imageIndex == 2)
                        _consoleFlash.alpha = 0.0f;
                    Graphics.Draw(_consoleFlash, _consolePos.x + 9f, _consolePos.y + 7f);
                    Graphics.Draw(_selectConsole, _consolePos.x, _consolePos.y);
                    if (_consoleFade > 0.00999999977648258)
                    {
                        _consoleHighlight.alpha = _consoleFade;
                        Graphics.Draw(_consoleHighlight, _consolePos.x, _consolePos.y);
                    }
                    Graphics.Draw(_readySign, x + 1f, y + 2f);

                    _aButton.position = new Vec2(x + 39f, y + 71f);

                    float num2 = 0.0f;
                    float num3 = 0.0f;
                    _font.scale = new Vec2(0.45f, 0.45f);
                    if (currentDisplayName.Length > 9)
                    {
                        _font.scale = new Vec2(0.4f, 0.4f);
                        num2 = 0f;
                        num3 = 1f;
                    }
                    if (currentDisplayName.Length > 12)
                    {
                        _font.scale = new Vec2(0.25f, 0.25f);
                        num2 = 0.5f;
                        num3 = 0f;
                    }
                    _font.Draw(currentDisplayName, (float)(x + 40.0 - _font.GetWidth(currentDisplayName, false, null) / 2.0) - num3, y + 52f + num2, Color.White, 0.7f, null, false);
                }
                else if (rightRoomValue == 0)
                {
                    _consolePos = new Vec2(x + 1f, y + 10f);
                    _consoleFlash.scale = new Vec2(0.75f, 0.75f);
                    if (_selectConsole.imageIndex == 0)
                        _consoleFlash.alpha = 0.3f;
                    else if (_selectConsole.imageIndex == 1)
                        _consoleFlash.alpha = 0.1f;
                    else if (_selectConsole.imageIndex == 2)
                        _consoleFlash.alpha = 0.0f;
                    Graphics.Draw(_consoleFlash, _consolePos.x + 9f, _consolePos.y + 7f);
                    Graphics.Draw(_selectConsole, _consolePos.x, _consolePos.y);
                    if (_consoleFade > 0.00999999977648258)
                    {
                        _consoleHighlight.alpha = _consoleFade;
                        Graphics.Draw(_consoleHighlight, _consolePos.x, _consolePos.y);
                    }
                    Graphics.Draw(_readySign, x + 53f, y + 2f);

                    _aButton.position = new Vec2(x + 39f, y + 71f);

                    float num2 = 0.0f;
                    float num3 = 0.0f;
                    _font.scale = new Vec2(0.45f, 0.45f);
                    if (currentDisplayName.Length > 9)
                    {
                        _font.scale = new Vec2(0.4f, 0.4f);
                        num2 = 0f;
                        num3 = 1f;
                    }
                    if (currentDisplayName.Length > 12)
                    {
                        _font.scale = new Vec2(0.35f, 0.35f);
                        num2 = 0.5f;
                        num3 = 1f;
                    }
                    _font.Draw(currentDisplayName, (float)(x + 37.0 - _font.GetWidth(currentDisplayName, false, null) / 2.0) - num3, y + 52f + num2, Color.White, 0.7f, null, false);
                }
            }
        }
    }
}