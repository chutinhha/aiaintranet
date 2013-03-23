using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using AIA.Intranet.Resources;
using AIA.Intranet.Common.Utilities;
using System.Web.UI.WebControls;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Model;
using System.IO;

namespace AIA.Intranet.Infrastructure
{
    public class DepartmentProcessing
    {
        public static void UpdateDepartmentLink(SPWeb web, int id, string link)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.GetList(Constants.DEPARTMENT_LIST_URL);
                    SPListItem itemUpdate = list.GetItemById(id);

                    if (!string.IsNullOrEmpty(link))
                    {
                        itemUpdate["Link"] = link;
                        itemUpdate.Update();
                    }

                    web.AllowUnsafeUpdates = false;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("UpdateDepartmentLink " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static void UpdateEmployeeCode(SPWeb web, int id)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    web.AllowUnsafeUpdates = true;

                    //SPList list = web.Lists[EmployeeResources.EmployeeListName];
                    SPList list = web.GetList(Constants.EMPLOYEE_LIST_URL);
                    SPListItem itemUpdate = list.GetItemById(id);

                    if (!id.Equals(-1))
                    {
                        itemUpdate["EmployeeCode"] = id.ToString();
                        itemUpdate.Update();
                    }

                    web.AllowUnsafeUpdates = false;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("UpdateEmployeeCode " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static void UpdateEmployeeName(SPWeb web, int id, string employeeName)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.GetList(Constants.EMPLOYEE_LIST_URL);
                    SPListItem itemUpdate = list.GetItemById(id);

                    if (!string.IsNullOrEmpty(employeeName))
                    {
                        itemUpdate["EmployeeName"] = employeeName;
                        itemUpdate.Update();
                    }

                    web.AllowUnsafeUpdates = false;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("UpdateEmployeeName " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static void UpdateEmployeeMonthOfBirth(SPWeb web, int id)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.GetList(Constants.EMPLOYEE_LIST_URL);
                    SPListItem itemUpdate = list.GetItemById(id);

                    try
                    {
                        itemUpdate["MonthOfBirth"] = DateTime.Parse(itemUpdate["BirthDay"].ToString()).Month;
                        itemUpdate.Update();
                    }
                    catch { }

                    web.AllowUnsafeUpdates = false;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("UpdateEmployeeMonthOfBirth " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static void UpdateEmployeeIsDeptHead(SPWeb web, int id, bool isDeptHead)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.GetList(Constants.EMPLOYEE_LIST_URL);
                    SPListItem itemUpdate = list.GetItemById(id);

                    itemUpdate["IsDeptHead"] = isDeptHead;

                    itemUpdate.Update();

                    web.AllowUnsafeUpdates = false;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("UpdateEmployeeIsDeptHead " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static void UpdateTopEmployee(SPWeb web, int id, string employeeCode, string employeeName, string position, string department, string phoneNumber, string beginDate, string monthQuarterYear, string note)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.Lists[CommonResources.TopEmployeesListName];
                    SPListItem itemUpdate = list.GetItemById(id);

                    if (!string.IsNullOrEmpty(employeeCode))
                    {
                        itemUpdate["Title"] = employeeCode;
                    }

                    if (!string.IsNullOrEmpty(employeeName))
                    {
                        itemUpdate["EmployeeName"] = employeeName;
                    }

                    if (!string.IsNullOrEmpty(position))
                    {
                        itemUpdate["Position"] = position;
                    }

                    if (!string.IsNullOrEmpty(department))
                    {
                        itemUpdate["Department"] = department;
                    }

                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        itemUpdate["PhoneNumber"] = phoneNumber;
                    }

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        itemUpdate["BeginDate"] = DateTime.Parse(beginDate);
                    }

                    if (!string.IsNullOrEmpty(monthQuarterYear))
                    {
                        itemUpdate["MonthQuarterYear"] = monthQuarterYear;
                    }

                    if (!string.IsNullOrEmpty(note))
                    {
                        itemUpdate["Note"] = note;
                    }

                    itemUpdate.Update();

                    web.AllowUnsafeUpdates = false;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("UpdateTopEmployee " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static void AddTopEmployee(SPWeb web, string employeeCode, string employeeName, string position, string department, string phoneNumber, string beginDate, string monthQuarterYear, string note)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    web.AllowUnsafeUpdates = true;

                    SPList list = web.Lists[CommonResources.TopEmployeesListName];
                    SPListItem item = list.AddItem();

                    if (!string.IsNullOrEmpty(employeeCode))
                    {
                        item["Title"] = employeeCode;
                    }

                    if (!string.IsNullOrEmpty(employeeName))
                    {
                        item["EmployeeName"] = employeeName;
                    }

                    if (!string.IsNullOrEmpty(position))
                    {
                        item["Position"] = position;
                    }

                    if (!string.IsNullOrEmpty(department))
                    {
                        item["Department"] = department;
                    }

                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        item["PhoneNumber"] = phoneNumber;
                    }

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        item["BeginDate"] = DateTime.Parse(beginDate);
                    }

                    if (!string.IsNullOrEmpty(monthQuarterYear))
                    {
                        item["MonthQuarterYear"] = monthQuarterYear;
                    }

                    if (!string.IsNullOrEmpty(note))
                    {
                        item["Note"] = note;
                    }

                    item.Update();

                    web.AllowUnsafeUpdates = false;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("AddTopEmployee " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static SPListItemCollection ListItemCollection(SPWeb web, string listUrl)
        {
            SPListItemCollection listItemCollection = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPList spList = web.GetList(listUrl);
                    listItemCollection = spList.Items;
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("ListItemCollection" + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
            return listItemCollection;
        }

        public static void BindDataToDropDownList(SPWeb web, string listUrl, DropDownList dropDownListControl, string text, string value)
        {
            try
            {
                SPListItemCollection spListItemCollection = ListItemCollection(web, listUrl);
                if (spListItemCollection != null && spListItemCollection.Count > 0)
                {
                    foreach (SPListItem item in spListItemCollection)
                    {
                        dropDownListControl.Items.Add(new ListItem(item[text].ToString(), item[value].ToString()));
                    }
                }
            }
            catch { }
        }

        public static void CreateListByListTemplate(SPWeb web, string listTempName, string fileTempPath, string listName, string listDescription)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        web.AllowUnsafeUpdates = true;

                        SPList profileList = null;
                        try
                        {
                            profileList = web.Lists[listName];
                        }
                        catch { }

                        if (profileList == null)
                        {
                            SPList list = web.Site.GetCatalog(SPListTemplateType.ListTemplateCatalog);

                            string listTemp = string.Empty;
                            foreach (SPListItem template in list.Items)
                            {
                                if (template.Title.Equals(listTempName))
                                {
                                    listTemp = template.Name;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(listTemp))
                            {
                                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileTempPath);
                                byte[] stp = System.IO.File.ReadAllBytes(fileTempPath);
                                list.RootFolder.Files.Add(fileInfo.Name, stp);

                                CreateListByListTemplate(web, listTempName, fileTempPath, listName, listDescription);
                            }
                            else
                            {
                                web.Lists.Add(listName, listDescription, web.Site.GetCustomListTemplates(web)[listTempName]);
                            }
                        }

                        web.AllowUnsafeUpdates = false;
                    });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("CreateListByListTemplate" + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
        }

        public static Guid CreateListByListTemplate(SPWeb web, string listName)
        {
            Guid listId = Guid.Empty;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite spSite = new SPSite(web.Site.ID))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb(web.ID))
                            {
                                spWeb.AllowUnsafeUpdates = true;

                                if (spWeb.Folders.Cast<SPFolder>().Any(p => p.Name == listName))
                                {
                                    listName = listName + "_" + Path.GetRandomFileName();
                                }
                                var employeeDocumentTemplate = spWeb.ListTemplates.Cast<SPListTemplate>()
                                                                                .FirstOrDefault(p => p.Type_Client.ToString() == ListTemplateIds.EMPLOYEE_DOCUMENT_TEMPLATE_ID);
                                if (employeeDocumentTemplate != null)
                                {
                                    listId = spWeb.Lists.Add(listName, string.Empty, listName, employeeDocumentTemplate.FeatureId.ToString(), employeeDocumentTemplate.Type_Client, "100", SPListTemplate.QuickLaunchOptions.Default);
                                }

                                spWeb.Update();
                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("CreateListByListTemplate " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
            }
            return listId;
        }

        public static bool isUserInDepartment(SPWeb web, string userLoginName, string department) 
        {
            bool isUserInDepartment = false;
            try
            {
                SPList list = web.GetList(Constants.EMPLOYEE_LIST_URL);

                string caml = Camlex.Query()
                                        .Where(x => ((string)x["Account"]).Contains(userLoginName)).ToString();

                SPListItemCollection queryCollection = list.GetItems(caml);
                if (queryCollection != null && queryCollection.Count > 0)
                {
                    if (queryCollection[0]["Department"].ToString().Contains(department))
                    {
                        isUserInDepartment = true;
                    }
                }
            }
            catch { }
            return isUserInDepartment;
        }

        public static string GetManagerLoginName(SPWeb web, string department, bool isDeptHead)
        {
            string GetManagerLoginName = string.Empty;
            try
            {
                SPList list = web.GetList(Constants.DEPARTMENT_LIST_URL);

                string caml = Camlex.Query()
                                        .Where(x => (string)x["Title"] == department).ToString();

                SPListItemCollection queryCollection = list.GetItems(caml);

                if (isDeptHead)
                {
                    if (queryCollection != null && queryCollection.Count > 0)
                    {
                        GetManagerLoginName = queryCollection[0]["DeptHead"].ToString();
                    }                    
                }
                else
                {                   
                    if (queryCollection != null && queryCollection.Count > 0)
                    {
                        string[] arr = queryCollection[0]["DeputyDeptHead"].ToString().Split(';');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (i.Equals(0))
                            {
                                GetManagerLoginName += arr[0] + "#";
                            }
                            if (i % 2 == 0)
                            {
                                GetManagerLoginName += arr[i] + "#";
                            }
                            if (i.Equals(arr.Length - 1))
                            {
                                GetManagerLoginName += arr[i];
                            } 
                        }
                    } 
                }                
            }
            catch { }
            return GetManagerLoginName;
        }

