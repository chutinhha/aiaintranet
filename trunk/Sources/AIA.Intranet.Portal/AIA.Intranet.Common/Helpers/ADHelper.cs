using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Configuration;
using System.DirectoryServices.ActiveDirectory;
using AIA.Intranet.Infrastructure.Models;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Infrastructure.Utilities;
using System.Collections;
using System.Net;


namespace AIA.Intranet.Infrastructure.ActiveDirectory
{
    public class ADHelper
    {
        private DirectoryEntry _directoryEntry = null;

        private DirectoryEntry SearchRoot
        {
            get
            {
                if (_directoryEntry == null)
                {
                    //_directoryEntry = new DirectoryEntry(LDAPPath, LDAPUser, LDAPPassword, AuthenticationTypes.Secure);
                    _directoryEntry =  CreateDictionaryEntry(LDAPPath);
                }
                return _directoryEntry;
            }
        }

        private string LDAPPath
        {
            get
            {
                string path = "LDAP://" +"S01SD-DEV17.i-office.dev/";// GetDomainName(true);
                path = "LDAP://" + GetDomainName(true);
                if (!path.EndsWith("/")) path += "/";
                //ADSetting settings = SharepointHelper.GetADSettingOfCurrentWeb();
                string currentOU = SharepointHelper.GetCurrentOU();
                //if(string.IsNullOrEmpty(currentOU)) currentOU ="users";
                //path += "/ou=" + currentOU + ",";
                path += GetDC();
                return path;
            }
        }

        private string LDAPPathWithOutOU
        {
            get
            {
                string path = "LDAP://" + GetDomainName();
                return path;
            }
        }

        private string GetDomainName()
        {
            return GetDomainName(false);
        }

        public string OuPath
        {
            get
            {
                string path = string.Empty;
                string currentOU = SharepointHelper.GetCurrentOU();
                path += "ou=" + currentOU + ",";
                path += GetDC();
                return path;
            }
        }

        private string LDAPUser
        {
            get
            {
                ADSetting settings = SharepointHelper.GetADSetting();
                if (settings != null)
                {
                    return settings.UserName;
                }
                return string.Empty;
            }
        }

        private string LDAPPassword
        {
            get
            {
                ADSetting settings = SharepointHelper.GetADSetting();
                if (settings != null)
                {
                    return settings.Password;
                }
                return string.Empty;
            }
        }

        private string LDAPDomain
        {
            get
            {
                //return ConfigurationManager.AppSettings["LDAPDomain"];
                return "";
            }
        }

        private string GetDC()
        {
            string dcStr = string.Empty;
            string domainName = GetDomainName();
            string[] dcs = domainName.Split('.');
            for (int i = 0; i < dcs.Length; i++)
            {
                dcStr += "dc=" + dcs[i];
                if (i < dcs.Length - 1)
                {
                    dcStr += ",";
                }
            }
            return dcStr;
        }

      

        private DirectoryEntry CreateDictionaryEntry(string path)
        {
            ADSetting settings = SharepointHelper.GetADSetting();
            DirectoryEntry entry = default(DirectoryEntry);
            if (settings.IsImpersonate)
            {
                entry = new DirectoryEntry(path);
            }
            else
            {
                //entry = new DirectoryEntry(path,settings.UserName, settings.Password,AuthenticationTypes.Secure);
                entry = new DirectoryEntry(path, settings.UserName, settings.Password, AuthenticationTypes.Secure);
            }
            return entry;
        }

