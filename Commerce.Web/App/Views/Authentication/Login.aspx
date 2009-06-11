<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Commerce.MVC.Web.Views.Authentication.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="checkout">
      <%=Html.DisplayError() %>
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
            <%using (Html.Form<OrderController>(x => x.Shipping())) { %>
            <input type="hidden" name="ReturnUrl" value="<%=Url.Action("Shipping")%>" />
              <%Html.RenderCommerceControl(CommerceControls.UserNamePasswordLogin, null);%>
            <%} %>
          </div>
        </div>
</div>
</asp:Content>
