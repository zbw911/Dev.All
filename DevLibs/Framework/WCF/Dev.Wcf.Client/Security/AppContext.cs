namespace Dev.Wcf.Client.Security
{

    /// <summary>
    /// 
    /// </summary>
    public static class AppContext
    {
        static private string userName;
        static private string password;

        static public  string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        static public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
