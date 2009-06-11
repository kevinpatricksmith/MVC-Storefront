<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Favorites.aspx.cs" Inherits="Commerce.MVC.Web.App.Views.Personalization.Favorites" %>

<p>
<%if (ViewData.Model.LastProductsViewed.Count > 0) { %>
    <h2>Recently Viewed</h2>
    <%foreach (Product p in ViewData.Model.LastProductsViewed.Distinct().Take(5)) { %>
    <li>
        <%Html.RenderPartial("ProductSummaryDisplay", p);%>
    </li>
    <%}
  }%>
</p>