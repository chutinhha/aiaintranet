using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model
{
   public class Constants
   {
       #region AIA
       public const string BANNER_LIBRARY_URL = "BannerLibrary";
       public const string BANNER_CONTENT_TYPE_ID = "0x01010200ec98bbf978904280be10a8bbde810ef7";
       public const string NEWS_CATEGORY_LIST_URL = "/Lists/NewsCategory";
       public const string NEWS_LIST_URL = "/Lists/News";
       public const string NEWS_DEFAULT_CATEGORY = "News";
       public const string NEWS_DEFAULT_LISTS_URL = "/Lists/News";

       public const string HEADER_MENU_LIST_URL = "Lists/HeaderMenu";
       public const string FOOTER_MENU_LIST_URL = "Lists/FooterMenu";
       public const string LEFT_MENU_LIST_URL = "Lists/LeftMenu";

       public const string ORDER_NUMBER_COLUMN = "OrderNumber";
       public const string MENU_KEYWORDS_COLUMN = "MenuKeywords";
       public const string ACTIVE_COLUMN = "Active";

       public const string CONTACT_INTERNAL_EMAIL_PROPERTY = "CONTACT_INTERNAL_EMAIL_PROPERTY";
       public const string CONTACT_EXTERNAL_EMAIL_PROPERTY = "CONTACT_EXTERNAL_EMAIL_PROPERTY";
       public const string CONTACT_TITLE_EMAIL_PROPERTY = "CONTACT_TITLE_EMAIL_PROPERTY";
       public const string CONTACT_BODY_HTML_EMAIL_PROPERTY = "CONTACT_BODY_HTML_EMAIL_PROPERTY";
       public const string CONTACT_ADD_DATE_EMAIL_PROPERTY = "CONTACT_ADD_DATE_EMAIL_PROPERTY";

       public const string TYPE_OF_ENQUIRY_LIST_URL = "/Lists/TypeOfEnquiry";
       public const string OPINION_LIST_URL = "/Lists/Opinions";

       public const string DATA_FEATURE_ID = "40361c29-4256-4f12-95e0-a34d43c12214";
       public const string NEWS_FEATURE_ID = "1f128cb6-3d86-4500-a339-6c0516ab7be5";
       #endregion AIA

       #region News
       public const string NEWS_HOME_PAGE = "/Pages/Home.aspx";
       public const string NEWS_LISTPAGE = "List"; //create List.aspx
       public const string NEWS_DISPLAYPAGE = "View"; //create View.aspx

       #endregion News

       public const string IMAGE_FIELD_TYPE_NAME = "ImageField";

    }
}
