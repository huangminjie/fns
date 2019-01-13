using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.Global;
using Microsoft.Extensions.Options;

namespace fns.Utils
{
    public class AppsettingsServices
    {
        private readonly IOptions<AppSettings> _appSettingsConfiguration;
        public AppsettingsServices(IOptions<AppSettings> appSettingsConfiguration)
        {
            _appSettingsConfiguration = appSettingsConfiguration;
        }

        public AppSettings AppSettingsConfigurations
        {
            get
            {
                return _appSettingsConfiguration.Value;
            }
        }
    }
}
