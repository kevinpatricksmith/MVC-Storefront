<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_CategoryList.aspx.cs" Inherits="Commerce.MVC.Web.Components.Catalog.Views.CategoryList" %>
<ul class="category-list">
    <%foreach (Category parent in ViewData.Model) { %>
    <li>
    <h3><%=parent.Name %></h3>
    <ul>
        <%foreach (Category child in parent.SubCategories) { %>
        <li>
          <%=Html.ActionLink<CatalogController>(x=>x.Index(child.ID),child.Name) %>
        </li>
        <%} %>
    </ul>
    </li>
    <%} %>
</ul>