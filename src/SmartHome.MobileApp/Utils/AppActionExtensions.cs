using SmartHome.MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Android.Renderscripts.ScriptGroup;

namespace SmartHome.MobileApp.Utils
{
    static partial class AppActionExtensions
    {

        [GeneratedRegex("(?<!^)(?=[A-Z])")]
        private static partial Regex CapsPattern();

        public static IEssentialsBuilder AddAppAction(this IEssentialsBuilder essentials, AppActionType actionType)
        {
            string display = string.Join(' ', CapsPattern().Split(actionType.ToString()));
            return essentials.AddAppAction(actionType.ToString(), display);
        }

        public static AppActionType GetAction(this AppAction appAction)
            => Enum.Parse<AppActionType>(appAction.Id);

    }
}
