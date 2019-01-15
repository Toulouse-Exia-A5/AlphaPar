using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using AlphaParWcfServiceLibrary.Models;
using System.ServiceModel;

namespace AlphaParWcfServiceLibrary
{
    class AuthService
    {
        private static List<UserToken> userTokens = new List<UserToken>();
        private static DataAccessService db = new DataAccessService();

        public string LoginWithAD(string username, string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "alphapar.ga"))
            {
                bool isvalid = pc.ValidateCredentials(username, password);
                UserToken userToken = new UserToken(username);
                userTokens.Add(userToken);
                if (isvalid)
                {
                    db.WriteLog(userToken.username, "Login success");
                    return userToken.token;
                }
                else
                {
                    db.WriteLog(null, "Login failed");
                    //throw new Exception("Error: Could not log you in");
                    throw new FaultException<ServiceFault>(new ServiceFault("Error: Could not log you in"));
                }
            }
        }

        public string getUsernameByToken(string token)
        {
            try
            {
                UserToken userToken = getUserTokenByToken(token);
                if (userToken != null)
                    return userToken.username;
                else
                    //throw new Exception("Error: Token expired or invalid");
                    throw new FaultException<ServiceFault>(new ServiceFault("Error: Token expired or invalid"));
            }
            catch(Exception ex)
            {
                //throw new Exception("Error: Token expired or invalid");
                throw new FaultException<ServiceFault>(new ServiceFault("Error: Token expired or invalid"));
            }
        }

        private UserToken getUserTokenByToken(string token)
        {
            return userTokens.Find(x => x.token.Equals(token));
        }
    }
}
