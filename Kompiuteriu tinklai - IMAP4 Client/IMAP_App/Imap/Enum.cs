using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAP_App.Imap
{
    class Enum
    {
        public enum ImapClientState
        {
            Connected,
            NotConnected,
            Authorized,
            NotAuthorized
        }

        public enum ImapCommandState
        {
            Ok,
            No,
            Bad,
            Undefined
        }
    }
}
