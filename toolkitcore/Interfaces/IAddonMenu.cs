﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ToolkitCore.Interfaces
{
    public interface IAddonMenu
    {
        List<FloatMenuOption> MenuOptions();
    }
}
