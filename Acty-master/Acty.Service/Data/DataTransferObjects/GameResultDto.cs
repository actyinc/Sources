﻿using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Acty
{
    public partial class GameResultDto : GameResultBase
    {
        public DateTimeOffset? DateCreated
        {
            get;
            set;
        }
    }
}