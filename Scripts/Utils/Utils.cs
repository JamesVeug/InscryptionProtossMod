using System.Collections.Generic;
using UnityEngine;

namespace ProtossMod
{
    public static class Utils
    {
        private static List<Texture> s_defaultDecals = null;
        public static List<Texture> GetDecals()
        {
            if (s_defaultDecals == null)
            {
                Texture2D decal = Utils.GetTextureFromPath(Plugin.DecalPath);
                s_defaultDecals = new List<Texture> { decal };
            }
            
            return s_defaultDecals;
        }
        
        public static Texture2D GetTextureFromPath(string path)
        {
            return StarCraftCore.Utils.GetTextureFromPath(path, Plugin.Directory);
        }
    }
}