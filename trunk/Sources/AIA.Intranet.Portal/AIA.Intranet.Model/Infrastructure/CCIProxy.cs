using System;
using System.Collections.Generic;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    public class CCIProxy
    {
        public CCIProxy()
        {
            DelegateUser = new CCIUser();
            CCUser = new List<CCIUser>();
        }
        public CCIUser DelegateUser { get; set; }
        public List<CCIUser> CCUser { get; set; }
        public string CCSubjectEmail { get; set; }
        public string CCBodyEmail { get; set; }
    }
     
    [Serializable]
    public class CCIUser
    {
        public string LoginName { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
