<%@ Page Title="Config" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="IntelligentShop.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="form-group">
            <asp:Label ID="lblProductId" runat="server" Text="Product ID:"></asp:Label>
            <asp:DropDownList ID="ddlProductId" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblRange1" runat="server" Text="Range1:"></asp:Label>
            <asp:TextBox ID="txtRange1" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
         <div class="form-group">
            <asp:Label ID="lblRange2" runat="server" Text="Range2:"></asp:Label>
            <asp:TextBox ID="txtRange2" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
         <div class="form-group">
            <asp:Label ID="lblRange3" runat="server" Text="Range3:"></asp:Label>
            <asp:TextBox ID="txtRange3" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
         <div class="form-group">
            <asp:Label ID="lblRange4" runat="server" Text="Range4:"></asp:Label>
            <asp:TextBox ID="txtRange4" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
         <div class="form-group">
            <asp:Label ID="lblRange5" runat="server" Text="Range5:"></asp:Label>
            <asp:TextBox ID="Range5" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-default" OnClick="btnSubmit_Click"/>
    </div>
</asp:Content>
