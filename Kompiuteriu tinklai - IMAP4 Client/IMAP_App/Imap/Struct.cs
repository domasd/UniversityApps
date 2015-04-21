using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAP_App.Imap;

namespace IMAP_App.Imap
{
    struct  ImapCommandResult
    {
        public string text;
        public Enum.ImapCommandState CommandState;
    }

}
