using HarmonyLib;

namespace MapVote.Patches
{
    [HarmonyPatch(typeof(MenuManager))]
    internal class MenuManagerPatch
    {
        [HarmonyPatch(nameof(MenuManager.PageOpen))]
        [HarmonyPostfix]
        private static void PageOpenPostfix(MenuPageIndex menuPageIndex)
        {
            if(menuPageIndex == MenuPageIndex.Lobby)
            {
                if(SemiFunc.IsMasterClientOrSingleplayer())
                {
                    MapVote.Reset();
                    MapVote.WonMap = null;
                }
                else
                {
                    // Non-host clients: clear local UI state so stale votes from the
                    // previous round don't persist if no SyncVotes event arrives
                    // (e.g. full lobby already present, no PlayerAdd fires).
                    MapVote.OwnVoteLevel = null;
                    MapVote.VoteOptionButtons.Clear();
                }

                if(!MapVote.HideInMenu.Value)
                {
                    MapVote.CreateVotePopup(true);
                }
                
                if (MapVote.IS_DEBUG && SemiFunc.IsMasterClientOrSingleplayer() && SemiFunc.RunIsLobbyMenu())
                {
                    DebugManager.InitializeDebug();
                }
            }
        }
    }
}