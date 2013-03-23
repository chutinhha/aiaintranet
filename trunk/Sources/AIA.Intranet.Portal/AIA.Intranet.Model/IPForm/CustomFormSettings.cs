namespace AIA.Intranet.Model.IPForm
{
    public class CustomFormSettings
    {
        public CustomFormSettings()
        {
            NewItemFormXsnLocation = string.Empty;
            DispItemFormXsnLocation = string.Empty;
            EditItemFormXsnLocation = string.Empty;
        }
        public string NewItemFormXsnLocation { get; set; }
        public string DispItemFormXsnLocation { get; set; }
        public string EditItemFormXsnLocation { get; set; }
        public string NewItemFormType { get; set; }
        public string DispItemFormType { get; set; }
        public string EditItemFormType { get; set; }
        public bool AllowChangeContentType { get; set; }
        public string LinkEditDocumentText { get; set; }
        public string LinkEditPropertiesText { get; set; }
        public LinkIPFormType EditDocumentLinkType { get; set; }
        public LinkIPFormType EditPropertiesLinkType { get; set; }
    }
   
    public class DefaultFormUrlSettings
    {
        public DefaultFormUrlSettings()
        {
            NewFormUrl = string.Empty;
            DispFormUrl = string.Empty;
            EditFormUrl = string.Empty;
        }
        public string NewFormUrl { get; set; }
        public string DispFormUrl { get; set; }
        public string EditFormUrl { get; set; }
    }
}
