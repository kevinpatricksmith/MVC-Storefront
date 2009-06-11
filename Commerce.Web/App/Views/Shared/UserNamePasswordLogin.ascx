<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserNamePasswordLogin.ascx.cs" Inherits="Commerce.MVC.Web.Views.Shared.UserNamePasswordLogin" %>
  
        <div>
            <table>
                <tr>
                    <td>Email:</td>
                    <td><%= Html.TextBox("login") %></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td><%= Html.Password("password") %></td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="checkbox" name="rememberMe" value="true" /> Remember me?</td>
                </tr>
                <tr>
                    <td colspan=2>
                        <input type="submit" value="Login" />
                    </td>
                </tr>
            </table>
        </div>
        