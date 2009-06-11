<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressEntry.ascx.cs" Inherits="Commerce.MVC.Web.Views.Shared.AddressEntry" %>
    <%
        Address thisAddress = new Address();
        if (ViewData.Model != null)
            thisAddress = ViewData.Model;
    %>
<fieldset class="address-form">

  <legend>Address Form</legend>
  <ol>
    <li>
      <label>First/Last</label>
      <%=Html.TextBox(this.NamePrefix + "FirstName", thisAddress.FirstName ?? "")%>
      <%=Html.TextBox(this.NamePrefix + "LastName", thisAddress.LastName ?? "")%>
    </li>
    <li>
      <label>Email</label>
      <%=Html.TextBox(this.NamePrefix + "Email", thisAddress.Email ?? "")%>
    </li>
    <li>
      <label>Address</label>
      <%=Html.TextBox(this.NamePrefix + "Street1", thisAddress.Street1 ?? "")%>
    </li>
    <li>
      <label>Address 2</label>
      <%=Html.TextBox(this.NamePrefix + "Street2", thisAddress.Street2 ?? "")%>
    </li>
    <li>
      <label>City</label>
      <%=Html.TextBox(this.NamePrefix + "City", thisAddress.City ?? "")%>
    </li>
    <li>
      <label>Country and State</label>
      <script type="text/javascript">
          var postState = '<%=thisAddress.StateOrProvince%>';
          var postCountry = '<%=thisAddress.Country%>';
      </script>
      <select id='countrySelect' name='country' onchange='populateState()'>
      </select>
      <select id='stateSelect' name='stateorprovince'>
      </select>
      <script type="text/javascript">initCountry('US'); </script>
    </li>
    <li>
      <label>Postal Code</label>
      <%=Html.TextBox(this.NamePrefix + "Zip", thisAddress.Zip ?? "")%>
    </li>
  </ol>
</fieldset>