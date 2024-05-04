using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarUI.Interfaces
{
    public interface PersonalizationInterface
    {
        // Window that allows user to change color of the app 

        void ChangeBackground(System.Windows.Media.Color color);

        void ChangeFontColor(System.Windows.Media.Color color);

        void ChangeBorderColor(System.Windows.Media.Color color);

        void ChangeForegroundColor(System.Windows.Media.Color color);

        void LoadPersonalization();


    }
}
