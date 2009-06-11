<%@ Page Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Commerce.MVC.Web.Views.Home.Index" %>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentTextDisplay">
        <h1>Welcome to the Store</h1>
        <%=Html.ActionLink<CatalogController>(x => x.Index(null), "View the catalog") %>
     </div>
</asp:Content>
