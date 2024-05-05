using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;
using CalendarUI.Interfaces;

namespace CalendarUI
{
    public interface CategoriesViewInterface: PersonalizationInterface
    {
        public void AddCategory();

        public void ShowError(string message);

        public void LoadCategoryTypes(List<string> categoryTypes);
    }
}
