<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenIDLogin.ascx.cs" Inherits="Commerce.MVC.Web.Views.Shared.OpenIDLogin" %>
    
    
    <!-- BEGIN ID SELECTOR -->
    <input type="text" size="40" id="openid.claimed_id" name="openid.claimed_id" />
    <script type="text/javascript" >
    idselector_input_id = "openid.claimed_id";
    </script>
    <script type="text/javascript" id="__openidselector" src="https://www.idselector.com/selector/d3d11da3d781d63fcfdc4f0662b0a8303687cdfc" charset="utf-8"></script>
    <!-- END ID SELECTOR -->
    <input type="submit" value="go" />
    <br />
