﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    public enum CtrlFlag
    {
        StartCCcomp=2000,
        SingleContour=1100,
		StartContour=1000,
		EndContour=100,
		EndCComp=200,
        FirstMove=500,
		EndOfFile=999,
        Unknown=0
    }
}
