<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WFormABMTarifaDeParcela.aspx.cs" Inherits="WSCliente.WFormABMTarifaDeParcela" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="MainContent">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>
<asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
        Width="900px">
<asp:TabPanel runat="server" ID="Tab0">
<HeaderTemplate>
Datos de la Lista de Precios
</HeaderTemplate>
<ContentTemplate>
    <table>
    <tr>
        <td><asp:Label ID="lblDescripcion" runat="server" Text="Descripción : " CssClass="LabelNegrita"></asp:Label>
</td>
        <td><asp:TextBox ID="txtDescripcion" runat="server" CssClass="TextBox3" SkinID="NOMBRE"></asp:TextBox>
</td>
    </tr>
    </table>    
    <div>
    <fieldset>
    <legend><h5>Vigencia</h5></legend>
    <table>
        <tr>
            <td><asp:Label ID="lblFechaDesde" runat="server" Text="Fecha Desde : " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:TextBox ID="txtFechaInicio" runat="server" CssClass="TextBox2" SkinID="FECHA_DESDE"></asp:TextBox>

                <asp:CalendarExtender ID="TxtFechaInicio_CalendarExtender" runat="server" 
                    ClearTime="True" TargetControlID="txtFechaInicio" Enabled="True" Format ="dd/MM/yyyy" 
                    ></asp:CalendarExtender>

            </td>
            <td><asp:Label ID="lblFechaHasta" runat="server" Text="Fecha Hasta : " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:TextBox ID="txtFechaHasta" runat="server" CssClass="TextBox2" SkinID="FECHA_HASTA"></asp:TextBox>

                <asp:CalendarExtender ID="txtFechaHasta_CalendarExtender" runat="server" 
                    ClearTime="True" TargetControlID="txtFechaHasta" Format="dd/MM/yyyy" 
                    Enabled="True"></asp:CalendarExtender>

            </td>
        </tr>
    </table>
    </fieldset>
    </div>
    <div>
    <fieldset>
    <table>
        <tr>
            <td><asp:Label ID="lblPrecioParcela" runat="server" Text="Precio Parcela: " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:TextBox ID="txtPrecioParcela" runat="server" CssClass="TextBox2" 
                    SkinID="PRECIO" Width="150px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtPrecioParcela_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtPrecioParcela" 
                    ValidChars ="0123456789." Enabled="True">
                </asp:FilteredTextBoxExtender>
</td>
            <td>
                <asp:Label ID="Label5" runat="server" CssClass="Label" ForeColor="Red" Text="*"></asp:Label>
            </td>
            <td><asp:Label ID="lblCantidadLugaresHabilitar" runat="server" Text="Cantidad Lugares a Habilitar: " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:TextBox ID="txtCantidadLugaresHabilitar" runat="server" CssClass="TextBox2" SkinID="CANT_LUGARES_HABILITA"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtCantidadLugaresHabilitar_FilteredTextBoxExtender" ValidChars ="0123456789." 
                    runat="server" TargetControlID="txtCantidadLugaresHabilitar" 
                    Enabled="True">
                </asp:FilteredTextBoxExtender>
</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td><asp:Label ID="lbldescuentoLista" runat="server" Text="Descuento: " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:TextBox ID="txtDescuentoLista" runat="server" CssClass="TextBox2" SkinID="DESCUENTO_LISTA"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtDescuentoLista_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtDescuentoLista" 
                    ValidChars ="0123456789.0" Enabled="True">
                </asp:FilteredTextBoxExtender>
</td>
            <td>
                &nbsp;</td>
            <td><asp:Label ID="lblMoneda" runat="server" Text="Moneda: " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:DropDownList ID="ddlMoneda" runat="server" CssClass="ComboBox2" SkinID="COD_MONEDA"></asp:DropDownList>
