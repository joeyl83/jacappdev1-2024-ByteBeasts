﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarUI.Interfaces
{
    public interface HomePageViewInterface : PersonalizationInterface
    {
        void OpenAddEventWindow();
        void OpenAddCategoryWindow();
        void OpenPersonalizationWindow();
    }
}