        public string GetDomainName( bool ipAddress)
        {
            string domainName = string.Empty;
            try
            {
                
                Domain objDomain = Domain.GetCurrentDomain();
                domainName = objDomain.Name;
                if (ipAddress)
                {
                    DirectoryContext mycontext = new DirectoryContext(DirectoryContextType.Domain, domainName);
                    DomainController dc = DomainController.FindOne(mycontext);
                    IPAddress DCIPAdress = IPAddress.Parse(dc.IPAddress);
                    System.Net.IPHostEntry ip =System.Net.Dns.GetHostEntry(dc.IPAddress);
                    string hostname = ip.HostName;

                    return hostname;
                    domainName = ip.HostName + "." + domainName;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
            }
            return domainName;
        }

        public bool IsUserAuthenticate(string userName, string password)
        {
            bool authenticated = false;
            try
            {
                string path = "LDAP://" + GetDomainName();
                DirectoryEntry entry = new DirectoryEntry(path, userName, password);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (DirectoryServicesCOMException cex)
            {
                //not authenticated; reason why is in cex 
                System.Diagnostics.Debug.Write(cex);
            }
            catch (Exception ex)
            {
                //not authenticated due to some other exception [this is optional] 
                System.Diagnostics.Debug.Write(ex);
            }
            return authenticated;
        }

        internal ADUser GetUserByFullName(String userName)
        {
            try
            {
                _directoryEntry = null;
                DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                directorySearch.Filter = "(&(objectClass=user)(cn=" + userName + "))";
                SearchResult results = directorySearch.FindOne();

                if (results != null)
                {
                    DirectoryEntry user = CreateDictionaryEntry(results.Path);
                    return ADUser.GetUser(user);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ADUser GetUserByLoginName(String userName)
        {
            try
            {
                _directoryEntry = null;
                DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
                SearchResult results = directorySearch.FindOne();

                if (results != null)
                {
                    DirectoryEntry user = CreateDictionaryEntry(results.Path);
                    return ADUser.GetUser(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// This function will take a DL or Group name and return list of users
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public List<ADUser> GetUserFromGroup(string groupName)
        {
            List<ADUser> userlist = new List<ADUser>();
            try
            {
                _directoryEntry = null;
                DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + groupName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results != null)
                {

                    //DirectoryEntry deGroup = new DirectoryEntry(results.Path, LDAPUser, LDAPPassword);
                    DirectoryEntry deGroup = CreateDictionaryEntry(results.Path);
                    System.DirectoryServices.PropertyCollection pColl = deGroup.Properties;
                    int count = pColl["member"].Count;


                    for (int i = 0; i < count; i++)
                    {
                        string respath = results.Path;
                        string[] pathnavigate = respath.Split("CN".ToCharArray());
                        respath = pathnavigate[0];
                        string objpath = pColl["member"][i].ToString();
                        string path = respath + objpath;


                        //DirectoryEntry user = new DirectoryEntry(path, LDAPUser, LDAPPassword);
                        DirectoryEntry user = CreateDictionaryEntry(path);
                        ADUser userobj = ADUser.GetUser(user);
                        userlist.Add(userobj);
                        user.Close();
                    }
                }
                return userlist;
            }
            catch (Exception ex)
            {
                return userlist;
            }

        }

        public List<ADUser> GetUsersByFirstName(string fName)
        {

            //UserProfile user;
            List<ADUser> userlist = new List<ADUser>();
            string filter = "";

            _directoryEntry = null;
            DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
            directorySearch.Asynchronous = true;
            directorySearch.CacheResults = true;
            filter = string.Format("(givenName={0}*", fName);
            //  filter = "(&(objectClass=user)(objectCategory=person)(givenName="+fName+ "*))";


            directorySearch.Filter = filter;

            SearchResultCollection userCollection = directorySearch.FindAll();
            foreach (SearchResult users in userCollection)
            {
                DirectoryEntry userEntry = CreateDictionaryEntry(users.Path);
                ADUser userInfo = ADUser.GetUser(userEntry);

                userlist.Add(userInfo);

            }

            directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + fName + "*))";
            SearchResultCollection results = directorySearch.FindAll();
            if (results != null)
            {

                foreach (SearchResult r in results)
                {
                    DirectoryEntry deGroup = CreateDictionaryEntry(r.Path);

                    ADUser agroup = ADUser.GetUser(deGroup);
                    userlist.Add(agroup);
                }

            }
            return userlist;
        }



        public List<ADGroup> GetGroups()
        {
            string strLDAPPath = this.LDAPPath;
            //string strLDAPPath = "LDAP://sharepoint.com";

            DirectoryEntry objADAM = default(DirectoryEntry);
            // Binding object.  
            DirectoryEntry objGroupEntry = default(DirectoryEntry);
            // Group Results.  
            DirectorySearcher objSearchADAM = default(DirectorySearcher);
            // Search object.  
            SearchResultCollection objSearchResults = default(SearchResultCollection);

            List<ADGroup> result = new List<ADGroup>();


            //objADAM = new DirectoryEntry(strLDAPPath);

            objADAM = CreateDictionaryEntry(strLDAPPath);
            try
            {
                objADAM.RefreshCache();
            }
            catch (Exception ex)
            {

            }
            


            // Get search object, specify filter and scope,  
            // perform search.  

            objSearchADAM = new DirectorySearcher(objADAM);
            objSearchADAM.Filter = "(&(objectClass=group))";
            objSearchADAM.SearchScope = SearchScope.Subtree;
            objSearchResults = objSearchADAM.FindAll();


            ADGroup group = null;
            // Enumerate groups  
            try
            {
                foreach (SearchResult objResult in objSearchResults)
                {
                    objGroupEntry = objResult.GetDirectoryEntry();
                    group = new ADGroup()
                    {
                        Name = objGroupEntry.Properties["cn"].Value.ToString(),
                        Description = objGroupEntry.Properties["description"].Value != null ? objGroupEntry.Properties["description"].Value.ToString() : string.Empty,
                        Path = objGroupEntry.Properties[ADProperties.DISTINGUISHEDNAME].Value.ToString(),
                        LoginName = objGroupEntry.Properties[ADProperties.LOGINNAME].ToString()
                    };
                    result.Add(group);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }

            return result;
        }


        public string GetUserPath(string userName, string password)
        {
            string path = string.Empty;

            DirectoryEntry de = new DirectoryEntry(this.LDAPPathWithOutOU, userName, password);
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if (!(results == null))
            {
                path = results.Path;
            }
            return path;
        }


        public void ChangePassword(string adPath, string userName, string oldPassword, string newPassword)
        {

            try
            {
                System.Diagnostics.Debug.Write(adPath);
                System.Diagnostics.Debug.Write(userName);
                DirectoryEntry oDE;
                oDE = new DirectoryEntry(adPath, userName, oldPassword);
                //oDE.Invoke("ChangePassword", new object[] { oldPassword, newPassword });
                oDE.Invoke("ChangePassword", oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                string exceptionMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    int endMsg = ex.InnerException.Message.IndexOf("(Exception from HRESULT");
                    if (endMsg > 0)
                    {
                        exceptionMsg = ex.InnerException.Message.Substring(0, endMsg);
                    }
                    else
                    {
                        exceptionMsg = ex.InnerException.Message;
                    }
                    //exceptionMsg = ex.InnerException.Message;
                }
                throw new Exception(exceptionMsg);
            }
        }

        public List<ADUser> GetUsers()
        {
            string strLDAPPath = this.LDAPPath;

            DirectoryEntry objADAM = default(DirectoryEntry);
            // Binding object.  
            DirectoryEntry objGroupEntry = default(DirectoryEntry);
            // Group Results.  
            DirectorySearcher objSearchADAM = default(DirectorySearcher);
            //objSearchADAM.ReferralChasing= ReferralChasingOption.All;
            // Search object.  
            SearchResultCollection objSearchResults = default(SearchResultCollection);

            List<ADUser> result = new List<ADUser>();

            try
            {
                objADAM = CreateDictionaryEntry(strLDAPPath);
                objADAM.RefreshCache();
            }
            catch (Exception e)
            {
                throw e;
            }

            // Get search object, specify filter and scope,  
            // perform search.  
            try
            {
                objSearchADAM = new DirectorySearcher(objADAM);
                objSearchADAM.ReferralChasing = ReferralChasingOption.All;
                objSearchADAM.Filter = "(&(objectClass=user))";
                objSearchADAM.SearchScope = SearchScope.Subtree;
                objSearchResults = objSearchADAM.FindAll();
            }
            catch (Exception e)
            {
                throw e;
            }

            ADUser user = null;
            // Enumerate groups  
            try
            {
                foreach (SearchResult objResult in objSearchResults)
                {
                    objGroupEntry = objResult.GetDirectoryEntry();
                    user = ADUser.GetUser(objGroupEntry);
                    result.Add(user);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }


        public ADUser GetUser(string userDn)
        {
            ADUser user = null;
            DirectoryEntry child = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" + userDn);
            user = ADUser.GetUser(child);
            return user;
        }

        public string CreateUserAccount(ADUser user)
        {
            string ldapPath = this.LDAPPath;
            string oGUID = string.Empty;
            try
            {
                
                string connectionPrefix = ldapPath;
                DirectoryEntry dirEntry = CreateDictionaryEntry(connectionPrefix);
                DirectoryEntry newUser = dirEntry.Children.Add("CN=" + user.DisplayName, "user");
                
                newUser.Properties[ADProperties.LOGINNAME].Value = user.LoginName;
                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    newUser.Properties[ADProperties.FIRSTNAME].Value = user.FirstName;
                }
                if (!string.IsNullOrEmpty(user.LastName))
                {
                    newUser.Properties[ADProperties.LASTNAME].Value = user.LastName;
                }
                if (!string.IsNullOrEmpty(user.DisplayName))
                {
                    newUser.Properties[ADProperties.DISPLAYNAME].Value = user.DisplayName;
                }
                if (!string.IsNullOrEmpty(user.Office))
                {
                    newUser.Properties[ADProperties.PHYSICALDELIVERYOFFICENAME].Value = user.Office;
                }
                if (!string.IsNullOrEmpty(user.EmailAddress))
                {
                    newUser.Properties[ADProperties.EMAILADDRESS].Value = user.EmailAddress;
                }
                

                newUser.CommitChanges();
                oGUID = newUser.Guid.ToString();


                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    
                    try
                    {
                        newUser.Invoke("SetPassword", new object[] { user.Password });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Write("---inner: " + ex.InnerException.Message);
                        if (ex.InnerException != null)
                        {
                            System.Diagnostics.Debug.Write("---inner: " + ex.InnerException.Message);
                        }
                        throw;
                    }
                });
                
                
                if (user.IsPassNeverExpired)
                {
                    int val = (int)newUser.Properties["userAccountControl"].Value;
                    newUser.Properties["userAccountControl"].Value = val | 0x80000;
                }
                if (user.IsCannotChagnePass)
                {
                    int val = (int)newUser.Properties["userAccountControl"].Value;
                    newUser.Properties["userAccountControl"].Value = val | 0x0040;
                }
                if (user.IsAccountIsDisable)
                {
                    int val = (int)newUser.Properties["userAccountControl"].Value;
                    newUser.Properties["userAccountControl"].Value = val | 0x0002;
                }
                else
                {
                    int val = (int)newUser.Properties["userAccountControl"].Value;
                    newUser.Properties["userAccountControl"].Value = val & ~0x2;
                }
                if (user.IsRequireChangePass)
                {
                    newUser.Properties["pwdLastSet"].Value = -1;
                }
                

                newUser.CommitChanges();
                dirEntry.Close();
                newUser.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.ObjectAlreadyExist:
                        throw new Exception(ErrorMessage.UserAlreadyExist);
                    default:
                        throw ex;
                }

            }
            return oGUID;
        }

        public void UpdateUserAccount(ADUser user)
        {
            DirectoryEntry adUser = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" + user.Path);

            SetValueForDirectoryEntry(ADProperties.LOGINNAME, adUser, user.LoginName);
            SetValueForDirectoryEntry(ADProperties.FIRSTNAME, adUser, user.FirstName);
            SetValueForDirectoryEntry(ADProperties.LASTNAME, adUser, user.LastName);
            SetValueForDirectoryEntry(ADProperties.DISPLAYNAME, adUser, user.DisplayName);
            SetValueForDirectoryEntry(ADProperties.PHYSICALDELIVERYOFFICENAME, adUser, user.Office);
            SetValueForDirectoryEntry(ADProperties.EMAILADDRESS, adUser, user.EmailAddress);

            adUser.CommitChanges();
            adUser.Close();
        }

        private void SetValueForDirectoryEntry(string propertyName, DirectoryEntry entry, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                entry.Properties[propertyName].Value = value;
            }
            else
            {
                entry.Properties[propertyName].Clear();
            }
        }

        public void CreateGroup(ADGroup group)
        {
            string ouPath = this.OuPath;

            //if (!DirectoryEntry.Exists("LDAP://"+GetDomainName()+"/CN=" + group.Name + "," + ouPath))
            //{
                try
                {
                    DirectoryEntry entry = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" +ouPath);
                    DirectoryEntry aDGroup = entry.Children.Add("CN=" + group.Name, "group");


                    aDGroup.Properties["sAmAccountName"].Value = group.Name;
                    if (!string.IsNullOrEmpty(group.Description))
                    {
                        aDGroup.Properties["description"].Value = group.Description;
                    }
                    
                    aDGroup.CommitChanges();
                }
                catch (System.DirectoryServices.DirectoryServicesCOMException ex)
                {
                    switch (ex.ErrorCode)
                    {
                        case ErrorCodes.ObjectAlreadyExist:
                            throw new Exception(ErrorMessage.GroupAlreadyExist);
                        default:
                            throw ex;
                    }
                }
            //}
            //else { Console.WriteLine(group.Name + " already exists"); }
        }

        public void Delete(string objectPath)
        {
            string ouPath = this.OuPath;

            try
            {
                string domainName = GetDomainName();
                DirectoryEntry entry = CreateDictionaryEntry("LDAP://" + domainName + "/" + ouPath);
                DirectoryEntry objectToDelete = CreateDictionaryEntry("LDAP://" + domainName + "/" + objectPath);
                entry.Children.Remove(objectToDelete);
                objectToDelete.CommitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        public void AddUserToGroup(string userDn, string groupDn)
        {
            try
            {
                DirectoryEntry dirEntry = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/"  + groupDn);
                dirEntry.Properties["member"].Add(userDn);
                dirEntry.CommitChanges();
                dirEntry.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }
        }

        public void RemoveUserFromGroup(string userDn, string groupDn)
        {
            try
            {
                DirectoryEntry dirEntry = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" + groupDn);
                dirEntry.Properties["member"].Remove(userDn);
                dirEntry.CommitChanges();
                dirEntry.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException ex)
            {
                System.Diagnostics.Debug.Write(ex);

            }
        }

        public void RenameGroup(string objectDn, string newName)
        {
            DirectoryEntry child = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" + objectDn);
            child.Rename("CN=" + newName);
            child.CommitChanges();
            child.Close();
        }

        public void UpdateGroup(ADGroup group)
        {
            DirectoryEntry child = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" + group.Path);
            //child.Properties["cn"].Value = group.Name;
            child.Rename("CN=" + group.Name);

            SetValueForDirectoryEntry("description", child, group.Description);
            
            child.CommitChanges();
            child.Close();
        }

        public ADGroup GetGroup(string groupDn)
        {
            ADGroup group = new ADGroup();
            DirectoryEntry child = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" + groupDn);
            group.Name = GetADProperty(child,"cn");
            group.Description = GetADProperty(child,"description");
            group.Path = groupDn;
            group.LoginName = GetADProperty(child, ADProperties.LOGINNAME);
            return group;
        }

        private string GetADProperty(DirectoryEntry entry, string property)
        {
            string result = string.Empty;
            if (entry.Properties[property].Value != null)
            {
                result = entry.Properties[property].Value.ToString();
            }
            return result;
        }

        public ArrayList AttributeValuesMultiString(string attributeName, string objectDn, ArrayList valuesCollection, bool recursive)
        {
            DirectoryEntry ent = CreateDictionaryEntry(objectDn);
            PropertyValueCollection ValueCollection = ent.Properties[attributeName];
            IEnumerator en = ValueCollection.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current != null)
                {
                    if (!valuesCollection.Contains(en.Current.ToString()))
                    {
                        valuesCollection.Add(en.Current.ToString());
                        if (recursive)
                        {
                            AttributeValuesMultiString(attributeName, "LDAP://" +en.Current.ToString(), valuesCollection, true);
                        }
                    }
                }
            }
            ent.Close();
            ent.Dispose();
            return valuesCollection;
        }

        public ArrayList GetGroupPathsOfUser(string userDn, bool recursive)
        {
            ArrayList groupMemberships = new ArrayList();
            string path = "LDAP://" + GetDomainName() + "/" + userDn;
            return AttributeValuesMultiString(ADProperties.MEMBEROF, path, groupMemberships, recursive);
        }

        public List<ADGroup> GetGroupsOfUser(string userDn)
        {
            List<ADGroup> groups = new List<ADGroup>();
            ArrayList groupPaths = GetGroupPathsOfUser(userDn, false);
            ADGroup group = null;
            foreach (var groupPath in groupPaths)
            {
                group = GetGroup(groupPath.ToString());
                groups.Add(group);
            }
            return groups;
        }

        public void ResetPassword(string userDn, string newPassword)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                DirectoryEntry adUser = CreateDictionaryEntry("LDAP://" + GetDomainName() + "/" + userDn);

                try
                {
                    adUser.Invoke("SetPassword", new object[] { newPassword });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write("---inner: " + ex.InnerException.Message);
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.Write("---inner: " + ex.InnerException.Message);
                    }
                    throw;
                }


                adUser.Properties["LockOutTime"].Value = 0; //unlock account

                adUser.Close();
            });

            
        }
    }
}