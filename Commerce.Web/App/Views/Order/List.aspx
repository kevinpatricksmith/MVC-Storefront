<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Commerce.MVC.Web.Views.Order.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Orders</h1>
    
    <table width="600">
        <tr>
            <td colspan="4">
            <%using(Html.Form()){ %>
            
            Status:
            <select name="statusFilter" onchange="this.form.submit()">
                <option value="2" <%if(Request["statusFilter"]=="2"){%> selected="selected"<%}%>>Submitted</option>
                <option value="3" <%if(Request["statusFilter"]=="3"){%> selected="selected"<%}%>>Verified</option>
                <option value="4" <%if(Request["statusFilter"]=="4"){%> selected="selected"<%}%>>Charged</option>
                <option value="5" <%if(Request["statusFilter"]=="5"){%> selected="selected"<%}%>>Packaging</option>
                <option value="6" <%if(Request["statusFilter"]=="6"){%> selected="selected"<%}%>>Shipped</option>
            </select>
            
            <%} %>
          
           </td>
        </tr>
        <tr>
            <td>Order Number</td>
            <td>Ordered By</td>
            <td>Amount</td>
            <td>Date</td>
            <td>Status</td>
        </tr>
    <%foreach (Order order in ViewData.Model) { %>
         <tr>
            <td><a href="<%=Url.Action("Manage", "Order", new {id=order.ID}) %>"><%=order.OrderNumber %></a></td>
            <td><%=order.BillingAddress.FullName %></td>
            <td><%=order.Total.ToString("c") %></td>
            <td><%=order.DateCreated.ToShortDateString() %></td>
            <td><%=order.Status.ToString() %></td>
        </tr>
    <%}%>
    </table>
</asp:Content>

<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server"></asp:Content>