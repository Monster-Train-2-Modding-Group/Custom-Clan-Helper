using CustomClanHelper.Plugin.Code;
using HarmonyLib;

namespace CustomClanHelper.Plugin.Patches
{
    [HarmonyPatch(typeof(CompendiumSectionChecklist), "InitializeImpl")]
    class CompendiumSectionChecklist_AddChecklistPage_Patch
    {
        public static void Prefix(List<ChecklistPage> ___checklistPages, CompendiumSectionChecklist __instance)
        {
            if (___checklistPages.Count == 2)
            {
                (var checkListPage, var gameObject) = ModdedChecklistPage.Create(0, ___checklistPages[0], __instance.transform);
                ___checklistPages.Add(checkListPage);
            }
        }
    }
}
