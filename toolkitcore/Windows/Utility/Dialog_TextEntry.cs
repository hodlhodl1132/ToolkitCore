using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ToolkitCore.Windows.Utility
{
    public class Dialog_TextEntry : Window
    {
        public Dialog_TextEntry(Action<string> onCloseAction, string title = "", string placeholder = "", bool preventEmptyOutput = true)
        {
            this.del = onCloseAction;
            this.title = title;
            this.input = placeholder;
            this.doCloseButton = true;
            this.preventEmptyOutput = preventEmptyOutput;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label(title);
            input = listing.TextEntry(input);

            listing.End();
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            if (preventEmptyOutput && input != "")
            {
                del.Invoke(input);
            }
        }

        public override Vector2 InitialSize => new Vector2(300, 140);

        string title;
        string input;
        Action<string> del;
        bool preventEmptyOutput;
    }
}
