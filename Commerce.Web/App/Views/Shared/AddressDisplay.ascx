<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressDisplay.ascx.cs" Inherits="Commerce.Web.Views.Shared.AddressDisplay" %>
<b><%=ViewData.Model.FullName %></b><br />
<%=ViewData.Model.Street1 %><br />
<%if (!String.IsNullOrEmpty(ViewData.Model.Street2)) { %>
<%=ViewData.Model.Street2 %><br />
<%} %>
<%=ViewData.Model.City %>, <%=ViewData.Model.StateOrProvince %><br />
<%=ViewData.Model.Country %>