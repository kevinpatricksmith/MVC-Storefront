<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddressBook.aspx.cs" Inherits="Commerce.MVC.Web.App.Views.Personalization.AddressBook" %>

<%if (ViewData.Model != null) { %>
    <%foreach (Address add in ViewData.Model) { %>
        <%using(Html.BeginForm()){ %>
        
        <input type="hidden" name="addressid" value="<%=add.ID %>" />
        
        <div style="margin-bottom:20px">
            <b><%=add.FullName%></b><br />
            <%=add.Street1%><br />
            <%if (!String.IsNullOrEmpty(add.Street2)) { %>
            <%=add.Street2%><br />
            <%} %>
            <%=add.City%>, <%=add.StateOrProvince%><br />
            <%=add.Country%><br />
            
            <input type="submit" value="Use this address" />
        </div>
        
        <%} %>
    <%  }
  }
%>
