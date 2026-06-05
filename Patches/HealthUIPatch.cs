using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace MapVote.Patches
{
    [HarmonyPatch(typeof(HealthUI))]
    public class HealthUIPatch
    {
        [HarmonyPatch(nameof(HealthUI.Start))]
        [HarmonyPostfix]
        static void PostfixStart(HealthUI __instance)
        {
            if (RunManager.instance.levelCurrent.name == MapVote.REQUEST_VOTE_LEVEL)
            {
                MapVote.Instance.StartCoroutine(MapVote.WaitForVote());
                MapVote.Instance.StartCoroutine(WaitForSelectedMapsAndCreatePopup());
            }
        }

        private static IEnumerator WaitForSelectedMapsAndCreatePopup()
        {
            // Master client selects maps inside CreateVotePopup itself, no waiting needed
            if (SemiFunc.IsMasterClientOrSingleplayer())
            {
                MapVote.CreateVotePopup(false);
                yield break;
            }

            // Non-host clients: wait until the host has synced SelectedMaps before
            // building the popup — otherwise SelectedMaps is still empty and every
            // level is shown instead of just the 3 the host picked.
            const float timeout = 5f;
            float elapsed = 0f;

            while (MapVote.SelectedMaps.Count == 0 && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (MapVote.SelectedMaps.Count == 0)
            {
                MapVote.Logger.LogWarning("HealthUIPatch: timed out waiting for SelectedMaps sync — opening popup with all levels as fallback.");
            }

            MapVote.CreateVotePopup(false);
        }
    }
}