</td>
            <td>
                <asp:Label ID="Label6" runat="server" CssClass="Label" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblArticulo" runat="server" Text="Articulo: " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:DropDownList ID="ddlArticulo" runat="server" CssClass="ComboBox2" SkinID="COD_ARTICULO"></asp:DropDownList>
</td>
            <td>
                &nbsp;</td>
            <td><asp:Label ID="lblDescuento" runat="server" Text="Descuento: " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:DropDownList ID="ddlDescuento" runat="server" CssClass="ComboBox2" SkinID="COD_CAT_DESCUENTO"></asp:DropDownList>
</td>
            <td>
                <asp:Label ID="Label4" runat="server" CssClass="Label" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblPrecioComision" runat="server" Text="Precio Comisión: " CssClass="LabelNegrita"></asp:Label>
</td>
            <td><asp:TextBox ID="txtPrecioComision" runat="server" CssClass="TextBox2" SkinID="PRECIO_COMISION"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtPrecioComision_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtPrecioComision" 
                    ValidChars ="0123456789." Enabled="True">
                </asp:FilteredTextBoxExtender>
</td>
            <td>
                &nbsp;</td>
            <td><asp:CheckBox ID="chkPermiteEditarPrecio" runat="server" Text="Permite Editar Precio" SkinID="PERMITE_EDITAR_PRECIOPARCELA" />
</td>
<td>
</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:Label ID="Label7" runat="server" CssClass="Label" ForeColor="Red" 
                    Text="* Dato obligatorio"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtPrecioParcela" ErrorMessage="Ingresar Precio" 
                    ValidationGroup="GrupoValidacionGrabar" ForeColor="White"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" 
                runat="server" TargetControlID="RequiredFieldValidator1" Enabled="True">
            </asp:ValidatorCalloutExtender>
            </td>
           
            <td>
                &nbsp;</td>
           
        </tr>
        <tr>
            <td colspan="5">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtDescuentoLista" ErrorMessage="Ingresar Descuento" 
                    ForeColor="White" ValidationGroup="GrupoValidacionGrabar"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                runat="server" TargetControlID="RequiredFieldValidator2" Enabled="True">
            </asp:ValidatorCalloutExtender>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:CompareValidator ID="CVTipoMonedas" runat="server" 
                    ControlToValidate="ddlMoneda" ErrorMessage="Seleccione un tipo de moneda" 
                    Operator="NotEqual" ValidationGroup="GrupoValidacionGrabar" 
                    ForeColor="White"></asp:CompareValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" 
                runat="server" TargetControlID="CVTipoMonedas" Enabled="True">
            </asp:ValidatorCalloutExtender>
                <asp:RangeValidator ID="RangeValidator3" runat="server" 
                    ControlToValidate="txtFechaInicio" ErrorMessage="Fecha incorrecta" 
                    MaximumValue="31/12/3000" MinimumValue="01/01/1900" Type="Date" 
                    ValidationGroup="GrupoValidacionGrabar" ForeColor="White"></asp:RangeValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" 
                runat="server" TargetControlID="RangeValidator3" Enabled="True">
            </asp:ValidatorCalloutExtender>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan ="5">
                <asp:RangeValidator ID="RangeValidator4" runat="server" 
                    ControlToValidate="txtFechaHasta" ErrorMessage="Fecha incorrecta" 
                    ForeColor="White" MaximumValue="31/12/3000" MinimumValue="01/01/1900" 
                    Type="Date" ValidationGroup="GrupoValidacionGrabar"></asp:RangeValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" 
                runat="server" TargetControlID="RangeValidator4" Enabled="True">
            </asp:ValidatorCalloutExtender>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </fieldset>
    </div>
    <div style="width:90%;">
    <fieldset>
    <legend><h5>Grilla Articulos</h5></legend>
        <table>
             <tr>
                <td><asp:Label ID="lblArticuloGrilla" runat="server" Text="Articulo" CssClass="LabelNegrita"></asp:Label>
