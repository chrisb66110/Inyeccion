using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoWeb.Models;

namespace ProyectoWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private ProyectoBDEntities db = new ProyectoBDEntities();

        //Get para cuando entra a index la primera vez
        [HttpGet]
        public ActionResult Index()
        {
            //Filtrado de los usuarios que se puede mostrar
            List<Usuario> usuariosValidos = db.Usuarios.Where(x => x.Mostrar==true).ToList();
            return View(usuariosValidos);
        }

        //Post para hacer busqueda de usuarios por el nombre de usuario
        [HttpPost]
        public ActionResult Index(string buscar)
        {
            
            //Conexion con la bd
            string cs = @"Data Source=DESKTOP-SBA28J1\SQLEXPRESS; Initial Catalog = ProyectoBD; Integrated Security = True";
            SqlConnection Con = new SqlConnection(cs);

            //Lista de usuarios a retornar
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                //Hacer la conexion
                Con.Open();

                //Consulta sql
                string sql = "";

                if (buscar != null)
                {
                    sql = "Select * from Usuarios where Usuario like '%" + buscar + "%' AND Mostrar = 1";
                    //La siguiente line ayuda a evitar la inyeccion, pues pone los parametros como literales
                    //sql = "Select * from Usuarios where Usuario like @Buscar AND Mostrar = 1";
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
                    usuarios.Add(new Usuario()
                    {
                        Usuario1 = rdr.GetString(rdr.GetOrdinal("Usuario")),
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

        // GET: Usuarios/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Usuario1,Contrasenna,Mostrar")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Usuario1,Contrasenna,Mostrar")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
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
