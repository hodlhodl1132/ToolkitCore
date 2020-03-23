using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ToolkitCore.Utilities
{
    public static class TCText
    {
        public static string BigText(string str)
        {
            return $"<size=32>{str}</size>";
        }

        public static string ColoredText(string str, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{str}</color>";
        }
    }
}
