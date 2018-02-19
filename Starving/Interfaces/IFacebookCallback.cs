using System;
using System.Collections.Generic;

namespace Starving.Interfaces
{
    public interface IFacebookCallback
    {
        void OnCancel();
        void OnError(Dictionary<string, object> error);
        void OnSuccess(Dictionary<string, object> result);
    }
}
