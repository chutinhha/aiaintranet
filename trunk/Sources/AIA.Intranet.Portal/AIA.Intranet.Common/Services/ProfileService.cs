using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities.Camlex;
using Microsoft.SharePoint;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common.Services
{
    public class ProfileService
    {
        public static UserInfomation GetUser(SPWeb web, string loginname)
        {
            SPList usersList = web.SiteUserInfoList;// Lists[Constants.USER_INFOMATION_LIS_NAME];

            CAMLListQuery<UserInfomation> query = new CAMLListQuery<UserInfomation>(usersList);

            string[] arr = loginname.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length == 2)
            {
                return query.GetItemById(int.Parse(arr[0]));
            }
            
            

            string caml = Camlex.Query()
                            .Where(x => (string)x["Name"] == loginname).ToString();

            var user = query.ExecuteSingleQuery(caml);
            return user;
        }
        /// <summary>
        /// This function are used to modify the picture .
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="url"></param>
        public static void UpdatePicture(string loginname, string url)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(url))
                {
                    using (SPWeb web = site.OpenWeb(site.RootWeb.ID))
                    {
                        try
                        {
                            web.AllowUnsafeUpdates = true;
                            SPList usersList = web.SiteUserInfoList;// Lists[Constants.USER_INFOMATION_LIS_NAME];

                            CAMLListQuery<UserInfomation> query = new CAMLListQuery<UserInfomation>(usersList);

                            string caml = Camlex.Query()
                                            .Where(x => (string)x["Name"] == loginname).ToString();


                            var user = query.GetItem(caml);
                            if (user != null)
                            {
                                user[SPBuiltInFieldId.Picture] = url;
                                user.Update();
                            }

                        }
                        catch (Exception ex)
                        {

                            CCIUtility.LogError(ex.Message + ex.StackTrace, IOfficeFeatures.IOfficeApp);
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }
        public static string GetTheme(SPUser user)
        {
            string theme = Constants.DEFAULT_THEME;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {

                    using (SPWeb web = site.OpenWeb(site.RootWeb.ID))
                    {
                        try
                        {
                            SPList usersList = web.SiteUserInfoList;//.Lists[Constants.USER_INFOMATION_LIS_NAME];

                            CAMLListQuery<UserInfomation> query = new CAMLListQuery<UserInfomation>(usersList);

                            string caml = Camlex.Query()
                                            .Where(x => (string)x["Name"] == user.LoginName).ToString();
                            
                            var infomation = query.ExecuteSingleQuery(caml);
                            if (infomation != null && !string.IsNullOrEmpty(infomation.ThemeName))
                            {
                                theme = infomation.ThemeName;
                            }

                        }
                        catch (Exception ex)
                        {

                            CCIUtility.LogError(ex.Message + ex.StackTrace, IOfficeFeatures.IOfficeApp);
                        }
                    }
                }

            });
            return theme;
        }

        /// <summary>
        /// Return current theme selected by login user
        /// </summary>
        /// <returns></returns>
        public static string GetTheme()
        {
            return GetTheme(SPContext.Current.Web.CurrentUser);
        }

        public static void SetTheme(string theme)
        {
            SetTheme(SPContext.Current.Web.CurrentUser, theme);
        }

        public static void SetTheme(SPUser user, string theme)
        {


            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {

                    using (SPWeb web = site.OpenWeb())
                    {
                        try
                        {
                            web.AllowUnsafeUpdates = true;

                            SPList usersList = web.SiteUserInfoList;//.Lists[Constants.USER_INFOMATION_LIS_NAME];

                            CAMLListQuery<UserInfomation> query = new CAMLListQuery<UserInfomation>(usersList);

                            string caml = Camlex.Query()
                                            .Where(x => (string)x["Name"] == user.LoginName).ToString();


                            var infomation = query.GetItem(caml);
                            if (infomation != null)
                            {
                                infomation[Constants.THEME_NAME_FIELD] = theme;
                                infomation.SystemUpdate();
                            }

                        }
                        catch (Exception ex)
                        {

                            CCIUtility.LogError(ex.Message + ex.StackTrace, IOfficeFeatures.IOfficeApp);
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }


                    }
                }

            });

        }

        public static void UpdateLastVisit()
        {
           var user =  SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {

                    using (SPWeb web = site.OpenWeb())
                    {
                        try
                        {
                            web.AllowUnsafeUpdates = true;

                            SPList usersList = web.SiteUserInfoList;//.Lists[Constants.USER_INFOMATION_LIS_NAME];

                            CAMLListQuery<UserInfomation> query = new CAMLListQuery<UserInfomation>(usersList);

                            string caml = Camlex.Query()
                                            .Where(x => (string)x["Name"] == user.LoginName).ToString();


                            var infomation = query.GetItem(caml);
                            if (infomation != null)
                            {
                                infomation[Constants.LAST_VISIT_FIELD_NAME] = DateTime.Now;
                                infomation.SystemUpdate();
                            }

                        }
                        catch (Exception ex)
                        {

                            CCIUtility.LogError(ex.Message + ex.StackTrace, IOfficeFeatures.IOfficeApp);
                        }
                        finally
                        {
                            web.AllowUnsafeUpdates = false;
                        }


                    }
                }

            });
        }

        public static int CountOnlineUsers(double interval)
        {
            int count = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {

                    using (SPWeb web = site.OpenWeb(site.RootWeb.ID))
                    {
                        try
                        {
                            SPList usersList = web.SiteUserInfoList;//.Lists[Constants.USER_INFOMATION_LIS_NAME];

                            CAMLListQuery<UserInfomation> query = new CAMLListQuery<UserInfomation>(usersList);

                            string caml = Camlex.Query()
                                            .Where(x => (DateTime)x[Constants.LAST_VISIT_FIELD_NAME] > DateTime.Now.AddMinutes(0- interval).IncludeTimeValue()).ToString();

                            var list = query.ExecuteListQuery(caml);
                            count= list.Count;

                        }
                        catch (Exception ex)
                        {

                            CCIUtility.LogError(ex.Message + ex.StackTrace, IOfficeFeatures.IOfficeApp);
                        }
                    }
                }

            });

            return count;
        }
    }
}
