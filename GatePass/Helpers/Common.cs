namespace GatePass.Helpers
{
    public class Common
    {
        public enum WebUserInformation
        {
            DomainName = 0,
            Username = 1
        }
        public static string GetWebCurrentUser(WebUserInformation webUserInformation)
        {
            string result = "";
            try
            {

                string[] userInformation = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.ToString().Split(@"\".ToCharArray());
                result = userInformation[(int)webUserInformation].ToString();
            }
            catch
            {
                result = "N/A";
            }
            return result;
        }
    }
}