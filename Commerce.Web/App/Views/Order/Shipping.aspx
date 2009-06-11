<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Shipping.aspx.cs" Inherits="Commerce.Web.Views.Order.Shipping" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
  <%=Html.RegisterJS(ScriptLibrary.StateDropDown) %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="checkout">
  <%=Html.DisplayError() %>
  <h2>Shipping</h2>
  <div class="cardspace">
    <%using (Html.Form<OrderController>(x => x.ShippingAddressFromCardSpace())) { %>
      <%Html.RenderCommerceControl(CommerceControls.CardSpaceLogin, null);%>
    <%} %>
  </div>
  <div class="shipping-form">
  
  
  <div class="addressBook">
    <%Html.RenderAction<PersonalizationController>(x => x.AddressBook()); %>
  </div>

  
    <%using (Html.Form<OrderController>(x => x.ShippingAddressFromForm())) { %>
      <%Html.RenderCommerceControl(CommerceControls.AddressEntry, ViewData.Model.CurrentOrder.ShippingAddress);%>
      <p class="action">
        <input type="submit" value="Billing &gt;&gt;&gt;" />
      </p>
    <%} %>
  </div>  
</div>
</asp:Content>
<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server">
</asp:Content>
