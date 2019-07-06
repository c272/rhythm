using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm
{
    /// <summary>
    /// Per-tab information class, encapsulates all information about a given tab. 
    /// </summary>
    public class EditorTabInfo
    {
        public string FileLocation = "";
        public bool SavedLatest = false;
        public string FileName = "Untitled";
    }
}
