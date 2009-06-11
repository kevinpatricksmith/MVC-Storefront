<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="Commerce.MVC.Web.App.Views.PayPal.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="checkout">  
        <div class="paypal">  
        <h2>Pay with PayPal</h2>
            <div class="wrap">
                You are about to be redirected to PayPal.com to purchase this order. 
                Once payment is completed, please be sure to let PayPal redirect you 
                back to this site. When PayPal redirects you, they send along your transaction 
                information which we need in order to complete and reconcile your payment.
            </div>
        <h2>Select Shipping</h2>
        <%using (Html.BeginForm()) { %>
            <%=Html.RadioButtonList("shippingMethod", ViewData["ShippingMethods"], "Display", "ID", ViewData.Model.ShippingMethod.ID.ToString(), new { onclick = "this.form.submit();" }).ToFormattedList(ListType.Unordered)%>
        <%} %>
        <h2>Order Details</h2>
        <div class="wrap">
            <%Html.RenderCommerceControl(CommerceControls.OrderHeader,ViewData.Model); %>
            <%Html.RenderCommerceControl(CommerceControls.OrderItems, ViewData.Model); %>
        </div>
        <form id="Paypal" name="Paypal" action="https://www.sandbox.paypal.com/cgi-bin/webscr" method="post">

            <%=Html.Hidden("cmd", "_cart")%>
            <%=Html.Hidden("upload", "1")%>
            <%=Html.Hidden("business", ConfigurationManager.AppSettings["PayPalMerchantEmail"])%>
            <%=Html.Hidden("custom", ViewData.Model.ID.ToString())%>
            <%=Html.Hidden("tax_cart", ViewData.Model.TaxAmount.ToLocalCurrency())%>
            <%=Html.Hidden("currency_code", ConfigurationManager.AppSettings["CurrencyCode"])%>
            
            <%=Html.Hidden("return", this.GetSiteUrl()+Url.Action("PDT"))%>
            <%=Html.Hidden("cancel_return", this.GetSiteUrl()+Url.Action("Show","Order"))%>
      
        <%if(ViewData.Model.ShippingAddress!=null){ %>

            <%=Html.Hidden("first_name", ViewData.Model.ShippingAddress.FirstName)%>
            <%=Html.Hidden("last_name", ViewData.Model.ShippingAddress.LastName)%>
            <%=Html.Hidden("address1", ViewData.Model.ShippingAddress.Street1)%>
            <%=Html.Hidden("address2", ViewData.Model.ShippingAddress.Street2)%>
            <%=Html.Hidden("city", ViewData.Model.ShippingAddress.City)%>
            <%=Html.Hidden("state", ViewData.Model.ShippingAddress.StateOrProvince)%>
            <%=Html.Hidden("country", ViewData.Model.ShippingAddress.Country)%>
            <%=Html.Hidden("zip", ViewData.Model.ShippingAddress.Zip)%>

        <%} 
        int itemIndex = 1;
        foreach(OrderItem item in ViewData.Model.Items){ %>

            <%=Html.Hidden("item_name_"+itemIndex, item.Product.Name)%>
            <%=Html.Hidden("amount_" + itemIndex, item.Product.Price.ToLocalCurrency())%>
            <%=Html.Hidden("item_number_" + itemIndex, item.Product.ProductCode)%>
            <%=Html.Hidden("quantity_" + itemIndex, item.Quantity.ToString())%>
            <%=Html.Hidden("shipping_" + itemIndex, (ViewData.Model.ShippingMethod.Cost/ViewData.Model.Items.Count).ToLocalCurrency())%>
           
            <%itemIndex++;
        } %>
        <div class="checkout-button">
        <input type="image" src="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" align="left" style="margin-right:7px;" />
        </div>
        </form>
        </div>
    </div>

</asp:Content>
