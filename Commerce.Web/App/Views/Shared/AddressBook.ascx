<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressBook.ascx.cs" Inherits="Commerce.Web.Views.Shared.AddressBook" %>
      
      
      <%if (User.Identity.IsAuthenticated && ViewData.Model.Count > 0){ %>
       <div class="addressBook">
            <div class="sectionheader">Address Book</div>
           <%foreach (Address add in ViewData.Model){ %>
                <div style="padding:10px">
                    <form action="<%=Request.Url.AbsoluteUri %>" method="post">
                        <input type=submit  value="Use This Address" /><br />
                        <%=Html.RenderCommerceControl(CommerceControls.AddressDisplay,add)%><br />
                        <input type=hidden name="addressID" value="<%=Html.Encode(add.ID) %>" />
                    </form>
                
                </div>
           <%} %>
        </div>
       <%} %> 