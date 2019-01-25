using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;

namespace ADExtended
{
    [DirectoryRdnPrefix("CN")]
    [DirectoryObjectClass("Person")]
    public class UserPrincipalEx : UserPrincipal
    {
        // Inplement the constructor using the base class constructor. 
        public UserPrincipalEx(PrincipalContext context)
            : base(context)
        { }

        // Implement the constructor with initialization parameters.    
        public UserPrincipalEx(PrincipalContext context,
                             string samAccountName,
                             string password,
                             bool enabled)
            : base(context, samAccountName, password, enabled)
        { }

        // Create the "Title" property.    
        [DirectoryProperty("title")]
        public string Title
        {
          get
          {
            if (ExtensionGet("title").Length != 1)
              return string.Empty;

            return (string)ExtensionGet("title")[0];
          }
          set { ExtensionSet("title", value); }
        }

        // Create the "Telephone Number" property.    
        [DirectoryProperty("homePhone")]
        public string homePhone
        {
          get
          {
            if (ExtensionGet("homePhone").Length != 1)
              return string.Empty;

            return (string)ExtensionGet("homePhone")[0];
          }
          set { ExtensionSet("homePhone", value); }
        }

        // Implement the overloaded search method FindByIdentity.
        public static new UserPrincipalEx FindByIdentity(PrincipalContext context, string identityValue)
        {
            return (UserPrincipalEx)FindByIdentityWithType(context, typeof(UserPrincipalEx), identityValue);
        }

        // Implement the overloaded search method FindByIdentity. 
        public static new UserPrincipalEx FindByIdentity(PrincipalContext context, IdentityType identityType, string identityValue)
        {
            return (UserPrincipalEx)FindByIdentityWithType(context, typeof(UserPrincipalEx), identityType, identityValue);
        }
    }
}