</td>
                <td style="width:800px;"><asp:DropDownList ID="ddlArticuloGrilla" runat="server" CssClass="ComboBox3"></asp:DropDownList>

                    &nbsp;<asp:Label ID="lblPorcentajeArticulo" runat="server" CssClass="LabelNegrita" Text="Porcentaje Articulo"></asp:Label>

                    &nbsp;<asp:TextBox ID="txtPorcentajeArticulo" runat="server" CssClass="TextBox1"></asp:TextBox>

                    &nbsp;<asp:ImageButton ID="btnAddArticuloGrilla" runat="server" 
                        ImageUrl="~/Images/Icons/add.png" ToolTip="Agregar Articulo" 
                        onclick="btnAddArticuloGrilla_Click" />
    
                </td>
            </tr>
        </table>
        <table>
            <tr style="width:100%">
                <td rowspan="5" style="width:100%;" >
                    <asp:GridView ID="grillaListaPreciosArticulos" runat="server" Width="100%" 
                        BackColor="White" BorderWidth="1px" AutoGenerateColumns="False" BorderStyle="Solid"
                        GridLines="None" BorderColor="#3366CC" 
                        onrowdatabound="grillaListaPreciosArticulos_RowDataBound1">
                        <AlternatingRowStyle CssClass ="GridAlternateRow" />
                        <Columns>
                        <asp:TemplateField>
                        <ItemTemplate>
                         <asp:ImageButton ID="btnEliminarListaPreciosArticulos" runat="server" ImageUrl="~/Images/Icons/delete.png" ToolTip="Eliminar Registro" />                            
                        </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <HeaderStyle  CssClass ="GridHeader"/>
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <RowStyle CssClass="GridRow" />
                        <SortedAscendingCellStyle BackColor="#EDF6F6" />
                        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                        <SortedDescendingCellStyle BackColor="#D6DFDF" />
                        <SortedDescendingHeaderStyle BackColor="#002876" />
                        </asp:GridView>
    
                </td>    
                <td></td>
            </tr>
            <tr style=" border-style:solid; border-color:Black; border-width:1px;width:100%">   
                <td></td>
            </tr>
            <tr style=" border-style:solid; border-color:Black; border-width:1px;width:100%">
                <td></td>
            </tr>
            <tr style=" border-style:solid; border-color:Black; border-width:1px;width:100%">
                <td></td>
            </tr>
            <tr style=" border-style:solid; border-color:Black; border-width:1px;width:100%">
                <td></td>
            </tr>
        </table>
    </fieldset>
    </div>
    <div>
    <fieldset>
        <table>
            <tr>
                <td><asp:Label ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Height="80px" Width="450px" CssClass="TextBox2" SkinID="OBSERVACION"></asp:TextBox>
                    
                </td>
            </tr>
        </table>
    </fieldset>
    </div>
</ContentTemplate>
</asp:TabPanel>


        <asp:TabPanel runat="server" ID="TabPanel1">
        <HeaderTemplate>
Aplicatibilidad y Condición de Ventas
</HeaderTemplate>
        
