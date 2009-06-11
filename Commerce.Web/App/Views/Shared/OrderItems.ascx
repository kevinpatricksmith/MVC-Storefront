<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderItems.ascx.cs" Inherits="Commerce.Web.Views.Shared.OrderItems" %>
        <table cellspacing="0" cellpadding="5" width="100%">
            <tr>
                <td><b>Quantity</b></td>
                <td ><b>Item</b></td>
                <td  align="right"><b>Regular</b></td>
                <td  align="right"><b>Total</b></td>
            </tr>

            <%foreach(OrderItem item in ViewData.Model.Items){%>
            <tr>
                <td ><%=item.Quantity %></td>
                <td ><%=item.Product.Name%></td>
                <td  align="right"><%=item.Product.Price.ToString("C")%></td>
                <td  align="right"><%=item.LineTotal.ToString("C") %></td>
            </tr>

            <%} %>
             <tr>
                <td colspan="4"><hr /></td>
             </tr>
             <tr>
                <td colspan="3" align="right">Subtotal</td>
                <td align="right"><%=ViewData.Model.SubTotal.ToString("C") %></td>
             </tr>
              <tr>
                <td colspan="3" align="right">Tax</td>
                <td align="right"><%=ViewData.Model.TaxAmount.ToString("C") %></td>
             </tr>
             <tr>
                <td colspan="3" align="right">Shipping (<%=ViewData.Model.ShippingMethod.Carrier%>: <%=ViewData.Model.ShippingMethod.ServiceName%>)</td>
                <td align="right"><%=ViewData.Model.ShippingMethod.Cost.ToString("C") %></td>
             </tr>
             <tr>
                <td colspan="3" align="right">Discount(<%=ViewData.Model.DiscountReason %>):</td>
                <td align="right">-<%=ViewData.Model.DiscountAmount.ToString("C")%></td>
             </tr>
              <tr>
                <td colspan="3" align="right">Grand Total</td>
                <td align="right"><b><%=ViewData.Model.Total.ToString("C")%></b></td>
             </tr>
        </table>