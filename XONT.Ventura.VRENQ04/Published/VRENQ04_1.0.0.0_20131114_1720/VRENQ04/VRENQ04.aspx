<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VRENQ04.aspx.cs" Inherits="XONT.Ventura.VRENQ04.VRENQ04" %>
<%@ Register Assembly="XONT.Ventura.Common.Prompt.Web" Namespace="XONT.Ventura.Common.Prompt"
    TagPrefix="cc1" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>VRENQ04</title>
    <link href="../App_Themes/Blue/StyleSheet.css" rel="stylesheet" type="text/css" />

  <%-- <script src="../js/gridcheckBox.js" type="text/javascript"></script>
     <script type="text/javascript" language="javascript"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
    <div>
    <asp:UpdateProgress ID="uprProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
            DisplayAfter="5">
            <ProgressTemplate>
                <div id="divProgressBack" runat="server" style="position: absolute; width: 100%;
                    z-index: 1000; top: 0; height: 100%; left: 0; background-color: White; filter: alpha(opacity=50);
                    opacity: 0.5;">
                    <img class="loading" alt="" align="center" style="position: absolute; vertical-align: middle;
                        left: 35%; top: 25%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1"  runat="server">
            <ContentTemplate>
             <table>
             
                            <tr>
                                <td>
                                    <cc1:ClassificationSelector ID="clsProductClassification" runat="server" ClassificationType="02"
                                        CodeTextWidth="120px" EnableUserInput="true" DescriptionTextWidth="250px" LabelWidth="130px"
                                        FindButtonCssClass="FindButton findpadding" TabIndex="1" GridViewCssClass="PromptGridtablestyle"/>
                                </td>
                            </tr>        
             </table>
             <table>
                            <tr>
                                <td width="130">
                                    <asp:Label ID="lblProduct" runat="server" Text="Product Code" CssClass="Captionstyle"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProduct" runat="server" CssClass="Textboxstyle" Width="120px"
                                        MaxLength="24" TabIndex="8" AutoPostBack="True" OnTextChanged="txtProduct_TextChanged"></asp:TextBox>
                                    <asp:Button ID="btnProduct" runat="server" BorderStyle="None" BorderWidth="0px" CausesValidation="False"
                                        CssClass="FindButton" Height="16px" Width="20px" OnClick="btnProduct_Click" TabIndex="9" />
                                    <asp:TextBox ID="txtProductDis" runat="server" CssClass="Textboxstyle" Width="250px"
                                        TabIndex="-1" ReadOnly="True"></asp:TextBox>
                                    <asp:Label ID="lblErrorProductCode" runat="server" CssClass="Errormessagetextstyle"
                                        Text="Invalid Product Code" Visible="False"></asp:Label>
                                    <cc1:ModalPrompt ID="mptProduct" runat="server" ButtonCssClass="PromptButtonStyle"
                                        EmptyDataText="" GridViewCssClass="PromptGridtablestyle" HeadingCssClass="PromptCaptionstyle"
                                        HideFirstColumn="False" PageSize="10" PromptTitle="" TableCssClass="PromptBasestyle"
                                        ViewHeight="0" ViewWidth="0" BackgroundCssClass="modalBackground" FindButtonCssClass=""
                                        ReturnAllFields="False" searchparameters-capacity="0" OnCancelled="mptProduct_Cancelled"
                                        OnItemSelected="mptProduct_ItemSelected" />
                                </td>
                            </tr>
                        
                            <tr>
                                <td style="margin-right: 40px">
&nbsp;</td>
                                <td>
                                    <input ID="btnList" runat="server" class="MainButtonStyle" name="btList" 
                                        onserverclick="btnList_Click" type="button" value="List" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" CssClass="MainButtonStyle" 
                                        OnClick="btnCancel_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        
                           <table>
                    <tr>
                        <td>
                             <div id="Div5" class="DivScroll DivScroll2" 
                                style="width: 1000px;">
                            <asp:GridView ID="grvPendigItems" runat="server" AllowSorting="True" AllowPaging="true" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"  PageSize=10 OnSorting="grvPendigItems_Sorting"
                                    CssClass="Gridtablestyle"  EmptyDataText="No Orders Found." Width="1000px" OnRowCreated="grvPendigItems_RowCreated"
                                    EmptyDataRowStyle-CssClass="Labelstyle"  OnPageIndexChanging="grvPendigItems_PageIndexChanging" >
                                    <RowStyle ForeColor="#000066" />
                                    <EmptyDataRowStyle CssClass="Labelstyle" />
                                    <Columns>
                                                                                
                                        <asp:BoundField DataField="ItemCode"  HeaderStyle-CssClass="GridPadding" 
                                            HeaderText="Item Code" SortExpression="ItemCode" >
                                            <HeaderStyle CssClass="GridPadding" Width="60px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="Description"  HeaderStyle-CssClass="GridPadding" 
                                        HeaderText="Description"  >
                                            <HeaderStyle Width="200px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="OrderQty"  HeaderStyle-CssClass="GridPadding" 
                                          HeaderText="Order Qty" ControlStyle-Width="100px"  >
                                            <HeaderStyle CssClass="GridPadding" Width="50px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="DeliveryDate"  HeaderStyle-CssClass="GridPadding"
                                         HeaderText="Delivery Date"  SortExpression="DeliveryDate"  >
                                            <HeaderStyle CssClass="GridPadding" Width="70px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="OrderDate"  HeaderStyle-CssClass="GridPadding" 
                                        HeaderText="Order Date" >
                                            <HeaderStyle CssClass="GridPadding" Width="70px" />
                                        </asp:BoundField>
                                                                                                                        
                                        <asp:BoundField DataField="Supplier"  HeaderStyle-CssClass="GridPadding"
                                         HeaderText="Supplier" ControlStyle-Width="80px"  >
                                            <HeaderStyle CssClass="GridPadding" Width="120px" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField DataField="PONo"  HeaderStyle-CssClass="GridPadding" 
                                        HeaderText="PO No" >
                                            <HeaderStyle CssClass="GridPadding" Width="50px" />
                                        </asp:BoundField>
                                                                               
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                            </div>
                        </td>
                    
                    </tr>
                              
           </table>
          </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
