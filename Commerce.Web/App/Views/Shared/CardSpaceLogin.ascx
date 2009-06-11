<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CardSpaceLogin.ascx.cs" Inherits="Commerce.MVC.Web.Views.Shared.CardSpaceLogin" %>
   
        
      
    <%if (Request.Url.AbsoluteUri.StartsWith("https:")){%>
          <%=Html.SubmitImage("cardSpaceSubmit",Page.ResolveUrl("~/content/images/infocard.png")) %>
          
          <object type="application/x-informationcard" name="xmlToken">
                <param name="tokenType" value="urn:oasis:names:tc:SAML:1.0:assertion" />
                <param name="issuer" 
                       value="http://schemas.xmlsoap.org/ws/2005/05/identity/issuer/self" />
                <param name="requiredClaims" 
                     value="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode
                            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier" />
          </object>
          <br />
          
    <%} %>
