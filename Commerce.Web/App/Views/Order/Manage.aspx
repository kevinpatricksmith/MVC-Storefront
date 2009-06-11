<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="Commerce.MVC.Web.Views.Order.Manage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><a href="<%=Url.Action("List") %>">Orders </a> >>> Manage Order: <%=ViewData.Model.OrderNumber %></h1>
    <h3>Order status: <%=ViewData.Model.Status.ToString() %></h3>

    <%=Html.Notify() %>


    <table cellpadding=5>
        <tr>
            <td width="60%">
                <div class="sectionheader">Order Details</div>
                <%Html.RenderCommerceControl(CommerceControls.OrderHeader, ViewData.Model); %>
                <%Html.RenderCommerceControl(CommerceControls.OrderItems, ViewData.Model); %>
            </td>
            <td valign=top>
                
                <fieldset>
                    <legend>Manage</legend>
                    <ul>
                        <%if(ViewData.Model.Status==OrderStatus.Verified){ %>
                        <li>
                        
                            <form action="<%=Url.Action("Charge","Order",new {id=ViewData.Model.ID}) %>" method="post">
                                <input type=submit value="Capture Charge" />
                            </form>
                        
                        </li>
                        <%} %>
                        <%if(ViewData.Model.Status==OrderStatus.Charged){ %>
                        <li>
                            <form action="<%=Url.Action("Ship","Order",new {id=ViewData.Model.ID.ToString()}) %>" method="post">
                               Ship and Close:
                               <br /><input type="text" name="trackingNumber" /> <input type="submit" value="go" />
                               <br /><i>enter tracking number</i>
                            </form>
                        </li>
                        <%} %>
                        <%if((bool)ViewData["CanCancel"]){ %>
                      
                        <li>
                            <form action="<%=Url.Action("Cancel","Order",new {id=ViewData.Model.ID.ToString()}) %>" method="post">
                                <input type=submit value="Cancel Order" />
                            </form>
                        </li>
                        <%} %>
                       
                    </ul>                
                </fieldset>
            

            </td>
        </tr>
    
    </table>
    

    
    
</asp:Content>


<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server"></asp:Content>