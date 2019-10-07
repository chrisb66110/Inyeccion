using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoWeb.Models;
using System.Data.SqlClient;

namespace ProyectoWeb.Controllers
{
    public class UsuariosInyeccionsController : Controller
    {
        private ProyectoBDEntities db = new ProyectoBDEntities();

        // GET: UsuariosInyeccions
        public ActionResult Index()
        {
            return View(db.UsuariosInyeccions.Where(x => x.Mostrar == true).ToList());
        }

        //Post para hacer busqueda de usuarios por el nombre de usuario
        [HttpPost]
        public ActionResult Index(string buscar)
        {

            //Conexion con la bd
            string cs = @"Server=10.1.4.55;Database=DB_CARNE;User Id=CARNE;Password=CONTRASENNA;";
            SqlConnection Con = new SqlConnection(cs);

            //Lista de usuarios a retornar
            List<UsuariosInyeccion> usuarios = new List<UsuariosInyeccion>();
            try
            {
                //Hacer la conexion
                Con.Open();

                //Consulta sql
                string sql = "";

                if (buscar != null)
                {
                    sql = "Select * from UsuariosInyeccion where Usuario like '%" + buscar + "%' AND Mostrar = 1";
                    //La siguiente line ayuda a evitar la inyeccion, pues pone los parametros como literales
                    //sql = "Select * from UsuariosInyeccion where Usuario like @Buscar AND Mostrar = 1";
                }

                //Se carga la consulta
                SqlCommand comando = new SqlCommand(sql, Con);
                //La siguiente line ayuda a evitar la inyeccion, pues pone los parametros como literales
                //comando.Parameters.Add("@Buscar", SqlDbType.VarChar,10).Value = "%" + buscar + "%";

                //Se ejecuta la consulta
                SqlDataReader rdr = comando.ExecuteReader();

                //Se hace la lista de usuarios a retornar
                while (rdr.Read())
                {
                    usuarios.Add(new UsuariosInyeccion()
                    {
                        Usuario = rdr.GetString(rdr.GetOrdinal("Usuario")),
                        Contrasenna = rdr.GetString(rdr.GetOrdinal("Contrasenna"))
                    });
                }
            }
            catch (Exception exp)
            {
            }
            finally
            {
                //Se cierra la conexion
                Con.Close();
            }

            return View(usuarios);
        }

        // GET: UsuariosInyeccions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosInyeccion usuariosInyeccion = db.UsuariosInyeccions.Find(id);
            if (usuariosInyeccion == null)
            {
                return HttpNotFound();
            }
            return View(usuariosInyeccion);
        }

        // GET: UsuariosInyeccions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuariosInyeccions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Usuario,Contrasenna,Mostrar")] UsuariosInyeccion usuariosInyeccion)
        {
            if (ModelState.IsValid)
            {
                db.UsuariosInyeccions.Add(usuariosInyeccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuariosInyeccion);
        }

        // GET: UsuariosInyeccions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosInyeccion usuariosInyeccion = db.UsuariosInyeccions.Find(id);
            if (usuariosInyeccion == null)
            {
                return HttpNotFound();
            }
            return View(usuariosInyeccion);
        }

        // POST: UsuariosInyeccions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Usuario,Contrasenna,Mostrar")] UsuariosInyeccion usuariosInyeccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuariosInyeccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuariosInyeccion);
        }

        // GET: UsuariosInyeccions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuariosInyeccion usuariosInyeccion = db.UsuariosInyeccions.Find(id);
            if (usuariosInyeccion == null)
            {
                return HttpNotFound();
            }
            return View(usuariosInyeccion);
        }

        // POST: UsuariosInyeccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UsuariosInyeccion usuariosInyeccion = db.UsuariosInyeccions.Find(id);
            db.UsuariosInyeccions.Remove(usuariosInyeccion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
