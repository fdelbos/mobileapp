﻿using System;
using Toggl.Foundation.Services;
using Foundation;
using Toggl.Daneel.ExtensionKit;

namespace Toggl.Daneel.Services
{
    public class PrivateSharedStorageService : IPrivateSharedStorageService
    {        
        public void SaveApiToken(string apiToken)
        {
            SharedStorage.instance.setApiToken(apiToken);
        }
    }
}