<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Finalize.aspx.cs" Inherits="Commerce.Web.Views.Order.Finalize" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="checkout">
    <h2>Shipping Method</h2>
    <%using (Html.Form<OrderController>(x => x.Finalize())) { %>
    <%=Html.RadioButtonList("shippingMethod", ViewData.Model.ShippingMethods, "Display", "ID", ViewData.Model.CurrentOrder.ShippingMethod.ID.ToString(), new { onclick = "this.form.submit();" }).ToFormattedList(ListType.Unordered)%>
    <%} %>
    <h2>Order Details</h2>
    <%Html.RenderCommerceControl(CommerceControls.OrderHeader,ViewData.Model.CurrentOrder); %>
    
    <%Html.RenderCommerceControl(CommerceControls.OrderItems, ViewData.Model.CurrentOrder); %>
    <%using (Html.Form<OrderController>(x => x.PlaceOrder())) { %>
    <div style="text-align:right">
        <input type="submit" value="Place Order" />
    </div>
    <%} %>
</div>
</asp:Content>

<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server"></asp:Content>
