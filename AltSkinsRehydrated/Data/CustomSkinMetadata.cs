using System.Collections.Generic;

namespace AltSkinsRehydrated.Data
{
    class CustomSkinMetadata
    {
        public int version;
        public string characterID;
        public string skinName;
        public string skinID;
        public string assetBundlePath;
        public string scenePath;
        public Dictionary<string, string> portraits;
    }
}
