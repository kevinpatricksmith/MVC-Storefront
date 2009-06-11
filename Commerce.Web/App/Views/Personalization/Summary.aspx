<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="Commerce.MVC.Web.Components.UserSession.Views.Summary" %>
<div id="useraccount">
    <%if (User.Identity.IsAuthenticated) { %>
        Welcome Back <%=this.GetFriendlyUserName()%> |
    <%} %>
    <img src="/content/images/cart.gif" />
    <%=Html.ActionLink<OrderController>(x=>x.Show(),"Your Cart") %> (<%=ViewData.Model.CurrentOrder.Items.Count %>) 
    
     <%if (User.Identity.IsAuthenticated) { %>
        | <%=Html.ActionLink<AuthenticationController>(x=>x.Logout(),"Logout") %>
    <% }
    else
    {%>
        | <%=Html.ActionLink<AuthenticationController>(x=>x.Login(),"Login", new { ReturnUrl = this.GetReturnUrl() })%>
    <%} %>
</div>