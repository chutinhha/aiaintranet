using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;

namespace AIA.Intranet.Infrastructure.WebParts.HomeNews
{
    [ToolboxItemAttribute(false)]
    public class HomeNews : Microsoft.SharePoint.WebPartPages.WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/AIA.Intranet.Infrastructure.WebParts/HomeNews/HomeNewsUserControl.ascx";

        private string webPartTitle = "GTO News &amp; Announcements";
        [WebBrowsable(true),
        Category("AIA Setting"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Title of box"),
        WebDescription("")]
        public string WebPartTitle
        {
            get { return webPartTitle; }
            set { webPartTitle = value; }
        }

        private int numberOfItem = 5;
        [WebBrowsable(true),
        Category("AIA Setting"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Number of Item show in the box (default 5 item)"),
        WebDescription("")]
        public int NumberOfItem
        {
            get { return numberOfItem; }
            set { numberOfItem = value; }
        }

        [WebBrowsable(true),
        Category("AIA Setting"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName(""),
        WebDescription("")]
        public List<CommingUpLink> CommingUpLink { get; set;}


        public override Microsoft.SharePoint.WebPartPages.ToolPart[] GetToolParts()
        {
            CustomToolPart customToolPart = new CustomToolPart();
            customToolPart.Title = "AIA Setting";

            Microsoft.SharePoint.WebPartPages.ToolPart[] toolParts = new Microsoft.SharePoint.WebPartPages.ToolPart[3];
            toolParts[0] = new Microsoft.SharePoint.WebPartPages.WebPartToolPart();
            toolParts[1] = new Microsoft.SharePoint.WebPartPages.CustomPropertyToolPart();
            toolParts[2] = customToolPart;


            return toolParts;
        }


        protected override void CreateChildControls()
        {
            HomeNewsUserControl control = Page.LoadControl(_ascxPath) as HomeNewsUserControl;
            control.WebPartTitle = this.WebPartTitle;
            control.NumberOfItem = this.NumberOfItem;
            control.CommingUpLink = this.CommingUpLink;
            Controls.Add(control);
        }
    }

    public class CustomToolPart : Microsoft.SharePoint.WebPartPages.ToolPart
    {
        protected System.Web.UI.WebControls.Label lblTitle;
        protected System.Web.UI.WebControls.TextBox txtCommingUpTitle;
        protected System.Web.UI.WebControls.TextBox txtCommingUpLink;
        protected System.Web.UI.WebControls.Button btnAdd;
        protected System.Web.UI.WebControls.GridView gridCommingUpLink;
        protected System.Web.UI.WebControls.Literal literalSpace;

        protected override void CreateChildControls()
        {
            HomeNews webPart = (HomeNews)this.WebPartToEdit;
            if (webPart == null)
                return;

            if (ViewState["FirstLoad"] == null || !bool.Parse(ViewState["FirstLoad"].ToString()))
                ViewState["CommingUp"] = webPart.CommingUpLink;

            ViewState["FirstLoad"] = true;

            lblTitle = new System.Web.UI.WebControls.Label();
            lblTitle.Text = "Comming up link : ";
            Controls.Add(lblTitle);

            literalSpace = new Literal();
            literalSpace.Text = "<br />Title:&nbsp;";
            Controls.Add(literalSpace);

            txtCommingUpTitle = new TextBox();
            txtCommingUpTitle.ID = "txtTitle";
            Controls.Add(txtCommingUpTitle);

            literalSpace = new Literal();
            literalSpace.Text = "<br />Link:&nbsp;";
            Controls.Add(literalSpace);

            txtCommingUpLink = new TextBox();
            txtCommingUpLink.ID = "txtLink";
            Controls.Add(txtCommingUpLink);

            literalSpace = new Literal();
            literalSpace.Text = "<br />";
            Controls.Add(literalSpace);

            btnAdd = new Button();
            btnAdd.ID = "btnAdd";
            btnAdd.Text = "Add";
            btnAdd.Click += new EventHandler(btnAdd_Click);
            Controls.Add(btnAdd);

            literalSpace = new Literal();
            literalSpace.Text = "<br /><br />";
            Controls.Add(literalSpace);

            gridCommingUpLink = new GridView();
            gridCommingUpLink.Style.Add("width", "100%");
            gridCommingUpLink.RowDeleting += new GridViewDeleteEventHandler(gridCommingUpLink_RowDeleting);
            gridCommingUpLink.AutoGenerateDeleteButton = true;

            gridCommingUpLink.DataSource = ViewState["CommingUp"] as List<CommingUpLink>;
            gridCommingUpLink.DataBind();
            Controls.Add(gridCommingUpLink);

            base.CreateChildControls();
        }

        void gridCommingUpLink_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;
            List<CommingUpLink> temp = ViewState["CommingUp"] as List<CommingUpLink>;
            temp.RemoveAt(index);
            ViewState["CommingUp"] = temp;
            gridCommingUpLink.DataSource = temp;
            gridCommingUpLink.DataBind();
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            List<CommingUpLink> commingUpLink = ViewState["CommingUp"] as List<CommingUpLink>; ;
            if (commingUpLink == null)
                commingUpLink = new List<CommingUpLink>();

            commingUpLink.Add(new CommingUpLink
            {
                Title = txtCommingUpTitle.Text,
                Link = txtCommingUpLink.Text
            });
            ViewState["CommingUp"] = commingUpLink;
            txtCommingUpTitle.Text = string.Empty;
            txtCommingUpLink.Text = string.Empty;
            gridCommingUpLink.DataSource = commingUpLink;
            gridCommingUpLink.DataBind();
        }

        public override void ApplyChanges()
        {
            HomeNews webPart = (HomeNews)this.ParentToolPane.SelectedWebPart;
            if (webPart != null)
                webPart.CommingUpLink = gridCommingUpLink.DataSource as List<CommingUpLink>;
            ViewState["FirstLoad"] = false;
        }
    }

    [Serializable]
    public class CommingUpLink
    {
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
