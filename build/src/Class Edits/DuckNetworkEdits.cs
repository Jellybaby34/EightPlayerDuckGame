using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DuckGame.EightPlayerDuckGame
{
    public class DuckNetworkEdits
    {

        // We need to use our new slots netmessage as the default only handles 4 slots.
        // We edit this method so that duck game uses our new netmessage and not the old one.
        [HarmonyPatch(typeof(DuckNetwork), "ChangeSlotSettings")]
        public static class DuckNetwork_ChangeSlotSettings_Prefix
        {
            [HarmonyPrefix]
            public static bool Prefix()
            {
                bool flag1 = true;
                bool flag2 = true;
                DuckNetwork.numSlots = 0;
                foreach (Profile profile in DuckNetwork.profiles)
                {
                    if (profile.connection != DuckNetwork.localConnection)
                    {
                        if (profile.slotType != SlotType.Friend)
                            flag1 = false;
                        if (profile.slotType != SlotType.Invite)
                            flag2 = false;
                        if (profile.slotType != SlotType.Closed)
                            ++DuckNetwork.numSlots;
                    }
                    else if (profile.slotType != SlotType.Closed)
                        ++DuckNetwork.numSlots;
                }
                if (!Network.isServer)
                    return false;
                if (Steam.lobby != null)
                {
                    Steam.lobby.type = !flag1 ? (!flag2 ? SteamLobbyType.Public : SteamLobbyType.Private) : SteamLobbyType.FriendsOnly;
                    Steam.lobby.maxMembers = 32;
                    Steam.lobby.SetLobbyData("numSlots", DuckNetwork.numSlots.ToString());
                }
                Send.Message(new NMChangeSlots8Player((byte)DuckNetwork.profiles[0].slotType, (byte)DuckNetwork.profiles[1].slotType, (byte)DuckNetwork.profiles[2].slotType, (byte)DuckNetwork.profiles[3].slotType, (byte)DuckNetwork.profiles[4].slotType, (byte)DuckNetwork.profiles[5].slotType, (byte)DuckNetwork.profiles[6].slotType, (byte)DuckNetwork.profiles[7].slotType));
                return false;
            }
        }

        // Join will by default only reset the custom data in the first 4 slots
        // We edit the index condition from index < 4 to index < 8 so that all slots get reset
        [HarmonyPatch(typeof(DuckNetwork), "Join")]
        public static class DuckNetwork_Join_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_4)
                    {
                        codes[i].opcode = OpCodes.Ldc_I4_8;
                        break;
                    }
                }
                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(DuckNetwork), "OnMessage")]
        public static class DuckNetwork_OnMessage_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                    if (codes[0x2E8].opcode == OpCodes.Ldc_I4_4)
                    {
                        codes[0x2E8].opcode = OpCodes.Ldc_I4_8;
                    };

                return codes.AsEnumerable();
            }
        }

        // The netmessages are enumerated on startup and if you send one which isn't enumerated, duck game crashes or at least I remember it doing that
        // We simply run this method to re-enumerate the netmessages after our mod has loaded.
        public static void AddNewNetmessageTypes()
        {
            FieldInfo typeToMessageIDField = typeof(Network).GetField("_typeToMessageID", BindingFlags.Static | BindingFlags.NonPublic);
            dynamic _typeToMessageID = typeToMessageIDField.GetValue(null) as Map<ushort, Type>;

            IEnumerable<Type> subclasses = Editor.GetSubclasses(typeof(NetMessage));
            _typeToMessageID.Clear();
            ushort key = 1;
            foreach (Type type in subclasses)
            {
                if (type.GetCustomAttributes(typeof(FixedNetworkID), false).Length != 0)
                {
                    FixedNetworkID customAttribute = (FixedNetworkID)type.GetCustomAttributes(typeof(FixedNetworkID), false)[0];
                    if (customAttribute != null)
                        _typeToMessageID.Add(type, customAttribute.FixedID);
                }
            }
            foreach (Type type in subclasses)
            {
                if (!_typeToMessageID.ContainsValue(type))
                {
                    while (_typeToMessageID.ContainsKey(key))
                        ++key;
                    _typeToMessageID.Add(type, key);
                    ++key;
                }
            }
        }
    }
}
