<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductSummaryDisplay.ascx.cs" Inherits="Commerce.MVC.Web.Views.Shared.ProductSummaryDisplay" %>
<div class="prod-wrap">
  <div class="prod-img">
    <a href="<%= Html.BuildUrlFromExpression<CatalogController>(x => x.Show(ViewData.Model.ProductCode)) %>" title="Go to <%= ViewData.Model.Name %> Details Page">
      <img src="/Content/ProductImages/<%=ViewData.Model.DefaultImagePath %>" />
    </a>
  </div>
  <div class="prod-info">
    <h3>
      <%=Html.ActionLink<CatalogController>(x => x.Show(ViewData.Model.ProductCode), ViewData.Model.Name, new { title="Go to " + ViewData.Model.Name + " Details Page" })%>
    </h3>
    <div class="price">
      <label>Price:</label>
      <%=ViewData.Model.Price.ToString("C")%>
    </div>
    <%if (this.UserIsAdmin()){ %>
    <%} %>
    <%=Html.ActionLink<CatalogController>(x=>x.Edit(ViewData.Model.ID),"Manage") %>
  </div>
</div>


        