<ContentTemplate>
        
        <table style="border-style:solid; border-width:1px; border-color:Black; width:100%">
        <tr style="height:50%">
            <td style="border-style:solid; border-width:1px; border-color:Black; width:50%">
                <div>
                <fieldset>
                <legend><h5>Grilla Zonas</h5></legend>
                    <table>
                    <tr>
                        <td><asp:Label ID="lblZonas" runat="server" Text="Zona" CssClass="LabelNegrita"></asp:Label></td>
                        <td><asp:DropDownList ID="ddlZonas" runat="server" CssClass="ComboBox3"></asp:DropDownList>
                            &nbsp;<asp:ImageButton ID="btnAddZona" runat="server" 
                                ImageUrl="~/Images/Icons/add.png" ToolTip="Agregar Zona" 
                                onclick="btnAddZona_Click"  />
                        </td>
                    </tr>
                    </table>
                    <asp:GridView ID="grillaZonas" runat="server" Width="100%" 
                        BackColor="White" BorderWidth="1px" AutoGenerateColumns="False" BorderStyle="Solid"
                        GridLines="None" BorderColor="#3366CC" 
                        onrowdatabound="grillaZonas_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarZonas" runat="server" 
                                    ImageUrl="~/Images/Icons/delete.png"  ToolTip="Eliminar Registro" 
                                    onclick="btnEliminarZonas_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass ="GridAlternateRow" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                    <HeaderStyle  CssClass ="GridHeader"/>
                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                    <RowStyle CssClass="GridRow" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                    </asp:GridView> 
                </fieldset>
                </div>
            </td>
            <td style="border-style:solid; border-width:1px; border-color:Black; width:50%">
                <div>
                <fieldset>
                <legend><h5>Grilla Plan Modelos</h5></legend>
                <table>
                <tr>
                    <td><asp:Label ID="lblPlanModelo" runat="server" Text="Plan Modelo" CssClass="LabelNegrita"></asp:Label></td>
                    <td><asp:DropDownList ID="ddlPlanModelo" runat="server" CssClass="ComboBox3"></asp:DropDownList>
                        &nbsp;<asp:ImageButton ID="btnAddPlanModelo" runat="server" 
                            ImageUrl="~/Images/Icons/add.png" ToolTip="Agregar Plan Modelo" 
                            onclick="btnAddPlanModelo_Click" />
                    </td>
                </tr>
                </table>
                    <asp:GridView ID="grillaPlanModelos" runat="server" Width="100%" 
                        BackColor="White" BorderWidth="1px" AutoGenerateColumns="False" BorderStyle="Solid"
                        GridLines="None" BorderColor="#3366CC" 
                        onrowdatabound="grillaPlanModelos_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarPlanModelos" runat="server" 
                                    ImageUrl="~/Images/Icons/delete.png" ToolTip="Eliminar Registro" 
                                    onclick="btnEliminarPlanModelos_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass ="GridAlternateRow" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                    <HeaderStyle  CssClass ="GridHeader"/>
                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                    <RowStyle CssClass="GridRow" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                    </asp:GridView>
                </fieldset>
                </div>
            </td>
        </tr>
        <tr style="height:50%">
            <td colspan="2" style="width:100%;border-style:solid; border-width:1px; border-color:Black;">
                <div>
                <fieldset>
                <legend><h5>Grilla Tipo de Contrato</h5></legend>
                <table>
                <tr>
                    <td><asp:Label ID="lblTipoContrato" runat="server" Text="Tipo Contrato" CssClass="LabelNegrita"></asp:Label></td>
                    <td><asp:DropDownList ID="ddlTipoContrato" runat="server" CssClass="ComboBox3"></asp:DropDownList>
                        &nbsp;<asp:ImageButton ID="btnAddTipoContrato" runat="server" 
                            ImageUrl="~/Images/Icons/add.png" ToolTip="Agregar Tipo Contrato" 
                            onclick="btnAddTipoContrato_Click" />
                    </td>
                </tr>
                </table>
                    <asp:GridView ID="grillaTipoContrato" runat="server" Width="100%" 
                        BackColor="White" BorderWidth="1px" AutoGenerateColumns="False" BorderStyle="Solid"
                        GridLines="None" BorderColor="#3366CC" 
                        onrowdatabound="grillaTipoContrato_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarTipoContrato" runat="server" 
                                    ImageUrl="~/Images/Icons/delete.png" ToolTip="Eliminar Registro" 
                                    onclick="btnEliminarTipoContrato_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass ="GridAlternateRow" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                    <HeaderStyle  CssClass ="GridHeader"/>
                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                    <RowStyle CssClass="GridRow" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                    </asp:GridView>
                </fieldset>
                </div>
            </td>
            <td></td>
        </tr>
        </table>
        
</ContentTemplate>
        
