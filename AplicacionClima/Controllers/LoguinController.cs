using AplicacionClima.wservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplicacionClima.Controllers
{
    public class LoguinController : Controller
    {
        // GET: Loguin
        public ActionResult Index()
        {
            return View();
        }

        public int validarUsuario(string usuario, string contrasena)
        {
            int usuarioExiste = 0;

            try
            {
                control_climaSoapClient ws = new control_climaSoapClient();

                usuarioExiste = ws.ValidarUsuario(usuario, contrasena);
            }
            catch (Exception ex)
            {
                usuarioExiste = 0;
            }
            return usuarioExiste;
        }

        public ActionResult Cerrar()
        {
            return RedirectToAction("Index");
        }
    }
}