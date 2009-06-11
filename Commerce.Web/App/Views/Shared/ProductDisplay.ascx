<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductDisplay.ascx.cs" Inherits="Commerce.MVC.Web.Views.Shared.ProductDisplay" %>
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
    <% if(this.DisplayDescription) { %>
      <%=ViewData.Model.Description%>
    <% } %>
    <div class="price">
      <label>Price:</label>
      <%=ViewData.Model.Price.ToString("C")%>
    </div>
    <%
      if (this.DisplayAddToCartButton) {
        using (Html.Form<OrderController>(x => x.AddItem(ViewData.Model.ID))) {
    %>
    <div class="add-to-bag">
      <%=Html.SubmitButton("btnAddItem", "Add Item To Cart >>>")%>
    </div>
    <% 
      }
    } 
    %>
    <%if (this.UserIsAdmin()){ %>
        <%=Html.ActionLink<CatalogController>(x=>x.Edit(ViewData.Model.ID),"Manage") %>
    <%} %>
  </div>
</div>


        