        public static string GetUserFromPeoplePicker(string peoplePicker)
        {
            string pp = string.Empty;
            if (!string.IsNullOrEmpty(peoplePicker))
            {
                if (peoplePicker.Contains("#"))
                {
                    pp = peoplePicker.Split('#')[1];
                }
                else
                {
                    pp = peoplePicker;
                }
            }
            return pp;
        }

        public static bool IsActionPlanFeatureActivated(SPWeb parentWeb)
        {
            #region Variables
            bool isActivated = false;
            string featureID = Constants.Infrastructure.FeatureID;
            #endregion

            try
            {
                foreach (SPFeature feature in parentWeb.Site.Features)
                {
                    if (feature.DefinitionId.Equals(new Guid(featureID)))
                    {
                        isActivated = true;
                        break;
                    }
                    else
                    {
                        isActivated = false;
                    }
                }
            }
            catch
            {
                isActivated = false;
                // No throw exception in case of can not checking feature
            }
            return isActivated;
        }        

        //public static void AddNewTopEmployee(SPWeb web, int EmployeeId, string employee, string note, string month)
        //{
        //    try
        //    {
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            web.AllowUnsafeUpdates = true;

        //            SPList list = web.Lists[CommonResources.TopEmployeesListName];
        //            SPListItem item = list.Items.Add();

