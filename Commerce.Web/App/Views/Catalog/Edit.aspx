<%@ Page Title="" Language="C#" MasterPageFile="~/App/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Commerce.MVC.Web.Views.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage: <%=ViewData.Model.Name %></h1>
    
    <table width="90%" cellpadding="5">
         <tr>
            <td>Images</td>
            <td>
                <%foreach (ProductImage image in ViewData.Model.Images) { %>
                <img src="/content/productimages/<%=image.ThumbnailPhoto %>" />
            
                <%} %>
            </td>
        </tr>   
        <tr>
            <td>Product Code</td>
            <td><input type="text" name="ProductCode" size="40" value="<%=ViewData.Model.ProductCode %>"/></td>
        </tr>        
        <tr>
            <td>Product</td>
            <td><input type="text" name="Name" size="40" value="<%=ViewData.Model.Name %>"/></td>
        </tr>
        <tr>
            <td>Price</td>
            <td><input type="text" name="Price" size="5" value="<%=ViewData.Model.Price %>"/></td>
        </tr>
        <tr>
            <td>Discounted Price</td>
            <td><%=ViewData.Model.DiscountedPrice.ToString("C")%></td>
        </tr>

        <tr>
            <td>Manufacturer</td>
            <td><input type="text" name="Manufacturer" size="40" value="<%=ViewData.Model.Manufacturer %>"/>
            </td>
        </tr>
        <tr>
            <td>Delivery Method</td>
            <td>
                <select name="DeliveryMethod">
                <%=Enum.GetValues(typeof(DeliveryMethod)).ToOptionList(ViewData.Model.Delivery)%>
                </select>
            </td>
        </tr>
        <tr>
            <td>Weight In Pounds</td>
            <td><input type="text" name="WeightInPounds" size="5" value="<%=ViewData.Model.WeightInPounds%>"/></td>
        </tr>
        <tr>
            <td>Is Taxable</td>
            <td><input type="checkbox" name="IsTaxable" <%=this.IsChecked(ViewData.Model.IsTaxable)%>/></td>
        </tr>
        <tr>
            <td>Allow Backorder</td>
            <td><input type="checkbox" name="AllowBackorder" <%=this.IsChecked(ViewData.Model.IsTaxable)%>/></td>
        </tr>
        <tr>
            <td>Estimated Delivery</td>
            <td><input type="text" name="Name" size="40" value="<%=ViewData.Model.EstimatedDelivery %>"/></td>
        </tr>
        <tr>
            <td valign="top">Descriptors</td>
            <td>
                <%foreach(ProductDescriptor desc in ViewData.Model.Descriptors){ %>
                <form action="" method="post">
                    <input type="hidden" name="ID" value="<%=desc.ID %>" />
                    <div>Title</div>
                    <div><input type="text" name="Name" size="30" value="<%=desc.Title%>"/></div>
                    <div>Body</div>
                    <div><textarea name="body" cols="50" rows="8"><%=desc.Body%></textarea></div>
                    <div><input type="submit" value="save"/></div>
                </form>               
                
                <%} %>
                <form action="" method="post">
                    <div>Title</div>
                    <div><input type="text" name="Name" size="30"/></div>
                    <div>Body</div>
                    <div><textarea name="body" cols="50" rows="8"></textarea></div>
                    <div><input type="submit" value="add"/></div>
                </form>
            </td>
        </tr>
        <tr>
            <td valign="top">Cross-sells</td>
            <td>
                 <select name="CrossSells" size="20" multiple="multiple">
                    <%foreach (Product p in ViewData["Products"] as List<Product>) { %>
                        <option value="<%=p.ID %>"  <%if(ViewData.Model.CrossSells.Contains(p)){%> selected="selected" <%}%>><%=p.Name %></option>
                    <%} %>
                 </select>
            </td>
        </tr>
        <tr>
            <td valign="top">Related</td>
            <td>
            
                 <select name="CrossSells" size="20" multiple="multiple">
                    <%foreach (Product p in ViewData["Products"] as List<Product>) { %>
                        <option value="<%=p.ID %>"  <%if(ViewData.Model.RelatedProducts.Contains(p)){%> selected="selected" <%}%>><%=p.Name %></option>
                    <%} %>
                 </select>
            
            </td>
        </tr>        

    </table>
    
    
</asp:Content>
