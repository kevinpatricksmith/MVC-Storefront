<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="Commerce.Web.Views.Order.Receipt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="checkout">
    <h1>Thank You!</h1>
    Your Order Number: <%=ViewData.Model.CurrentOrder.OrderNumber%>
    <h2>Order Details</h2>
    <%Html.RenderCommerceControl(CommerceControls.OrderHeader,ViewData.Model.CurrentOrder); %>
    
    <%Html.RenderCommerceControl(CommerceControls.OrderItems, ViewData.Model.CurrentOrder); %>

</div>


</asp:Content>

<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server"></asp:Content>
