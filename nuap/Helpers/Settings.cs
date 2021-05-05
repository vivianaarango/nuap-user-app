namespace nuap.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        const string token = "Token";
        const string id = "Id";
        const string userType = "UserType";
        const string phone = "Phone";
        const string email = "Email";
        const string cartList = "CartList";
        static readonly string stringDefault = string.Empty;

        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault(token, null);
            }
            set
            {
                AppSettings.AddOrUpdateValue(token, value);
            }
        }

        public static string Id
        {
            get
            {
                return AppSettings.GetValueOrDefault(id, null);
            }
            set
            {
                AppSettings.AddOrUpdateValue(id, value);
            }
        }

        public static string UserType
        {
            get
            {
                return AppSettings.GetValueOrDefault(userType, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userType, value);
            }
        }

        public static string Phone
        {
            get
            {
                return AppSettings.GetValueOrDefault(phone, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(phone, value);
            }
        }

        public static string Email
        {
            get
            {
                return AppSettings.GetValueOrDefault(email, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(email, value);
            }
        }

        public static string CartList
        {
            get
            {
                return AppSettings.GetValueOrDefault(cartList, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(cartList, value);
            }
        }
    }
}