        //            item["Title"] = EmployeeId;
        //            item["Employee"] = employee;
        //            item["Note"] = note;
        //            item["Month"] = month;

        //            item.Update();

        //            web.AllowUnsafeUpdates = false;
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        CCIUtility.LogError("AddNewTopEmployee" + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
        //    }

        //}

        //public static void UpdateTopEmployee(SPWeb web, int id, int EmployeeId, string employee, string note, string month)
        //{
        //    try
        //    {
        //        SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            web.AllowUnsafeUpdates = true;

        //            SPList list = web.Lists[CommonResources.TopEmployeesListName];
        //            SPListItem item = list.GetItemById(id);

        //            if (EmployeeId != -1)
        //            {
        //                item["Title"] = EmployeeId;
        //            }

        //            if (string.IsNullOrEmpty(employee))
        //            {
        //                item["Employee"] = employee;
        //            }

        //            if (!string.IsNullOrEmpty(note))
        //            {
        //                item["Note"] = note;
        //            }

        //            if (!string.IsNullOrEmpty(month))
        //            {
        //                item["Month"] = month;
        //            }
                    
        //            item.Update();

        //            web.AllowUnsafeUpdates = false;
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        CCIUtility.LogError("AddNewTopEmployee" + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.DepartmentProcessing");
        //    }

        //}
    }
}
