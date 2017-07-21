﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Projectiles
{
    class BallistaSpear : Projectile
    {
        void Awake()
        {
            SetProjectileForce(1000);
            isDestroyableAfterImpact = false;
        }
    }
}
