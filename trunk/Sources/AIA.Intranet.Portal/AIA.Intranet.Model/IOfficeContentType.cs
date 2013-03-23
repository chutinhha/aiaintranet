using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model
{
    public static class IOfficeContentType
    {
        public const string APPROVAL_WF_TASK = "0x01080100e6fa232bca3b4b25b9df4b2e3791d3dd";
        public const string IOFFICE_APPROVAL_TASK = "0x01080100e6fa232bca3b4b25b9df4b2e3791d3fc";
        
        public const string ITEM_RATING_CONTENTTYPE = "0x0100a64e3c24fa0a477bada67bc8cce19e07";
        public const string ITEM_COMMENT_CONTENTTYPE = "0x01001f4d5d513dd74966b47635f2003a212f";

        public const string RECIPIENT_WF_TASK_CONTENTTYPE = "0x01080100bd5e50b1789640c5a168e8a7e5c14faa";
        public const string APPROVER_WF_TASK_CONTENTTYPE = "0x0108010015ec506abe6b44f5ac21b0ffa24e2cd3";
        public const string IMPLEMENTATIONUNIT_WF_TASK_CONTENTTYPE = "0x010801006e5b2413040346da8951879e94f2414b";
        public const string FOCALPOINTUNIT_WF_TASK_CONTENTTYPE = "0x010801009191ad090ca84204a14755bcefcb2f8b";
    }
    public class IOfficeContentTypeId
    {
        public const string ApprovalTask = "I-Office Approval Task";
        public const string CategoryContentType = "0x012000055bc0236c214030873f64adeb28a328";
        public const string NewsCategoryContentType = "0x012000055bc0236c214030873f64adeb28a32800fcc6d37c98094d10a0a5c076f0f3440a";

        //Official Document
        public const string OfficialDocument = "0x01010046a7c43668704f129f54390693fce879";
        public const string LegalDocument = "0x01010046a7c43668704f129f54390693fce87900768d123b9db6448d941d5a45a8dfd0ea";
        public const string AdministrativeRegulation = "0x01010046a7c43668704f129f54390693fce87900f76960825e0541a38a5ff7527ff2da4d";
        public const string IncomingDocument = "0x01010046a7c43668704f129f54390693fce879005567f45a2d9440a394325b42fca622ec";
        public const string OutgoingDocument = "0x01010046a7c43668704f129f54390693fce879007ac91eb3e50b418b988713b6af4a3567";
        public const string OfficialForm = "0x01010046a7c43668704f129f54390693fce87900c4e13c0f7f904dcabf2ab59e62c90c0f";
    }
  

}
