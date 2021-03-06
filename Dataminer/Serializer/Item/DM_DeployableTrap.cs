﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SideLoader;

namespace Dataminer
{
    public class DM_DeployableTrap : DM_Item
    {
        public List<DM_TrapRecipe> TrapRecipeEffects = new List<DM_TrapRecipe>();

        public override void SerializeItem(Item item, DM_Item holder)
        {
            base.SerializeItem(item, holder);

            var trap = item as DeployableTrap;

            var recipes = (TrapEffectRecipe[])At.GetField(trap, "m_trapRecipes");

            foreach (var recipe in recipes)
            {
                var dmRecipe = new DM_TrapRecipe();
                dmRecipe.Serialize(recipe, trap);
                TrapRecipeEffects.Add(dmRecipe);
            }
        }
    }

    public class DM_TrapRecipe
    {
        public List<string> CompatibleItemIDs = new List<string>();
        public List<string> CompatibleItemTags = new List<string>();

        public List<DM_Effect> StandardEffects;
        public List<DM_Effect> HiddenEffects;

        public void Serialize(TrapEffectRecipe recipe, DeployableTrap trap)
        {
            var items = (Item[])At.GetField(recipe, "m_compatibleItems");
            var tags = (TagSourceSelector[])At.GetField(recipe, "m_compatibleTags");

            trap.GetCompatibleFilters();

            var compatibleTags = ((List<Tag>[])At.GetField<DeployableTrap>("COMPATIBLE_TAGS"))[(int)trap.CurrentTrapType];

            if (items != null)
            {
                foreach (var item in items)
                {
                    switch (trap.CurrentTrapType)
                    {
                        case DeployableTrap.TrapType.PressurePlateTrap:
                            if (!item.HasTag(TagSourceManager.PlateTrapComponent) && !HasAnyTag(item, compatibleTags))
                                continue;
                            break;

                        case DeployableTrap.TrapType.TripWireTrap:
                            if (!item.HasTag(TagSourceManager.TripWireTrapComponent) && !HasAnyTag(item, compatibleTags))
                                continue;
                            break;
                    }

                    this.CompatibleItemIDs.Add(item.Name);
                }
            }

            
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    this.CompatibleItemTags.Add(tag.Tag.TagName);
                }
            }

            if (recipe.TrapEffectsPrefab)
            {
                this.StandardEffects = new List<DM_Effect>();
                foreach (var effect in recipe.TrapEffectsPrefab.GetComponents<Effect>())
                {
                    this.StandardEffects.Add(DM_Effect.ParseEffect(effect));
                }
            }
            if (recipe.HiddenTrapEffectsPrefab)
            {
                this.HiddenEffects = new List<DM_Effect>();
                foreach (var effect in recipe.HiddenTrapEffectsPrefab.GetComponents<Effect>())
                {
                    this.HiddenEffects.Add(DM_Effect.ParseEffect(effect));
                }
            }
        }

        internal bool HasAnyTag(Item item, IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                if (item.HasTag(tag))
                    return true;
            }

            return false;
        }
    }
}
