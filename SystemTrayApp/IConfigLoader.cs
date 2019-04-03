using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTrayApp.DataObjects;

namespace SystemTrayApp
{
    interface IConfigLoader
    {
        Configuration CreateBaseConfig();

        Configuration LoadConfig(string path);

        Configuration TryGetStoredConfig(out bool freshConfig);
    }
}
