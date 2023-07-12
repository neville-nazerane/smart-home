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
            => essentials.AddAppAction(actionType.ToString(), actionType.GetDisplayname());

        public static AppActionType GetAction(this AppAction appAction)
            => Enum.Parse<AppActionType>(appAction.Id);

        public static string GetDisplayname(this AppActionType actionType) => string.Join(' ', CapsPattern().Split(actionType.ToString()));
    }
}