</asp:TabPanel>


        <asp:TabPanel runat="server" ID="TabPanel2">
        <HeaderTemplate>
Precios y Condiciones de Venta
</HeaderTemplate>
        
<ContentTemplate>
        <table>
        <tr>
            <td><asp:Label ID="lblCondicionVenta" runat="server" Text="Condición Venta" CssClass="LabelNegrita"></asp:Label></td>
            <td><asp:DropDownList ID="ddlCondicionVenta" runat="server" CssClass="ComboBox2"></asp:DropDownList></td>
            <td><asp:Label ID="lblPrecio" runat="server" Text="Precio" CssClass="LabelNegrita"></asp:Label></td>
            <td><asp:TextBox ID="txtPrecio" runat="server" CssClass="TextBox2"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtPrecio_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtPrecio" ValidChars ="0123456789." 
                    Enabled="True" >
                </asp:FilteredTextBoxExtender>
            </td>
            <td><asp:Label ID="lblPrecioComisiones" runat="server" Text="Precio Comision"></asp:Label></td>
            <td><asp:TextBox ID="txtPrecioComisiones" runat="server" CssClass="TextBox2"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtPrecioComisiones_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtPrecioComisiones" 
                    ValidChars ="0123456789." Enabled="True">
                </asp:FilteredTextBoxExtender>
            </td>
            <td></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblMaxPorcentajeDesc" runat="server" Text="Max % Desc"></asp:Label></td>
            <td><asp:TextBox ID="txtMaxPorcentajeDesc" runat="server" CssClass="TextBox1"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtMaxPorcentajeDesc_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtMaxPorcentajeDesc" 
                    ValidChars ="0123456789." Enabled="True">
                </asp:FilteredTextBoxExtender>
            </td>
            <td><asp:Label ID="lblMaxPesoDesc" runat="server" Text="Max $ Desc"></asp:Label></td>
            <td><asp:TextBox ID="txtMaxPesoDesc" runat="server" CssClass="TextBox2"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtMaxPesoDesc_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtMaxPesoDesc" ValidChars ="0123456789." 
                    Enabled="True" >
                </asp:FilteredTextBoxExtender>
            </td>
            <td><asp:Label ID="lblMinAnticipo" runat="server" Text="Min Anticipo"></asp:Label></td>
            <td><asp:TextBox ID="txtMinAnticipo" runat="server" CssClass="TextBox2"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtMinAnticipo_FilteredTextBoxExtender" 
                    runat="server" TargetControlID="txtMinAnticipo" ValidChars ="0123456789." 
                    Enabled="True">
                </asp:FilteredTextBoxExtender>
            </td>
            <td><asp:ImageButton ID="btnAddPrecioCondicionVenta" runat="server" 
                    ImageUrl="~/Images/Icons/add.png" ToolTip="Agregar Precio Condicioón Venta" 
                    onclick="btnAddPrecioCondicionVenta_Click" /></td>
        </tr>
        <tr>
            <td colspan ="7">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan ="7">
                <asp:RangeValidator ID="RangeValidator11" runat="server" ErrorMessage="Ingresar un valor numérico" 
                    MaximumValue="999999" MinimumValue="0" Type="Double" 
                    ValidationGroup="GrupoValidacionGrabar" ControlToValidate ="txtMinAnticipo" ForeColor="White"></asp:RangeValidator>
                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender14" 
                runat="server" TargetControlID="RangeValidator11" Enabled="True">
            </asp:ValidatorCalloutExtender>
            </td>
        </tr>
        </table>
        <table>
        <tr>
            <td>
            <div>
            <fieldset>
            <legend><h5>Grilla Precios y Condiciones de Venta</h5></legend>
            <asp:GridView ID="grillaPrecioCondicionVenta" runat="server" Width="100%" 
                    BackColor="White" BorderWidth="1px" AutoGenerateColumns="False" BorderStyle="Solid"
                    GridLines="None" BorderColor="#3366CC" 
                    onrowdatabound="grillaPrecioCondicionVenta_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarPrecioCondicionVenta" runat="server" 
                                    ImageUrl="~/Images/Icons/delete.png"  ToolTip="Eliminar Registro" 
                                    onclick="btnEliminarPrecioCondicionVenta_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass ="GridAlternateRow" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                    <HeaderStyle  CssClass ="GridHeader"/>
                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                    <RowStyle CssClass="GridRow" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                </asp:GridView> 
            </fieldset>
            </div>
            </td>
        </tr>
        </table>
        
