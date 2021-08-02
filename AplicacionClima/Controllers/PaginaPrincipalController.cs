using AplicacionClima.wservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionClima.Controllers
{
    public class PaginaPrincipalController : Controller
    {
        // GET: PaginaPrincipal
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarClimas() {
            control_climaSoapClient ws = new control_climaSoapClient();

            var listaClima = ws.ListarClimas();

            return Json(listaClima, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listarMaestro(string tabla, string filtro)
        {
            control_climaSoapClient ws = new control_climaSoapClient();

            var listaMaestro = ws.ListarMaestro(tabla, filtro);

            return Json(listaMaestro, JsonRequestBehavior.AllowGet);
        }

        public JsonResult consultarClima(int ciudad)
        {
            control_climaSoapClient ws = new control_climaSoapClient();

            var listaClima = ws.ConsultarClima(ciudad);

            return Json(listaClima, JsonRequestBehavior.AllowGet);
        }

        public JsonResult recuperarClima(int id)
        {
            control_climaSoapClient ws = new control_climaSoapClient();

            var listaClima = ws.RecuperarClima(id);

            return Json(listaClima, JsonRequestBehavior.AllowGet);
        }

        public int guardarClima(Clima clima)
        {
            control_climaSoapClient ws = new control_climaSoapClient();

            if (clima.id == 0)
            {
                int regAfectados = ws.CrearClima(clima);

                return regAfectados;
            }
            else {
                int regAfectados = ws.GuardarClima(clima);

                return regAfectados;
            }
            
        }

        public int eliminarClima(int id)
        {
            control_climaSoapClient ws = new control_climaSoapClient();

            int regAfectados = ws.EliminarClima(id);

            return regAfectados;
        }
    }
}