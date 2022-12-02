using Nick;
using SlapNetwork;
using System.Collections.Generic;

namespace AltSkinsRehydrated
{
    class OnlineManager
    {
        public static OnlineLobby Lobby;

        const string CUSTOM_SKIN_KEY = "customskin";

        public static void SetOnlineSkin(string skinID)
        {
            var data = new List<LobbyDataPair>() { new LobbyDataPair(CUSTOM_SKIN_KEY, skinID) };

            Lobby.BaseLobby.SetUserData(data);
        }

        public static bool TryGetOnlineSkinIdForUser(IUser user, out string result)
        {
            AltSkinsPlugin.LogInfo($"Getting Skin ID for {user.UserId}");
            result = Lobby.BaseLobby.GetUserData(user, CUSTOM_SKIN_KEY);
            AltSkinsPlugin.LogInfo($"Got Skin: {result}");
            return !string.IsNullOrEmpty(result);
        }
    }
}
