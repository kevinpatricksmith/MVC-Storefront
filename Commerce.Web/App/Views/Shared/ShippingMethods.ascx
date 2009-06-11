<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingMethods.ascx.cs" Inherits="Commerce.MVC.Web.App.Views.Shared.ShippingMethods" %>

    <%=Html.RadioButtonList("shippingMethod", ViewData.Model, "Display", "ID").ToFormattedList(ListType.Unordered)%>
