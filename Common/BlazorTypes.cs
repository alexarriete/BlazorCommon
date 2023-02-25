using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public enum MessageType
    {
        error,
        warning,
        success,
        info
        
    }
    public enum ModalType
    {
        AcceptDecline,
        GetSingleText,
        Alert
    }

    public enum TabType
    {
        Text,
        HtmlText,
        Grid
    }
}
