<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="Commerce.MVC.Web.Views.ShoppingCart.Show" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div id="shopping-cart">
    <%=Html.Notify() %>
    <h1>Your Shopping Cart</h1>
    <%if (ViewData.Model.Items.Count == 0) { %>
    <h4><a href="/catalog">There's nothing here! Go buy something...</a></h4>
    <%} else { %>
    <div class="action">
      <%=Html.ActionLink<OrderController>(x=>x.Checkout(),"Checkout >>> ") %>
    </div>
    <table>
      <thead>
        <tr>
          <th></th>
          <th>Item</th>
          <th>Price</th>
          <th class="quantity">Quantity</th>
          <th class="remove">Remove</th>
          <th>Total</th>
        </tr>
      </thead>
      <tbody>
      <%foreach (OrderItem item in ViewData.Model.Items) { %>
      <tr>
        <td>
            <img src="/Content/ProductImages/<%=item.Product.DefaultImagePath%>" />
        </td>
        <td>
          <h3><%=Html.ActionLink<CatalogController>(x=>x.Show(item.Product.ProductCode),item.Product.Name) %></h3>
          <p class="description"><%=item.Product.Description %></p>
          <p class="added-on">Added on <%=item.DateAdded.ToString()%></p>
        </td>
        <td>
          <%= item.Product.Price.ToString("C") %>
        </td>
        <td class="quantity">
          <%using (Html.Form("Order", "UpdateItem")) {%>
          <input type="hidden" name="productid" value="<%=item.Product.ID.ToString() %>" />
          <input type="text" name="Quantity" value="<%=item.Quantity.ToString() %>" size="2" length="2" onchange="this.form.submit();" />
          <%} %>
        </td>
        <td class="remove">
          <%using (Html.Form("Order", "RemoveItem")) {%>
          <input type="hidden" name="productid" value="<%=item.Product.ID.ToString() %>" />
          <input type="image" src="/Content/Images/delete.gif" />
          <%} %>
        </td>
        <td>
          <%= item.LineTotal.ToString("C") %>
        </td>
      </tr>
      <%}%>
      <tr>
        <td colspan="6" align="right">
            <form action="<%=Url.Action("ApplyCoupon") %>" method="post">
                Enter discount coupon
                <input type="text" name="couponCode" />
                <input type="submit" value="go" />
            </form>
        </td>
      </tr>
      </tbody>
     
    </table>
    <div class="action">
      <%=Html.ActionLink<OrderController>(x=>x.Checkout(),"Checkout >>> ") %>
    </div>
     <%} %>
  </div>
</asp:Content>
