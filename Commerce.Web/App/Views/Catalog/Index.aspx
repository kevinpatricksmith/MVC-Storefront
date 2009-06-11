<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Commerce.MVC.Web.Views.Catalog.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="favorites">
    <%Html.RenderAction<PersonalizationController>(x => x.Favorites()); %>
</div>

<ul class="product-results">
  <%foreach (Product product in ViewData.Model.Products) { %>
    <li>
      <%Html.RenderPartial("ProductSummaryDisplay", product); %>
    </li>
  <%} %>
</ul>



</asp:Content>
