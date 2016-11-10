namespace DuckGame.IncreasedPlayerLimit
{
    public class NMChangeSlotsEdits : NMEvent
    {
        public byte slot1;
        public byte slot2;
        public byte slot3;
        public byte slot4;
        public byte slot5;
        public byte slot6;
        public byte slot7;
        public byte slot8;

        public NMChangeSlotsEdits()
        {
        }

        public NMChangeSlotsEdits(byte varslot1, byte varslot2, byte varslot3, byte varslot4, byte varslot5, byte varslot6, byte varslot7, byte varslot8 )
        {
            slot1 = varslot1;
            slot2 = varslot2;
            slot3 = varslot3;
            slot4 = varslot4;
            slot5 = varslot5;
            slot6 = varslot6;
            slot7 = varslot7;
            slot8 = varslot8;
        }

        public override void Activate()
        {
            if (!Network.isServer)
            {
                DuckNetwork.profiles[0].slotType = (SlotType)slot1;
                DuckNetwork.profiles[1].slotType = (SlotType)slot2;
                DuckNetwork.profiles[2].slotType = (SlotType)slot3;
                DuckNetwork.profiles[3].slotType = (SlotType)slot4;
                DuckNetwork.profiles[4].slotType = (SlotType)slot5;
                DuckNetwork.profiles[5].slotType = (SlotType)slot6;
                DuckNetwork.profiles[6].slotType = (SlotType)slot7;
                DuckNetwork.profiles[7].slotType = (SlotType)slot8;
            }
            base.Activate();
        }
    }
}