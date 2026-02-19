using CustomClanHelper.Plugin.Code;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CustomClanHelper.Plugin.patches
{
    [HarmonyPatch(typeof(StandardChecklistPage), nameof(StandardChecklistPage.Initialize))]
    internal class StandardChecklistPatch
    {
        public static readonly FieldInfo ClassDataField = AccessTools.Field(typeof(ClanChecklistSection), "classData");
        public static void Postfix(List<ClanChecklistSection> ___clanChecklistSections)
        {
            foreach (var section in ___clanChecklistSections)
            {
                var classData = ClassDataField.GetValue(section) as ClassData;
                if (classData != null && classData.IsModdedClan())
                {
                    section.gameObject.SetActive(false);
                }
            }
        }
    }
}
