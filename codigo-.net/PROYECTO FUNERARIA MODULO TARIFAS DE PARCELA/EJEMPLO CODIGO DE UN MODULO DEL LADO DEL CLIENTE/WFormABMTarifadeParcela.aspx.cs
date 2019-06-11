using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WSCliente.Negocio;
using AjaxControlToolkit;
using System.Collections;

namespace WSCliente
{
    public partial class WFormABMTarifaDeParcela : System.Web.UI.Page
    {
        cFormABM FrmABM = new cFormABM();
        cFuncionesGrilla objListaPreciosArticulos;      
        cFuncionesGrilla objGrillaZonas;
        cFuncionesGrilla objTipoContrato;
        cFuncionesGrilla objPlanModelos; 
        cFuncionesGrilla objListaPreciosCondicionVenta;
        cFuncionesGrilla objGrillaDescuentos;
        cFuncionesGrilla objGrillaCatCuotasPeriodicas;


        String cod_listaPrecios;

        public String Cod_ListaPrecios
        {
            set
            {
                cod_listaPrecios = value;
            }
            get
            {
                return cod_listaPrecios;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["objListaPreciosArticulos"] == null)
                objListaPreciosArticulos = new cFuncionesGrilla();
            else
                objListaPreciosArticulos = (cFuncionesGrilla)Session["objListaPreciosArticulos"];
            
            if (Session["objGrillaZonas"] == null)
                objGrillaZonas = new cFuncionesGrilla();
            else
                objGrillaZonas = (cFuncionesGrilla)Session["objGrillaZonas"];
            
            if (Session["objTipoContrato"] == null)
                objTipoContrato = new cFuncionesGrilla();
            else
                objTipoContrato = (cFuncionesGrilla)Session["objTipoContrato"];
            
            if (Session["objPlanModelos"] == null)
                objPlanModelos = new cFuncionesGrilla();
            else
                objPlanModelos = (cFuncionesGrilla)Session["objPlanModelos"];
            
            if (Session["objListaPreciosCondicionVenta"] == null)
                objListaPreciosCondicionVenta = new cFuncionesGrilla();
            else
                objListaPreciosCondicionVenta = (cFuncionesGrilla)Session["objListaPreciosCondicionVenta"];

            if (Session["objGrillaDescuentos"] == null)
                objGrillaDescuentos = new cFuncionesGrilla();
            else
                objGrillaDescuentos = (cFuncionesGrilla)Session["objGrillaDescuentos"];

            if (Session["objGrillaCatCuotasPeriodicas"] == null)
                objGrillaCatCuotasPeriodicas = new cFuncionesGrilla();
            else
                objGrillaCatCuotasPeriodicas = (cFuncionesGrilla)Session["objGrillaCatCuotasPeriodicas"]; 

            FrmABM.setNombreDeArchivo(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            FrmABM.ConfiguracionInicial(Page, "LISTA_PRECIO", "COD_LISTA_PRECIO", "");

            FrmABM.RegistrarGrilla(ref grillaListaPreciosArticulos, "LISTAS_PRECIOS_PAR_ARTICULOS", "COD_ARTICULO;PORCENTAJE_EN_FACTURA", "COD_LISTA_PRECIO", "COD_ARTICULO");
            FrmABM.CrearLookup(grillaListaPreciosArticulos, "ARTICULOS", "COD_ARTICULO", "DESCRIPCION", "COD_ARTICULO");
            FrmABM.CrearLookup(grillaListaPreciosArticulos, "LISTA_PRECIO", "COD_LISTA_PRECIO", "NOMBRE", "COD_LISTA_PRECIO");

            FrmABM.RegistrarGrilla(ref grillaZonas, "LISTA_PRECIO_ZONA", "COD_ZONA", "COD_LISTA_PRECIO", "COD_ZONA");
            FrmABM.CrearLookup(grillaZonas, "ZONA", "COD_ZONA", "NOMBRE", "COD_ZONA");

            FrmABM.RegistrarGrilla(ref grillaPlanModelos, "LISTA_PRECIO_PV_MODELOS", "COD_PLAN_VENTA_MODELO", "COD_LISTA_PRECIO", "COD_PLAN_VENTA_MODELO");
            FrmABM.CrearLookup(grillaPlanModelos, "PLANES_VENTAS_MODELOS", "COD_PLAN_VENTA_MODELO", "NOMBRE", "COD_PLAN_VENTA_MODELO");

            FrmABM.RegistrarGrilla(ref grillaTipoContrato, "LISTA_PRECIO_TIPO_CONTRATO", "COD_TIPO_CONTRATO", "COD_LISTA_PRECIO", "COD_TIPO_CONTRATO");
            FrmABM.CrearLookup(grillaTipoContrato, "TIPOS_CONTRATOS", "COD_TIPO_CONTRATO", "NOMBRE", "COD_TIPO_CONTRATO");

            FrmABM.RegistrarGrilla(ref grillaPrecioCondicionVenta, "LISTA_PRECIO_COND_VENTAS", "COD_CONDICION_VENTA;PRECIO;MAX_PORCENTAJE_DESCUENTO;MONTO_MIN_ANTICIPO;MONTO_MAX_DESCUENTO;PRECIO_COMISION;DESCUENTO_DEFAULT", "COD_LISTA_PRECIO", "COD_CONDICION_VENTA");
            FrmABM.CrearLookup(grillaPrecioCondicionVenta, "CONDICIONES_VENTAS", "COD_CONDICION_VENTA", "DESCRIPCION", "COD_CONDICION_VENTA");
            FrmABM.CrearLookup(grillaPrecioCondicionVenta, "LISTA_PRECIO", "COD_LISTA_PRECIO", "NOMBRE", "COD_LISTA_PRECIO");

            FrmABM.RegistrarGrilla(ref grillaDescuentos, "LISTA_PRECIO_CAT_DESCUENTO", "COD_CAT_DESCUENTO;MAX_PORCENTAJE_DESCUENTO;MONTO_MAX_DESCUENTO;DESCUENTO_DEFAULT", "COD_LISTA_PRECIO", "COD_CAT_DESCUENTO");
            FrmABM.CrearLookup(grillaDescuentos, "CAT_DESCUENTOS", "COD_CAT_DESCUENTO", "CATEGORIA", "COD_CAT_DESCUENTO");

            FrmABM.RegistrarGrilla(ref grillacatCuotaPeriodica, "LISTA_PRECIO_CAT_CUOTAS_PER", "COD_CAT_CUOTA_PERIODICA", "COD_LISTA_PRECIO", "COD_CAT_CUOTA_PERIODICA");
            FrmABM.CrearLookup(grillacatCuotaPeriodica, "CAT_CUOTAS_PERIODICAS", "COD_CAT_CUOTA_PERIODICA", "CATEGORIA", "COD_CAT_CUOTA_PERIODICA");

            cAgente.setFormABM(FrmABM);
            if (!Page.IsPostBack)
            {
                if (Session["Usuario"] == null)
                {
                    Response.Redirect("~/WFormULogin.aspx");
                    return;
                }
                if (Session["BrowABM_Codigo"] != null)
                {
                    Cod_ListaPrecios = (String)Session["BrowABM_Codigo"];
                    Session["BrowABM_Codigo"] = null;
                }
                cAgente.setTipoDePagina(Page, cAgente.TiposDePagina.ABM);
                cMapCampos MapCampos = new cMapCampos();
                MapCampos.setNombreDeArchivo(Request.Url.Segments[Request.Url.Segments.Length - 1]);                
                MapCampos.Agregar("NOMBRE", "Nombre", true, true, true);
                MapCampos.Agregar("NIVELES", "Niveles", true, true, false);
                MapCampos.Agregar("COD_MONEDA", "Cod_moneda", false, false, true);
                MapCampos.PrepararBusqueda("LISTA_PRECIO", "COD_LISTA_PRECIO");
                Session.Add("MapCampos", MapCampos);
                CVTipoMonedas.ValueToCompare = cConstantes.TextoPorDefectoDDList;  
                CargarDatosGenerales();
                if ((Cod_ListaPrecios != "") && (Cod_ListaPrecios != null))
                {
                    cAgente.setColumnasPrimariasModificaElimina("COD_LISTA_PRECIO");
                    cAgente.setValoresPrimariosModificaElimina(Cod_ListaPrecios);
                    MiObjetoDB miODB = new MiObjetoDB();                    
                    DataSet miDS = miODB.CargarDatosEnControles(Page, FrmABM, Cod_ListaPrecios);
                                        
                    DataSet dsGrilla = FrmABM.CargarDatosEnGrillaRegistrada(ref grillaListaPreciosArticulos, miDS, Cod_ListaPrecios.ToString());
                    if (dsGrilla.Tables != null)
                        objListaPreciosArticulos.SourceGrilla = dsGrilla.Tables[0];

                    dsGrilla = FrmABM.CargarDatosEnGrillaRegistrada(ref grillaZonas, miDS, Cod_ListaPrecios.ToString());
                    if (dsGrilla.Tables != null)
                        objGrillaZonas.SourceGrilla = dsGrilla.Tables[0];

                    dsGrilla = FrmABM.CargarDatosEnGrillaRegistrada(ref grillaTipoContrato, miDS, Cod_ListaPrecios.ToString());
                    if (dsGrilla.Tables != null)
                        objTipoContrato.SourceGrilla = dsGrilla.Tables[0];

                    dsGrilla = FrmABM.CargarDatosEnGrillaRegistrada(ref grillaPlanModelos, miDS, Cod_ListaPrecios.ToString());
                    if (dsGrilla.Tables != null)
                        objPlanModelos.SourceGrilla = dsGrilla.Tables[0];

                    dsGrilla = FrmABM.CargarDatosEnGrillaRegistrada(ref grillaPrecioCondicionVenta, miDS, Cod_ListaPrecios.ToString());
                    if (dsGrilla.Tables != null)
                        objListaPreciosCondicionVenta.SourceGrilla = dsGrilla.Tables[0];

                    dsGrilla = FrmABM.CargarDatosEnGrillaRegistrada(ref grillaDescuentos, miDS, Cod_ListaPrecios.ToString());
                    if (dsGrilla.Tables != null)
                        objGrillaDescuentos.SourceGrilla = dsGrilla.Tables[0];

                    Cod_ListaPrecios = "";
                }
            }
        }

        public void CargarDatosGenerales()
        {
            MostrarEncabezadoGrillaArticulos();
            MostrarEncabezadoGrillagrillaPrecioCondicionVenta();
            MostrarEncabezadoGrillaZona();
            MostrarEncabezadoGrillaPlanModelos();
            MostrarEncabezadoGrillaTipoContratos();
            mostrarEncabezadoGrillaDescuentos();
            mostrarEncabezadoGrillaCatCuotaPeriodica();
            CargarResultadoReclamo();
            CargarZonas();
            CargarTipoContrato();
            CargarPlanVentaModelo();
            CargarArticulos();
            CargarArticulosGrilla();
            CargarPlanCondicionVenta();
            cargarComboDescuentos();
            cargarComboCuotaPeriodica();
        }
           
        private void MostrarEncabezadoGrillaArticulos()
        {
            if (Session["objListaPreciosArticulos"] == null)
            {
                objListaPreciosArticulos.SeteargrillaBrowser("COD_ARTICULO", "COD_ARTICULO", false, 10);
                objListaPreciosArticulos.SeteargrillaBrowser("Articulo", "DESCRIPCION", true, 400);
                objListaPreciosArticulos.SeteargrillaBrowser("Porcentaje", "PORCENTAJE_EN_FACTURA", true, 200);
                Session["objListaPreciosArticulos"] = objListaPreciosArticulos;
            }
            objListaPreciosArticulos.MostrarGrilla(ref grillaListaPreciosArticulos,  "COD_ARTICULO");
            Session["objListaPreciosArticulos"] = objListaPreciosArticulos;
        }

        private void MostrarEncabezadoGrillaZona()
        {
            if (Session["objGrillaZonas"] == null)
            {
                objGrillaZonas.SeteargrillaBrowser("COD_ZONA", "COD_ZONA", false, 10);
                objGrillaZonas.SeteargrillaBrowser("Descripcion", "NOMBRE", true, 300);
                Session["objGrillaZonas"] = objGrillaZonas;
            }
            objGrillaZonas.MostrarGrilla(ref grillaZonas, "COD_ZONA");
            Session["objGrillaZonas"] = objGrillaZonas;
        }

        private void MostrarEncabezadoGrillaPlanModelos()
        {
            if (Session["objPlanModelos"] == null)
            {
                objPlanModelos.SeteargrillaBrowser("COD_PLAN_VENTA_MODELO", "COD_PLAN_VENTA_MODELO", false, 10);
                objPlanModelos.SeteargrillaBrowser("Plan de Venta Modelo", "NOMBRE", true, 300);
                Session["objPlanModelos"] = objPlanModelos;
            }
            objPlanModelos.MostrarGrilla(ref grillaPlanModelos,  "COD_PLAN_VENTA_MODELO");
            Session["objPlanModelos"] = objPlanModelos;
        }

        private void MostrarEncabezadoGrillaTipoContratos()
        {
            if (Session["objTipoContrato"] == null)
            {
                objTipoContrato.SeteargrillaBrowser("COD_TIPO_CONTRATO", "COD_TIPO_CONTRATO", false, 10);
                objTipoContrato.SeteargrillaBrowser("Tipos de Contrato", "NOMBRE", true, 600);
                Session["objTipoContrato"] = objTipoContrato;
            }
            objTipoContrato.MostrarGrilla(ref grillaTipoContrato,  "COD_TIPO_CONTRATO");
            Session["objTipoContrato"] = objTipoContrato;
        }

        private void MostrarEncabezadoGrillagrillaPrecioCondicionVenta()
        {
            if (Session["objListaPreciosCondicionVenta"] == null)
            {
                objListaPreciosCondicionVenta.SeteargrillaBrowser("codCondicionVenta", "COD_CONDICION_VENTA", false, 10);
                objListaPreciosCondicionVenta.SeteargrillaBrowser("Descripción", "DESCRIPCION", true, 400);
                objListaPreciosCondicionVenta.SeteargrillaBrowser("Precio", "PRECIO", true, 150);
                objListaPreciosCondicionVenta.SeteargrillaBrowser("Max % Desc", "MAX_PORCENTAJE_DESCUENTO", true, 150);
                objListaPreciosCondicionVenta.SeteargrillaBrowser("Max $ Desc", "MONTO_MAX_DESCUENTO", true, 150);
                objListaPreciosCondicionVenta.SeteargrillaBrowser("Min $ Desc", "MONTO_MIN_ANTICIPO", true, 150);
                objListaPreciosCondicionVenta.SeteargrillaBrowser("Precio Comision", "PRECIO_COMISION", true, 150);
                objListaPreciosCondicionVenta.SeteargrillaBrowser("descuentoDefault", "DESCUENTO_DEFAULT", false, 5);
                Session["objListaPreciosCondicionVenta"] = objListaPreciosCondicionVenta;
            }
            objListaPreciosCondicionVenta.MostrarGrilla(ref grillaPrecioCondicionVenta,  "COD_CONDICION_VENTA");
            Session["objListaPreciosCondicionVenta"] = objListaPreciosCondicionVenta;
            
        }

        private void CargarResultadoReclamo()
        {
            cMoneda moneda = new cMoneda();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlMoneda, moneda.getTiposDeMonedas(), "MONEDA", "COD_MONEDA");
        }
        private void CargarArticulos()
        {
            cArticulo articulo = new cArticulo();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlArticulo, articulo.getTodosLosArticulos(), "DESCRIPCION", "COD_ARTICULO");
        }
        private void CargarArticulosGrilla()
        {
            cArticulo articulo = new cArticulo();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlArticuloGrilla, articulo.getTodosLosArticulos(), "DESCRIPCION", "COD_ARTICULO");
        }
        private void CargarZonas()
        {
            cZona zona = new cZona();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlZonas, zona.getTodasLasZonas(), "NOMBRE", "COD_ZONA");
        }
        private void CargarTipoContrato()
        {
            cTipoContrato tipoContrato = new cTipoContrato();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlTipoContrato, tipoContrato.getTodosLosTiposContrato(), "NOMBRE", "COD_TIPO_CONTRATO");
        }
        private void CargarPlanVentaModelo()
        {
            cPlanVentaModelo planVentaModelo = new cPlanVentaModelo();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlPlanModelo, planVentaModelo.getTodosLosPlanVentaModelo(), "NOMBRE", "COD_PLAN_VENTA_MODELO");
        }
        private void CargarPlanCondicionVenta()
        {
            cCondicionVenta condicionVenta = new cCondicionVenta();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlCondicionVenta, condicionVenta.getTodasLasCondicionesVentas(), "DESCRIPCION", "COD_CONDICION_VENTA");
        }

        private void cargarComboDescuentos()
        {
            cDescuento desc = new cDescuento();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlDescuentos, desc.getDescuentos(), "CATEGORIA", "COD_CAT_DESCUENTO");
        }



        protected void btnAddZona_Click(object sender, ImageClickEventArgs e)
        {
            int index;
            index = ddlZonas.SelectedIndex;
            if (index == 0)
            {
                return;
            }
            int COD_ZONA;
            string NOMBRE;
            COD_ZONA = Convert.ToInt32(ddlZonas.SelectedValue);
            NOMBRE = ddlZonas.SelectedItem.Text;
            
            if (objGrillaZonas.ValidarRegistros( "COD_ZONA", COD_ZONA.ToString()))
            {
                return;
            }
            string ListaValores;
            ListaValores = COD_ZONA + ";";
            ListaValores = ListaValores + NOMBRE;
            objGrillaZonas.AgregarRegistroGrilla(grillaListaPreciosArticulos, ListaValores);
        }

        protected void grillaZonas_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            objGrillaZonas.OcultarColumnas(grillaZonas, e);
            objGrillaZonas.MantenerCaracteres(grillaZonas, e);
        }

        protected void btnEliminarZonas_Click(object sender, ImageClickEventArgs e)
        {
            string COD_ZONA;
            ImageButton btnEliminarZona= sender as ImageButton;
            GridViewRow row = (GridViewRow)btnEliminarZona.NamingContainer;
            COD_ZONA = row.Cells[1].Text;
            
            objGrillaZonas.EliminarRegistroGrilla(grillaZonas ,  "COD_ZONA", COD_ZONA.ToString(), "COD_ZONA");
            Session["objGrillaZonas"] = objGrillaZonas;     
        }
        //
        protected void btnAddPlanModelo_Click(object sender, ImageClickEventArgs e)
        {
            int index;
            index = ddlPlanModelo.SelectedIndex;
            if (index == 0)
            {
                return;
            }
            int COD_PLAN_VENTA_MODELO;
            string NOMBRE;
            COD_PLAN_VENTA_MODELO = Convert.ToInt32(ddlPlanModelo.SelectedValue);
            NOMBRE = ddlPlanModelo.SelectedItem.Text;

            if (objPlanModelos.ValidarRegistros("COD_PLAN_VENTA_MODELO", COD_PLAN_VENTA_MODELO.ToString()))
            {
                return;
            }
            string ListaValores;
            ListaValores = COD_PLAN_VENTA_MODELO + ";";
            ListaValores = ListaValores + NOMBRE;
            objPlanModelos.AgregarRegistroGrilla(grillaPlanModelos, ListaValores);
        }

        protected void grillaPlanModelos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            objPlanModelos.OcultarColumnas(grillaPlanModelos, e);
            objPlanModelos.MantenerCaracteres(grillaPlanModelos, e);
        }

        protected void btnEliminarPlanModelos_Click(object sender, ImageClickEventArgs e)
        {
            string COD_PLAN_VENTA_MODELO;
            ImageButton btnEliminarPlanVentaModelo = sender as ImageButton;
            GridViewRow row = (GridViewRow)btnEliminarPlanVentaModelo.NamingContainer;
            COD_PLAN_VENTA_MODELO = row.Cells[1].Text;
            
            objPlanModelos.EliminarRegistroGrilla(grillaPlanModelos,  "COD_PLAN_VENTA_MODELO", COD_PLAN_VENTA_MODELO.ToString(), "COD_PLAN_VENTA_MODELO");
            Session["objPlanModelos"] = objPlanModelos;     
        }
        //
        
        protected void btnAddTipoContrato_Click(object sender, ImageClickEventArgs e)
        {
            int index;
            index = ddlTipoContrato.SelectedIndex;
            if (index == 0)
            {
                return;
            }
            int COD_TIPO_CONTRATO;
            string NOMBRE;
            COD_TIPO_CONTRATO = Convert.ToInt32(ddlTipoContrato.SelectedValue);
            NOMBRE = ddlTipoContrato.SelectedItem.Text;            
            if (objTipoContrato.ValidarRegistros( "COD_TIPO_CONTRATO", COD_TIPO_CONTRATO.ToString()))
            {
                return;
            }
            string ListaValores;
            ListaValores = COD_TIPO_CONTRATO + ";";
            ListaValores = ListaValores + NOMBRE;
            objTipoContrato.AgregarRegistroGrilla(grillaTipoContrato,  ListaValores);
        }
        
        protected void grillaTipoContrato_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            objTipoContrato.OcultarColumnas(grillaTipoContrato, e);
            objTipoContrato.MantenerCaracteres(grillaTipoContrato, e);
        }

        protected void btnEliminarTipoContrato_Click(object sender, ImageClickEventArgs e)
        {
            string COD_TIPO_CONTRATO;
            ImageButton btnEliminarTipoContrato = sender as ImageButton;
            GridViewRow row = (GridViewRow)btnEliminarTipoContrato.NamingContainer;
            COD_TIPO_CONTRATO = row.Cells[1].Text;
            
            objTipoContrato.EliminarRegistroGrilla(grillaTipoContrato,  "COD_TIPO_CONTRATO", COD_TIPO_CONTRATO.ToString(), "COD_TIPO_CONTRATO");
            Session["objTipoContrato"] = objTipoContrato;     
        }
        //
        
        protected void btnAddArticuloGrilla_Click(object sender, ImageClickEventArgs e)
        {            
            if (ddlArticuloGrilla.SelectedIndex==0 || txtPorcentajeArticulo.Text == "" || txtPorcentajeArticulo.Text == null)
            {
                return;
            }

            int i;
            int cod_articulo;
            string nombre_articulo, porcentaje_articulo;
            cod_articulo = Convert.ToInt32(ddlArticuloGrilla.SelectedValue);
            nombre_articulo = ddlArticuloGrilla.SelectedItem.Text.ToString();
            porcentaje_articulo = txtPorcentajeArticulo.Text.ToString();

            for (i = 0; i < grillaListaPreciosArticulos.Rows.Count; i++)
            {
                if (grillaListaPreciosArticulos.Rows[i].Cells[1].Text == cod_articulo.ToString() && 
                    grillaListaPreciosArticulos.Rows[i].Cells[2].Text == nombre_articulo.ToString())
                {
                    return;
                }
            }
            string ListaValores;
            ListaValores = cod_articulo + ";";
            ListaValores = ListaValores + nombre_articulo + ";";
            ListaValores = ListaValores + porcentaje_articulo;
            objListaPreciosArticulos.AgregarRegistroGrilla(grillaListaPreciosArticulos, ListaValores);
            Session["objListaPreciosArticulos"] = objListaPreciosArticulos;
        }

        protected void grillaListaPreciosArticulos_RowDataBound1(object sender, GridViewRowEventArgs e)
        {            
            objListaPreciosArticulos.OcultarColumnas(grillaListaPreciosArticulos, e);
            objListaPreciosArticulos.MantenerCaracteres(grillaListaPreciosArticulos, e);
        }

        protected void btnEliminarListaPreciosArticulos_Click(object sender, ImageClickEventArgs e)
        {
            string cod_articulo;
            ImageButton btnEliminarListaPreciosArticulos = sender as ImageButton;
            GridViewRow row = (GridViewRow)btnEliminarListaPreciosArticulos.NamingContainer;
            cod_articulo = row.Cells[1].Text;

            objListaPreciosArticulos.EliminarRegistroGrilla(grillaListaPreciosArticulos, "COD_ARTICULO", cod_articulo.ToString(), "COD_ARTICULO");
            Session["objListaPreciosArticulos"] = objListaPreciosArticulos;
        }
        //

        protected void btnAddPrecioCondicionVenta_Click(object sender, ImageClickEventArgs e)
        {
            objListaPreciosCondicionVenta = (cFuncionesGrilla)Session["objListaPreciosCondicionVenta"];
            if (ddlCondicionVenta.SelectedIndex == 0 || txtPrecio.Text == "" || txtPrecio.Text == null ||
                txtPrecioComisiones.Text == "" || txtPrecioComisiones== null || txtMaxPorcentajeDesc.Text == "" || txtMaxPorcentajeDesc.Text==null||
                txtMaxPesoDesc.Text == "" || txtMaxPesoDesc.Text == null || txtMinAnticipo.Text == "" || txtMinAnticipo.Text == null)
            {
                return;
            }

            int i;
            int COD_CONDICION_VENTA, precio, precioComisiones, maxPorcentajeDesc, maxPesoDesc, minAnticipo;
            string condicionVenta;

            COD_CONDICION_VENTA = Convert.ToInt32(ddlCondicionVenta.SelectedValue);
            condicionVenta = ddlCondicionVenta.SelectedItem.Text.ToString();

            precio = Convert.ToInt32(txtPrecio.Text.ToString());
            precioComisiones = Convert.ToInt32(txtPrecioComisiones.Text.ToString());
            maxPorcentajeDesc = Convert.ToInt32(txtMaxPorcentajeDesc.Text.ToString());
            maxPesoDesc = Convert.ToInt32(txtMaxPesoDesc.Text.ToString());
            minAnticipo = Convert.ToInt32(txtMinAnticipo.Text.ToString());


            for (i = 0; i < grillaPrecioCondicionVenta.Rows.Count; i++)
            {
                if (grillaPrecioCondicionVenta.Rows[i].Cells[1].Text == COD_CONDICION_VENTA.ToString() &&
                    grillaPrecioCondicionVenta.Rows[i].Cells[2].Text == condicionVenta.ToString() &&
                    grillaPrecioCondicionVenta.Rows[i].Cells[3].Text == precio.ToString() &&
                    grillaPrecioCondicionVenta.Rows[i].Cells[3].Text == precioComisiones.ToString())
                {
                    return;
                }
            }
            string ListaValores;
            ListaValores = COD_CONDICION_VENTA + ";";
            ListaValores = ListaValores + condicionVenta + ";";
            ListaValores = ListaValores + precio + ";";
            ListaValores = ListaValores + precioComisiones + ";";
            ListaValores = ListaValores + maxPorcentajeDesc + ";";
            ListaValores = ListaValores + maxPesoDesc + ";";
            ListaValores = ListaValores + minAnticipo + ";";
            ListaValores = ListaValores + 0 ;
            objListaPreciosCondicionVenta.AgregarRegistroGrilla(grillaPrecioCondicionVenta, ListaValores);
            Session["objListaPreciosCondicionVenta"] = objListaPreciosCondicionVenta;
        }

        protected void grillaPrecioCondicionVenta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            objListaPreciosCondicionVenta.OcultarColumnas(grillaPrecioCondicionVenta, e);
            objListaPreciosCondicionVenta.MantenerCaracteres(grillaPrecioCondicionVenta, e);
        }

        protected void btnEliminarPrecioCondicionVenta_Click(object sender, ImageClickEventArgs e)        
        {
            string COD_CONDICION_VENTA;
            ImageButton btnEliminarPrecioCondicionVenta = sender as ImageButton;
            GridViewRow row = (GridViewRow)btnEliminarPrecioCondicionVenta.NamingContainer;
            COD_CONDICION_VENTA = row.Cells[1].Text;

            objListaPreciosCondicionVenta.EliminarRegistroGrilla(grillaPrecioCondicionVenta, "COD_CONDICION_VENTA", COD_CONDICION_VENTA.ToString(), "COD_CONDICION_VENTA");
            Session["objListaPreciosCondicionVenta"] = objListaPreciosCondicionVenta;
        }
        //grilla descuentos 
        protected void btnAgregarDescuento_Click(object sender, ImageClickEventArgs e)
        {
            int codDescuento=0;
            string descuento="";
            double porcentaje;
            double monto;
            int descuentoPoDefecto;

            if (ddlDescuentos.SelectedIndex != 0)
            {
                codDescuento = int.Parse(ddlDescuentos.SelectedValue);
                descuento = ddlDescuentos.SelectedItem.Text.Trim();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MessageBox", "MessageBoxAceptar('Validacion','Debe seleccionar un tipo de descuento.','');", true);
                ddlDescuentos.Focus();
                return;
            }
            if (txtPorcentaje.Text.Trim() != "")
                porcentaje = double.Parse(txtPorcentaje.Text.Trim());
            else
                porcentaje = 0;
            if (txtMonto.Text.Trim() != "")
                monto = double.Parse(txtMonto.Text.Trim());
            else
                monto = 0;
            if (chkDescuento.Checked)
                descuentoPoDefecto = 1;
            else
                descuentoPoDefecto = 0;
                        
            string ListaValores;
            ListaValores = descuentoPoDefecto + ";";
            ListaValores = ListaValores + codDescuento + ";";
            ListaValores = ListaValores + descuento + ";";
            ListaValores = ListaValores + porcentaje + ";";
            ListaValores = ListaValores + monto;

            objGrillaDescuentos.AgregarRegistroGrilla(grillaDescuentos, ListaValores);
            Session["objGrillaDescuentos"] = objGrillaDescuentos;
            ActivarCheck();
            limpiar();
        }

        private void limpiar()
        {
            ddlDescuentos.SelectedIndex = 0;
            txtMonto.Text = "";
            txtPorcentaje.Text = "";
            chkDescuento.Checked = false;
        }

        private void mostrarEncabezadoGrillaDescuentos()
        {
            if (Session["objGrillaDescuentos"] == null)
            {
                objGrillaDescuentos.SeteargrillaBrowser("descuentoPorDefecto", "DESCUENTO_DEFAULT", false, 20);
                objGrillaDescuentos.SeteargrillaBrowser("codDescuento", "COD_CAT_DESCUENTO", false, 10);
                objGrillaDescuentos.SeteargrillaBrowser("Descuento", "CATEGORIA", true, 150);
                objGrillaDescuentos.SeteargrillaBrowser("Max. % Desc.", "MAX_PORCENTAJE_DESCUENTO", true, 100);
                objGrillaDescuentos.SeteargrillaBrowser("Monto $ Max.", "MONTO_MAX_DESCUENTO", true, 150);

                Session["objGrillaDescuentos"] = objGrillaDescuentos;
            }
            objGrillaDescuentos.MostrarGrilla(ref grillaDescuentos, "COD_CAT_DESCUENTO");
            Session["objGrillaDescuentos"] = objGrillaDescuentos;
        }

        protected void grillaDescuentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            objGrillaDescuentos.OcultarColumnas(grillaDescuentos, e);
            objGrillaDescuentos.MantenerCaracteres(grillaDescuentos, e);
        }

        protected void btngrillaDescuentos_Click(object sender, ImageClickEventArgs e)
        {
            string codDescuento;
            ImageButton btngrillaDescuentos = sender as ImageButton;
            GridViewRow row = (GridViewRow)btngrillaDescuentos.NamingContainer;
            codDescuento = row.Cells[3].Text;

            objGrillaDescuentos.EliminarRegistroGrilla(grillaDescuentos, "COD_CAT_DESCUENTO", codDescuento.ToString(), "COD_CAT_DESCUENTO");
            Session["objGrillaDescuentos"] = objGrillaDescuentos;
        }       
        private void ActivarCheck()
        {
            foreach (GridViewRow row in grillaDescuentos.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkDescuento");
                if (row.Cells[2].Text == "1")
                    chk.Checked = true;
                else
                    chk.Checked = false;
            }
        }

        //grilla cat cuotas periodicas

        public void cargarComboCuotaPeriodica()
        {
            cCatCuotaPeriodica catcuota = new cCatCuotaPeriodica();
            MiObjetoDB db = new MiObjetoDB();
            db.LlenarCBoxDB(ddlCatCuotaPeriodica, catcuota.getCatCuotasPeriodicas(), "CATEGORIA", "COD_CAT_CUOTA_PERIODICA");
        }
        protected void btnAddCuotaperiodica_Click(object sender, ImageClickEventArgs e)
        {
            int catcuotaperiodica = 0;
            string cuotaperiodica = "";

            if (ddlCatCuotaPeriodica.SelectedIndex != 0)
            {
                catcuotaperiodica = int.Parse(ddlCatCuotaPeriodica.SelectedValue);
                cuotaperiodica = ddlCatCuotaPeriodica.SelectedItem.Text.Trim();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "MessageBox", "MessageBoxAceptar('Validacion','Debe seleccionar una Categoría de cuota periodica.','');", true);
                ddlCatCuotaPeriodica.Focus();
                return;
            }

            string ListaValores="";

            ListaValores = ListaValores + catcuotaperiodica + ";" ;
            ListaValores = ListaValores + cuotaperiodica;
            objGrillaCatCuotasPeriodicas.ValidarRegistros("COD_CAT_CUOTA_PERIODICA",catcuotaperiodica.ToString());
            objGrillaCatCuotasPeriodicas.AgregarRegistroGrilla(grillacatCuotaPeriodica, ListaValores);
            Session["objGrillaCatCuotasPeriodicas"] = objGrillaCatCuotasPeriodicas;
            ddlCatCuotaPeriodica.SelectedIndex = 0;
        }

       

        private void mostrarEncabezadoGrillaCatCuotaPeriodica()
        {
            if (Session["objGrillaCatCuotasPeriodicas"] == null)
            {
                objGrillaCatCuotasPeriodicas.SeteargrillaBrowser("codcatcuotaperiodica", "COD_CAT_CUOTA_PERIODICA", false, 20);
                objGrillaCatCuotasPeriodicas.SeteargrillaBrowser("Categoría", "CATEGORIA", true, 100);

                Session["objGrillaCatCuotasPeriodicas"] = objGrillaCatCuotasPeriodicas;
            }
            objGrillaCatCuotasPeriodicas.MostrarGrilla(ref grillacatCuotaPeriodica, "COD_CAT_CUOTA_PERIODICA");
            Session["objGrillaCatCuotasPeriodicas"] = objGrillaCatCuotasPeriodicas;
        }

        protected void grillacatCuotaPeriodica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            objGrillaCatCuotasPeriodicas.OcultarColumnas(grillacatCuotaPeriodica, e);
            objGrillaCatCuotasPeriodicas.MantenerCaracteres(grillacatCuotaPeriodica, e);
        }

        protected void btnEliminarCatCuotaPeriodica_Click(object sender, ImageClickEventArgs e)
        {
            string codcatcuotaPeriodica;
            ImageButton btnEliminarCatCuotaPeriodica = sender as ImageButton;
            GridViewRow row = (GridViewRow)btnEliminarCatCuotaPeriodica.NamingContainer;
            codcatcuotaPeriodica = row.Cells[1].Text;

            objGrillaCatCuotasPeriodicas.EliminarRegistroGrilla(grillacatCuotaPeriodica, "COD_CAT_CUOTA_PERIODICA", codcatcuotaPeriodica.ToString(), "COD_CAT_CUOTA_PERIODICA");
            Session["objGrillaCatCuotasPeriodicas"] = objGrillaCatCuotasPeriodicas;
        }       

        //end grilla cat cuotas periodicas




        protected void txtPorcentaje_TextChanged(object sender, EventArgs e)
        {
            if(txtPorcentaje.Text.Trim()!="")
                txtPorcentaje.Text = cFunciones.FormatearNumero(txtPorcentaje.Text.Trim(), 2);

            txtMonto.Focus();

        }

        protected void txtMonto_TextChanged(object sender, EventArgs e)
        {
            if (txtMonto.Text.Trim() != "")
                txtMonto.Text = cFunciones.FormatearNumero(txtMonto.Text.Trim(), 2);

        }


    }
}