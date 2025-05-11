using System.Windows.Controls;
using kursovoiproekt.Services;

namespace kursovoiproekt.Services
{
    public class NavigationService
    {
        private Frame _mainFrame;

        public void Initialize(Frame frame)
        {
            _mainFrame = frame;
        }

        public void Navigate(Page page)
        {
            _mainFrame.Navigate(page);
        }

        public void GoBack()
        {
            if (_mainFrame.CanGoBack)
                _mainFrame.GoBack();
        }
    }
}