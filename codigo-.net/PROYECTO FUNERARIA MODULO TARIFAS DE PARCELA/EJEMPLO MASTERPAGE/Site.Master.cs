using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WSCliente.Negocio;
using System.Data;

namespace WSCliente
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        cFormABM FrmABMGeneral;

        // eventos para heredar
        public event EventHandler AntesGuardar;
        public event EventHandler DespuesGuardar;

        public event EventHandler AntesInsertar;
        public event EventHandler DespuesInsertar;

        public event EventHandler AntesModificar;
        public event EventHandler DespuesModificar;

        public event EventHandler AntesEliminar;
        public event EventHandler DespuesEliminar;

        // fin de eventos para heredar

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            Page.MaintainScrollPositionOnPostBack = true;
            NavigationMenu.Visible = false;

            if ((Request.QueryString["tp"] != "") && (Request.QueryString["tp"] != null))
                cAgente.setTituloPrincipal(Request.QueryString["tp"]);
            else
                cAgente.setTituloPrincipal("");

            if ((Request.QueryString["nmo"] != "") && (Request.QueryString["nmo"] != null))
                cAgente.setNombreModuloSeleccionado(Request.QueryString["nmo"]);
            else
                cAgente.setNombreModuloSeleccionado("");

            lblTituloPrincipal.Visible = false;
            if (cAgente.getTituloPrincipal().Trim() != "")
            {
                lblTituloPrincipal.Visible = true;
                lblTituloPrincipal.Text = cAgente.getTituloPrincipal().Trim();
            }
            if (cAgente.getNombreModuloSeleccionado().Trim() != "")
            {
                lblModuloSeleccionado.Visible = true;
                lblModuloSeleccionado.Text = cAgente.getNombreModuloSeleccionado().Trim();
            }

            switch (cAgente.getTipoDePagina())
            {
                case cAgente.TiposDePagina.ABM:
                    cAgente.setTipoDeBrowser(cAgente.TiposDeBrowser.BrowserABM);

                    FrmABMGeneral = new cFormABM();
                    FrmABMGeneral = cAgente.getFormABM();

                    IBtnNuevo.Visible = true;
                    if ((cAgente.getEstadoDePagina() == cAgente.EstadoDePagina.Insertando) || (cAgente.getEstadoDePagina() == cAgente.EstadoDePagina.Modificando))
                    {
                        IBtnGrabar.Visible = true;
                        IBtnModificar.Visible = true;
                    }
                    else
                    {
                        if (cAgente.getEstadoDePagina() == cAgente.EstadoDePagina.MostrandoResultadoBrowser)
                        {
                            IBtnGrabar.Visible = false;
                            IBtnModificar.Visible = true;
                            IBtnEliminar.Visible = true;
                        }
                        else
                        {
                            IBtnGrabar.Visible = false;
                            IBtnModificar.Visible = false;
                            IBtnEliminar.Visible = false;
                        }
                    }

                    IBtnEliminar.Attributes.Add("onClick", "if(!confirm('Esta seguro de eliminar el registro seleccionado?')){return false;};");
                    IBtnCancelar.Visible = true;

                    IBtnBuscar.Visible = true;
                    IBtnImprimir.Visible = true;
                    IBtnMenuPrincipal.Visible = true;
                    IBtnMenuModulo.Visible = true;

                    ContentPlaceHolderAtributosBrowser.Visible = false;                    
                    break;
                case cAgente.TiposDePagina.Menuprincipal:
                    cAgente.setTipoDeBrowser(cAgente.TiposDeBrowser.BrowserMenuprincipal);
                    IBtnGrabar.Visible = false;
                    IBtnNuevo.Visible = false;
                    IBtnModificar.Visible = false;
                    IBtnEliminar.Visible = false;
                    IBtnCancelar.Visible = false;

                    IBtnBuscar.Visible = true;
                    IBtnImprimir.Visible = true;
                    IBtnMenuPrincipal.Visible = true;
                    IBtnMenuModulo.Visible = true;

                    ContentPlaceHolderAtributosBrowser.Visible = false;                    
                    break;
                case cAgente.TiposDePagina.Browser:
                    switch (cAgente.getTipoDeBrowser())
                    {
                        case cAgente.TiposDeBrowser.BrowserABM:                            
                            ContentPlaceHolderAtributosBrowser.Visible = true;
                            break;
                        case cAgente.TiposDeBrowser.BrowserMenuprincipal:                            
                            ContentPlaceHolderAtributosBrowser.Visible = false;
                            break;
                    }
                    IBtnGrabar.Visible = false;
                    IBtnNuevo.Visible = false;
                    IBtnModificar.Visible = false;
                    IBtnEliminar.Visible = false;
                    IBtnCancelar.Visible = false;

                    IBtnBuscar.Visible = false;
                    IBtnImprimir.Visible = true;
                    IBtnMenuPrincipal.Visible = true;
                    IBtnMenuModulo.Visible = true;
                    break;
                case cAgente.TiposDePagina.Vacia:
                    cAgente.setTipoDeBrowser(cAgente.TiposDeBrowser.BrowserMenuprincipal);
                    IBtnGrabar.Visible = false;
                    IBtnNuevo.Visible = false;
                    IBtnModificar.Visible = false;
                    IBtnEliminar.Visible = false;
                    IBtnCancelar.Visible = false;

                    IBtnBuscar.Visible = false;
                    IBtnImprimir.Visible = false;
                    IBtnMenuPrincipal.Visible = false;
                    IBtnMenuModulo.Visible = false;

                    ContentPlaceHolderAtributosBrowser.Visible = false;                    
                    break;
            }

            //cVariableGlobal._Modulo = "";
            if ((Request.QueryString["modulo"] != "") && (Request.QueryString["modulo"] != null) && (Request.QueryString["modulo"] != "null"))
            {
                cVariableGlobal._Modulo = Request.QueryString["modulo"];            
                cGestorPermiso gestorPermiso;
                Menu MiMenu;
                LimpiaSession();                

                switch (cVariableGlobal._Modulo)
                {
                    case cModulo.K_MOD_CENTRAL:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_CENTRAL);
                        lblModuloSeleccionado.Text = "Módulo Central";                        
                        break;
                    case cModulo.K_MOD_COMPRAS:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_COMPRAS);
                        lblModuloSeleccionado.Text = "Módulo Compras";
                        break;
                    case cModulo.K_MOD_CONTABILIDAD:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_CONTABILIDAD);
                        lblModuloSeleccionado.Text = "Módulo Contabilidad";
                        break;
                    case cModulo.K_MOD_FONDOS:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_FONDOS);
                        lblModuloSeleccionado.Text = "Módulo Fondos";
                        break;
                    case cModulo.K_MOD_LEGAJOS:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_LEGAJOS);
                        lblModuloSeleccionado.Text = "Módulo Legajos";
                        break;
                    case cModulo.K_MOD_PERSONAL:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_PERSONAL);
                        lblModuloSeleccionado.Text = "Módulo Personal";
                        break;
                    case cModulo.K_MOD_STOCK:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_STOCK);
                        lblModuloSeleccionado.Text = "Módulo Stock";
                        break;
                    case cModulo.K_MOD_VENTAS:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_VENTAS);
                        lblModuloSeleccionado.Text = "Módulo Ventas";
                        break;
                    case cModulo.K_MOD_VISITAS:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_VISITAS);
                        lblModuloSeleccionado.Text = "Módulo Visitas";
                        break;
                    case cModulo.K_MOD_PARQUE:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_PARQUE);
                        lblModuloSeleccionado.Text = "Módulo Parque";
                        break;
                    case cModulo.K_MOD_SERV_SOC:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_SERV_SOC);
                        lblModuloSeleccionado.Text = "Módulo Servicios Sociales";
                        break;
                    case cModulo.K_MOD_ESTADISTICAS:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_ESTADISTICAS);
                        lblModuloSeleccionado.Text = "Módulo Estadísticas";
                        break;
                    case cModulo.K_MOD_FUNERARIA:
                        gestorPermiso = new cGestorPermiso(cVariableGlobal._CodUsuario);
                        MiMenu = NavigationMenu;
                        gestorPermiso.LlenarMenu(Page, MiMenu, cModulo.K_MOD_FUNERARIA);
                        lblModuloSeleccionado.Text = "Módulo Funeraria";
                        break;
                    default:
                        {
                            lblModuloSeleccionado.Text = "Modulo Selector";
                            break;
                        }
                }
            }
            if ((cVariableGlobal._Modulo.Trim() == "") && ((Request.QueryString["modulo"] == "") || (Request.QueryString["modulo"] == null) || (Request.QueryString["modulo"] == "null")))
            {
                NavigationMenu.Visible = true;
            }
            else
            {
                if ((cVariableGlobal._Modulo.Trim() != "") && ((Request.QueryString["modulo"] != "") && (Request.QueryString["modulo"] != null) && (Request.QueryString["modulo"] != "null")))
                {
                    NavigationMenu.Visible = true;
                }
                else
                    if (Request.QueryString["modulo"] != null)
                    {
                        if (Request.QueryString["modulo"] == "null")
                        {
                            NavigationMenu.Visible = true;
                        }
                        else
                        {
                            NavigationMenu.Visible = false;
                        }
                    }
                    else
                    {
                        NavigationMenu.Visible = false;
                    }
            }
        }

        private void LimpiaSession()
        {
            ///Limpia el listado de las variables de sesiones
            ///Solo mantiene las que pasamos por listado
            ///
            cAgente.LimpiarValiablesdeSesion(new List<string>(new string[] { "Usuario", "Usuario_Psw" }));
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (Session["Usuario"] != null)
            {
                Session.Abandon();
            }
            Response.Redirect("~/WFormULogin.aspx");
        }

        protected void IBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            if (AntesInsertar != null)
                AntesInsertar(this, e);
            Boolean cancelar = false;
            if (Session["CancelarEventosIntermedios"] != null)
                cancelar = (Boolean)Session["CancelarEventosIntermedios"];

            if (!cancelar)
            {
                IBtnGrabar.Visible = true;
                IBtnNuevo.Visible = false;
                IBtnModificar.Visible = false;
                IBtnEliminar.Visible = false;
                cAgente.LimpiarPagina(Page, FrmABMGeneral.getListaDeGrillasRegistradas());
                cAgente.HabilitarEdicion(Page);
                cAgente.setEstadoDePagina(cAgente.EstadoDePagina.Insertando);

                if (DespuesInsertar != null)
                    DespuesInsertar(this, e);
            }
        }

        protected void IBtnModificar_Click(object sender, ImageClickEventArgs e)
        {
            if (AntesModificar != null)
                AntesModificar(this, e);

            Boolean cancelar = false;
            if (Session["CancelarEventosIntermedios"] != null)
                cancelar = (Boolean)Session["CancelarEventosIntermedios"];

            if (!cancelar)
            {
                IBtnGrabar.Visible = true;
                IBtnNuevo.Visible = false;
                IBtnModificar.Visible = false;
                IBtnEliminar.Visible = false;
                cAgente.HabilitarEdicion(Page);
                cAgente.setEstadoDePagina(cAgente.EstadoDePagina.Modificando);


                if (DespuesModificar != null)
                    DespuesModificar(this, e);
            }
        }

        protected void IBtnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            IBtnGrabar.Visible = false;
            IBtnNuevo.Visible = true;
            IBtnModificar.Visible = false;
            IBtnEliminar.Visible = false;
            cAgente.setEstadoDePagina(cAgente.EstadoDePagina.SinEstado);
            cAgente.LimpiarPagina(Page, FrmABMGeneral.getListaDeGrillasRegistradas());
            cAgente.DeshabilitarEdicion(Page);
        }

        protected void IBtnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            IBtnGrabar.Visible = false;
            IBtnNuevo.Visible = false;
            IBtnModificar.Visible = true;
            IBtnEliminar.Visible = true;

            if (Session["Usuario"] != null)
            {
                if (cAgente.getTipoDeBrowser() == cAgente.TiposDeBrowser.BrowserABM)
                {
                    Response.Redirect(HttpUtility.HtmlDecode("~/BrowserABM.aspx?tp=" + cAgente.getTituloPrincipal()) + "&nmo=" + cAgente.getNombreModuloSeleccionado());
                }
                else
                {
                    Response.Redirect("~/Browser.aspx?tp=" + cAgente.getTituloPrincipal() + "&nmo=" + cAgente.getNombreModuloSeleccionado());
                }
                return;
            }
        }

        protected void IBtnGrabar_Click(object sender, ImageClickEventArgs e)
        {
            if (AntesGuardar != null)
                AntesGuardar(this, e);

            Boolean cancelar = false;
            if (Session["CancelarEventosIntermedios"] != null)
                cancelar = (Boolean)Session["CancelarEventosIntermedios"];

            if (!cancelar)
            {
                String resultado = "";

                if (cAgente.getEstadoDePagina() == cAgente.EstadoDePagina.Insertando)
                {
                    resultado = FrmABMGeneral.GuardarNuevoRegistro();
                    if (resultado.Trim() == "")
                    {
                        if (DespuesGuardar != null)
                            DespuesGuardar(this, e);

                        IBtnGrabar.Visible = false;
                        IBtnNuevo.Visible = true;
                        IBtnModificar.Visible = false;
                        IBtnEliminar.Visible = false;
                        cAgente.setEstadoDePagina(cAgente.EstadoDePagina.SinEstado);
                        cAgente.DeshabilitarEdicion(Page);
                        cAgente.LimpiarPagina(Page, FrmABMGeneral.getListaDeGrillasRegistradas());
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "miMessageBox", "MessageBoxAceptar('Información', 'Grabación realizada con éxito', '');", true);
                    }
                    else
                    {
                        IBtnGrabar.Visible = true;
                        IBtnNuevo.Visible = false;
                        IBtnModificar.Visible = false;
                        IBtnEliminar.Visible = false;
                        //resultado = resultado.Replace("'","\'");
                        String mensaje = "Ocurrió un error al intentar grabar los datos. " + resultado;
                        mensaje = cMensaje.Preparar(mensaje);
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "miMessageBox", "MessageBoxAceptar('Error', '" + mensaje + "', '');", true);
                    }
                }
                else
                    if (cAgente.getEstadoDePagina() == cAgente.EstadoDePagina.Modificando)
                    {
                        resultado = FrmABMGeneral.ActualizarRegistro();
                        if (resultado.Trim() == "")
                        {
                            if (DespuesGuardar != null)
                                DespuesGuardar(this, e);

                            IBtnGrabar.Visible = false;
                            IBtnNuevo.Visible = true;
                            IBtnModificar.Visible = false;
                            IBtnEliminar.Visible = false;
                            cAgente.setEstadoDePagina(cAgente.EstadoDePagina.SinEstado);
                            cAgente.DeshabilitarEdicion(Page);
                            cAgente.LimpiarPagina(Page, FrmABMGeneral.getListaDeGrillasRegistradas());
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "miMessageBox", "MessageBoxAceptar('Información', 'Actualización realizada con éxito', '');", true);
                        }
                        else
                        {
                            IBtnGrabar.Visible = true;
                            IBtnNuevo.Visible = false;
                            IBtnModificar.Visible = false;
                            IBtnEliminar.Visible = false;
                            String mensaje = "Ocurrió un error al intentar grabar los datos. " + resultado;
                            mensaje = cMensaje.Preparar(mensaje);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "miMessageBox", "MessageBoxAceptar('Error', '" + mensaje + "', '');", true);
                        }
                    }
            }
        }

        protected void IBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            if (AntesEliminar != null)
                AntesEliminar(this, e);
            Boolean cancelar = false;
            if (Session["CancelarEventosIntermedios"] != null)
                cancelar = (Boolean)Session["CancelarEventosIntermedios"];

            if (!cancelar)
            {
                IBtnGrabar.Visible = false;
                IBtnNuevo.Visible = true;
                IBtnModificar.Visible = false;
                IBtnEliminar.Visible = false;
                cAgente.setEstadoDePagina(cAgente.EstadoDePagina.Eliminando);
                String resultado = FrmABMGeneral.EliminarRegistro();
                if (resultado.Trim() == "")
                {
                    if (DespuesEliminar != null)
                        DespuesEliminar(this, e);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "miMessageBox", "MessageBoxAceptar('Información', 'Eliminación realizada con éxito', '');", true);
                    cAgente.LimpiarPagina(Page, FrmABMGeneral.getListaDeGrillasRegistradas());
                    cAgente.DeshabilitarEdicion(Page);
                }
                else
                {
                    String mensaje = "Ocurrió un error al intentar eliminar los datos. " + resultado;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "miMessageBox", "MessageBoxAceptar('Error', '" + resultado + "', '');", true);
                }
            }
        }

        protected void BtnAntiCierreSesion_Click(object sender, EventArgs e)
        {
            // no tiene que tener nada....
        }

        protected void IBtnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            switch (Session["ImpresionReporte"].ToString().ToUpper())
            {
                case "LISTADOPLANCUENTA":
                    break;
            }
        }

        protected void IBtnMenuPrincipal_Click(object sender, ImageClickEventArgs e)
        {
            cVariableGlobal._Modulo = "";
            Response.Redirect("~/Main2.aspx");
        }

        protected void btnCerrarSesion_Click1(object sender, ImageClickEventArgs e)
        {
            if (Session["Usuario"] != null)
            {
                Session.Abandon();
            }
            Response.Redirect("~/WFormULogin.aspx");
        }

        protected void IBtnMenuModulo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Main2.aspx?modulo=" + cVariableGlobal._Modulo);
        }



        // -------------------------------------------------- METODOS GENERALES --------------------------------------------------


        // -------------------------------------------------- METODOS DE ABM -----------------------------------------------------


        // -------------------------------------------------- METODOS DE Menuprincipal ------------------------------------------------


        // -------------------------------------------------- METODOS DE BROWSER -------------------------------------------------

    }



}
