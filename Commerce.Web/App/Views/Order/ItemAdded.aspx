<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="ItemAdded.aspx.cs" Inherits="Commerce.MVC.Web.Views.ShoppingCart.ItemAdded" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div id="product-details" class="item-added">
    <h1>Just Added To Your Cart:</h1>
    <%-- Html.RenderCommerceControl(CommerceControls.ProductDisplay, ViewData.Model.ItemAdded);--%>
    <%Html.RenderPartial("ProductSummaryDisplay", ViewData.Model.ItemAdded); %>
    
    <%if (ViewData.Model.ItemAdded.CrossSells.Count > 0) { %>
    <h3>You might also be interested in</h3>
        <%foreach (Product cross in ViewData.Model.ItemAdded.CrossSells) { %>
            <%Html.RenderCommerceControl(CommerceControls.ProductDisplay, cross);%>
        <%} %>
    <%} %>
  </div>
  <%if (ViewData.Model.ItemAdded.Recommended.Count > 0) { %>
  

    <h3>Others who bought <%=ViewData.Model.ItemAdded.Name%> also bought:</h3>
    <ul class="product-results">
      <%foreach (Product product in ViewData.Model.ItemAdded.Recommended) { %>
        <li><%Html.RenderPartial("ProductSummaryDisplay", product); %></li>
      <%}%>
    </ul>
    <%}%>
</asp:Content>

<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server"></asp:Content>
