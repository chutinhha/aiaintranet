using System;
using System.Web.UI;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Helpers;
using Microsoft.SharePoint;
using AIA.Intranet.Model;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.Pages
{
    public class TaskRulePage : LayoutsPageBase
    {
        #region Fields
        private SPContentType _contentType = null;
        private SPList _list = null;
        private SPListItem _listItem = null;
        private int _listItemId = -1;
        private string _sessionKey = string.Empty;
        #endregion

        #region Properties
        protected string SessionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_sessionKey))
                    _sessionKey = RequestHelper.GetString("skey", string.Empty);
                return _sessionKey;
            }
        }

        protected bool FormReadOnly
        {
            get
            {
                return bool.Parse(RequestHelper.GetString("read", string.Empty));
            }
        }

        protected TaskRuleMode FormMode
        {
            get
            {
                string mode = (RequestHelper.GetString("mode", "0"));
                return (TaskRuleMode)Enum.Parse(typeof(TaskRuleMode), mode);
            }
        }


        protected SPContentType CurrentContentType
        {
            get
            {
                if (_contentType == null)
                {
                    string contentTypeId = RequestHelper.GetString("ctype", string.Empty);
                    _contentType = SPContext.Current.Web.FindContentType(new SPContentTypeId(contentTypeId));
                }
                return _contentType;
            }
        }

        protected SPList CurrentList
        {
            get
            {
                if (_list == null)
                {
                    string listId = RequestHelper.GetString("List", string.Empty);
                    _list = SPContext.Current.Web.Lists.GetList(new Guid(listId), false);
                }
                return _list;
            }
        }

        protected SPListItem CurrentListItem
        {
            get
            {
                if (_listItem == null)
                {
                    if (CurrentList != null && ListItemId > 0)
                        _listItem = CurrentList.GetItemById(ListItemId);
                }
                return _listItem;
            }
        }

        protected int ListItemId
        {
            get
            {
                if (_listItemId == -1)
                {
                    string strItemId = RequestHelper.GetString("ID", string.Empty);
                    if (!string.IsNullOrEmpty(strItemId))
                    {
                        try
                        {
                            _listItemId = Convert.ToInt32(strItemId);
                        }
                        catch { }
                    }
                }
                return _listItemId;
            }
        }
        #endregion

        #region Methods
        public virtual void TaskRuleEditor_AfterSaveSuccess()
        {
            string script = string.Empty;
            script = "<script language='javascript'>";
            script += "    function CallbackAndClose(){";
            script += "        setModalDialogReturnValue(parent.window, '');\r\n                   ";
            script += "        parent.window.close();\r\n   ;";
            script += "    }";
            script += "    if (window.addEventListener){";
            script += "        window.addEventListener('load', CallbackAndClose, false);";
            script += "    }";
            script += "    else if (window.attachEvent){";
            script += "        window.attachEvent('onload', CallbackAndClose)";
            script += "    }";
            script += "</script>";
            this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "CallbackAndClose", script);
        }
        #endregion
    }
}
