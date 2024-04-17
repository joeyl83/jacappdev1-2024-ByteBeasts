using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarUI
{
    public interface CategoriesViewInterface
    {
        public void AddCategory();

        public void ShowError(string message);
    }
}
