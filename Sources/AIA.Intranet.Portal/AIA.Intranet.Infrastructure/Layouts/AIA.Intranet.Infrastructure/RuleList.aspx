<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleList.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.RuleList" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="/_controltemplates/ButtonSection.ascx" %> 


<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" language="javascript" src="/_layouts/groupeditempicker.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Security Rule List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Security Rule List: <%= CurrentList.Title %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server" >
<asp:Table runat="server" id="tblMain" CellPadding="0" CellSpacing="0" Width="100%" BorderStyle="None"> 
    <asp:TableRow>
        <asp:TableCell CssClass="ms-sectionheader">
              <!-- Grid Rule -->
                      <SharePoint:SPGridView EnableViewState ="true"  ID="RuleListGrid" runat="server"
                       onrowdatabound="RuleListGrid_RowDataBound" ondatabound="RuleListGrid_DataBound"
                       AutoGenerateColumns="False"      >
                      <Columns>                   

                        <asp:TemplateField HeaderText="Rule Name" ItemStyle-Width="50%" ItemStyle-Wrap="False">
                            <ItemTemplate>                        
                                <a href="EditRule.aspx?List=<%= CurrentList.ID %><%# BuildQSContentType(DataBinder.Eval (Container.DataItem, "ContentTypeId").ToString()) %>&RuleId=<%# DataBinder.Eval (Container.DataItem, "Id") %>&Source=<%= CurrentUrlWithoutSource %>"  > <%# DataBinder.Eval (Container.DataItem, "Name") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                         
                        <asp:BoundField DataField="Id" 
                         HeaderText="RuleId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                         
                        <asp:TemplateField HeaderText="Order" >
                            <ItemTemplate>
                                <asp:DropDownList runat="server" id="Order" onchange="IrReorder(this, Ir_Default)" EnableViewState="true"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                         
                         </Columns>
                      </SharePoint:SPGridView>                            
        </asp:TableCell>
    </asp:TableRow>  
    <asp:TableRow>
        <asp:TableCell>
             <wssuc:ButtonSection runat="server" ShowStandardCancelButton="false" >
	            <Template_Buttons>	    
	                <asp:Button runat="server" class="ms-ButtonHeightWidth" 
	                Text="Save" OnClick="BtnSave_Click"
	                id="BtnSave"  />				
	                <asp:Button runat="server" class="ms-ButtonHeightWidth" 
	                Text="Add Rule" OnClick="BtnAdd_Click" 
	                id="BtnAdd"  />				
        	        	        
                    <asp:Button runat="server" class="ms-ButtonHeightWidth" 
	                Text="Cancel" causesvalidation="false"
	                id="BtnCancel"  OnClick="BtnCancel_Click" 
	                accesskey="<%$Resources:wss,okbutton_accesskey%>"  />		
        	        
                </Template_Buttons>
            </wssuc:ButtonSection>										
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
 
 <!-- This table is visible when load setting error-->
<asp:Table  runat="server" ID="tblError" CellPadding="0" CellSpacing="0" Width="100%" BorderStyle="None" >
    <asp:TableRow>
        <asp:TableCell ForeColor="Red" CssClass="ms-error">
            There is an error in Security setting.
        </asp:TableCell>    
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <wssuc:ButtonSection runat="server" ShowStandardCancelButton="false" >
	                <Template_Buttons>	    
                        <asp:Button ID="BtnClearSetting" 
                        class="ms-ButtonHeightWidth"  
                        runat="server" 
                        Text="Clear Setting" 
                        OnClick="BtnClearSetting_Click"
                         />                                      	        
                    </Template_Buttons>
            </wssuc:ButtonSection>									
        </asp:TableCell>    
    </asp:TableRow>
</asp:Table>

<style type="text/css">
    .hiddencol
    {
        display:none;
    }
    .viscol
    {
        display:block;
    }
</style>

<asp:Literal ID="ScriptPlaceHolder" runat="server"></asp:Literal>
<script type="text/javascript">
    try {
        if (Ir_Default != null && Ir_Default != 'undefined')
            IrInitialize(Ir_Default)
    }
    catch (err) {
        //Handle errors here
    }

   </script>
</asp:Content>