<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="Commerce.MVC.Web.Views.Order.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="checkout">
  <%=Html.DisplayError() %>
  <%if (!User.Identity.IsAuthenticated) { %>
        <h1>Checkout Is Quick and Simple</h1>
        <p>If you've been here before and would like to use the information we have on record
        for you, please login below using the method you prefer.</p>

        <div class="open-id">
          <h2>Use Your Open ID</h2>
          <div class="wrap">
            <form method="post" action="<%=Url.Action("OpenIdLogin","Authentication")%>?ReturnUrl=<%=Url.Action("Shipping")%>">
            <%Html.RenderCommerceControl(CommerceControls.OpenIDLogin, null);%>
            </form>
            <p class="hint">
              Open ID is an identity system that allows you to sign in to websites like ours with
              a single account. Your username and password are secure with Open ID - and you don't
              have to remember yet another password.
            </p>
          </div>
        </div>
        
        <div class="email-password">
          <h2>... Or an Email address with Password</h2>
          <div class="wrap">
            <form method="post" action="<%=Url.Action("Login","Authentication")%>?ReturnUrl=<%=Url.Action("Shipping")%>">
            <input type="hidden" name="ReturnUrl" value="<%=Url.Action("Shipping")%>" />
              <%Html.RenderCommerceControl(CommerceControls.UserNamePasswordLogin, null);%>
            </form>
          </div>
        </div>

        <div class="guest">
          <h2>... Or skip this part entirely</h2>
          <div class="wrap">
            <%using (Html.Form<OrderController>(x => x.Shipping())) { %>
              <input type="submit" name="anon" value="Checkout As Guest &gt;&gt;&gt;" />
            <%} %>
          </div>
        </div>
  <%} %>
</div>
</asp:Content>
<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server">
</asp:Content>
