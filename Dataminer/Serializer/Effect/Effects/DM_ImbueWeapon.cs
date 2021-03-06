﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dataminer
{
    public class DM_ImbueWeapon : DM_ImbueObject
    {
        public Weapon.WeaponSlot Imbue_Slot;

        public override void SerializeEffect<T>(T effect, DM_Effect holder)
        {
            base.SerializeEffect(effect, holder);

            Imbue_Slot = (effect as ImbueWeapon).AffectSlot;
        }
    }
}
