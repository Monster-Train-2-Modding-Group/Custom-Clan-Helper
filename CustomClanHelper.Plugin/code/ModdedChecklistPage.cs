using ShinyShoe;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CustomClanHelper.Plugin.Code
{
    public sealed class ModdedChecklistPage : ChecklistPage
    {
        public LayoutGroup? clanSectionsLayoutAllClans;
        private List<ClanChecklistSection>? clanChecklistSections;
        private int page = 0;

        internal static (ChecklistPage, GameObject) Create(int page, ChecklistPage vanillaPage, Transform parent)
        {
            GameObject checklistPageGO = new()
            {
                name = $"Modded checklist page {page}"
            };

            var checklistPage = checklistPageGO.AddComponent<ModdedChecklistPage>();
            checklistPage.page = page;
            var rectTransform = checklistPageGO.AddComponent<RectTransform>();
            rectTransform.SetParent(parent);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(400, 0);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(-400, 0);
            
            GameObject layout = new()
            {
                name = "Layout"
            };
            layout.SetActive(true);
            layout.transform.localPosition = new Vector3(0, 45, 0);
            
            var gridLayout = layout.AddComponent<GridLayoutGroup>();
            gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            gridLayout.cellSize = new Vector2(710, 157);
            gridLayout.spacing = new Vector2(20, 14);
            gridLayout.constraint = GridLayoutGroup.Constraint.Flexible;
            gridLayout.constraintCount = 2;
            gridLayout.childAlignment = TextAnchor.UpperLeft;
            var rTransform = layout.GetComponent<RectTransform>();
            rTransform.SetParent(checklistPageGO.transform);
            rTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rTransform.offsetMin = new Vector2(-720, -378);
            rTransform.offsetMax = new Vector2(720, 468);
            rTransform.sizeDelta = new Vector2(1440, 846);

            checklistPage.clanSectionsLayoutAllClans = gridLayout;

            GameObject template = new();
            var prefab = vanillaPage.transform.Find("All launch clans layout/Clan checklist section all clans")?.gameObject;
            if (prefab == null)
            {
                Plugin.Logger.LogError("Failed to find ChecklistPage item to copy");
            }
            else
            {
                
                //template.CopyPrefabToObject(prefab);
                template = GameObject.Instantiate(prefab);
                template.name = "Clan checklist section";
                template.SetActive(true);
                var ccsection = template.GetComponent<ClanChecklistSection>();
                template.GetComponent<RectTransform>().SetParent(layout.transform);
            }

            return (checklistPage, checklistPageGO);
        }

        public override void Initialize(SaveManager saveManager)
        {
            List<ClassData> list = [];
            foreach (ClassData item in saveManager.GetBalanceData().GetClassDatas())
            {
                if (item.IsModdedClan())
                    list.Add(item);
            }
            if (!list.IsNullOrEmpty())
            {
                clanSectionsLayoutAllClans!.gameObject.SetActive(value: true);
                clanChecklistSections = GameObjectUtil.PopulateViewList<ClassData, ClanChecklistSection, SaveManager>(clanSectionsLayoutAllClans, list, saveManager);
                foreach (ClanChecklistSection clanChecklistSection in clanChecklistSections)
                {
                    clanChecklistSection.ReparentCrewVictoryItems();
                }
            }
            else
            {
                clanSectionsLayoutAllClans!.gameObject.SetActive(value: false);
            }
        }

        public override bool ApplyChanges(IReadOnlyList<ChecklistChangeData> changeDatas)
        {
            base.HasChanges = false;
            foreach (ChecklistChangeData changeData in changeDatas)
            {
                if (changeData.feature == ChecklistFeature.CardsMastered && changeData.mainClassId == null && changeData.dlc == DLC.None)
                {
                    base.HasChanges = true;
                }
            }
            foreach (ClanChecklistSection clanChecklistSection in clanChecklistSections!)
            {
                base.HasChanges |= clanChecklistSection.SetChangeData(changeDatas);
            }
            return base.HasChanges;
        }

        public override IEnumerator AnimateChangesCoroutine(BalanceData.ChecklistChangeAnimationTiming timing, Action<Vector3> handleChangeApplied)
        {
            if (clanChecklistSections.IsNullOrEmpty())
            {
                yield break;
            }
            foreach (ClanChecklistSection clanChecklistSection in clanChecklistSections!)
            {
                yield return clanChecklistSection.AnimateChangeCoroutine(timing, handleChangeApplied);
            }
        }

        public override IGameUIComponent? GetDefaultGameUISelectable()
        {
            return clanChecklistSections!.GetValueOrDefault(0)?.GetDefaultGameUISelectable();
        }
    }
}
