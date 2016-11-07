using System.Linq;

namespace DuckGame.IncreasedPlayerLimit
{
    public class DuckNetworkCoreEdits
    {
        public void RecreateProfiles()
        {

            DuckNetworkCore DuckNetworkCore = DuckNetwork.core;

            DuckNetworkCore.profiles.Clear();
            for (int index = 0; index < 8; ++index)
                DuckNetworkCore.profiles.Add(new Profile("Netduck" + (index + 1).ToString(), InputProfile.GetVirtualInput(index), null, Persona.all.ElementAt(index), true, null)
                {
                    networkIndex = (byte)index
                });

        }
    }
}