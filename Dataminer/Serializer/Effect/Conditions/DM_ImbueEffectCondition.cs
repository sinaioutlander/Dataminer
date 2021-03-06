﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dataminer
{
    public class DM_ImbueEffectCondition : DM_EffectCondition
    {
        public int ImbuePresetID;
        public bool AnyImbue;
        public Weapon.WeaponSlot WeaponToCheck;

        public override void SerializeEffect<T>(EffectCondition component, T template)
        {
            var comp = component as ImbueEffectCondition;
            var holder = template as DM_ImbueEffectCondition;

            holder.AnyImbue = comp.AnyImbue;
            holder.WeaponToCheck = comp.WeaponToCheck;
            holder.ImbuePresetID = comp.ImbueEffectPreset?.PresetID ?? -1;
        }
    }
}
