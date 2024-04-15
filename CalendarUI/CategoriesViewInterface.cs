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

        public void RemoveCategory();

        public void SetError(string message);

        public void ClearError();
    }
}
