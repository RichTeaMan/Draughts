using System;
using System.Collections.Generic;
using System.Text;

namespace Draughts.Web.UI.Domain
{
    public class MoveRequest
    {
        public string PlayerId { get; set; }

        public int StartX { get; set; }
        public int StartY { get; set; }

        public int EndX { get; set; }
        public int EndY { get; set; }
    }
}
