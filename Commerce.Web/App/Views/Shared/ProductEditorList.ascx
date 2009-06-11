<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductEditorList.ascx.cs" Inherits="Commerce.MVC.Web.App.Views.Shared.ProductList" %>
<table width="100%">
    <tr>
        <td align="left" valign="top">
        <select name="bob" size="20" >
            <%=ViewData.Model.ToOptionList("","ID","Name") %>
        </select>
        </td>
        <td align="left" valign="top">
        
        </td>
    </tr>
</table>