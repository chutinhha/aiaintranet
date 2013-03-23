using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Data;

namespace AIA.Intranet.Model.Entities
{
    public class ForumPostItem : BaseEntity
    {
        
        public int ViewCounts { get; set; }
        public int ReplyCounts { get; set; }
        public string PublicUrl { get; set; }
        public int ThreadID { get; set; }

        public ForumPostItem(SPListItem item)
            : base(item)
        {
            try
            {
                //ParentCatIds = new List<int>();
                //if (item[IOfficeColumnId.Forum.ParentCat] != null)
                //{
                //    var lk = item[IOfficeColumnId.Forum.ParentCat] as SPFieldLookupValueCollection;
                //    foreach (SPFieldLookupValue lkValue in lk)
                //    {
                //        ParentCatIds.Add(lkValue.LookupId); 
                //    }
                    
                //}
                //if(item[IOfficeColumnId.Forum.TopicListId]!= null ){
                //    TopicListId = new Guid(item[IOfficeColumnId.Forum.TopicListId].ToString());
                //}
                //Console.Write("aaa");
            }
            catch (Exception)
            {
                
            }
        }

        public ForumPostItem(DataRow item)
            : base(item)
        {
            
        }



        public string PostContent { get; set; }

        public string Tags { get; set; }



        public bool Pinned { get; set; }

        public bool Locked { get; set; }
    }
}
