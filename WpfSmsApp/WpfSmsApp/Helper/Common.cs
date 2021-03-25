using NLog;
using WpfSmsApp.Model;

namespace WpfSmsApp
{
    public class Common
    {
        // NLog static instance 
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();
        public static User LOGINED_USER;
    }
}
