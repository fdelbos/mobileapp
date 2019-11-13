using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Toggl.Shared;

namespace Toggl.Core.Interactors.Timezones
{
    public sealed class GetSupportedTimezonesInteractor : IInteractor<List<string>>
    {
        public List<string> Execute()
        {
            string json = Resources.TimezonesJson;
            return JsonConvert.DeserializeObject<List<string>>(json);
        }
    }
}
