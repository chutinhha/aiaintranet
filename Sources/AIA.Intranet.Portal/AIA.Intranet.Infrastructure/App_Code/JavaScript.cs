#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using AIA.Intranet.Common.Utilities;
#endregion

namespace AIA.Intranet.Infrastructure
{
    /// <summary>
    ///  Class for client script
    /// </summary>
    public class JavaScript
    {
        #region Variables
        private string confirmScripText = string.Empty;
        private bool isIE;
        private bool isInAsyncPostBack;
        private bool isWaitForPageLoad;
        private Page pg;
        #endregion

        #region Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScript"/> class.
        /// </summary>
        /// <param name="pageInstance">The page instance.</param>
        /// <param name="waitLoad">if set to <c>true</c> [wait load].</param>
        private JavaScript(Page pageInstance, bool waitLoad)
        {
            try
            {
                this.pg = pageInstance;
                this.isInAsyncPostBack = System.Web.UI.ScriptManager.GetCurrent(this.pg).IsInAsyncPostBack;
                this.isIE = HttpContext.Current.Request.Browser.Browser == "IE";
                this.isWaitForPageLoad = waitLoad;
            }
            catch (Exception ex)
            {

                CCIUtility.LogError("JavaScript " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.JavaScript");
                throw;
            }

        }

        /// <summary>
        /// After the page has loaded
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <returns></returns>
        public static JavaScript AfterPageLoad(Page page)
        {
            return new JavaScript(page, true);
        }

        /// <summary>
        /// Alerts the user with the message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public JavaScript Alert(string message, params object[] args)
        {
            try
            {
                if ((args != null) && (args.Length > 0))
                {
                    message = string.Format(message, args);
                }
                message = message.Replace("\r", "");
                message = message.Replace("\n", @"\n");
                message = message.Replace("'", @"\'");
                this.RegisterScript(this.GetConfirmScript() + "alert('" + message + "');");
                return this;
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("Alert " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.JavaScript");
                return null;
            }
        }

        /// <summary>
        /// Before the page loads
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public static JavaScript BeforePageLoad(Page page)
        {
            return new JavaScript(page, false);
        }

        /// <summary>
        /// Closes the popup window.
        /// </summary>
        /// <returns></returns>
        public JavaScript ClosePopupWindow()
        {
            this.RegisterScript(this.GetConfirmScript() + "window.close();");
            return this;
        }

        /// <summary>
        /// Confirms the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public JavaScript Confirm(string message, params object[] args)
        {
            this.Confirm(true, message, args);
            return this;
        }

        /// <summary>
        /// Confirms the specified boolean expr.
        /// </summary>
        /// <param name="booleanExpr">if set to <c>true</c> [boolean expr].</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        private void Confirm(bool booleanExpr, string message, params object[] args)
        {
            try
            {
                if ((args != null) && (args.Length > 0))
                {
                    message = string.Format(message, args);
                }
                message = message.Replace("\r", "");
                message = message.Replace("\n", @"\n");
                message = message.Replace("'", @"\'");
                this.confirmScripText = "if (" + (booleanExpr ? "" : "!") + "confirm('" + message + "')) ";
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("Confirm " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.JavaScript");
                throw;
            }

        }

        /// <summary>
        /// Confirms the not.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public JavaScript ConfirmNot(string message, params object[] args)
        {
            this.Confirm(false, message, args);
            return this;
        }

        /// <summary>
        /// Evals the specified script text.
        /// </summary>
        /// <param name="scriptText">The script text.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public JavaScript Eval(string scriptText, params object[] args)
        {
            try
            {
                if ((args != null) && (args.Length > 0))
                {
                    scriptText = string.Format(scriptText, args);
                }
                this.RegisterScript(this.GetConfirmScript() + "eval(" + scriptText + ");");
                return this;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Executes the custom script.
        /// </summary>
        /// <param name="scriptText">The script text.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public JavaScript ExecuteCustomScript(string scriptText, params object[] args)
        {
            try
            {
                if ((args != null) && (args.Length > 0))
                {
                    scriptText = string.Format(scriptText, args);
                }
                this.RegisterScript(scriptText);
                return this;
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("ExecuteCustomScript " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.JavaScript");
                return null;
            }

        }

        /// <summary>
        /// Gets the confirm script.
        /// </summary>
        /// <returns></returns>
        private string GetConfirmScript()
        {
            try
            {
                if (this.confirmScripText.Length > 0)
                {
                    string confirmScript = this.confirmScripText;
                    this.confirmScripText = string.Empty;
                    return confirmScript;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {

                CCIUtility.LogError("GetConfirmScript " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.JavaScript");
                return string.Empty;
            }

        }

        /// <summary>
        /// Redirects the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public JavaScript Redirect(string url, params object[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                url = string.Format(url, args);
            }
            this.RegisterScript(this.GetConfirmScript() + "window.location.replace('" + url.Replace("'", @"\'") + "');");
            return this;
        }

        /// <summary>
        /// Registers the script.
        /// </summary>
        /// <param name="scriptText">The script text.</param>
        private void RegisterScript(string scriptText)
        {
            try
            {
                if (this.isWaitForPageLoad && !this.isInAsyncPostBack)
                {
                    if (this.isIE)
                    {
                        scriptText = "window.attachEvent('onload', function() {" + scriptText + "});";
                    }
                    else
                    {
                        scriptText = "window.addEventListener('load', function() {" + scriptText + "}, false);";
                    }
                }
                if (!this.isWaitForPageLoad && !this.isInAsyncPostBack)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this.pg, this.pg.GetType(), Guid.NewGuid().ToString(), scriptText, true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.pg, this.pg.GetType(), Guid.NewGuid().ToString(), scriptText, true);
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("RegisterScript " + ex.ToString(), "AIA.Intranet.Infrastructure.App_Code.JavaScript");
                throw;
            }

        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        /// <returns></returns>
        public JavaScript Reload()
        {
            this.RegisterScript(this.GetConfirmScript() + "window.location.replace(window.location.href);");
            return this;
        }

        /// <summary>
        /// Commits the popup.
        /// </summary>
        /// <param name="Context">The context.</param>
        public static void CommitPopup(HttpContext Context)
        {
            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();
        }

        /// <summary>
        /// Commits the popup.
        /// </summary>
        /// <param name="page">The page.</param>
        public static void CommitPopup(Page page, string strevent)
        {
            page.Response.Write("<script type=\"text/javascript\">window.frameElement.commonModalDialogClose(1, '" + strevent + "');</script>");
            page.Response.Flush();
            page.Response.End();
        }
        #endregion
    }
}
