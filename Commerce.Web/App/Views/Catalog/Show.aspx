<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="Commerce.MVC.Web.Views.Catalog.Show" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div id="product-details">
  
  <%Html.RenderCommerceControl(CommerceControls.ProductDisplay, ViewData.Model);%>
  
  <div >
  <%foreach (ProductDescriptor pd in ViewData.Model.Descriptors) {%>
       <h3><%=pd.Title %></h3>
       <p>
            <%=pd.Body %>
       </p>
  <%}%>
  </div>
</div> 

 <%if (ViewData.Model.Recommended.Count>0) { %>
    <div class="product-results">
    <h3>Others who bought <%=ViewData.Model.Name%> also bought:</h3>
    <ul class="product-results">
        <%foreach (Product product in ViewData.Model.Recommended) { %>
          <li><%Html.RenderPartial("ProductSummaryDisplay", product); %></li>
        <%}%>
    </ul>
  </div>
    <%}%>

<div id="related-products">
  <h3>You Might Also Like</h3>
  <ul>
  <%foreach (Product p in ViewData.Model.RelatedProducts) { %>
      <li>
          <img src="/content/productimages/<%=p.DefaultImagePath%>" alt="<%=p.Name %>"/>
          <%=Html.ActionLink<CatalogController>(x=>x.Show(p.ProductCode),p.Name) %>
      </li>
  <%} %>
  </ul>
</div>





</asp:Content>
