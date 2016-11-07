using System;
using System.Collections.Generic;
using System.Reflection;

namespace DuckGame.IncreasedPlayerLimit
{
    public class TeamSelect2Edits
    {
        public void OnNetworkConnecting(Profile p)
        {
            Type typea = typeof(ProfileBox2);
            FieldInfo info2 = typea.GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            List<ProfileBox2> _profiles = info2.GetValue(null) as List<ProfileBox2>;

            if (p.networkIndex > 4)
            {
                int index = (p.networkIndex % 4);
                _profiles[index].PrepareDoor();
            }
            _profiles[p.networkIndex].PrepareDoor();
        }
    }
}