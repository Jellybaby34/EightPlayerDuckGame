using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DuckGame.IncreasedPlayerLimit
{
    public class LevelEdits
    {
        public static void UpdateLevelChange()
        {
            // I would like to thank the Pen Testing program Gray Storm and its relevant paper for the idea applied here
            // We inject a call into the method assembly so that when its called, it calls our method :D

            RuntimeHelpers.PrepareMethod(typeof(Level).GetMethod("UpdateLevelChange").MethodHandle);
            RuntimeHelpers.PrepareMethod(typeof(LevelEdits).GetMethod("UpdateLevelChangeReplace").MethodHandle);

            IntPtr ptr = typeof(Level).GetMethod("UpdateLevelChange").MethodHandle.GetFunctionPointer(); // Where we want to change

            int target = (int)typeof(LevelEdits).GetMethod("UpdateLevelChangeReplace").MethodHandle.GetFunctionPointer(); // Where we want the program to go

            long dstAddress = (long)target;//new address to call
            long srcAddress = (long)ptr + 5; //memory location of caller + size of call sequence (5 bytes, 1 for the opcode, 4 for the offset)
            long newCallPtr = dstAddress - srcAddress;

            byte[] redirector = new byte[] {
                        0xE8,   // CALL INSTRUCTION + TARGET ADDRESS IN LITTLE ENDIAN
                        (byte)(newCallPtr & 0xff),
                        (byte)((newCallPtr >> 8) & 0xff),
                        (byte)((newCallPtr >> 16) & 0xff),
                        (byte)((newCallPtr >> 24) & 0xff),
                        0xC3 // Return OpCode so we don't run the rest of the function
                    };
            Marshal.Copy(redirector, 0, ptr, redirector.Length);
        }

        public static void DoInitialize()
        {
            FieldInfo _initialized = typeof(Level).GetField("_initialized", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (Level.current.waitingOnNewData)
            {
                FieldInfo _initializeLater = typeof(Level).GetField("_initializeLater", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                _initializeLater.SetValue(Level.current, true);
                _initialized.SetValue(Level.current, true);
            }
            else if (!Level.current.initialized)
            {
                Assembly assembly = Assembly.GetAssembly(typeof(Level));
                Type StaticRenderertype = assembly.GetType("DuckGame.StaticRenderer");
                dynamic StaticRenderertypeUpdate = StaticRenderertype.GetMethod("Update", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                bool _centeredView = (bool)typeof(Level).GetField("_centeredView", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(Level.current);

                // Our custom initialize routine so it doesn't crash. Hopefully anyway
                RockScoreboardEdits.Initialize();

                if (!Network.isActive || Level.current is TeamSelect2)
                {
                    MethodInfo doStart = typeof(Level).GetMethod("DoStart", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    doStart.Invoke(Level.current, null);
                }
                Level.current.things.RefreshState();
                Level.current.CalculateBounds();
                _initialized.SetValue(Level.current, true);
                if (_centeredView)
                    Level.current.camera.centerY -= (float)(((double)(DuckGame.Graphics.aspect * Level.current.camera.width) - (double)(9f / 16f * Level.current.camera.width)) / 2.0);
                if (VirtualTransition.active)
                    return;
                StaticRenderertypeUpdate.Invoke(null, null);
            }
            else
            {
                foreach (Thing thing in Level.current.things)
                    thing.AddToLayer();
            }
        }

        public static void UpdateLevelChangeReplace()
        {
            Type type = typeof(Level);

            Assembly assembly = Assembly.GetAssembly(type);
            Type HUDtype = assembly.GetType("DuckGame.HUD");
            dynamic HUDClearCorners = HUDtype.GetMethod("ClearCorners", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            Type ConnectionStatusUIType = assembly.GetType("DuckGame.ConnectionStatusUI");
            dynamic ConnectionStatusUIShow = ConnectionStatusUIType.GetMethod("Show", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic ConnectionStatusUIHide = ConnectionStatusUIType.GetMethod("Hide", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            FieldInfo _readyForTransitionField = type.GetField("_readyForTransition", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _readyForTransition = _readyForTransitionField.GetValue(Level.current);

            if (Level.core.nextLevel != null)
            {

                FieldInfo _sentLevelChangeField = type.GetField("_sentLevelChange", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                dynamic _sentLevelChange = _sentLevelChangeField.GetValue(Level.current);

                FieldInfo _networkStatusField = type.GetField("_networkStatus", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                dynamic _networkStatus = _networkStatusField.GetValue(Level.current);

                if (Level.core.currentLevel is IHaveAVirtualTransition && Level.core.nextLevel is IHaveAVirtualTransition && !(Level.core.nextLevel is TeamSelect2))
                    VirtualTransition.GoVirtual();
                if (Network.isActive && Level.activeLevel != null && !_sentLevelChange)
                {
                    //                    DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Performing level swap (" + (object)DuckNetwork.levelIndex + ")", -1);
                    GhostManager.context.Clear();
                    foreach (Profile profile in Profiles.active)
                    {
                        if (profile.connection != null)
                            profile.connection.manager.Reset();
                    }
                    if (Level.core.currentLevel is TeamSelect2 && !(Level.core.nextLevel is TeamSelect2))
                        DuckNetwork.ClosePauseMenu();
                    if (!(Level.core.currentLevel is TeamSelect2) && Level.core.nextLevel is TeamSelect2)
                        DuckNetwork.ClosePauseMenu();
                    if (Network.isServer)
                    {
                        ++DuckNetwork.levelIndex;
                        DuckNetwork.compressedLevelData = (MemoryStream)null;
                        //                        DevConsole.Log(DCSection.GhostMan, "|DGYELLOW|Incrementing level index (" + (object)((int)DuckNetwork.levelIndex - 1) + "->" + (object)DuckNetwork.levelIndex + ")", -1);
                        uint varChecksum = 0;
                        bool varNeedsChecksum = false;
                        string lev = "";
                        if (!(Level.core.currentLevel is TeamSelect2) && Level.core.nextLevel is TeamSelect2)
                            lev = "@TEAMSELECT";
                        else if (Level.core.nextLevel is GameLevel)
                        {
                            GameLevel nextLevel = Level.core.nextLevel as GameLevel;
                            if (nextLevel.customLevel)
                            {
                                varNeedsChecksum = true;
                                varChecksum = nextLevel.checksum;
                                DuckNetwork.compressedLevelData = new MemoryStream(nextLevel.compressedData, 0, nextLevel.compressedData.Length, false, true);
                            }
                            lev = nextLevel.level;
                        }

                        else if (!(Level.core.nextLevel is TeamSelect2) && !(Level.core.nextLevel is GameLevel) && !(Level.core.nextLevel is RockScoreboard))
                            lev = "@ROCKINTRO"; // This need correcting at some point and probably will break things. Must get the internal type

                        else if (Level.core.nextLevel is RockScoreboard)
                        {
                            GhostManager.context.SetGhostIndex((NetIndex16)1);
                            lev = (Level.core.nextLevel as RockScoreboard).mode != ScoreBoardMode.ShowScores ? (!(Level.core.nextLevel as RockScoreboard).afterHighlights ? "@ROCKTHROW|SHOWWINNER" : "@ROCKTHROW|SHOWEND") : "@ROCKTHROW|SHOWSCORE";
                        }
                        if (lev != "")
                        {
                            foreach (Profile profile in DuckNetwork.profiles)
                            {
                                if (profile.connection != null)
                                {
                                    profile.connection.manager.ClearAllMessages();
                                    if (Level.core.nextLevel is GameLevel && (Level.core.nextLevel as GameLevel).level == "RANDOM")
                                        Send.Message((NetMessage)new NMSwitchLevelRandom(lev, DuckNetwork.levelIndex, (ushort)(int)GhostManager.context.currentGhostIndex, (Level.core.nextLevel as GameLevel).seed), NetMessagePriority.ReliableOrdered, profile.connection);
                                    else
                                        Send.Message((NetMessage)new NMSwitchLevel(lev, DuckNetwork.levelIndex, (ushort)(int)GhostManager.context.currentGhostIndex, varNeedsChecksum, varChecksum), NetMessagePriority.ReliableOrdered, profile.connection);
                                }
                            }
                        }
                    }
                    _sentLevelChange = true;
                }
                if (!VirtualTransition.active)
                {
                    Level.InitChanceGroups();
                    DamageManager.ClearHits();
                    Layer.ResetLayers();
                    HUDClearCorners.Invoke(null, null);
                    if (Level.core.currentLevel != null)
                        Level.core.currentLevel.Terminate();
                    Level.core.currentLevel = Level.core.nextLevel;
                    Level.core.nextLevel = (Level)null;
                    Layer.lighting = false;
                    SequenceItem.sequenceItems.Clear();
                    GC.Collect(1, GCCollectionMode.Optimized);
                    foreach (Profile profile in Profiles.active)
                        profile.duck = (Duck)null;
                    SFX.StopAllSounds();

                    if (Level.core.currentLevel is RockScoreboard)
                    {
                        LevelEdits.DoInitialize();
                    }
                    else
                    {
                        Level.core.currentLevel.DoInitialize();
                    }

                    if (MonoMain.pauseMenu != null)
                        Level.core.currentLevel.AddThing((Thing)MonoMain.pauseMenu);
                    if (Network.isActive && DuckNetwork.duckNetUIGroup != null && DuckNetwork.duckNetUIGroup.open)
                        Level.core.currentLevel.AddThing((Thing)DuckNetwork.duckNetUIGroup);
                    if (Recorder.globalRecording != null)
                        Recorder.globalRecording.UpdateAtlasFile();
                    _networkStatus = NetLevelStatus.WaitingForDataTransfer;
                    if (!(Level.core.currentLevel is IOnlyTransitionIn) && Level.core.currentLevel is IHaveAVirtualTransition && (!(Level.core.currentLevel is TeamSelect2) && VirtualTransition.isVirtual))
                    {
                        if (_readyForTransition)
                        {
                            VirtualTransition.GoUnVirtual();
                            DuckGame.Graphics.fade = 1f;
                        }
                        else
                        {
                            Level.current._waitingOnTransition = true;
                            if (Network.isActive)
                                ConnectionStatusUIShow.Invoke(null, null);
                        }
                    }
                }
            }
            if (!Level.current._waitingOnTransition || !_readyForTransition)
                return;
            Level.current._waitingOnTransition = false;
            VirtualTransition.GoUnVirtual();
            if (!Network.isActive)
                return;
            ConnectionStatusUIHide.Invoke(null, null);
        }

        public static void AddReplace(Thing thing)
        {
            if (Level.core.currentLevel == null)
                return;
            Level.core.currentLevel.AddThing(thing);
            if (thing.GetType() == typeof(Duck))
            {
                StateBinding _profileIndexBinding = new StateBinding("netProfileIndex", 3, false);

                FieldInfo _profilesField = typeof(Duck).GetField("_profileIndexBinding", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                _profilesField.SetValue(thing, _profileIndexBinding);
            }
        }

    }
}