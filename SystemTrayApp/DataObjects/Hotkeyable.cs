using System;

namespace SystemTrayApp.DataObjects
{
    [Serializable]
    public class Hotkeyable
    {
        public HotkeyableActionType Type;


        public string Topic; // for MQTT

        public string TargetUrl; // for WebAPI


        public string Method; // for WebAPI

        public string Data;

        //TODO : Make a hotkey encoder/decider.
        public string Hotkey;
    }
}
