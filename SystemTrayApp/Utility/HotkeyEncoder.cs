using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GlobalHotKey;

namespace SystemTrayApp.Utility
{
    public static class HotkeyEncoder
    {
        public const string ModKeyShift = "shift";

        public const string ModKeyControl = "ctrl";

        public const string ModKeyAlt = "alt";

        public const string ModKeyWin = "win";

        public static Dictionary<string, ModifierKeys> StringModKeyStore = new Dictionary<string, ModifierKeys>
        {
            {ModKeyShift, ModifierKeys.Shift},
            {ModKeyControl, ModifierKeys.Control},
            {ModKeyAlt, ModifierKeys.Alt},
            {ModKeyWin, ModifierKeys.Windows}
        };

        public static Dictionary<ModifierKeys, string> ModStringKeyStore = new Dictionary<ModifierKeys, string>
        {
            {ModifierKeys.Shift, ModKeyShift},
            {ModifierKeys.Control, ModKeyControl},
            {ModifierKeys.Alt, ModKeyAlt},
            {ModifierKeys.Windows, ModKeyWin}
        };

        public static string HotkeyToString(HotKey hotkey)
        {
            StringBuilder hotkeyString = new StringBuilder();
            foreach (var modKeyPair in ModStringKeyStore)
            {
                if ((hotkey.Modifiers & modKeyPair.Key) != 0)
                {
                    hotkeyString.Append(modKeyPair.Value);
                    hotkeyString.Append("-");
                }
            }

            hotkeyString.Append(hotkey.Key.ToString());
            return hotkeyString.ToString();
        }

        public static HotKey StringToHotkey(string str)
        {
            bool b = false;
            return StringToHotkey(str, out b);
        }

        public static HotKey StringToHotkey(string str, out bool success)
        {
            HotKey hk = new HotKey();

            string[] split = str.Split('-');
            string targetKey = split.First(key => !StringModKeyStore.ContainsKey(key));

            ModifierKeys mod = ModifierKeys.None;
            foreach (var modKey in split.Where(key => StringModKeyStore.ContainsKey(key)))
            {
                mod |= StringModKeyStore[modKey];
            }

            Key parsedKey = Key.None;
            Key.TryParse(targetKey, out parsedKey);

            hk.Key = parsedKey;
            hk.Modifiers = mod;

            if (parsedKey != Key.None)
            {
                success = true;
            }
            else
            {
                success = false; 
            }

            return hk;
        }
    }
}
