<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="CreditCard.aspx.cs" Inherits="Commerce.Web.Views.Order.Payment" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%=Html.RegisterJS(ScriptLibrary.StateDropDown) %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="checkout">
    <%=Html.DisplayError() %>
    <div class="cardspace">
      <%using(Html.Form<OrderController>(x=>x.BillingAddressFromCardSpace())){ %>
          <%Html.RenderCommerceControl(CommerceControls.CardSpaceLogin, null);%>
      <%} %>
    </div>
    <div class="paypal">
      <h2>Pay with PayPal</h2>
      <p>
          <form action="<%=Url.Action("PayPalCheckout") %>" method="post">
              <input type="image" src="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" />
          </form>
      </p>
    </div>           
    <div class="credit-card">
      <h2>Credit Card Information</h2>
      <%using (Html.Form<OrderController>(x => x.CreditCard())) { %>
      <fieldset class="credit-card-form">
        <ol>
          <li>
            <label>Credit Card Type</label>
            <select name="CardType">
                <option value="Visa" selected="selected">Visa</option>
                <option value="MasterCard">MasterCard</option>
                <option value="Amex">Amex</option>
            </select>
          </li>
          <li>
            <label>Credit Card Number</label>
            <%=Html.TextBox("AccountNumber", "4586 9748 7358 4049", new { size = 40, maxlength = 40 })%>
          </li>
          <li>
            <label>Security Code</label>
            <%=Html.TextBox("VerificationCode", "000", new { size = 3, maxlength = 3 })%>
          </li>
          <li>
            <label>Expiration</label>
            <select name="ExpirationMonth">
                <%for (int i = 1; i <= 12; i++) { %>
                <option value="<%=i%>"><%=i%></option>
                <%} %>
            </select>
            <select name="ExpirationYear">
                <%for (int i = 0; i <= 6; i++) { %>
                <option value="<%=DateTime.Now.Year+i%>"><%=DateTime.Now.Year + i%></option>
                <%} %>
            </select>
          </li>
        </ol>
        </fieldset>
        <div>
        <h2>Billing Address</h2>
           <%Html.RenderCommerceControl(CommerceControls.AddressEntry, ViewData.Model.CurrentOrder.BillingAddress);%>
          <p class="action">
            <input type="submit" value="Finalize &gt;&gt;&gt;" />
          </p>
        <%} %>
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="sideNavContent" ContentPlaceHolderID="SideNavContent" runat="server"></asp:Content>
