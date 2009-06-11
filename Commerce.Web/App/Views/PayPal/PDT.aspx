<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/App/Views/Shared/Site.Master" CodeBehind="PDT.aspx.cs" Inherits="Commerce.MVC.Web.App.Views.PayPal.PDT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Thank you!</h1>
    Thank you for your payment - it has been received and your order is being processed.
    
    <div>
    <%=ViewData["message"] %>
    </div>
    
</asp:Content>
