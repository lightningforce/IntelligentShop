﻿<%@ Page Title="Welcome" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IntelligentShop.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="col-md-12"></div>
        <div class="col-md-5 well well-inverse2 f-cloud">
            <div class="col-md-12">
                <p align="center">จำนวนสินค้าคงเหลือ</p>
                <asp:GridView ID="gvProduct" runat="server" CssClass="table table-condensed" AutoGenerateColumns="False" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="productName" HeaderText="ชื่อสินค้า" />
                        <asp:BoundField DataField="amount" HeaderText="จำนวนคงเหลือ" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="col-md-8 well well-inverse2 f-cloud">
            <div class="col-md-12">
                <p align="center">Shopping Cart</p>
                <asp:GridView ID="gvCart" runat="server" CssClass="table table-condensed" AutoGenerateColumns="False" GridLines="None" EmptyDataText="ไม่มีสินค้า">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="productName" HeaderText="ชื่อสินค้า" />
                        <asp:BoundField DataField="quantity" HeaderText="จำนวน" />
                        <asp:BoundField DataField="unitPrice" HeaderText="ราคาต่อหน่วย" />
                        <asp:BoundField DataField="totalPrice" HeaderText="ราคารวม" />
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <p></p>
                <strong>
                    <asp:Label ID="lblTotalText" runat="server" Text="Total:" Visible="false"></asp:Label>
                    <asp:Label ID="lblTotal" runat="server" EnableViewState="false" Visible="false"></asp:Label>
                </strong>
            </div>
        </div>
    </div>
</asp:Content>
