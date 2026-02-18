using HarmonyLib;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace CustomClanHelper.Plugin.Patches
{
    /// <summary>
    /// Overwrite function
    /// </summary>
    [HarmonyPatch(typeof(CompendiumSectionChecklist), "ApplyChanges")]
    public class LogbookPagePatches
    {
        private static readonly MethodInfo SetPage = AccessTools.Method(typeof(CompendiumSectionChecklist), "SetPage", [typeof(ChecklistPage)]);

        public static bool Prefix(CompendiumSectionChecklist __instance, ref IReadOnlyList<ChecklistChangeData> ___changeDatas, List<ChecklistPage> ___checklistPages, CovenantRankMeter ___covenantRankMeter, List<ChecklistWinStreakUI> ___winstreakUIs)
        {
            foreach (ChecklistChangeData change in ___changeDatas)
            {
                if (change.feature == ChecklistFeature.CovenantRank)
                {
                    ___covenantRankMeter.SetChangeData(change);
                }
                else if (change.feature == ChecklistFeature.BestWinStreak)
                {
                    ___winstreakUIs.ForEach(delegate (ChecklistWinStreakUI ui)
                    {
                        ui.SetChangeData(change);
                    });
                }
            }
            /* 
               Replaces this section of the original
              	
		       if (!checklistPages[0].HasChanges && checklistPages[1].HasChanges)
		       {
			       SetPage(checklistPages[1]);
		       }
            */
            ChecklistPage setPage = ___checklistPages[0];
            foreach (ChecklistPage checklistPage in ___checklistPages)
            {
                checklistPage.ApplyChanges(___changeDatas);
                if (checklistPage.HasChanges)
                    setPage = checklistPage;
            }
            SetPage.Invoke(__instance, [setPage]);
            ___changeDatas = [];

            return false;
        }
    }

    [HarmonyPatch(typeof(CompendiumSectionChecklist), nameof(CompendiumSectionChecklist.AnimateChangesCoroutine))]
    public class LogbookPagePatches2
    {
        public static readonly MethodInfo SetPage =
            AccessTools.Method(typeof(CompendiumSectionChecklist), "SetPage", [typeof(ChecklistPage)]);

        public static readonly MethodInfo TurnPageIfNeededCoroutine =
            AccessTools.Method(typeof(CompendiumSectionChecklist), "TurnPageIfNeededCoroutine",
                [typeof(ChecklistPage), typeof(BalanceData.ChecklistChangeAnimationTiming), typeof(int)]);

        public static readonly MethodInfo HandleChangeApplied =
            AccessTools.Method(typeof(CompendiumSectionChecklist), "HandleChangeApplied", [typeof(Vector3)]);

        static bool Prefix(CompendiumSectionChecklist __instance, ref IEnumerator __result, SaveManager ___saveManager, ref int ___sfxIndex, List<ChecklistPage> ___checklistPages, List<ChecklistWinStreakUI> ___winstreakUIs, CovenantRankMeter ___covenantRankMeter)
        {
            ___sfxIndex = 0;
            __result = Replacement(__instance, ___saveManager, ___checklistPages, ___winstreakUIs, ___covenantRankMeter);
            return false; // skip original
        }

        static IEnumerator Replacement(CompendiumSectionChecklist ui, SaveManager saveManager, List<ChecklistPage> checklistPages, List<ChecklistWinStreakUI> winstreakUIs, CovenantRankMeter covenantRankMeter)
        {
            if (saveManager == null)
                yield break;

            var timing = saveManager.GetBalanceData().GetChecklistAnimationTimingData();

            // Bind the private callback as a delegate (replace Action<T> with the real type!)
            var onApplied = MakePrivateDelegate<Action<Vector3>>(ui, HandleChangeApplied);

            // --- Fix the "problematic" initial page-selection logic ---
            // Instead of hard-coded [0]/[1], pick the first page that has changes.
            //if (!checklistPages[0].HasChanges && checklistPages[1].HasChanges)
            //{
            //    SetPage(checklistPages[1]);
            //    base.TurnPage(1);
            //    base.PageCountChangedSignal.Dispatch();
            //}
            int firstChanged = FindFirstChangedPageIndex(checklistPages);
            if (firstChanged >= 0)
            {
                var targetPage = checklistPages[firstChanged];

                // Call private SetPage(targetPage)
                SetPage.Invoke(ui, [targetPage]);

                ui.TurnPage(firstChanged);
                ui.PageCountChangedSignal.Dispatch();
            }

            // Winstreaks
            if (!winstreakUIs.IsNullOrEmpty())
            {
                foreach (var ws in winstreakUIs)
                    yield return ws.AnimateChangeCoroutine(timing, onApplied);
            }

            // Meter
            yield return covenantRankMeter.AnimateChangeCoroutine(timing, onApplied);

            // --- Rewrite to support N pages ---
            // Original code.
            // if (checklistPages[0].HasChanges)
            // {
            //     yield return TurnPageIfNeededCoroutine(checklistPages[0], timing, -1);
            //     yield return checklistPages[0].AnimateChangesCoroutine(timing, HandleChangeApplied);
            // }
            // if (checklistPages[1].HasChanges)
            // {
            //     yield return TurnPageIfNeededCoroutine(checklistPages[1], timing, 1);
            //     yield return checklistPages[1].AnimateChangesCoroutine(timing, HandleChangeApplied);
            // }
            for (int i = 0; i < checklistPages.Count; i++)
            {
                var page = checklistPages[i];
                if (page == null || !page.HasChanges)
                    continue;

                int direction = i;

                // Call private TurnPageIfNeededCoroutine(page, timing, direction) and yield it
                var turnEnum = (IEnumerator)TurnPageIfNeededCoroutine.Invoke(ui, [page, timing, direction]);

                yield return turnEnum;

                // Then animate that page
                yield return page.AnimateChangesCoroutine(timing, onApplied);
            }
        }

        static int FindFirstChangedPageIndex(List<ChecklistPage> pages)
        {
            for (int i = 0; i < pages.Count; i++)
                if (pages[i] != null && pages[i].HasChanges)
                    return i;
            return -1;
        }

        static TDelegate MakePrivateDelegate<TDelegate>(object instance, MethodInfo method)
            where TDelegate : Delegate
            => (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), instance, method);
    }
}
