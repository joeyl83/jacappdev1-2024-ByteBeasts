using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;
namespace CalendarUI
{
    class CategoriesPresenter
    {
        private readonly CategoriesViewInterface _view;
        private readonly HomeCalendar _model;
        public CategoriesPresenter(CategoriesViewInterface v, HomeCalendar model)
        {
            _view = v;
            _model = model;
        }
        public void ProcessInput(string categoryName,Category.CategoryType type)
        {
            _model.categories.Add(categoryName, type);
            _view.AddCategory();
        }
    }
}
