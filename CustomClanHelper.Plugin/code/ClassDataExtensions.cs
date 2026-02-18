using System;
using System.Collections.Generic;
using System.Text;

namespace CustomClanHelper.Plugin.Code
{
    public static class ClassDataExtensions
    {
        internal static readonly HashSet<string> VANILLA_CLANS = [
                "0df83271-5359-48df-9365-e73b7b2d0130",
                "fd119fcf-c2cf-469e-8a5a-e9b0f265560d",
                "4fe56363-b1d9-46b7-9a09-bd2df1a5329f",
                "9317cf9a-04ec-49da-be29-0e4ed61eb8ba",
                "fda62ada-520e-42f3-aa88-e4a78549c4a2",
                "46ae87db-d92e-4fcb-a3bc-67c723d7bebd",
                "9d1f0ee1-3e81-4b65-8438-aecd6db91929",
                "5be08e27-c1e6-4b9d-b506-e4781e111dc8",
                "9aaf1009-3fbe-4ac5-9f99-a3a702ff7f27",
                "c595c344-d323-4cf1-9ad6-41edc2aebbd0",
                "3c98c8eb-fc7c-4b35-925e-6b5ab0f69896",
                "ab9c9f6f-2543-4ca5-b7e5-e2eb125445b8",
        ];

        public static bool IsModdedClan(this ClassData clan)
        {
            return !VANILLA_CLANS.Contains(clan.GetID());
        }

        public static bool IsVanillaClan(this ClassData clan)
        {
            return VANILLA_CLANS.Contains(clan.GetID());
        }
    }
}