</ContentTemplate>
        
</asp:TabPanel>

        <asp:TabPanel runat="server" ID="TabPanel3">
        <HeaderTemplate>
        Comisiones
        </HeaderTemplate>
        
<ContentTemplate>
        <table>
            <tr>
                <td><asp:Label ID="lblPorcentajeVendedorContado" runat="server" CssClass="LabelNegrita" Text="Porcentaje Vendedor Contado"></asp:Label></td>
                <td><asp:TextBox ID="txtPorcenVendedorContado" runat="server" CssClass="TextBox3" SkinID="PORC_COMI_VENDEDOR_CONTADO"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtPorcenVendedorContado_FilteredTextBoxExtender" 
                        runat="server" TargetControlID="txtPorcenVendedorContado" 
                        ValidChars ="0123456789." Enabled="True">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblPorcenVendedorFinanciado" runat="server" CssClass="LabelNegrita" Text="Porcentaje Vendedor Financiado"></asp:Label></td>
                <td><asp:TextBox ID="txtPorcenVendedorFinanciado" runat="server" CssClass="TextBox3" SkinID="PORC_COMI_VENDEDOR_FINAN"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtPorcenVendedorFinanciado_FilteredTextBoxExtender" 
                        runat="server" TargetControlID="txtPorcenVendedorFinanciado" 
                        ValidChars ="0123456789." Enabled="True">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblPorcenJefeVendedorContado" runat="server"  CssClass="LabelNegrita" Text="Porcentaje Jefe Vendedor Contado"></asp:Label></td>
                <td><asp:TextBox ID="txtPorcenJefeVendedorContado" runat="server" CssClass="TextBox3" SkinID="PORC_COMI_JEFE_VEND_CONTA"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtPorcenJefeVendedorContado_FilteredTextBoxExtender" 
                        runat="server" TargetControlID="txtPorcenJefeVendedorContado" 
                        ValidChars ="0123456789." Enabled="True" >
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblPorcenJefeVendedorFinanciado" runat="server" CssClass="LabelNegrita" Text="Porcentaje Jefe Vendedor Financiado"></asp:Label></td>
                <td><asp:TextBox ID="txtPorcenJefeVendedorFinanciado" runat="server" CssClass="TextBox3" SkinID="PORC_COMI_JEFE_VEND_FINAN"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtPorcenJefeVendedorFinanciado_FilteredTextBoxExtender" 
                        runat="server" TargetControlID="txtPorcenJefeVendedorFinanciado" ValidChars ="0123456789.">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td colspan ="2"></td>
            </tr>
        </table>
        
        
</ContentTemplate>
        
