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
        info,
        ads
        
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
        Grid,
        Main
    }

    public enum LinkCollectionType
    {
        Vertical,
        Horizontal,
        ButtonHorizontal
    }
}
