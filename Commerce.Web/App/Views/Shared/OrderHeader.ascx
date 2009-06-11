<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderHeader.ascx.cs" Inherits="Commerce.Web.Views.Shared.OrderHeader" %>
        <table width="400">

            <tr>
                <td width="50%">
                    <p><b>Shipping To:</b></p>
                    <p><%Html.RenderCommerceControl(CommerceControls.AddressDisplay,ViewData.Model.ShippingAddress); %></p>
                </td>
                 <td width="50%">
                    <p><b>Billing To:</b></p>
                    <p><%Html.RenderCommerceControl(CommerceControls.AddressDisplay, ViewData.Model.BillingAddress); %></p>
               </td>
            </tr>
        </table>