</asp:TabPanel>

        <asp:TabPanel runat="server" ID="TabPanel4">
                <HeaderTemplate>
                Descuentos y tipos de comprobantes
                </HeaderTemplate>
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="Label1" CssClass="LabelNegrita" runat="server" Text="Tipo Descuento"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" CssClass="LabelNegrita" runat="server" Text="Max % Desc"></asp:Label>                            
                            </td>
                            <td>
                                <asp:Label ID="Label3" CssClass="LabelNegrita" runat="server" Text="Max Monto $"></asp:Label>                            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkDescuento" CssClass="CheckBox" Text="Desc. por Defecto" runat="server" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDescuentos" CssClass="ComboBox2" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPorcentaje" CssClass="ComboBox1" runat="server" 
                                    AutoPostBack="True" ontextchanged="txtPorcentaje_TextChanged"></asp:TextBox>
                                <asp:FilteredTextBoxExtender TargetControlID="txtPorcentaje" 
                                    ValidChars="0123456789.," ID="txtPorcentaje_FilteredTextBoxExtender" 
                                    runat="server" Enabled="True">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMonto" CssClass="ComboBox2" runat="server" 
                                    AutoPostBack="True" ontextchanged="txtMonto_TextChanged"></asp:TextBox> 
                                 <asp:FilteredTextBoxExtender TargetControlID="txtMonto" 
                                    ValidChars="0123456789.," ID="txtMonto_FilteredTextBoxExtender" runat="server" 
                                    Enabled="True">
                                </asp:FilteredTextBoxExtender>                                
                            </td>
                            <td>
                                <asp:ImageButton ID="btnAgregarDescuento" ImageUrl="~/Images/Icons/add.png" 
                                    runat="server" onclick="btnAgregarDescuento_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                 <asp:GridView ID="grillaDescuentos" runat="server" Width="100%" 
                                    BackColor="White" BorderWidth="1px" AutoGenerateColumns="False" BorderStyle="Solid"
                                    GridLines="None" BorderColor="#3366CC" 
                                    onrowdatabound="grillaDescuentos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btngrillaDescuentos" runat="server" 
                                                    ImageUrl="~/Images/Icons/delete.png"  ToolTip="Eliminar Registro" 
                                                    onclick="btngrillaDescuentos_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Desc.">                                            
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkDescuento" runat="server" Checked ="false"   
                                                Enabled ="false" Visible="true"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass ="GridAlternateRow" />
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <HeaderStyle  CssClass ="GridHeader"/>
                                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                    <RowStyle CssClass="GridRow" />
                                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                    <SortedDescendingHeaderStyle BackColor="#002876" />
                                </asp:GridView> 
                            
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
        </asp:TabPanel>
         <asp:TabPanel runat="server" ID="TabPanel5">
                <HeaderTemplate>
                    Cuotas periodicas
                </HeaderTemplate>
                <ContentTemplate>
                <div>
                <fieldset>
                <legend><h5>Grilla Cuotas periodicas</h5></legend>
                <table>
                <tr>
                    <td><asp:Label ID="Label8" runat="server" Text="Cat cuota periodica" CssClass="LabelNegrita"></asp:Label></td>
                    <td><asp:DropDownList ID="ddlCatCuotaPeriodica" runat="server" CssClass="ComboBox3"></asp:DropDownList>
                        &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" 
                            ImageUrl="~/Images/Icons/add.png" ToolTip="Agregar Plan Modelo" 
                            onclick="btnAddCuotaperiodica_Click" />
                    </td>
                </tr>
                </table>
                    <asp:GridView ID="grillacatCuotaPeriodica" runat="server"  
                        BackColor="White" BorderWidth="1px" AutoGenerateColumns="False" BorderStyle="Solid"
                        GridLines="None" BorderColor="#3366CC" 
                        onrowdatabound="grillacatCuotaPeriodica_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEliminarCatCuotaPeriodica" runat="server" 
                                    ImageUrl="~/Images/Icons/delete.png" ToolTip="Eliminar Registro" 
                                    onclick="btnEliminarCatCuotaPeriodica_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass ="GridAlternateRow" />
                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                    <HeaderStyle  CssClass ="GridHeader"/>
                    <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                    <RowStyle CssClass="GridRow" />
                    <SortedAscendingCellStyle BackColor="#EDF6F6" />
                    <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                    <SortedDescendingCellStyle BackColor="#D6DFDF" />
                    <SortedDescendingHeaderStyle BackColor="#002876" />
                    </asp:GridView>
                </fieldset>
                </div>
                    
                        
                 </ContentTemplate>
             </asp:TabPanel>

                
        </asp:TabContainer>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

