using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Data;

namespace AIA.Intranet.Model.Entities
{
    public class ForumCategory : BaseEntity
    {
        public string CatPic { get; set; }
        public string CatDesc { get; set; }
        public int ParentCat { get; set; }
        public string ReadPermissions { get; set; }
        public string WritePermission { get; set; }
        public Guid TopicListId { get; set; }
        public string TopicLink { get; set; }
        public int ViewCounts { get; set; }
        public int ReplyCounts { get; set; }
        public int TopicCounts { get; set; }

        [Field(Ignore=true)]
        public ForumPostItem LastestPost { get; set; }

        public ForumCategory(SPListItem item)
            : base(item)
        {
            try
            {
                ParentCatIds = new List<int>();
                if (item[IOfficeColumnId.Forum.ParentCat] != null)
                {
                    var lk = item[IOfficeColumnId.Forum.ParentCat] as SPFieldLookupValueCollection;
                    foreach (SPFieldLookupValue lkValue in lk)
                    {
                        ParentCatIds.Add(lkValue.LookupId); 
                    }
                    
                }
                if(item[IOfficeColumnId.Forum.TopicListId]!= null ){
                    TopicListId = new Guid(item[IOfficeColumnId.Forum.TopicListId].ToString());
                }

                Console.Write("aaa");
            }
            catch (Exception)
            {
                
            }
        }

        public ForumCategory(DataRow item)
            : base(item)
        {
            
        }


        [Field(Ignore=true)]
        public List<int>ParentCatIds { get; set; }
        [Field(Ignore = true)]
        public List<ForumCategory> Childrens { get; set; }
    }
}
